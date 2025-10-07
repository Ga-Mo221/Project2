using System.Collections;
using UnityEngine;

public class CameraTutorial : MonoBehaviour
{
    [SerializeField] private GameObject _cameraObj;
    private Camera _camera;
    private Coroutine moveRoutine;
    private Coroutine zoomRoutine;

    void Awake()
    {
        _camera = _cameraObj.GetComponent<Camera>();
    }

    public void Move(Vector2 target, float time = 2f)
    {
        if (_camera == null || target == null)
        {
            Debug.LogWarning("Camera hoặc Target chưa được gán!");
            return;
        }

        // Hủy coroutine cũ (nếu đang chạy)
        if (moveRoutine != null)
            StopCoroutine(moveRoutine);

        moveRoutine = StartCoroutine(MoveToTarget(target, time));
    }

    private IEnumerator MoveToTarget(Vector2 target, float time)
    {
        Vector3 startPos = _camera.transform.position;
        Quaternion startRot = _camera.transform.rotation;

        Vector3 endPos = target;
        endPos.z = _camera.transform.position.z;
        Quaternion endRot = transform.rotation;

        float elapsed = 0f;

        while (elapsed < time)
        {
            // ✅ DÙNG unscaledDeltaTime để vẫn chạy khi timeScale = 0
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / time);

            _camera.transform.position = Vector3.Lerp(startPos, endPos, t);
            _camera.transform.rotation = Quaternion.Slerp(startRot, endRot, t);

            yield return null;
        }

        _camera.transform.position = endPos;
        _camera.transform.rotation = endRot;
        moveRoutine = null;
    }

    // 🟢 HÀM ZOOM OUT / IN (cho Camera 2D)
    public void Zoom(float targetSize, float duration = 1f)
    {
        if (zoomRoutine != null)
            StopCoroutine(zoomRoutine);

        zoomRoutine = StartCoroutine(ZoomRoutine(targetSize, duration));
    }

    private IEnumerator ZoomRoutine(float targetSize, float duration)
    {
        float startSize = _camera.orthographicSize;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // ✅ Giống vậy
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            _camera.orthographicSize = Mathf.Lerp(startSize, targetSize, t);
            yield return null;
        }

        _camera.orthographicSize = targetSize;
        zoomRoutine = null;
    }
}
