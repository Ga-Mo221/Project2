using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dynamic Formation Slot Manager - THREAD-SAFE VERSION
/// - Sửa lỗi 2 units bị assign cùng 1 slot
/// - Giữ slot reserve khi reassign
/// - Thêm debug logs chi tiết
/// </summary>
public class FormationSlotManager : MonoBehaviour
{
    [Header("Formation Settings")]
    [SerializeField] private Transform _centerTarget;
    [SerializeField] private float _slotSpacing = 0.5f;
    [SerializeField] private bool _drawGizmos = true;
    [SerializeField] private int _maxSlotsPerRing = 24;
    [SerializeField] private float _ringRadiusTolerance = 0.1f;

    [Header("Debug")]
    [SerializeField] private bool _enableDebugLogs = true;

    // Dữ liệu nội bộ
    [SerializeField] private List<Ring> _rings = new();
    [SerializeField] private List<UnitSlotData> _unitSlotList = new();

    /// <summary>
    /// Ring chứa thông tin về 1 vòng tròn formation
    /// </summary>
    [System.Serializable]
    private class Ring
    {
        public float radius;
        public List<Vector3> slots;
        public List<bool> occupied;
        public List<string> unitNames; // ⭐ NEW: Track unit names for debug

        public Ring(float radius)
        {
            this.radius = radius;
            this.slots = new List<Vector3>();
            this.occupied = new List<bool>();
            this.unitNames = new List<string>();
        }

        public int GetNextFreeIndex()
        {
            for (int i = 0; i < occupied.Count; i++)
            {
                if (!occupied[i]) return i;
            }
            return -1;
        }

        public int OccupiedCount()
        {
            int count = 0;
            foreach (bool b in occupied)
                if (b) count++;
            return count;
        }
    }

    /// <summary>
    /// Struct lưu thông tin unit assignment
    /// </summary>
    [System.Serializable]
    private class UnitSlotData
    {
        public GameObject unit;
        public int ringIndex;
        public int slotIndex;
        public string unitName; // For debug
        public float assignTime; // ⭐ NEW: Track when assigned

        public UnitSlotData(GameObject unit, int ringIndex, int slotIndex)
        {
            this.unit = unit;
            this.ringIndex = ringIndex;
            this.slotIndex = slotIndex;
            this.unitName = unit != null ? unit.name : "NULL";
            this.assignTime = Time.time;
        }
    }

    private void Awake()
    {
        if (_rings == null) _rings = new List<Ring>();
        if (_unitSlotList == null) _unitSlotList = new List<UnitSlotData>();
        
        CleanUpNullUnits();
    }

    /// <summary>
    /// ⭐ MAIN FIX: Gán slot cho unit - KHÔNG release slot cũ tự động
    /// </summary>
    public Vector3 AssignSlot(GameObject unit, float range)
    {
        if (unit == null)
        {
            LogError("Unit is null!");
            return Vector3.zero;
        }

        if (_centerTarget == null)
        {
            LogWarning("Center Target chưa được set!");
            return unit.transform.position;
        }

        if (range <= 0)
        {
            LogWarning($"Invalid range: {range}. Using default 1.0f");
            range = 1f;
        }

        // ⭐ CHECK: Xem unit đã có slot chưa
        UnitSlotData existingData = FindUnitData(unit);
        
        if (existingData != null)
        {
            // ⭐ Nếu cùng range -> giữ nguyên slot cũ
            if (existingData.ringIndex >= 0 && existingData.ringIndex < _rings.Count)
            {
                Ring ring = _rings[existingData.ringIndex];
                
                // Check nếu slot vẫn hợp lệ và cùng radius
                if (Mathf.Abs(ring.radius - range) < _ringRadiusTolerance)
                {
                    if (existingData.slotIndex >= 0 && existingData.slotIndex < ring.slots.Count)
                    {
                        Log($"✓ {unit.name} KEEPING slot: ring {existingData.ringIndex}, slot {existingData.slotIndex}");
                        return ring.slots[existingData.slotIndex];
                    }
                }
                else
                {
                    // Range khác -> release slot cũ và tìm slot mới
                    Log($"⚠ {unit.name} CHANGING range from {ring.radius} to {range}");
                    ReleaseSlot(unit);
                }
            }
            else
            {
                // Data không hợp lệ -> xóa
                LogWarning($"Invalid slot data for {unit.name}, removing...");
                _unitSlotList.Remove(existingData);
            }
        }

        // ⭐ ASSIGN MỚI: Tìm hoặc tạo ring
        Ring targetRing = FindOrCreateRing(range);
        int ringIndex = _rings.IndexOf(targetRing);

        // Tìm slot trống
        int freeIndex = targetRing.GetNextFreeIndex();
        
        if (freeIndex >= 0)
        {
            // Có slot trống -> assign
            AssignUnitToSlot(unit, targetRing, ringIndex, freeIndex);
            Log($"✓ {unit.name} assigned to EXISTING slot: ring {ringIndex}, slot {freeIndex}");
            return targetRing.slots[freeIndex];
        }

        // Không có slot trống -> tạo slot mới
        if (targetRing.slots.Count < _maxSlotsPerRing)
        {
            Vector3 newSlot = CreateNewSlotOnRing(targetRing);
            int newIndex = targetRing.slots.Count;

            targetRing.slots.Add(newSlot);
            targetRing.occupied.Add(true);
            targetRing.unitNames.Add(unit.name);

            _unitSlotList.Add(new UnitSlotData(unit, ringIndex, newIndex));
            
            Log($"✓ {unit.name} assigned to NEW slot: ring {ringIndex}, slot {newIndex} (total slots: {targetRing.slots.Count})");
            return newSlot;
        }

        // Ring đã đầy
        LogWarning($"Ring {range} FULL ({targetRing.slots.Count} slots)! Using fallback.");
        return GetFallbackPosition(range);
    }

