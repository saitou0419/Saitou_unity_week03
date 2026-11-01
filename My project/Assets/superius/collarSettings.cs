using Unity.VisualScripting;
using UnityEngine;

public class collarSettings: MonoBehaviour
{
    [SerializeField] private GameObject[] collars;

    public void Collar1()
    {
        ChangeCollar(0);
    }
    public void Collar2()
    {
        ChangeCollar(1);
    }
    public void Collar3()
    {
        ChangeCollar(2);
    }
    public void Collar4()
    {
        ChangeCollar(3);
    }
    public void Collar5()
    {
        ChangeCollar(4);
    }
    private void ChangeCollar(int num)
    {
        for (int i =0;i<collars.Length;i++)
        {
            collars[i].SetActive(false);
        }
        Debug.Log("選択番号：" + num);

        // すべての背景を非表示
        for (int i = 0; i < collars.Length; i++)
        {
            collars[i].SetActive(false);
        }

        // 選択した背景を表示
        collars[num].SetActive(true);

        Debug.Log("表示した背景：" + collars[num].name);
    }

}
