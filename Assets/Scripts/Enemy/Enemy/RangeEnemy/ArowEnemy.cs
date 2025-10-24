using System.Collections;
using UnityEngine;

public class ArrowEnemy : MonoBehaviour
{
    [SerializeField] private float _maxSpeed = 10f;
    private float _speed;
    private float _damage;
    private Transform _target;
    private Coroutine _autoDisable;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rb;
    [SerializeField] private UnitAudio _audio;
    private bool _hasHit; // ← THÊM flag này
    private bool _on = true;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer.sortingOrder = 1000000;
    }

    void Update()
    {
        if (_on)
        {
            _on = false;
            if (_autoDisable != null)
                StopCoroutine(_autoDisable);
            if (gameObject.activeInHierarchy)
                _autoDisable = StartCoroutine(AutoDisableAfter(3f));
        }
        
        if (_target == null || _hasHit) // ← Kiểm tra thêm flag
            return;

        Vector2 direction = ((Vector2)_target.position - _rb.position).normalized;
        _rb.linearVelocity = direction * _speed;
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (Vector3.Distance(transform.position, _target.position) <= 0.3f)
        {
            TryHitTarget();
        }
    }

    public void setProperties(Transform target, float damage, float Xspeed, Vector3 scale)
    {
        _on = true;
        _spriteRenderer.enabled = true;
        _target = target;
        _damage = damage;
        _speed = _maxSpeed * Xspeed;
        transform.localScale = scale;
        _hasHit = false; // ← Reset flag
    }

    private void TryHitTarget()
    {
        if (_target == null || _hasHit) return; // ← Tránh hit 2 lần
        
        bool isHit = false;

        if (PlayerTag.checkTag(_target.tag))
        {
            var h = _target.GetComponent<PlayerHealth>();
            if (h != null) { h.takeDamage(_damage); isHit = true; }
        }
        else if (_target.CompareTag("House"))
        {
            var h = _target.GetComponent<HouseHealth>();
            if (h != null && h.getCanDetec()) { h.takeDamage(_damage); isHit = true; }
        }
        else if (_target.CompareTag("Animal"))
        {
            var a = _target.GetComponent<AnimalHealth>();
            if (a != null && !a._animalAi.getDie()) { a.takeDamage(_damage, gameObject); isHit = true; }
        }

        if (isHit)
        {
            _hasHit = true; // ← Set flag
            _rb.linearVelocity = Vector2.zero; // ← Dừng ngay lập tức
            _audio.PlayFarmOrHitDamageSound();
            _target = null;

            if (_autoDisable != null)
                StopCoroutine(_autoDisable);
            if (gameObject.activeInHierarchy)
                _autoDisable = StartCoroutine(AutoDisableAfter(0.1f)); // ← Tắt nhanh hơn
        }
    }

    private IEnumerator AutoDisableAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        _spriteRenderer.enabled = false;
        gameObject.SetActive(false);
        _autoDisable = null;
    }

    private void OnDisable() // ← THÊM cleanup
    {
        if (_autoDisable != null)
        {
            StopCoroutine(_autoDisable);
            _autoDisable = null;
        }
        _rb.linearVelocity = Vector2.zero;
    }
}