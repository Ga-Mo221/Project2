using System.Collections;
using NaughtyAttributes;
using UnityEngine;

public class PatrolEnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyPatrol _patrol;

    [Header("Spawn Settings")]
    [SerializeField] private float minRadius = 10f;
    [SerializeField] private float maxRadius = 20f;
    [SerializeField] private int minEnemyCount = 3;
    [SerializeField] private int maxEnemyCount = 7;

    [Header("Spawn Rates (Must sum to 1.0)")]
    [Range(0f, 1f)] [SerializeField] private float chancefish = 0.15f;
    [Range(0f, 1f)] [SerializeField] private float chanceGnoll = 0.15f;
    [Range(0f, 1f)] [SerializeField] private float chanceLancer = 0.15f;
    [Range(0f, 1f)] [SerializeField] private float chanceOrc = 0.45f;
    [Range(0f, 1f)] [SerializeField] private float chanceTNT = 0.04f;
    [Range(0f, 1f)] [SerializeField] private float chanceShaman = 0.03f;
    [Range(0f, 1f)] [SerializeField] private float chanceMinotaur = 0.03f;

    [Foldout("Enemy Prefabs")]
    [SerializeField] private GameObject fishPrefab;
    [Foldout("Enemy Prefabs")]
    [SerializeField] private GameObject gnollPrefab;
    [Foldout("Enemy Prefabs")]
    [SerializeField] private GameObject lancerPrefab;
    [Foldout("Enemy Prefabs")]
    [SerializeField] private GameObject orcPrefab;
    [Foldout("Enemy Prefabs")]
    [SerializeField] private GameObject tntPrefab;
    [Foldout("Enemy Prefabs")]
    [SerializeField] private GameObject shamanPrefab;
    [Foldout("Enemy Prefabs")]
    [SerializeField] private GameObject minotaurPrefab;

    private float _spawnRadius;
    private float _innerRadius;
    private int _enemyCount;

    private void Start()
    {
        if (_patrol == null)
        {
            Debug.LogError("Patrol reference is missing!", this);
            return;
        }

        ValidateSpawnRates();
        
        _spawnRadius = Random.Range(minRadius, maxRadius);
        _enemyCount = Random.Range(minEnemyCount, maxEnemyCount);
        _innerRadius = _patrol.set(_spawnRadius, _enemyCount);

        StartCoroutine(SpawnEnemies());
    }

    private void ValidateSpawnRates()
    {
        float total = chancefish + chanceGnoll + chanceLancer + chanceOrc + 
                     chanceTNT + chanceShaman + chanceMinotaur;
        
        if (Mathf.Abs(total - 1f) > 0.01f)
        {
            Debug.LogWarning($"Spawn rates sum to {total:F2}, not 1.0! Normalizing...", this);
        }
    }

    private IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(0.2f);

        float totalChance = chancefish + chanceGnoll + chanceLancer + chanceOrc + 
                           chanceTNT + chanceShaman + chanceMinotaur;

        for (int i = 0; i < _enemyCount; i++)
        {
            Vector2 spawnPos = GetRandomPointInAnnulus(transform.position, _innerRadius, _spawnRadius);
            GameObject prefab = ChooseEnemyPrefab(Random.value * totalChance);

            if (prefab != null)
            {
                GameObject enemy = Instantiate(prefab, spawnPos, Quaternion.identity, transform);
                EnemyAI ai = enemy.GetComponentInChildren<EnemyAI>(true);
                if (ai == null)
                {
                    Debug.LogError($"Không tìm thấy EnemyAI trong prefab {enemy.name}", enemy);
                }
                else
                {
                    ai.setPatrol(_patrol);
                    _patrol.checkInEnemy(ai);
                    EnemyHouse.Instance._listEnemy.Add(ai);
                }
            }
        }
    }

    private Vector2 GetRandomPointInAnnulus(Vector2 center, float innerRadius, float outerRadius)
    {
        float radius = Random.Range(innerRadius, outerRadius);
        float angle = Random.Range(0f, Mathf.PI * 2f);
        return center + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
    }

    private GameObject ChooseEnemyPrefab(float randomValue)
    {
        if (randomValue < chancefish) return fishPrefab;
        randomValue -= chancefish;

        if (randomValue < chanceGnoll) return gnollPrefab;
        randomValue -= chanceGnoll;

        if (randomValue < chanceLancer) return lancerPrefab;
        randomValue -= chanceLancer;

        if (randomValue < chanceOrc) return orcPrefab;
        randomValue -= chanceOrc;

        if (randomValue < chanceTNT) return tntPrefab;
        randomValue -= chanceTNT;

        if (randomValue < chanceShaman) return shamanPrefab;

        return minotaurPrefab;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, minRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, maxRadius);
    }
}