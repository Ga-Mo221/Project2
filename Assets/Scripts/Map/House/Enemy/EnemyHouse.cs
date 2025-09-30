using System.Collections.Generic;
using UnityEngine;

public class EnemyHouse : MonoBehaviour
{
    public static EnemyHouse Instance { get; private set; }

    public List<EnemyPatrol> _listPatrol;
    public List<EnemyAI> _listEnemy;
    public List<EnemyAI> _listEnemyCreate;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }


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

}
