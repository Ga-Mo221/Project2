using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArowEnemy : MonoBehaviour
{
    [SerializeField] private float _maxSpeed = 10;
    [SerializeField] private float _speed = 0;
    private float _damage;
    [SerializeField] private Transform _target;
    private Coroutine _setActive;

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rb;
    [SerializeField] private UnitAudio _audio;

    private Coroutine _fix;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer.sortingOrder = 1000000;
    }

    void Update()
    {
        if (_fix == null && gameObject.activeInHierarchy)
            _fix = StartCoroutine(FixActive());

        if (_target == null && _setActive == null && gameObject.activeInHierarchy)
        {
            StopCoroutine(_fix);
            _setActive = StartCoroutine(setActive());
        }
        if (_setActive != null) return;

        Vector2 direction = ((Vector2)_target.position - _rb.position).normalized;

        // Gán vận tốc cho Rigidbody2D
        _rb.linearVelocity = direction * _speed;

        // Xoay mũi tên theo hướng bay (để mũi tên trông tự nhiên hơn)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        float dis = Vector3.Distance(transform.position, _target.position);
        if (dis <= 0.2)
        {
            bool IsHit = false;

            if (_target != null && PlayerTag.checkTag(_target.gameObject.tag))
            {
                var health = _target.gameObject.GetComponent<PlayerHealth>();
                if (health != null)
                {
                    health.takeDamage(_damage);
                    IsHit = true;
                    _target = null;
                }
            }
            if (_target != null && checkTagHouse(_target.gameObject))
            {
                var health = _target.gameObject.GetComponent<HouseHealth>();
                if (health != null && health.getCanDetec())
                {
                    health.takeDamage(_damage);
                    IsHit = true;
                    _target = null;
                }
            }
            if (_target != null && checkTagAnimal(_target.gameObject))
            {
                var animalHealth = _target.gameObject.GetComponent<AnimalHealth>();
                if (animalHealth != null && !animalHealth._animalAi.getDie())
                {
                    animalHealth.takeDamage(_damage, gameObject);
                    IsHit = true;
                    _target = null;
                }
            }

            if (IsHit)
                _audio.PlayFarmOrHitDamageSound();
        }
    }

    private IEnumerator setActive()
    {
        _spriteRenderer.enabled = false;
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }

    private IEnumerator FixActive()
    {
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }

    public void setProperties(Transform target, float damage, float Xspeed, Vector3 scele)
    {
        _setActive = null;
        _fix = null;
        _spriteRenderer.enabled = true;
        _target = target;
        _damage = damage;
        _speed = _maxSpeed * Xspeed;
        transform.localScale = scele;
    }

    private bool checkTagHouse(GameObject collision)
        => collision.CompareTag("House");
    private bool checkTagAnimal(GameObject collision)
        => collision.CompareTag("Animal");
}
