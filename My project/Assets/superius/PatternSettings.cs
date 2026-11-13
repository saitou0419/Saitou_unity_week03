
using UnityEngine;

public class PatternSettings : MonoBehaviour
{
    [Header("対象のShapeオブジェクト群 (square, Circle, diamond, Rectangle)")]
    [SerializeField] private GameObject[] shapes;

    private Texture selectedTexture = null;

    private static readonly int PatternTexId = Shader.PropertyToID("_PatternTexture");
    private static readonly int PatternOpacityId = Shader.PropertyToID("_PatternOpacit");

    private void Start()
    {
        ApplyPatternToAllShapes();
    }

    public void SetPattern(Texture newTexture)
    {
        // ★クリック時にログを出す
        if (newTexture == null)
        {
            Debug.LogWarning("[PatternSettings] ボタンが押されましたが、渡されたテクスチャが null です！引数の設定を確認してください。");
        }
        else
        {
            Debug.Log($"[PatternSettings] 模様を選択しました: {newTexture.name}");
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
            Debug.LogError("[PatternSettings] shapes 配列が空です！InspectorでShapeオブジェクトを設定してください。");
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

    public Texture SelectedTexture => selectedTexture;
}