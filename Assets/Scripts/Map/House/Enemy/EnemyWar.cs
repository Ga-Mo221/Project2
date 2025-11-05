using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWar : MonoBehaviour
{
    [Header("=== WAR SETTINGS ===")]
    [Tooltip("War chính – spawn ở vị trí gần Castle nhất")]
    [SerializeField] private List<War> _mainWar = new List<War>();

    [Tooltip("War phụ – spawn ở các vị trí xa hơn")]
    [SerializeField] private List<War> _sideWar = new List<War>();

    [Tooltip("War đặc biệt – spawn tùy theo sự kiện")]
    [SerializeField] private List<War> _specialWar = new List<War>();

    [Header("Spawn Control")]
    [SerializeField] private float _enemySpawnRadius = 2f;
    [SerializeField] private bool _logSpawnActivity = true;

    private int _currentDay = 1;
    private List<Coroutine> _activeWaves = new List<Coroutine>();
    private bool _isWarActive = false;

    #region Unity Loop
    void Update()
    {
        var gm = GameManager.Instance;
        if (gm == null) return;

        // Khi sang ngày mới và ban đêm bắt đầu
        if (gm._night && gm._currentDay != _currentDay && !_isWarActive)
        {
            _currentDay = gm._currentDay;
            StartCoroutine(StartWarSequence());
            gm.UIonEnemyRespawn(true);
        }
    }
    #endregion

    #region WAR SEQUENCE
    private IEnumerator StartWarSequence()
    {
        _isWarActive = true;
        yield return new WaitForSeconds(0.5f);

        // Lấy danh sách spawn points một lần duy nhất
        List<Transform> spawnPoints = EnemyHouse.Instance?.getWarPos();
        if (spawnPoints == null || spawnPoints.Count == 0)
        {
            Debug.LogWarning("[EnemyWar] No valid spawn points!");
            _isWarActive = false;
            yield break;
        }

        // Xử lý từng loại war theo thứ tự ưu tiên
        yield return StartCoroutine(ProcessWarList(_mainWar, spawnPoints, 0)); // Index 0 = gần Castle nhất
        yield return StartCoroutine(ProcessWarList(_sideWar, spawnPoints, 1));  // Index 1+ = xa hơn
        yield return StartCoroutine(ProcessWarList(_specialWar, spawnPoints, 0)); // Special cũng ưu tiên gần

        _isWarActive = false;
    }

    private IEnumerator ProcessWarList(List<War> wars, List<Transform> spawnPoints, int baseSpawnIndex)
    {
        foreach (War war in wars)
        {
            if (war == null || war._listWave == null) continue;

            // Kiểm tra ngày hợp lệ
            int dayIndex = _currentDay - 1;
            if (dayIndex < war._startDay || dayIndex > war._endDay)
                continue;

            // Lấy spawn index an toàn
            int spawnIndex = war._spawnIndex >= 0 ? war._spawnIndex : baseSpawnIndex;
            spawnIndex = Mathf.Clamp(spawnIndex, 0, spawnPoints.Count - 1);
            Transform spawnPoint = spawnPoints[spawnIndex];

            if (_logSpawnActivity)
                Debug.Log($"[EnemyWar] Day {_currentDay}: Starting war at {spawnPoint.name}");

            // Spawn waves tuần tự (không đồng thời)
            foreach (Wave wave in war._listWave)
            {
                yield return StartCoroutine(StartWave(wave, spawnPoint));
            }
        }
    }
    #endregion

    #region WAVE HANDLING
    private IEnumerator StartWave(Wave wave, Transform spawnPoint)
    {
        if (wave == null || wave._listEnemy == null)
            yield break;

        yield return new WaitForSeconds(wave._delay);

        if (spawnPoint == null)
        {
            Debug.LogWarning("[EnemyWar] Spawn point destroyed during wave!");
            yield break;
        }

        // Spawn tất cả enemy trong wave này
        foreach (var enemyPrefab in wave._listEnemy)
        {
            if (enemyPrefab == null) continue;
            yield return StartCoroutine(SpawnEnemyBatch(enemyPrefab, spawnPoint));
        }
    }
    #endregion

    #region SPAWN LOGIC
    private IEnumerator SpawnEnemyBatch(EnemyPrefab enemyPrefab, Transform spawnPoint)
    {
        yield return new WaitForSeconds(enemyPrefab._delay);

        if (EnemyHouse.Instance == null || Castle.Instance == null)
            yield break;

        int spawnCount = Random.Range((int)enemyPrefab._count.x, (int)enemyPrefab._count.y + 1);

        for (int i = 0; i < spawnCount; i++)
        {
            SpawnSingleEnemy(enemyPrefab._enemy, spawnPoint);
            yield return new WaitForSeconds(0.1f); // Spacing giữa các enemy
        }
    }

    private void SpawnSingleEnemy(EnemyType type, Transform spawnPoint)
    {
        // Tìm enemy đã chết để tái sử dụng
        EnemyAI reusableEnemy = FindReusableEnemy(type);
        Vector3 spawnPos = RandomPointAround(spawnPoint);
        if (reusableEnemy != null)
        {
            // Respawn enemy cũ
            reusableEnemy.respawn(spawnPos);
            Debug.Log($"<color=#FF4444>[War]</color> [{reusableEnemy.name}] for [{spawnPos}] In [{spawnPoint.name}]", spawnPoint);
            reusableEnemy.setTarget(Castle.Instance.gameObject);
            reusableEnemy.gameObject.SetActive(true);

            if (_logSpawnActivity)
                Debug.Log($"[EnemyWar] Reused {type} at {spawnPoint.name}");
        }
        else
        {
            // Tạo enemy mới
            GameObject prefab = GetPrefab(type);
            if (prefab == null)
            {
                Debug.LogWarning($"[EnemyWar] Prefab not found for {type}");
                return;
            }

            GameObject enemy = Instantiate(prefab, spawnPos, Quaternion.identity, spawnPoint);
            Debug.Log($"<color=#FF4444>[War]</color> [{enemy.name}] for [{spawnPos}] In [{spawnPoint.name}]", spawnPoint);
            var enemyAI = enemy.GetComponent<EnemyAI>();
            if (enemyAI != null)
            {
                enemyAI.setIsCreate(true);
                enemyAI.setTarget(Castle.Instance.gameObject);
                EnemyHouse.Instance._listEnemyCreate.Add(enemyAI);
            }

            if (_logSpawnActivity)
                Debug.Log($"[EnemyWar] Spawned NEW {type} at {spawnPoint.name}");
        }
    }

    private EnemyAI FindReusableEnemy(EnemyType type)
    {
        if (EnemyHouse.Instance == null) return null;

        foreach (var enemyAI in EnemyHouse.Instance._listEnemyCreate)
        {
            if (enemyAI != null && enemyAI._type == type && enemyAI.getDie())
                return enemyAI;
        }
        return null;
    }

    private Vector3 RandomPointAround(Transform origin)
    {
        Vector2 randomCircle = Random.insideUnitCircle * _enemySpawnRadius;
        return origin.position + new Vector3(randomCircle.x, randomCircle.y, 0f);
    }
    #endregion

    #region PREFAB HANDLER
    private GameObject GetPrefab(EnemyType type)
    {
        var gm = GameManager.Instance;
        if (gm == null) return null;

        switch (type)
        {
            case EnemyType.Lancer: return gm._enemy_LancerPrefab;
            case EnemyType.Orc: return gm._enemy_OrcPrefab;
            case EnemyType.Gnoll: return gm._enemy_GnollPrefab;
            case EnemyType.Fish: return gm._enemy_FishPrefab;
            case EnemyType.Minotaur: return gm._enemy_MinotaurPrefab;
            case EnemyType.Shaman: return gm._enemy_ShamanPrefab;
            case EnemyType.TNT: return gm._enemy_TNTRedPrefab;
            default: 
                Debug.LogWarning($"[EnemyWar] Unknown enemy type: {type}");
                return null;
        }
    }
    #endregion

    #region CLEANUP
    void OnDisable()
    {
        StopAllCoroutines();
        _activeWaves.Clear();
        _isWarActive = false;
    }
    #endregion
}