    /// <summary>
    /// ⭐ Helper: Assign unit vào slot cụ thể
    /// </summary>
    private void AssignUnitToSlot(GameObject unit, Ring ring, int ringIndex, int slotIndex)
    {
        // Double check slot chưa bị chiếm
        if (ring.occupied[slotIndex])
        {
            LogError($"⚠⚠⚠ CONFLICT: Slot {slotIndex} in ring {ringIndex} already occupied by {ring.unitNames[slotIndex]}!");
            
            // Force assign anyway nhưng log warning
            ring.unitNames[slotIndex] = $"{ring.unitNames[slotIndex]} + {unit.name} (CONFLICT!)";
        }
        else
        {
            ring.occupied[slotIndex] = true;
            
            // Đảm bảo unitNames có đủ phần tử
            while (ring.unitNames.Count <= slotIndex)
                ring.unitNames.Add("");
            
            ring.unitNames[slotIndex] = unit.name;
        }

        _unitSlotList.Add(new UnitSlotData(unit, ringIndex, slotIndex));
    }

    /// <summary>
    /// Giải phóng slot khi unit rời đi
    /// </summary>
    public void ReleaseSlot(GameObject unit)
    {
        if (unit == null) return;

        UnitSlotData data = FindUnitData(unit);
        if (data != null)
        {
            if (data.ringIndex >= 0 && data.ringIndex < _rings.Count)
            {
                Ring ring = _rings[data.ringIndex];
                if (data.slotIndex >= 0 && data.slotIndex < ring.occupied.Count)
                {
                    ring.occupied[data.slotIndex] = false;
                    
                    if (data.slotIndex < ring.unitNames.Count)
                        ring.unitNames[data.slotIndex] = "";
                    
                    Log($"✗ Released slot for {unit.name}: ring {data.ringIndex}, slot {data.slotIndex}");
                }
            }
            
            _unitSlotList.Remove(data);
        }
    }

    /// <summary>
    /// ⭐ NEW: Release slot chỉ khi thay đổi target
    /// </summary>
    public void ReleaseSlotIfDifferentTarget(GameObject unit, GameObject newTarget)
    {
        if (unit == null) return;

        UnitSlotData data = FindUnitData(unit);
        if (data != null)
        {
            // Chỉ release nếu target khác
            var currentFormation = newTarget?.GetComponent<FormationSlotManager>();
            if (currentFormation != this)
            {
                ReleaseSlot(unit);
                Log($"Released {unit.name} (different target)");
            }
        }
    }

    public void ClearSlots()
    {
        _rings.Clear();
        _unitSlotList.Clear();
        Log("All slots cleared");
    }

    public int GetAssignedUnitCount()
    {
        CleanUpNullUnits();
        return _unitSlotList.Count;
    }

    public int GetRingOccupiedCount(float radius)
    {
        Ring ring = FindRingByRadius(radius);
        return ring?.OccupiedCount() ?? 0;
    }

    // ==================== PRIVATE HELPERS ====================

    private void CleanUpNullUnits()
    {
        _unitSlotList.RemoveAll(u => u.unit == null);
    }

    private UnitSlotData FindUnitData(GameObject unit)
    {
        return _unitSlotList.Find(u => u.unit == unit);
    }

    private Ring FindRingByRadius(float radius)
    {
        foreach (var ring in _rings)
        {
            if (Mathf.Abs(ring.radius - radius) < _ringRadiusTolerance)
                return ring;
        }
        return null;
    }

    private Ring FindOrCreateRing(float radius)
    {
        Ring ring = FindRingByRadius(radius);
        if (ring != null) return ring;

        ring = new Ring(radius);
        
        Vector3 firstSlot = _centerTarget.position + new Vector3(radius, 0, 0);
        ring.slots.Add(firstSlot);
        ring.occupied.Add(false);
        ring.unitNames.Add("");

        _rings.Add(ring);
        Log($"Created new ring at radius {radius}");
        return ring;
    }

    private Vector3 CreateNewSlotOnRing(Ring ring)
    {
        if (_centerTarget == null) return Vector3.zero;

        int slotCount = ring.slots.Count;
        float angleStep = 360f / _maxSlotsPerRing;
        float currentAngle = angleStep * slotCount;

        float angleRad = currentAngle * Mathf.Deg2Rad;
        float x = Mathf.Cos(angleRad) * ring.radius;
        float y = Mathf.Sin(angleRad) * ring.radius;
        
        return _centerTarget.position + new Vector3(x, y, 0);
    }

