using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject winCanvas;
    public GameObject loseCanvas;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        winCanvas.SetActive(false);
        loseCanvas.SetActive(false);
    }

    public void Win()
    {
        PlayerPrefs.SetInt("ActiveScene", PlayerPrefs.GetInt("ActiveScene") + 1);
        LoadScene(PlayerPrefs.GetInt("ActiveScene"));
        winCanvas.SetActive(true);
    }

    public void Lose()
    {
        LoadScene(PlayerPrefs.GetInt("ActiveScene"));
        loseCanvas.SetActive(true);
    }

    private void LoadScene(int sceneID)
    {
        Debug.Log($"Loading {sceneID}");
        SceneLoader.Instance.LoadSceneAsync(sceneID);
    }
}
