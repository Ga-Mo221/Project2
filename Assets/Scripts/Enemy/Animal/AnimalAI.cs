using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using NaughtyAttributes;
using System.Collections.Generic;

public class AnimalAI : MonoBehaviour
{
    #region Value
    [SerializeField] private AnimalClass _animalAI;
    private bool IsSheep => _animalAI == AnimalClass.Sheep;

    [Foldout("Stats")]
    public float _maxHealth = 100;
    [Foldout("Stats")]
    public float _health = 0f;
    [Foldout("Stats")]
    [HideIf(nameof(IsSheep))]
    public float _damage;
    [Foldout("Stats")]
    [HideIf(nameof(IsSheep))]
    [SerializeField] private float _speedAttack = 2f;
    [Foldout("Stats")]
    [SerializeField] private float _speed = 5f;
    [Foldout("Stats")]
    [HideIf(nameof(IsSheep))]
    [SerializeField] private float _range = 3f;

    [Foldout("Find")]
    private float _repathRate = 0.5f;
    [Foldout("Find")]
    [SerializeField] private FindPath path;
    [Foldout("Find")]
    public GameObject target;
    [Foldout("Find")]
    [SerializeField] private float _radius = 20f;

    [Foldout("Respawm")]
    [SerializeField] private int _respawmTime = 10;


    [Foldout("Patrol")]
    public float _PatrolRadius = 10f;
    [Foldout("Patrol")]
    [SerializeField] private Vector3 _patrolTarget;
    [Foldout("Patrol")]
    [SerializeField] private float _timeDelayPatrol = 1.0f;
    private Coroutine _delay;


    [Foldout("Status")]
    [SerializeField] private bool _Die = false;
    [Foldout("Status")]
    [SerializeField] private bool _canPatrol;
    [Foldout("Status")]
    [SerializeField] private bool _canAction = false;
    [Foldout("Status")]
    [SerializeField] private bool detec = false;






    [Foldout("Other")]
    [SerializeField] private GameObject _gfx;
    [Foldout("Other")]
    [SerializeField] private GameObject _HPCanvas;
    [Foldout("Other")]
    [SerializeField] private GameObject _OutLine;
    [Foldout("Other")]
    [SerializeField] private Image _hpBar;
    [Foldout("Other")]
    [SerializeField] private GameObject _MiniMapIcon;
    [Foldout("Other")]
    [SerializeField] private Transform _centerTransform;



    private Animator _anim; // animation của đối tượng
    private Rigidbody2D _rb;
    #endregion

    protected virtual void Start()
    {

        path.setPropety(_speed, _range);

        InvokeRepeating("UpdatePath", 0f, _repathRate);
        _health = _maxHealth;


        //seeker = GetComponent<Seeker>();

        _rb = GetComponent<Rigidbody2D>();
        if (target == null && !_Die)
        {
            SetNewPatrol(Vector2.zero);
        }
    }

    private bool _isDead = false;
    protected virtual void Update()
    {
        _hpBar.fillAmount = _health / _maxHealth;
        flip(target, _canAction);

        if (_Die && !_isDead)
        {
            _isDead = true;  // đánh dấu là đã chết
            Die();
        }
    }


    #region Create Path
    public void UpdatePath()
    {
        if (_Die) return;
        if (path._seeker.IsDone())
        {
            if (target != null && !_Die)
            {
                // có enemy -> đuổi
                path.setTarget(target.transform.position);
            }
            else if (_canPatrol && !_Die)
            {
                _canAction = false;
                path.setTarget(_patrolTarget);
                float dist = Vector3.Distance(transform.position, _patrolTarget);
                //Debug.Log(dist);
                if (dist < 1.5)
                {
                    if (_delay == null)
                    {
                        _delay = StartCoroutine(setDelayPatrol());
                    }
                }
            }
            path.UpdatePath();
        }
    }

    #endregion

