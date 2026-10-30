using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class title:MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clickSE;
    Vector3 originalScale;//‘å‚«‚³•Û‘¶

    void Start()
    {
        originalScale = transform.localScale;//“o˜^
    }
    public void OnClickButto()
    {
        StartCoroutine(ChangeScene());
    }


    public IEnumerator ChangeScene()
    {
        transform.localScale = originalScale * 0.9f;

        //audioSource.PlayOneShot(clickSE);

        yield return new WaitForSeconds(0.1f);

        transform.localScale = originalScale;

        yield return new WaitForSeconds(0.3f);

        SceneManager.LoadScene("main");
    }
    

}
