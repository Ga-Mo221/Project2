using UnityEngine;
using UnityEngine.SceneManagement;

public class GetMainCam : MonoBehaviour
{
    void Start()
    {
        setupCamera();
    }

    public void setupCamera()
    {
        Canvas canvas = transform.GetComponent<Canvas>();
        if (canvas != null)
        {
            Camera cam = null;
            if (GameScene.Instance != null
                && (SceneManager.GetActiveScene().name == GameScene.Instance._mainMenuScene
                || SceneManager.GetActiveScene().name == GameScene.Instance._loadGenerateMapScene))
                cam = Camera.main;
            else if (CameraInfo.Instance != null)
                cam = CameraInfo.Instance.cameraMain;

            if (cam != null)
            {
                canvas.worldCamera = cam;
                //canvas.sortingLayerName = "UI";
            }
            else
            {
                Debug.LogWarning("[GetMainCam] mainCamObj không chứa component Camera.");
            }
        }
        else
        {
            Debug.LogWarning("[GetMainCam] Không tìm thấy Canvas hoặc mainCamObj null.");
        }
    }
}
