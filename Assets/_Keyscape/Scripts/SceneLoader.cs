using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    public TransitionScreen transitionScreen;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadSceneAsync(int sceneToLoadIndex)
    {
        StartCoroutine(LoadSceneAsyncCoroutine(sceneToLoadIndex));
    }

    private IEnumerator LoadSceneAsyncCoroutine(int sceneToLoadIndex)
    {
        //Start fade in
        yield return new WaitForSeconds(transitionScreen.StartIn());
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoadIndex);
        asyncLoad.allowSceneActivation = true;

        while (asyncLoad.isDone == false)
        {
            yield return null;
        }

        yield return new WaitForSeconds(.4f);

        transitionScreen.StartOut();
    }
}
