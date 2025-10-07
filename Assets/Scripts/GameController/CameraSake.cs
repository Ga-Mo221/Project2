using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    [Header("Shake Settings")]
    [SerializeField] private float shakeDuration = 0.3f;   // Thời gian rung
    [SerializeField] private float shakeMagnitude = 0.15f; // Độ mạnh rung

    private Vector3 originalPos;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    /// <summary>
    /// Gọi để shake camera
    /// </summary>
    public void ShakeCamera(float duration = -1f, float magnitude = -1f)
    {
        originalPos = transform.localPosition;
        if (duration <= 0) duration = shakeDuration;
        if (magnitude <= 0) magnitude = shakeMagnitude;

        StopAllCoroutines(); // Dừng các shake trước đó nếu có
        StartCoroutine(ShakeRoutine(duration, magnitude));
    }

    private IEnumerator ShakeRoutine(float duration, float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // Giảm dần magnitude theo thời gian
            float damper = 1f - (elapsed / duration);

            float x = Random.Range(-1f, 1f) * magnitude * damper;
            float y = Random.Range(-1f, 1f) * magnitude * damper;

            transform.localPosition = originalPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Reset về vị trí gốc
        //transform.localPosition = originalPos;
    }
}