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

    [Header("Scene Main Menu")]
    public string _mainMenuScene = "MainMenu";
    [Header("Scene Load Map")]
    public string _loadMapScene = "LoadMap";
    public string _loadGenerateMapScene = "LoadMap_RenderMap";
    [Header("Scene Game")]
    public string _gameScene = "Game";
    public string _gameGenerateScene = "Game_RenderMap";

    public void OpenSceneGame()
    {
        SceneManager.LoadScene(_gameScene);
    }

    public void OpenSceneMainMenu()
    {
        SceneManager.LoadScene(_mainMenuScene);
    }

    public void OpenSceneLoadMap()
    {
        //SceneManager.LoadScene("Game");
        SceneManager.LoadScene(_loadMapScene);
    }

    public void OpenSceneGameTutorial()
    {
        SceneManager.LoadScene(_loadMapScene);
    }
}
