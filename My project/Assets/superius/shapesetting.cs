using UnityEngine;

public class shapesetting : MonoBehaviour
{
    [SerializeField] private GameObject[] shape;
    private Color selectedColor = Color.white;

    // _BaseColor プロパティのID
    private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");

    public void Square() => ChangeShape(0);
    public void Circle() => ChangeShape(1);
    public void diamond() => ChangeShape(2);
    public void Rectangle() => ChangeShape(3);

    private void ChangeShape(int num)
    {
        for (int i = 0; i < shape.Length; i++)
        {
            shape[i].SetActive(false);
        }
        shape[num].SetActive(true);
        ApplyColorToShape(shape[num]);
    }

    public void Red() => ChangeColor(new Color(0.55f, 0.0f, 0.0f));
    public void Blue() => ChangeColor(new Color(0.35f, 0.65f, 1.0f));
    public void Green() => ChangeColor(new Color(0.0f, 0.75f, 0.4f));
    public void Orenge() => ChangeColor(new Color(1.0f, 0.65f, 0.1f));
    public void Pink() => ChangeColor(new Color(1.0f, 0.55f, 0.6f));

    private void ChangeColor(Color color)
    {
        selectedColor = color;
        for (int i = 0; i < shape.Length; i++)
        {
            if (shape[i].activeSelf)
            {
                ApplyColorToShape(shape[i]);
            }
        }
    }

    // _BaseColor を確実に設定するヘルパーメソッド
    private void ApplyColorToShape(GameObject obj)
    {
        if (obj != null && obj.TryGetComponent<Renderer>(out var renderer))
        {
            renderer.material.SetColor(BaseColorId, selectedColor);
        }
    }
}