    #region Find Enemy || Player
    public GameObject findEnemyorPlayer()
    {
        if (_Die) return null;
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
                var _script = hit.gameObject.GetComponent<Item>();
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
    private string[] tagAnimalorEnrmy = { "Enemy", "Archer", "Warrior", "Lancer", "Healer", "TNT", "Enemy" };

    private Coroutine _attackSpeed;

    protected virtual AnimalAI attack(GameObject _nearest)
    {
        if (_nearest != null && System.Array.Exists(tagAnimalorEnrmy, tag => _nearest.CompareTag(tag)))
        {
            float dist = Vector2.Distance(transform.position, _nearest.transform.position);
            if (dist <= _range)
            {
                _canAction = true;
                if (_attackSpeed == null)
                    _attackSpeed = StartCoroutine(attackSpeed());
            }
            else
                _canAction = false;
        }
        return null;
    }

    private IEnumerator attackSpeed()
    {
        _anim.SetTrigger("Attack");
        yield return new WaitForSeconds(_speedAttack);
        _attackSpeed = null;
    }

    #endregion

    public bool checkTag(Collider2D hit)
    {
        List<string> _tag = new List<string> { "Warrior", "Archer", "Lancer", "TNT", "Healer" };
        if (_tag.Contains(hit.tag))
        {
            var playerAI = hit.GetComponent<PlayerAI>();
            if (!playerAI.getDie()) return true;
        }
        if (hit.CompareTag("Enemy"))
        {
            var enemyAi = hit.GetComponent<EnemyAI>();
            if (!enemyAi.getDie()) return true;
        }
        return false;
    }

    #region Draw
    protected virtual void OnDrawGizmosSelected()
    {
        // find
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _radius);

    }
    #endregion

    #region Flip
    public GameObject flip(GameObject _nearest, bool canAction)
    {
        if (canAction)
        {
            if (_nearest != null)
            {
                // lật theo hướng của nearest
                float _nearestX = _nearest.transform.position.x;
                float _X = transform.position.x;
                if (_nearestX > _X)
                    _gfx.transform.localScale = new Vector3(1, 1, 1);
                else if (_nearestX < _X)
                    _gfx.transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        else
        {
            // Lật sprite theo hướng di chuyển
            if (Mathf.Abs(_rb.linearVelocity.x) > 0.01f)
            {
                float sx = _rb.linearVelocity.x > 0 ? 1f : -1f;
                _gfx.transform.localScale = new Vector3(sx, 1f, 1f);
            }
        }
        return _nearest;
    }
    #endregion

    public void setdetec(bool _detec)
    {
        detec = _detec;
        path.setDetec(_detec);
    }

    #region Patrol
    public void SetNewPatrol(Vector2 dir)
    {
        Vector2 randomPoint;
        Vector2 newDir;
        float angleLimit = 60f; // giới hạn góc lệch tối đa (độ)

        do
        {
            // random 1 điểm trong phạm vi patrol
            randomPoint = (Random.insideUnitCircle * _PatrolRadius) + (Vector2)_centerTransform.position;
            if (dir != Vector2.zero)
            {
                // vector hướng từ vị trí hiện tại đến điểm random
                newDir = (randomPoint - (Vector2)transform.position).normalized;

                // kiểm tra góc giữa hướng random và hướng dir
                if (Vector2.Angle(dir.normalized, newDir) > angleLimit)
                    continue;
            }
        }
        while ((randomPoint - (Vector2)transform.position).magnitude < 2f);

        // gán target
        _patrolTarget = new Vector3(randomPoint.x, randomPoint.y, 0);
        path.setTarget(_patrolTarget);
    }
    private IEnumerator setDelayPatrol()
    {
        yield return new WaitForSeconds(_timeDelayPatrol);
        SetNewPatrol(_rb.linearVelocity); // truyền hướng hiện tại vào
        _delay = null;
    }
    #endregion

    #region Die
    public void Die()
    {
        _Die = true;
        _anim.SetBool("Die", true);
        path.setTarget(transform.position);

        //Debug.Log("die rr");
        _MiniMapIcon.SetActive(false);
        _HPCanvas.SetActive(false);
        _OutLine.SetActive(false);

        StartCoroutine(Respawm(_respawmTime));

        //Debug.Log("die r");
    }
    #endregion

    #region RunAway Sheep


    public virtual void FleeFrom(GameObject attackr)
    {
    }
    #endregion

    #region Respawm
    private IEnumerator Respawm(int delay)
    {
        yield return new WaitForSeconds(delay);
        _Die = false;
        _gfx.GetComponent<SpriteRenderer>().enabled = true;
        _health = _maxHealth;
        _anim.SetBool("Die", false);
        _OutLine.SetActive(true);
        SetNewPatrol(Vector2.zero);
    }
    #endregion
    
    public bool getDie() => _Die;
}

public enum AnimalClass
{
    Bear,
    Snake,
    Sheep,
    Spider
}