using System.Collections;
using UnityEngine;

public class ArcherUP : MonoBehaviour
{
    [Header("Propety")]
    [SerializeField] private float _radius = 10f;
    [SerializeField] private float _damage = 10f;
    [SerializeField] private float _attackSpeedd = 1f;

    [Header("Prefab")]
    [SerializeField] private GameObject _arowPrefab;
    [Header("Start Arrow")]
    [SerializeField] private Transform _shootPos;
    [Header("target")]
    [SerializeField] private GameObject _target;

    private Animator _anim;

    [SerializeField] private Collider2D[] _hits;
    [SerializeField] private GameObject[] _listArrow = new GameObject[5];

    void Start()
    {
        _anim = GetComponent<Animator>();
        if (!_shootPos)
            Debug.LogError($"[{transform.name}] [ArcherUP] Chưa gán 'shoot pos'");
        if (!_arowPrefab)
            Debug.LogError($"[{transform.name}] [ArcherUP] Chưa gán 'ArowPrefab'");
    }

    void Update()
    {
        _target = FindEnemy();
        if (_target == null) return;

        if (_target != null)
        {
            // lật theo hướng của nearest
            float _targetX = _target.transform.position.x;
            float _X = transform.position.x;
            if (_targetX > _X)
                transform.localScale = new Vector3(1, 1, 1);
            else if (_targetX < _X)
                transform.localScale = new Vector3(-1, 1, 1);
        }

        attack(_target);
    }

    public void setDamage(int damage) => _damage += damage;

    private GameObject FindEnemy()
    {
        GameObject nearest = null;
        float minDist = Mathf.Infinity;
        _hits = null;
        _hits = Physics2D.OverlapCircleAll(transform.position, _radius);

        foreach (var hit in _hits)
        {
            if (hit == null) continue;
            if (hit.CompareTag("Enemy"))
            {
                var enemyAi = hit.GetComponent<EnemyAI>();
                if (enemyAi != null && !enemyAi.getDie())
                {
                    float dist = Vector3.Distance(transform.position, hit.transform.position);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        nearest = hit.gameObject;
                    }
                }
            }
            else if (hit.CompareTag("Animal"))
            {
                var animalAi = hit.GetComponent<AnimalAI>();
                if (animalAi != null && !animalAi.getDie())
                {
                    float dist = Vector3.Distance(transform.position, hit.transform.position);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        nearest = hit.gameObject;
                    }
                }
            }
        }
        return nearest;
    }


    private Coroutine _attackSpeed;
    protected virtual PlayerAI attack(GameObject _target)
    {
        // Kiểm tra _target có null không, có tag "Item" không, và có nằm trong vùng farm không
        if (_target != null && _target.CompareTag("Enemy"))
        {
            float dist = Vector2.Distance(transform.position, _target.transform.position);
            if (dist <= _radius)
            {
                if (_attackSpeed == null)
                    _attackSpeed = StartCoroutine(attackSpeed());
            }
        }
        if (_target != null && _target.CompareTag("Animal"))
        {
            float dist = Vector2.Distance(transform.position, _target.transform.position);
            if (dist <= _radius)
            {
                if (_attackSpeed == null)
                    _attackSpeed = StartCoroutine(attackSpeed());
            }
        }
        return null;
    }
    private IEnumerator attackSpeed()
    {
        _anim.SetTrigger("attack");
        yield return new WaitForSeconds(_attackSpeedd);
        _attackSpeed = null;
    }


    public void spawnArrow()
    {
        if (_target == null) return;
        for (int i = 0; i < _listArrow.Length; i++) // hoặc _listArrow.Count
        {
            if (_listArrow[i] != null)
            {
                var _script = _listArrow[i].GetComponent<Arrow>();

                if (_script.getTarget() == null)
                {
                    _listArrow[i].transform.position = _shootPos.position;
                    _script.setTarget(false, _target, false, _damage, 20f);
                    _listArrow[i].SetActive(true);
                    break;
                }
            }
            else
            {
                GameObject _arrow = Instantiate(_arowPrefab, _shootPos.position, Quaternion.identity, _shootPos);
                _listArrow[i] = _arrow;
                var _script = _arrow.GetComponent<Arrow>();
                _script.setTarget(false, _target, false, _damage, 20f);
                break;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}
