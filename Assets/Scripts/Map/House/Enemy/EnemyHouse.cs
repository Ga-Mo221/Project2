using System.Collections.Generic;
using UnityEngine;

public class EnemyHouse : MonoBehaviour
{
    public static EnemyHouse Instance { get; private set; }

    private int _currentSTRTime = 4;
    private int _currentDay = 1;

    public List<EnemyPatrol> _listPatrol;
    public List<EnemyAI> _listEnemy;
    public List<EnemyAI> _listEnemyCreate;
    public List<Transform> _listSpawnPoint;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    #region Update
    void Update()
    {
        if (GameManager.Instance._timeRTS == _currentSTRTime
            && GameManager.Instance._currentDay != _currentDay)
        {
            _currentDay = GameManager.Instance._currentDay;
            respawnEnemy();
            GameManager.Instance.UIonEnemyRespawn(false);
        }
    }
    #endregion


    #region Get Target Patrol
    public EnemyPatrol getTargetPatrol(EnemyAI enemy)
    {
        if (_listPatrol == null || _listPatrol.Count == 0)
            return null;

        int roll = Random.Range(0, 100);

        // 70% ưu tiên patrol đầu tiên
        if (roll < 70)
        {
            if (_listPatrol[0].checkInEnemy(enemy))
            {
                return _listPatrol[0];
            }
        }

        // Nếu không chọn được patrol đầu tiên hoặc roll >= 70
        if (_listPatrol.Count > 1)
        {
            // Lấy danh sách patrol còn lại
            int index = Random.Range(1, _listPatrol.Count);

            // Nếu patrol random được full thì thử tìm patrol khác còn chỗ
            for (int i = 0; i < _listPatrol.Count; i++)
            {
                EnemyPatrol patrol = _listPatrol[(index + i) % _listPatrol.Count];
                if (patrol.checkInEnemy(enemy))
                {
                    return patrol;
                }
            }
        }

        // Nếu tất cả patrol đều full thì trả về null
        return null;
    }
    #endregion


    #region Respawn Enemy
    private void respawnEnemy()
    {
        foreach (var enemy in _listEnemy)
        {
            if (enemy.getDie())
            {
                enemy.respawn(getMinDirection(enemy));
                enemy.gameObject.SetActive(true);
            }
        }
    }
    #endregion


    #region Get Pos Spawn Enemy
    private Vector3 getMinDirection(EnemyAI enemy)
    {
        var _patrol = enemy.getPatrol();
        Vector3 pos = transform.position;
        float minDist = Mathf.Infinity;
        foreach (var point in _listSpawnPoint)
        {
            float dist = Vector3.Distance(point.position, _patrol.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                pos = point.position;
            }
        }
        return pos;
    }
    #endregion


    #region get War Pos
    public Transform getWarPos()
    {
        Vector3 castlePos = Castle.Instance.transform.position;
        Transform pos = transform;
        float minDist = Mathf.Infinity;
        foreach (var point in _listSpawnPoint)
        {
            var _script = point.GetComponent<EnemyHuoseController>();
            if (!_script.getDie())
            {
                float dist = Vector3.Distance(point.position, castlePos);
                if (dist < minDist)
                {
                    minDist = dist;
                    pos = point;
                }
            }
        }
        return pos;
    }
    #endregion
}
