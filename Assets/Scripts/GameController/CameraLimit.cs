using NaughtyAttributes;
using UnityEngine;

public class CameraLimit : MonoBehaviour
{
    [Header("Camera Limit (World Bounds)")]
    public bool useLimit = true;
    [ShowIf(nameof(useLimit))] public Vector2 minBound = new Vector2(-50, -30);
    [ShowIf(nameof(useLimit))] public Vector2 maxBound = new Vector2(50, 30);
    // --- Gizmos để hiển thị vùng giới hạn ---
    private void OnDrawGizmosSelected()
    {
        if (!useLimit) return;

        Gizmos.color = Color.red;
        Vector3 center = new Vector3(
            (minBound.x + maxBound.x) / 2f,
            (minBound.y + maxBound.y) / 2f,
            0
        );
        Vector3 size = new Vector3(
            Mathf.Abs(maxBound.x - minBound.x),
            Mathf.Abs(maxBound.y - minBound.y),
            0
        );

        Gizmos.DrawWireCube(center, size);
    }
}
