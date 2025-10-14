using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToManager : MonoBehaviour
{
    public static MoveToManager Instance;

    [SerializeField] private GameObject moveToPrefab;
    [SerializeField] private Transform moveToParent;

    [SerializeField] private readonly List<MoveTo> activeMoveTos = new List<MoveTo>();
    [SerializeField] private readonly Queue<MoveTo> pool = new Queue<MoveTo>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (moveToParent == null)
            Debug.LogError($"{transform.name} [MoveToManager] Hãy Tạo một GameOBJEmty rồi kéo thả vào 'MoveToParent'");
    }

    /// <summary>
    /// Tạo một MoveTo mới hoặc lấy từ pool ra để tái sử dụng
    /// </summary>
    public MoveTo CreateMovePoint(List<GameObject> chosen, Vector2 pos, bool rally = false, bool add = false, bool active = true, float radius = -1)
    {
        MoveTo moveTo = GetMoveToFromPool();

        moveTo.transform.position = pos;
        moveTo.SetChosen(chosen, rally, add, active, radius);
        StartCoroutine(setActive(moveTo));

        activeMoveTos.Add(moveTo);
        return moveTo;
    }

    private IEnumerator setActive(MoveTo moveTo)
    {
        yield return new WaitForSeconds(0.1f);
        moveTo.gameObject.SetActive(true);
    }

    private MoveTo GetMoveToFromPool()
    {
        MoveTo moveTo;
        if (pool.Count > 0)
        {
            moveTo = pool.Dequeue();
        }
        else
        {
            GameObject obj = Instantiate(moveToPrefab, moveToParent);
            moveTo = obj.GetComponent<MoveTo>();
        }
        return moveTo;
    }

    /// <summary>
    /// Đưa MoveTo về pool sau khi bị disable
    /// </summary>
    public void ReturnToPool(MoveTo moveTo)
    {
        if (activeMoveTos.Contains(moveTo))
            activeMoveTos.Remove(moveTo);

        moveTo.gameObject.SetActive(false);
        pool.Enqueue(moveTo);
    }

    /// <summary>
    /// Xóa toàn bộ MoveTo đang hoạt động
    /// </summary>
    public void ClearAll()
    {
        foreach (var move in activeMoveTos)
        {
            move.gameObject.SetActive(false);
            pool.Enqueue(move);
        }
        activeMoveTos.Clear();
    }
}
