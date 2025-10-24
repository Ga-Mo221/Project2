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
    private bool _on = false;

    void Start()
    {
        EnemyHouse.Instance._listPatrol.Add(this);
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _radius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                EnemyAI enemy = hit.GetComponent<EnemyAI>();
                if (enemy != null)
                {
                    if (checkInEnemy(enemy))
                        enemy.setPatrol(this);
                }
            }
        }
    }

    void Update()
    {
        if (GameManager.Instance._timeRTS == 18 && _isFire && !_on)
        {
            _on = !_on;
            float delay = Random.Range(0, 1.5f);
            StartCoroutine(setActive(delay, _on));
        }
        else if (GameManager.Instance._timeRTS == 6 && _isFire && _on)
        {
            _on = !_on;
            float delay = Random.Range(0, 1.5f);
            StartCoroutine(setActive(delay, _on));
        }
    }

    public float getRadius() => _radius;
    public void setRadius(float radius) => _radius = radius;

    public int getMaxEnemy() => _maxEnemy;
    public void setMaxEnemy(int value) => _maxEnemy = value;

    private IEnumerator setActive(float delay, bool amount)
    {
        yield return new WaitForSeconds(delay);
        _fire.SetActive(amount);
    }

    public bool checkInEnemy(EnemyAI enemy)
    {
        if (_listEnemy.Count >= _maxEnemy) return false;
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
}
