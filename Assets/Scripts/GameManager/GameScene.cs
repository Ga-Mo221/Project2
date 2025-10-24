using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScene : MonoBehaviour
{
    public static GameScene Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void OpenSceneGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void OpenSceneMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OpenSceneLoadMap()
    {
        //SceneManager.LoadScene("Game");
        SceneManager.LoadScene("LoadMap");
    }
}
