using MyBox;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    [ButtonMethod()]
    public void LoadGame()
    {
        PlayerPrefs.SetInt("ActiveScene", 1);

        Debug.Log(PlayerPrefs.GetInt("ActiveScene"));
        SceneLoader.Instance.LoadSceneAsync(PlayerPrefs.GetInt("ActiveScene"));
    }
}
