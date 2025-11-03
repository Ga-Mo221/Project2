using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private float _radius = 10f;
    [SerializeField] private int _maxEnemy = 5;
    [SerializeField] private bool _isFire = false;
    [ShowIf(nameof(_isFire))]
    [SerializeField] private GameObject _fire;
    [SerializeField] private List<EnemyAI> _listEnemy;

    private CircleCollider2D _col;

    void Awake()
    {
        _col = GetComponent<CircleCollider2D>();
    }

    void Start()
    {
        if (_isFire)
            _fire.SetActive(false);

        EnemyHouse.Instance._listPatrol.Add(this);
        // Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _radius);
        // foreach (var hit in hits)
        // {
        //     if (hit.CompareTag("Enemy"))
        //     {
        //         EnemyAI enemy = hit.GetComponent<EnemyAI>();
        //         if (enemy != null)
        //         {
        //             if (checkInEnemy(enemy))
        //                 enemy.setPatrol(this);
        //         }
        //     }
        // }
    }

    void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            Debug.Log("Dang Ky Su Kien Thanh Cong", this);
            GameManager.Instance.On_onLight += onLight;
            GameManager.Instance.On_offLight += offLight;
        }
        else
        {
            Debug.LogWarning("Dang Ky Su Kien Khong Thanh Cong", this);
        }
    }

    void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.On_onLight -= onLight;
            GameManager.Instance.On_offLight -= offLight;
        }
    }

    private void onLight()
    {
        float delay = Random.Range(0, 1.5f);
        StartCoroutine(setActive(delay, true));
    }

    private void offLight()
    {
        float delay = Random.Range(0, 1.5f);
        StartCoroutine(setActive(delay, false));
    }

    public float getRadius() => _radius;
    public void setRadius(float radius) => _radius = radius;

    public int getMaxEnemy() => _maxEnemy;
    public void setMaxEnemy(int value) => _maxEnemy = value;

    private IEnumerator setActive(float delay, bool amount)
    {
        yield return new WaitForSeconds(delay);
        if (_isFire)
            _fire.SetActive(amount);
    }

    public bool checkInEnemy(EnemyAI enemy)
    {
        if (_listEnemy.Count >= _maxEnemy || !_listEnemy.Contains(enemy)) return false;
        _listEnemy.Add(enemy);
        return true;
    }

    public bool checkOutEnemy(EnemyAI enemy)
    {
        if (_listEnemy.Count > 0 && _listEnemy.Contains(enemy))
        {
            _listEnemy.Remove(enemy);
            return true;
        }
        return false;
    }

    public Vector3 GetRandomPoint()
    {
        // Random trong vòng tròn bán kính = 1, rồi nhân với radius
        Vector2 randomPos = Random.insideUnitCircle * _radius;

        // Trả về Vector3 với z = 0
        return new Vector3(transform.position.x + randomPos.x, transform.position.y + randomPos.y, 0f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }

    public float set(float radius, int maxEnemy)
    {
        _radius = radius;
        _maxEnemy = maxEnemy;
        return _col.radius;
    }
}
