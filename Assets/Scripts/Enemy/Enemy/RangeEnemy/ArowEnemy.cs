using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArowEnemy : MonoBehaviour
{
    [SerializeField] private float _maxSpeed = 10;
    [SerializeField] private float _speed = 0;
    [SerializeField] private bool _isAttack = false; // kiem tra da goi takedamage chua
    private float _damage;
    private Transform _target;

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rb;
    [SerializeField] private UnitAudio _audio;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (_isAttack) gameObject.SetActive(true);
        _spriteRenderer.sortingOrder = -(int)(transform.position.y * 100) + 100000;
        if (_target == null) return;

        Vector2 direction = ((Vector2)_target.position - _rb.position).normalized;

        // Gán vận tốc cho Rigidbody2D
        _rb.linearVelocity = direction * _speed;

        // Xoay mũi tên theo hướng bay (để mũi tên trông tự nhiên hơn)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (_target == null) gameObject.SetActive(false);
        float dis = Vector3.Distance(transform.position, _target.position);
        if (dis <= 0.2)
        {
            bool IsHit = false;

            if (PlayerTag.checkTag(_target.gameObject.tag))
            {
                var health = _target.gameObject.GetComponent<PlayerHealth>();
                if (health != null)
                {
                    health.takeDamage(_damage);
                    IsHit = true;
                    _isAttack = true;
                }
            }
            if (checkTagHouse(_target.gameObject))
            {
                var health = _target.gameObject.GetComponent<HouseHealth>();
                if (health != null && health.getCanDetec())
                {
                    health.takeDamage(_damage);
                    IsHit = true;
                    _isAttack = true;
                }
            }
            if (checkTagAnimal(_target.gameObject))
            {
                var animalHealth = _target.gameObject.GetComponent<AnimalHealth>();
                if (animalHealth != null && !animalHealth._animalAi.getDie())
                {
                    animalHealth.takeDamage(_damage, gameObject);
                    IsHit = true;
                    _isAttack = true;
                }
            }

            if (IsHit)
            {
                _audio.PlayFarmOrHitDamageSound();
                if (gameObject.activeInHierarchy)
                    StartCoroutine(setActive());
            }
        }
    }

    private IEnumerator setActive()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }

    public void setProperties(Transform target, float damage, float Xspeed, Vector3 scele)
    {
        _isAttack = false;
        _target = target;
        _damage = damage;
        _speed = _maxSpeed * Xspeed;
        transform.localScale = scele;
    }

    // void OnTriggerEnter2D(Collider2D collision)
    // {
    //     if (collision == null) return;
    //     bool IsHit = false;

    //     if (PlayerTag.checkTag(collision.tag))
    //     {
    //         var health = collision.GetComponent<PlayerHealth>();
    //         if (health != null)
    //         {
    //             health.takeDamage(_damage);
    //             IsHit = true;
    //         }
    //     }
    //     if (checkTagHouse(collision))
    //     {
    //         var health = collision.GetComponent<HouseHealth>();
    //         if (health != null && health.getCanDetec())
    //         {
    //             health.takeDamage(_damage);
    //             IsHit = true;
    //         }
    //     }
    //     if (checkTagAnimal(collision))
    //     {
    //         var animalHealth = collision.GetComponent<AnimalHealth>();
    //         if (animalHealth != null && !animalHealth._animalAi.getDie())
    //         {
    //             animalHealth.takeDamage(_damage, gameObject);
    //             IsHit = true;
    //         }
    //     }

    //     if (IsHit)
    //         gameObject.SetActive(false);
    // }

    private bool checkTagHouse(GameObject collision)
        => collision.CompareTag("House");
    private bool checkTagAnimal(GameObject collision)
        => collision.CompareTag("Animal");
}
