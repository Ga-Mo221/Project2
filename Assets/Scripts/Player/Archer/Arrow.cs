using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private PlayerAI _playerAI;
    [SerializeField] private Transform _target;
    private float _damage;
    [SerializeField] private float _maxSpeed = 10f;
    [SerializeField] private float _speed= 0;
    private Rigidbody2D _rb;
    [SerializeField] public bool Skill = false;
    [SerializeField] private GameObject _normal;
    [SerializeField] private GameObject _Skill;
    private Vector2 direction = Vector2.zero;
    [SerializeField] private bool ok = false;
    private bool change = false;
    [SerializeField] private bool _isPlayer = true;
    [SerializeField] private PlayerHitDamage _hitDamage;
    [SerializeField] private UnitAudio _audio;

    private SpriteRenderer _arrow1;
    private SpriteRenderer _arrow2;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _arrow1 = _normal.GetComponent<SpriteRenderer>();
        _arrow2 = _Skill.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        int _yoder = -(int)(transform.position.y * 100) + 100000;
        _arrow1.sortingOrder = _yoder;
        _arrow2.sortingOrder = _yoder;
        if (_target == null) gameObject.SetActive(false);
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

        if (_target == null) return;
        float dis = Vector3.Distance(transform.position, _target.position);
        if (dis <= 0.2 && !Skill)
        {
            if (_target.CompareTag("Enemy") || _target.CompareTag("Animal") || _target.CompareTag("EnemyHouse"))
            {
                _hitDamage.setPlayerAI(_playerAI);
                _hitDamage.attack(_isPlayer, _damage, _target.gameObject);
                gameObject.SetActive(false);
                _target = null;
                _audio.PlayFarmOrHitDamageSound();
            }
        }
    }

    public void setTarget(bool isPlayer, PlayerAI script, bool Skills, float damage, float speed, Vector3 scale)
    {
        transform.localScale = scale;
        if (speed != 0)
            _speed = speed;
        else _speed = _maxSpeed;

        _isPlayer = isPlayer;
        _playerAI = script;
        Skill = Skills;
        if (script.target != null)
            _target = script.target.transform;
        _damage = damage;
        change = false;
        ok = false;
    }
    public void setTarget(bool isPlayer, GameObject obj, bool Skills, float damage, float speed, Vector3 scale)
    {
        transform.localScale = scale;
        if (speed != 0)
            _speed = speed;
        else _speed = _maxSpeed;

        _isPlayer = isPlayer;
        Skill = Skills;
        _target = obj.transform;
        _damage = damage;
        change = false;
        ok = false;
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
        change = false;
        gameObject.SetActive(false);
    } 

    // void OnTriggerEnter2D(Collider2D collision)
    // {
    //     if (collision == null) return;
    //     if (!Skill) return;
    //     if (collision.CompareTag("Enemy") || collision.CompareTag("Animal") || collision.CompareTag("EnemyHouse"))
    //     {
    //         _hitDamage.setPlayerAI(_playerAI);
    //         _hitDamage.attack(_isPlayer, _damage, collision.gameObject, true);
    //     }
    // }
}
