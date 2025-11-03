using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class SpawnerPatrol : MonoBehaviour
{
    [SerializeField] private PolygonCollider2D _col; // chỉ spawn trong phạm vi này

    [SerializeField] private bool castle = false;

    [Header("Setting")]
    [SerializeField] private int patrol_count = 1;
    [SerializeField] private float patrol_distance = 5f; // khoảng cách giữa các patrol
    
    [ShowIf(nameof(castle))]
    [SerializeField] private int min_TNT_Count = 4;
    [ShowIf(nameof(castle))]
    [SerializeField] private int max_TNT_Count = 8;
    [ShowIf(nameof(castle))]
    [SerializeField] private int min_Boss_Count = 2;
    [ShowIf(nameof(castle))]
    [SerializeField] private int max_Boss_Count = 3;
    [ShowIf(nameof(castle))]
    [Range(2f, 5f)] [SerializeField] private float enemy_distance = 3f;

    [Foldout("Prefab")]
    [SerializeField] private GameObject EnemyPatrol_Prefab;
    [ShowIf(nameof(castle))]
    [Foldout("Prefab")]
    [SerializeField] private GameObject TNT_Prefab;
    [ShowIf(nameof(castle))]
    [Foldout("Prefab")]
    [SerializeField] private List<GameObject> Boss_Prefab;

    void Start()
    {
        if (_col == null)
        {
            Debug.LogError("PolygonCollider Null", this);
            return;
        }

        StartCoroutine(StartSpawn());
    }

    private IEnumerator StartSpawn()
    {
        float delay = Random.Range(0.4f, 0.8f);
        yield return new WaitForSeconds(delay);
        
        if (castle)
        {
            SpawnEnemy();
            yield return new WaitForEndOfFrame();
        }
        
        SpawnPatrol();
    }

    /// <summary>
    /// Spawn TNT và Boss GẦN Castle (bên trong polygon)
    /// </summary>
    private void SpawnEnemy()
    {
        if (Castle.Instance == null)
        {
            Debug.LogWarning("Castle Instance not found!");
            return;
        }

        Vector2 castlePos = Castle.Instance.transform.position;
        List<Vector2> spawnedPositions = new List<Vector2>();

        // Spawn TNT
        int tntCount = Random.Range(min_TNT_Count, max_TNT_Count + 1);
        for (int i = 0; i < tntCount; i++)
        {
            Vector2 spawnPos = GetSpawnPositionNearTarget(castlePos, spawnedPositions);
            if (spawnPos != Vector2.zero)
            {
                Instantiate(TNT_Prefab, spawnPos, Quaternion.identity, _col.transform);
                spawnedPositions.Add(spawnPos);
            }
        }

        // Spawn Boss
        if (Boss_Prefab != null && Boss_Prefab.Count > 0)
        {
            int bossCount = Random.Range(min_Boss_Count, max_Boss_Count + 1);
            for (int i = 0; i < bossCount; i++)
            {
                Vector2 spawnPos = GetSpawnPositionNearTarget(castlePos, spawnedPositions);
                if (spawnPos != Vector2.zero)
                {
                    GameObject bossPrefab = Boss_Prefab[Random.Range(0, Boss_Prefab.Count)];
                    Instantiate(bossPrefab, spawnPos, Quaternion.identity, _col.transform);
                    spawnedPositions.Add(spawnPos);
                }
            }
        }
    }

    /// <summary>
    /// Spawn patrol Ở RÌA NGOÀI của polygon và HƯỚNG về Castle
    /// </summary>
    private void SpawnPatrol()
    {
        if (Castle.Instance == null)
        {
            Debug.LogWarning("Castle Instance not found!");
            return;
        }

        if (EnemyPatrol_Prefab == null)
        {
            Debug.LogError("EnemyPatrol_Prefab is null!");
            return;
        }

        Vector2 castlePos = Castle.Instance.transform.position;
        List<Vector2> spawnedPositions = new List<Vector2>();

        for (int i = 0; i < patrol_count; i++)
        {
            Vector2 spawnPos = GetSpawnPositionAtEdge(castlePos, spawnedPositions);
            if (spawnPos != Vector2.zero)
            {
                // Spawn và xoay hướng về Castle
                Instantiate(EnemyPatrol_Prefab, spawnPos, Quaternion.identity, _col.transform);
                spawnedPositions.Add(spawnPos);
            }
        }
    }


    /// <summary>
    /// Lấy vị trí spawn GẦN Castle (ưu tiên vị trí gần target)
    /// </summary>
    private Vector2 GetSpawnPositionNearTarget(Vector2 targetPos, List<Vector2> occupiedPositions)
    {
        int maxAttempts = 50;
        Vector2 bestPosition = Vector2.zero;
        float closestDistance = float.MaxValue;

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            Vector2 randomPos = GetRandomPointInPolygon();
            float distanceToTarget = Vector2.Distance(randomPos, targetPos);

            // Kiểm tra không trùng với các vị trí đã spawn
            bool tooClose = false;
            foreach (Vector2 occupied in occupiedPositions)
            {
                if (Vector2.Distance(randomPos, occupied) < enemy_distance)
                {
                    tooClose = true;
                    break;
                }
            }

            // Tìm vị trí GẦN Castle nhất
            if (!tooClose && distanceToTarget < closestDistance)
            {
                closestDistance = distanceToTarget;
                bestPosition = randomPos;
            }
        }

        if (bestPosition == Vector2.zero)
        {
            Debug.LogWarning("Could not find valid spawn position near target");
        }

        return bestPosition;
    }

    /// <summary>
    /// Lấy vị trí spawn Ở RÌA NGOÀI (Gần playercastle nhất)
    /// </summary>
    private Vector2 GetSpawnPositionAtEdge(Vector2 targetPos, List<Vector2> occupiedPositions)
    {
        int maxAttempts = 100;
        Vector2 bestPosition = Vector2.zero;
        float nearestDistance = float.MaxValue;

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            Vector2 randomPos = GetRandomPointInPolygon();

            // Kiểm tra không trùng với các vị trí đã spawn
            bool tooClose = false;
            foreach (Vector2 occupied in occupiedPositions)
            {
                if (Vector2.Distance(randomPos, occupied) < patrol_distance)
                {
                    tooClose = true;
                    break;
                }
            }

            if (tooClose)
                continue;

            // Tính khoảng cách tới PlayerCastle
            float distanceToTarget = Vector2.Distance(randomPos, targetPos);

            // Ưu tiên vị trí gần PlayerCastle nhất
            if (distanceToTarget < nearestDistance)
            {
                nearestDistance = distanceToTarget;
                bestPosition = randomPos;
            }
        }

        if (bestPosition == Vector2.zero)
        {
            Debug.LogWarning("Could not find valid spawn position at edge");
        }

        return bestPosition;
    }

    /// <summary>
    /// Lấy vị trí random trong PolygonCollider2D
    /// </summary>
    private Vector2 GetRandomPointInPolygon()
    {
        Bounds bounds = _col.bounds;
        int maxAttempts = 50;

        for (int i = 0; i < maxAttempts; i++)
        {
            Vector2 randomPoint = new Vector2(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y)
            );

            // Kiểm tra điểm có nằm trong polygon không
            if (_col.OverlapPoint(randomPoint))
            {
                return randomPoint;
            }
        }

        // Fallback: trả về tâm của bounds
        return bounds.center;
    }

    // Vẽ gizmos để debug
    private void OnDrawGizmosSelected()
    {
        if (_col == null) return;

        // Vẽ polygon
        Vector2[] points = _col.points;
        Gizmos.color = castle ? Color.red : Color.yellow;
        for (int i = 0; i < points.Length; i++)
        {
            Vector2 worldPoint = transform.TransformPoint(points[i]);
            Vector2 nextWorldPoint = transform.TransformPoint(points[(i + 1) % points.Length]);
            Gizmos.DrawLine(worldPoint, nextWorldPoint);
        }

        // Vẽ Castle position nếu có
        if (Castle.Instance != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(Castle.Instance.transform.position, 1f);
            
            // Vẽ đường từ center polygon đến Castle
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, Castle.Instance.transform.position);
        }

        // Vẽ khoảng cách spawn
        Gizmos.color = castle ? Color.magenta : Color.green;
        float dist = castle ? enemy_distance : patrol_distance;
        Gizmos.DrawWireSphere(transform.position, dist);
    }
}