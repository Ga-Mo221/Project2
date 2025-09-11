using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    private float _damage;
    [SerializeField] private float _speed = 10f;
    private Rigidbody2D _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveToTarget();
    }

    public void setTarget(Transform target, float damage)
    {
        _target = target;
        _damage = damage;
    }
    public Transform getTarget() => _target;

    private void moveToTarget()
    {
        if (_target == null) return;

        // Tính vector hướng từ mũi tên -> target
        Vector2 direction = ((Vector2)_target.position - _rb.position).normalized;

        // Gán vận tốc cho Rigidbody2D
        _rb.linearVelocity = direction * _speed;

        // Xoay mũi tên theo hướng bay (để mũi tên trông tự nhiên hơn)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;
        if (collision.CompareTag("Enemy") || collision.CompareTag("Animal"))
        {
            _target = null;
            gameObject.SetActive(false);
        }
    }
}
