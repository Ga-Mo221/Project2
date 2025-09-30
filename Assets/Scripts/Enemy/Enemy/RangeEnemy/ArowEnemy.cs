using System.Collections.Generic;
using UnityEngine;

public class ArowEnemy : MonoBehaviour
{
    [SerializeField] private float _maxSpeed = 10;
    [SerializeField] private float _speed = 0;
    private float _damage;
    private Transform _target;

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rb;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        _spriteRenderer.sortingOrder = -(int)(transform.position.y * 100) + 10000;
        if (_target == null) return;

        Vector2 direction = ((Vector2)_target.position - _rb.position).normalized;

        // Gán vận tốc cho Rigidbody2D
        _rb.linearVelocity = direction * _speed;

        // Xoay mũi tên theo hướng bay (để mũi tên trông tự nhiên hơn)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void setProperties(Transform target, float damage, float Xspeed, Vector3 scele)
    {
        _target = target;
        _damage = damage;
        _speed = _maxSpeed * Xspeed;
        transform.localScale = scele;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;
        bool IsHit = false;

        if (checkTagPlayer(collision))
        {
            var health = collision.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.takeDamage(_damage);
                IsHit = true;
            }
        }
        if (checkTagHouse(collision))
        {
            var health = collision.GetComponent<HouseHealth>();
            if (health != null && health.getCanDetec())
            {
                health.takeDamage(_damage);
                IsHit = true;
            }
        }
        if (checkTagAnimal(collision))
        {
            var animalHealth = collision.GetComponent<AnimalHealth>();
            if (animalHealth != null && !animalHealth._animalAi.getDie())
            {
                animalHealth.takeDamage(_damage, gameObject);
                IsHit = true;
            }
        }

        if (IsHit)
            gameObject.SetActive(false);
    }

    private bool checkTagPlayer(Collider2D collision)
    {
        List<string> _tag = new List<string> { "Warrior", "Archer", "Lancer", "TNT", "Healer" };
        return _tag.Contains(collision.tag);
    }
    private bool checkTagHouse(Collider2D collision)
        => collision.CompareTag("House");
    private bool checkTagAnimal(Collider2D collision)
        => collision.CompareTag("Animal");
}
