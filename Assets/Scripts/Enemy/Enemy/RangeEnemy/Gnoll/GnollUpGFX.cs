using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GnollUpGFX : MonoBehaviour
{
    [SerializeField] private GameObject _bonePrefab;
    [SerializeField] private Transform _spawnPoint;

    [SerializeField] private GameObject _target;
    [SerializeField] private float _damage;
    [SerializeField] private float _radius = 10f;
    [SerializeField] private float _attackspeed = 2f;
    [SerializeField] private List<GameObject> _listPrefeb = new List<GameObject>();
    

    private Animator _anim;

    void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        _target = Find();

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

        attack();
    }

    public void setDamage(float damage) => _damage = damage;


    #region Find
    private GameObject Find()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _radius);
        GameObject nearest = null;
        float minDist = Mathf.Infinity;
        if (hits == null) return null;
        foreach (var hit in hits)
        {
            if (hit == null) continue;
            if (checkTag(hit))
            {
                float dist = Vector3.Distance(transform.position, hit.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    nearest = hit.gameObject;
                }
            }
        }
        return nearest;
    }
    #endregion

    #region Attack
    private Coroutine _attackSpawn;
    public virtual void attack()
    {
        if (_target == null) return;
        float dist = Vector3.Distance(transform.position, _target.transform.position);
        if (dist <= _radius)
        {
            if (_attackSpawn == null)
                _attackSpawn = StartCoroutine(attackSpawn(_attackspeed));
        }
    }
    private IEnumerator attackSpawn(float delay)
    {
        yield return new WaitForSeconds(delay);
        _anim.SetTrigger("Attack");
        _attackSpawn = null;
    }
    #endregion

    #region Check Tag
    private bool checkTag(Collider2D col)
    {
        if (PlayerTag.checkTag(col.tag))
        {
            var player = col.GetComponent<PlayerAI>();
            if (player != null && !player.getDie())
                return true;
        }
        if (col.CompareTag("Animal"))
        {
            var animal = col.GetComponent<AnimalAI>();
            if (animal != null && !animal.getDie())
                return true;
        }
        return false;
    }
    #endregion

    public void spawnPrefab()
    {
        bool isSpawn = false;
        foreach (var obj in _listPrefeb)
        {
            if (!obj.activeSelf && _target != null)
            {
                var script = obj.GetComponent<ArowEnemy>();
                script.setProperties(_target.transform, _damage, 2f, transform.localScale);
                obj.transform.position = _spawnPoint.position;
                obj.SetActive(true);
                isSpawn = true;
                break;
            }
        }
        if (!isSpawn && _target != null)
        {
            GameObject obj = Instantiate(_bonePrefab, _spawnPoint.position, Quaternion.identity, _spawnPoint);
            var script = obj.GetComponent<ArowEnemy>();
            script.setProperties(_target.transform, _damage, 2f, transform.localScale);
            _listPrefeb.Add(obj);
        }
    }

    #region Draw
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
    #endregion
}
