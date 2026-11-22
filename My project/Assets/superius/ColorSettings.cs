
using UnityEngine;

public class ColorSettings : MonoBehaviour
{
    [Header("色選択ボタンの背景")]
    [SerializeField] private GameObject[] Colors;

    [Header("切り替えるページ")]
    [SerializeField] private GameObject[] collars;


    // 色選択ボタンの背景切り替え
    public void Color1() => ChangeColor(0);
    public void Color2() => ChangeColor(1);
    public void Color3() => ChangeColor(2);
    public void Color4() => ChangeColor(3);
    public void Color5() => ChangeColor(4);

    private void ChangeColor(int num)
    {
        for (int i = 0; i < Colors.Length; i++)
        {
            Colors[i].SetActive(false);
        }

        Colors[num].SetActive(true);

        Debug.Log("選択した色番号：" + num);
    }


    // ページ切り替え
    public void Collar1() => ChangeCollar(0);
    public void Collar2() => ChangeCollar(1);
    public void Collar3() => ChangeCollar(2);
    public void Collar4() => ChangeCollar(3);
    public void Collar5() => ChangeCollar(4);

    private void ChangeCollar(int num)
    {
        for (int i = 0; i < collars.Length; i++)
        {
            collars[i].SetActive(false);
        }

        collars[num].SetActive(true);

        Debug.Log("表示したページ：" + collars[num].name);
    }
}