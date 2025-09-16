using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    private float _damage;
    [SerializeField] private float _speed = 10f;
    private Rigidbody2D _rb;
    [SerializeField] public bool Skill = false;
    [SerializeField] private GameObject _normal;
    [SerializeField] private GameObject _Skill;
    private Vector2 direction = Vector2.zero;
    private bool ok = false;
    private bool change = false;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Skill && !change)
        {
            change = true;
            _Skill.SetActive(true);
            _normal.SetActive(false);
        }
        else if (!Skill && !change)
        {
            change = true;
            _normal.SetActive(true);
            _Skill.SetActive(false);
        }
        moveToTarget();
    }

    public void setTarget(Transform target,bool Skills ,float damage)
    {
        Skill = Skills;
        _target = target;
        _damage = damage;
    }
    public Transform getTarget() => _target;

    private void moveToTarget()
    {
        if (_target == null) return;

        if (!Skill)
        {
            // Tính vector hướng từ mũi tên -> target
            direction = ((Vector2)_target.position - _rb.position).normalized;
        }
        else if (Skill && !ok)
        {
            ok = true;
            direction = ((Vector2)_target.position - _rb.position).normalized;
            _speed *= 2.5f;
            StartCoroutine(setactive());
        }

        // Gán vận tốc cho Rigidbody2D
            _rb.linearVelocity = direction * _speed;

        // Xoay mũi tên theo hướng bay (để mũi tên trông tự nhiên hơn)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private IEnumerator setactive()
    {
        yield return new WaitForSeconds(2f);
        ok = false;
        Skill = false;
        change = false;
        gameObject.SetActive(false);
    } 

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;
        if (collision.CompareTag("Enemy") || collision.CompareTag("Animal"))
        {
            change = false;
            _target = null;
            if (!Skill)
                gameObject.SetActive(false);
        }
    }
}
