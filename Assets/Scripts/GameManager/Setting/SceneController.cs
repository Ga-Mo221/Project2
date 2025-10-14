using UnityEngine;

public class SceneController : MonoBehaviour
{
    public void ExitGameEven()
    {
        if (SettingManager.Instance._playing)
        {
            SettingManager.Instance._playing = false;
            gameObject.SetActive(false);
            GameScene.Instance.OpenSceneMainMenu();
            transform.parent.gameObject.SetActive(false);
        }
    }

    public void OpenSceneGame()
    {
        GameScene.Instance.OpenSceneGame();
    }

    public void OpenSceneLoadMap()
    {
        GameScene.Instance.OpenSceneLoadMap();
    }
}
