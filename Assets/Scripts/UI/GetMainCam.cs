using UnityEngine;

public class GetMainCam : MonoBehaviour
{
    void Start()
    {
        setupCamera();
    }

    public void setupCamera()
    {
        GameObject mainCamObj = Camera.main?.gameObject;

        Canvas canvas = transform.GetComponent<Canvas>();
        if (canvas != null && mainCamObj != null)
        {
            Camera cam = mainCamObj.GetComponent<Camera>();
            if (cam != null)
            {
                canvas.worldCamera = cam;
                canvas.sortingLayerName = "UI";
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
