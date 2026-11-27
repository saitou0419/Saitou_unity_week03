using UnityEngine;

public class PatternSettings : MonoBehaviour
{
    [Header("対象のShapeオブジェクト")]
    [SerializeField] private GameObject[] shapes;

    [Header("パーツを配置する場所")]
    [SerializeField] private Transform partsRoot;

    [Header("パーツ表示位置")]
    [SerializeField] private Vector3 partPosition = new Vector3(0, 0.52f, 0);

    [Header("パーツ回転")]
    [SerializeField] private Vector3 partRotation = new Vector3(0, 0, 0);

    [Header("パーツ大きさ")]
    [SerializeField] private Vector3 partScale = new Vector3(0.25f, 0.25f, 0.25f);

    private Texture selectedTexture = null;

    private static readonly int PatternTexId = Shader.PropertyToID("_PatternTexture");
    private static readonly int PatternOpacityId = Shader.PropertyToID("_PatternOpacit");

    // パーツの描画順（最前面管理用）
    public static int topSortingOrder = 0;

    private void Start()
    {
        ApplyPatternToAllShapes();
    }

    public void SetPattern(Texture newTexture)
    {
        if (newTexture == null)
        {
            Debug.LogWarning("模様がありません");
            return;
        }

        selectedTexture = newTexture;
        ApplyPatternToAllShapes();
    }

    public void ClearPattern()
    {
        selectedTexture = null;
        ApplyPatternToAllShapes();
    }

    public void ApplyPatternToAllShapes()
    {
        if (shapes == null || shapes.Length == 0)
        {
            Debug.LogError("Shapesがありません。対象オブジェクト：" + gameObject.name);
            return;
        }

        for (int i = 0; i < shapes.Length; i++)
        {
            if (shapes[i] != null && shapes[i].TryGetComponent<Renderer>(out var renderer))
            {
                if (!renderer.material.HasProperty(PatternTexId))
                {
                    Debug.LogError($"[PatternSettings] {shapes[i].name} のマテリアル({renderer.material.name})には PatternShader が設定されていません！");
                }

                renderer.material.SetTexture(PatternTexId, selectedTexture);

                float opacity = (selectedTexture != null) ? 1.0f : 0.0f;
                renderer.material.SetFloat(PatternOpacityId, opacity);
            }
        }
    }

    /// <summary>
    /// パーツを画面に追加する（複数配置可能）
    /// </summary>
    public void SetPart(Sprite partSprite)
    {
        if (partSprite == null)
        {
            Debug.LogWarning("パーツがありません");
            return;
        }
        if (partsRoot == null)
        {
            Debug.LogWarning("PartsRootが設定されてません");
            return;
        }

        // ※以前の削除ループを外したことで、ボタンを押すたびに何個でも追加可能になりました！

        // 重なりを防ぐため少しだけ位置をずらして出現
        Vector3 spawnOffset = new Vector3(Random.Range(-0.04f, 0.04f), 0, Random.Range(-0.04f, 0.04f));

        GameObject newPart = new GameObject(partSprite.name);
        newPart.transform.SetParent(partsRoot, false);
        newPart.transform.localPosition = partPosition + spawnOffset;
        newPart.transform.localRotation = Quaternion.Euler(partRotation);
        newPart.transform.localScale = partScale;

        SpriteRenderer spriteRenderer = newPart.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = partSprite;

        // 後から出たパーツが手前に表示されるように設定
        topSortingOrder++;
        spriteRenderer.sortingOrder = topSortingOrder;

        // 当たり判定 (Zの厚みを0.2fにしてクリックを確実に検知)
        BoxCollider boxCollider = newPart.AddComponent<BoxCollider>();
        Vector3 spriteSize = spriteRenderer.sprite.bounds.size;
        boxCollider.center = spriteRenderer.sprite.bounds.center;
        boxCollider.size = new Vector3(spriteSize.x, spriteSize.y, 0.2f);

        // 移動・拡縮スクリプトを追加
        newPart.AddComponent<PartsChange>();

        Debug.Log($"パーツを追加しました: {newPart.name} (合計 {partsRoot.childCount} 個)");
    }

    /// <summary>
    /// すべてのパーツを一括削除する（リセット用）
    /// </summary>
    public void ClearAllParts()
    {
        if (partsRoot == null) return;
        for (int i = partsRoot.childCount - 1; i >= 0; i--)
        {
            Destroy(partsRoot.GetChild(i).gameObject);
        }
        topSortingOrder = 0;
        Debug.Log("すべてのパーツを削除しました");
    }

    public Texture SelectedTexture => selectedTexture;
}
