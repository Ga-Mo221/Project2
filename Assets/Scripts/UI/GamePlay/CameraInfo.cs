using UnityEngine;

public class CameraInfo : MonoBehaviour
{
    public static CameraInfo Instance { get; private set; }

    public Camera cameraMain;
    public CameraLimit cameraLimit;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SyncLimitWithMap(SizeMap map)
    {
        if (cameraLimit != null && map != null)
            cameraLimit.SetLimitFromMap(map);
        else
            Debug.LogWarning("[CameraInfo] Missing reference: cameraLimit or map.");
    }
}
