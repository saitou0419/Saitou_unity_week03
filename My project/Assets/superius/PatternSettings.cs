
using UnityEngine;
public class PatternSettings : MonoBehaviour
{
    [Header("対象のShapeオブジェクト")]
    [SerializeField] private GameObject[] shapes;

    [Header("パーツ表示位置")]
    [SerializeField]private Vector3 partPosition =new Vector3(0, 0.52f, 0);

    [Header("パーツ回転")]
    [SerializeField]private Vector3 partRotation = new Vector3(-90, 0, 0);

    [Header("パーツ大きさ")]
    [SerializeField] private Vector3 partScale = new Vector3(0.25f, 0.25f, 0.25f); 

    //private Sprite currentSprite;
    //private Vector2 currentBaseSize;

    private Texture selectedTexture = null;

    private static readonly int PatternTexId = Shader.PropertyToID("_PatternTexture");
    private static readonly int PatternOpacityId = Shader.PropertyToID("_PatternOpacit");

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
            Debug.LogError("Shapesがありません");
            return;
        }

        for (int i = 0; i < shapes.Length; i++)
        {
            if (shapes[i] != null && shapes[i].TryGetComponent<Renderer>(out var renderer))
            {
                // マテリアルのシェーダーを確認
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
    public void SetPart(Sprite partSprite)
    {
        if(partSprite==null)
        {
            Debug.LogWarning("パーツがありませんl");
            return;
        } 
        GameObject activeShape = null;
        for(int i=0;i<shapes.Length;i++)
        {
            if (shapes[i] != null && shapes[i].activeInHierarchy)
            {
                activeShape = shapes[i];
                break;  
            }
        }
       if(activeShape==null)
       {
            Debug.LogWarning("選択されてる型がありません");
            return;
       }
        Transform partsRoot = activeShape.transform.Find("PartsRoot");
        if(partsRoot ==null)
        {
            Debug.LogWarning("PartsRootが見つかりません");
            return; 
        }
        GameObject newPart = new GameObject(partSprite.name);
        newPart.transform.SetParent(partsRoot, false);
        newPart.transform.localPosition = partPosition;
        newPart.transform.localRotation = Quaternion.Euler(partRotation);
        newPart.transform.localScale = partScale;

        SpriteRenderer spriteRenderer = newPart.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = partSprite;
        Debug.Log("パーツを追加しました：" + partSprite.name);

    }
    public Texture SelectedTexture => selectedTexture;
}