using System.Collections.Generic;
using UnityEngine;

public class fireMagic : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRender;
    [SerializeField] private float _speed = 5f;
    private float _damage;
    private Transform _target;
    private Rigidbody2D _rb;
    private Animator _anim;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    public void setProperties(Transform target, float damage)
    {
        _damage = damage;
        _target = target;
    }

    void Update()
    {
        _spriteRender.sortingOrder = -(int)(transform.position.y * 100) + 10000;
        if (_target == null) {
            gameObject.SetActive(false);
            return;
        }
        float dis = Vector2.Distance(transform.position, _target.position);
        if (dis > 0.2f)
        {
            Vector2 dir = (transform.position - _target.position).normalized;
            _rb.linearVelocity = -dir * _speed;
        }
        else
        {
            _rb.linearVelocity = Vector2.zero;
            _anim.SetTrigger("explode");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;

        if (PlayerTag.checkTag(collision.tag))
        {
            var health = collision.GetComponent<PlayerHealth>();
            if (health != null)
                health.takeDamage(_damage);
        }
        if (checkTagHouse(collision))
        {
            var health = collision.GetComponent<HouseHealth>();
            if (health != null && health.getCanDetec())
                health.takeDamage(_damage);
        }
        if (checkTagAnimal(collision))
        {
            var animalHealth = collision.GetComponent<AnimalHealth>();
            if (animalHealth != null && !animalHealth._animalAi.getDie())
                animalHealth.takeDamage(_damage, gameObject);
        }
    }

    private bool checkTagHouse(Collider2D collision)
        => collision.CompareTag("House");
    private bool checkTagAnimal(Collider2D collision)
        => collision.CompareTag("Animal");

    public void setActive()
    {
        gameObject.SetActive(false);
    }
}
