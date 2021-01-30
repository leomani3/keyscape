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
        DontDestroyOnLoad(gameObject);
        
        winCanvas.SetActive(false);
        loseCanvas.SetActive(false);
    }

    public void Win()
    {
        winCanvas.SetActive(true);
    }

    public void Lose()
    {
        loseCanvas.SetActive(true);
    }
}
