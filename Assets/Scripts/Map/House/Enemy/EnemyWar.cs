using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWar : MonoBehaviour
{
    [SerializeField] private List<War> _enemyWar = new List<War>();

    private int _currentDay = 1;
    //private Coroutine _currentWave;


    #region Update
    void Update()
    {
        if (GameManager.Instance._night
            && GameManager.Instance._currentDay != _currentDay)
        {
            _currentDay = GameManager.Instance._currentDay;
            startWar();
            GameManager.Instance.UIonEnemyRespawn(true);
        }
    }
    #endregion


    #region Start War
    private void startWar()
    {
        foreach (War war in _enemyWar)
        {
            if (war._startDay <= _currentDay - 1 && war._endDay >= _currentDay - 1)
            {
                foreach (Wave wave in war._listWave)
                {
                    StartCoroutine(startWave(wave));
                }
            }
        }
    }
    #endregion


    #region Start Wave
    private IEnumerator startWave(Wave wave)
    {
        yield return new WaitForSeconds(wave._delay);
        foreach (var enemyPrefab in wave._listEnemy)
        {
            StartCoroutine(spawnEnemy(enemyPrefab));
        }
    }
    #endregion


    #region  Spawn Enemy
    private IEnumerator spawnEnemy(EnemyPrefab enemyPrefab)
    {
        yield return new WaitForSeconds(enemyPrefab._delay);
        Transform _spawnPoint = EnemyHouse.Instance.getWarPos();
        int _count = Random.Range((int)enemyPrefab._count.x, (int)enemyPrefab._count.y + 1);
        for (int i = 0; i < _count; i++)
        {
            bool _IsCreate = true;
            foreach (var enemyAi in EnemyHouse.Instance._listEnemyCreate)
            {
                if (enemyPrefab._enemy == enemyAi._type && enemyAi.getDie())
                {
                    enemyAi.respawn(randomPoint(_spawnPoint));
                    enemyAi.setTarget(Castle.Instance.gameObject);
                    enemyAi.gameObject.SetActive(true);
                    _IsCreate = false;
                    break;
                }
            }
            if (_IsCreate)
            {
                GameObject enemy = Instantiate(getPrefab(enemyPrefab._enemy), randomPoint(_spawnPoint), Quaternion.identity, _spawnPoint);
                var enemyAI = enemy.GetComponent<EnemyAI>();
                if (enemyAI != null)
                {
                    enemyAI.setIsCreate(true);
                    enemyAI.setTarget(Castle.Instance.gameObject);
                }
            }
        }
    }
    #endregion


    #region Random Point
    private Vector3 randomPoint(Transform pos, float _radius = 2f)
    {
        Vector2 randomCircle = Random.insideUnitCircle * _radius;
        // 2D top-down => Z không dùng
        Vector3 spawnPos = pos.position + new Vector3(randomCircle.x, randomCircle.y, 0f);
        return spawnPos;
    }
    #endregion


    private GameObject getPrefab(EnemyType type)
    {
        GameObject prefab = null;
        switch (type)
        {
            case EnemyType.Lancer:
                prefab = GameManager.Instance._enemy_LancerPrefab;
                break;
            case EnemyType.Orc:
                prefab = GameManager.Instance._enemy_OrcPrefab;
                break;
            case EnemyType.Gnoll:
                prefab = GameManager.Instance._enemy_GnollPrefab;
                break;
            case EnemyType.Fish:
                prefab = GameManager.Instance._enemy_FishPrefab;
                break;
            case EnemyType.Minotaur:
                prefab = GameManager.Instance._enemy_MinotaurPrefab;
                break;
            case EnemyType.Shaman:
                prefab = GameManager.Instance._enemy_ShamanPrefab;
                break;
            case EnemyType.TNT:
                prefab = GameManager.Instance._enemy_TNTRedPrefab;
                break;
        }
        return prefab;
    }
}