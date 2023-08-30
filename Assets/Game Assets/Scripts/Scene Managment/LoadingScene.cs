using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    public GameObject LoaderUI;
    public Slider progressSlider;

    // public method that loads the scene by inputing the scene name
    public void LoadScene(string sceneName)
    {
        Debug.Log("load scene is being called");
        StartCoroutine(LoadScene_Coroutine(sceneName));
    }

    public IEnumerator LoadScene_Coroutine(string sceneName)
    {
        Debug.Log("Coroutine is starting");
        progressSlider.value = 0;
        LoaderUI.SetActive(true);
        Debug.Log("LoaderUI is on");

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;
        float progress = 0;

        Debug.Log("while loop is about to start");
        while (!asyncOperation.isDone)
        {
            progress = Mathf.MoveTowards(progress, asyncOperation.progress, Time.deltaTime);
            progressSlider.value = progress;
            if (progress >= 0.9f)
            {
                progressSlider.value = 1;
                asyncOperation.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    public void LoadSceneNoUI(string sceneName)
    {
        Debug.Log("load scene no ui is being called");
        SceneManager.LoadSceneAsync(sceneName);
    }
}