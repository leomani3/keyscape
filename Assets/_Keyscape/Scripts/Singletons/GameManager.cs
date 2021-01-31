using MyBox;
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

        ResetCanvas();
        DontDestroyOnLoad(gameObject);
    }

    private void LoadScene(int sceneID)
    {
        Debug.Log($"Loading {sceneID}");
        SceneLoader.Instance.LoadSceneAsync(sceneID);
    }

    [ButtonMethod()]
    public void Win()
    {
        if (PlayerPrefs.GetInt("ActiveScene") + 1 > 4)
        {
            PlayerPrefs.SetInt("ActiveScene", 0);
        }
        else
        {
            PlayerPrefs.SetInt("ActiveScene", PlayerPrefs.GetInt("ActiveScene") + 1);
        }

        LoadScene(PlayerPrefs.GetInt("ActiveScene"));
        winCanvas.SetActive(true);
    }

    public void Lose()
    {
        LoadScene(PlayerPrefs.GetInt("ActiveScene"));
        loseCanvas.SetActive(true);
    }

    public void ResetCanvas()
    {
        winCanvas.SetActive(false);
        loseCanvas.SetActive(false);
    }
}
