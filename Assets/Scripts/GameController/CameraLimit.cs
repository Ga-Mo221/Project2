using NaughtyAttributes;
using UnityEngine;

public class CameraLimit : MonoBehaviour
{
    [Header("Camera Limit (World Bounds)")]
    public bool useLimit = true;
    [ShowIf(nameof(useLimit))] [SerializeField] private Vector2 minBound = new Vector2(-50, -30);
    [ShowIf(nameof(useLimit))] [SerializeField] private Vector2 maxBound = new Vector2(50, 30);
    // --- Trả về vị trí world thực tế ---
    public Vector2 WorldMinBound => (Vector2)transform.position + minBound;
    public Vector2 WorldMaxBound => (Vector2)transform.position + maxBound;

    // --- Gizmos hiển thị vùng giới hạn ---
    private void OnDrawGizmosSelected()
    {
        if (!useLimit) return;

        Gizmos.color = Color.red;

        // Cộng thêm vị trí origin để vùng di chuyển theo object
        Vector3 center = new Vector3(
            (minBound.x + maxBound.x) / 2f + transform.position.x,
            (minBound.y + maxBound.y) / 2f + transform.position.y,
            0
        );

        Vector3 size = new Vector3(
            Mathf.Abs(maxBound.x - minBound.x),
            Mathf.Abs(maxBound.y - minBound.y),
            0
        );

        Gizmos.DrawWireCube(center, size);
    }

    // --- (Tùy chọn) Giới hạn vị trí camera ---
    public Vector3 ClampPosition(Vector3 targetPos)
    {
        if (!useLimit) return targetPos;

        float clampedX = Mathf.Clamp(targetPos.x, WorldMinBound.x, WorldMaxBound.x);
        float clampedY = Mathf.Clamp(targetPos.y, WorldMinBound.y, WorldMaxBound.y);

        return new Vector3(clampedX, clampedY, targetPos.z);
    }
}
