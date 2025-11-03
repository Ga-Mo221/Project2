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

    public void OpenSceneLoadMap(string sceneName)
    {
        // Delay để animation chạy xong rồi mới load scene
        if (SettingManager.Instance != null && SettingManager.Instance._gameSettings._Tutorial)
        {
            StartCoroutine(LoadSceneTutorialDelay(2f));
        }
        else StartCoroutine(LoadSceneAfterDelay(sceneName, 2f));
    }

    private System.Collections.IEnumerator LoadSceneAfterDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    private System.Collections.IEnumerator LoadSceneTutorialDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (GameScene.Instance != null)
            GameScene.Instance.OpenSceneGameTutorial();
    }
}