    private Vector3 GetFallbackPosition(float range)
    {
        if (_centerTarget == null) return Vector3.zero;

        float randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float x = Mathf.Cos(randomAngle) * (range + _slotSpacing);
        float y = Mathf.Sin(randomAngle) * (range + _slotSpacing);
        
        return _centerTarget.position + new Vector3(x, y, 0);
    }

    // ==================== LOGGING ====================

    private void Log(string msg)
    {
        if (_enableDebugLogs)
            Debug.Log($"[Formation] {msg}");
    }

    private void LogWarning(string msg)
    {
        if (_enableDebugLogs)
            Debug.LogWarning($"[Formation] {msg}");
    }

    private void LogError(string msg)
    {
        Debug.LogError($"[Formation] {msg}");
    }

    // ==================== GIZMOS ====================

    private void OnDrawGizmos()
    {
        if (!_drawGizmos || _centerTarget == null) return;

        for (int r = 0; r < _rings.Count; r++)
        {
            Ring ring = _rings[r];
            
            // Vẽ ring
            Gizmos.color = new Color(1f, 0.8f, 0.2f, 0.3f);
            DrawCircle2D(_centerTarget.position, ring.radius, 64);

            // Vẽ slots
            for (int i = 0; i < ring.slots.Count; i++)
            {
                bool isOccupied = (i < ring.occupied.Count && ring.occupied[i]);
                
                Gizmos.color = isOccupied 
                    ? new Color(1f, 0.2f, 0.2f, 0.9f)  // Đỏ - Occupied
                    : new Color(0.2f, 1f, 0.2f, 0.6f); // Xanh - Free
                
                Gizmos.DrawSphere(ring.slots[i], 0.15f);

                #if UNITY_EDITOR
                // Hiển thị tên unit
                if (isOccupied && i < ring.unitNames.Count && !string.IsNullOrEmpty(ring.unitNames[i]))
                {
                    UnityEditor.Handles.Label(ring.slots[i] + Vector3.up * 0.3f, ring.unitNames[i]);
                }
                #endif
            }
        }

        // Vẽ center
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(_centerTarget.position, 0.3f);

        #if UNITY_EDITOR
        UnityEditor.Handles.Label(_centerTarget.position + Vector3.up * 2, 
            $"Rings: {_rings.Count}\nUnits: {GetAssignedUnitCount()}");
        #endif
    }

    private void DrawCircle2D(Vector3 center, float radius, int segments)
    {
        float angleStep = 360f / segments;
        Vector3 prevPoint = center + new Vector3(radius, 0, 0);

        for (int i = 1; i <= segments; i++)
        {
            float angle = angleStep * i * Mathf.Deg2Rad;
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;
            Vector3 newPoint = center + new Vector3(x, y, 0);
            
            Gizmos.DrawLine(prevPoint, newPoint);
            prevPoint = newPoint;
        }
    }

    // ==================== PUBLIC API ====================

    public void SetCenter(Transform target) => _centerTarget = target;
    public Transform GetCenter() => _centerTarget;
    public void SetMaxSlotsPerRing(int count) => _maxSlotsPerRing = Mathf.Max(4, count);
    public void SetSlotSpacing(float spacing) => _slotSpacing = Mathf.Max(0.1f, spacing);
    public void SetRadiusTolerance(float tolerance) => _ringRadiusTolerance = Mathf.Max(0.01f, tolerance);

    /// <summary>
    /// ⭐ NEW: Lấy vị trí slot hiện tại của unit (không assign mới)
    /// </summary>
    public Vector3 GetSlotForUnit(GameObject unit)
    {
        if (unit == null) return Vector3.zero;

        UnitSlotData data = FindUnitData(unit);
        if (data != null)
        {
            if (data.ringIndex >= 0 && data.ringIndex < _rings.Count)
            {
                Ring ring = _rings[data.ringIndex];
                if (data.slotIndex >= 0 && data.slotIndex < ring.slots.Count)
                {
                    return ring.slots[data.slotIndex];
                }
            }
        }
        
        return Vector3.zero;
    }

    /// <summary>
    /// ⭐ NEW: Check xem unit đã có slot chưa
    /// </summary>
    public bool HasSlot(GameObject unit)
    {
        return FindUnitData(unit) != null;
    }

    public void LogFormationState()
    {
        Debug.Log($"=== FORMATION STATE ({gameObject.name}) ===");
        Debug.Log($"Rings: {_rings.Count}, Assigned Units: {GetAssignedUnitCount()}");
        
        for (int i = 0; i < _rings.Count; i++)
        {
            Ring r = _rings[i];
            Debug.Log($"  Ring {i}: radius={r.radius:F1}, slots={r.slots.Count}, occupied={r.OccupiedCount()}");
            
            for (int j = 0; j < r.slots.Count; j++)
            {
                if (j < r.occupied.Count && r.occupied[j] && j < r.unitNames.Count)
                {
                    Debug.Log($"    Slot {j}: {r.unitNames[j]}");
                }
            }
        }
    }
}