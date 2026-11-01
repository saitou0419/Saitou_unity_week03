using UnityEngine;
using UnityEditor;

public class Checkfinished : MonoBehaviour
{
    [SerializeField] private GameObject finishedGame;

    public void Confirm()//営業終了ボタンを押した
    {
        finishedGame.SetActive(true);
    }
    public void Cancel()//いいえ　ボタン
    {
        finishedGame.SetActive(false);
    }
    public void Exit()//はい　ボタン
    {
        Application.Quit();
    }


}

