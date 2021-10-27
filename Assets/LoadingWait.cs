using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingWait : MonoBehaviour
{
    [SerializeField] Transform value;
    [SerializeField] TMP_Text text;
    [SerializeField] GameObject unload;
    private void Start()
    {
        DontDestroyOnLoad(unload);
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return null;
        yield return new WaitForSeconds(1f);
        //Begin to load the Scene you specify
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(1);
        while (!asyncOperation.isDone)
        {
            //Output the current progress
            value.transform.localScale = new Vector3(asyncOperation.progress, 1, 1);
            text.text = (asyncOperation.progress * 100).ToString("F0") + "%";
            unload.GetComponent<Animator>().Play("UnLoading");
            yield return null;
        }
    }
}
