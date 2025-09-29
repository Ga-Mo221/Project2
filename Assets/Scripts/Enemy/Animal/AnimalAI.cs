using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using NaughtyAttributes;
using NUnit.Framework.Constraints;

public class AnimalAI : MonoBehaviour
{
    #region Value
    [SerializeField] private AnimalClass _animalAI;
    [SerializeField] private float _radius = 20f;

    public float _maxHealth = 100;
    public float _health;

    public float _speed;

    [HideIf(nameof(IsSheep))]
    public float _damage;

    [HideIf(nameof(IsSheep))]
    public float _range;
    
    [HideIf(nameof(IsSheep))]
    public float _speedAttack;

    private bool IsSheep => _animalAI == AnimalClass.Sheep;

    [SerializeField] private FindPath path;

    [SerializeField] private GameObject _gfx;

    [SerializeField] public Animator _anim; // animation của đối tượng

    private bool detec = false;
    public bool _isdetec = false;
    private float _repathRate = 0.5f;

    public bool _Die = false;

    [Header("GFX")]
    [SerializeField] private GameObject _HPCanvas;
    [SerializeField] private GameObject _OutLine;
    [SerializeField] private Image _hpBar;
    [SerializeField] public GameObject _MiniMapIcon;
    [SerializeField] private bool _canPatrol;
    [SerializeField] private Transform _centerTransform;
    // [SerializeField] private Transform _PatrolPoin;
    [SerializeField] public float _PatrolRadius = 10f;

    private Vector3 _patrolTarget;
    private Coroutine _delay;
    [SerializeField] private float _timeDelayPatrol = 1.0f;
    //private Seeker seeker;
    public GameObject target;

    public bool _canAction = false;

    private Rigidbody2D _rb;
    #endregion

    protected virtual void Start()
    {

        path.setPropety(_speed, _range);

        InvokeRepeating("UpdatePath", 0f, _repathRate);

        //seeker = GetComponent<Seeker>();

        _rb = GetComponent<Rigidbody2D>();
        if (target == null && !_Die)
        {
            SetNewPatrol();
        }
    }

    private bool _isDead = false;
    protected virtual void Update()
    {
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
            if (target != null)
                {
                    // có enemy -> đuổi
                    path.setTarget(target.transform.position);
                }
                else if (_canPatrol)
                {
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
    [SerializeField] private string[] tagAnimalorEnrmy = { "Enemy", "Archer", "Warrior", "Lancer", "Healer", "TNT" };

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
        bool check = hit.CompareTag("Enemy")
        || hit.CompareTag("Archer")
        || hit.CompareTag("Warrior")
        || hit.CompareTag("Lancer")
        || hit.CompareTag("Healer")
        || hit.CompareTag("TNT");
        return check;
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
    /*
    Dùng cho tất cả các Unit Class
    để lật đối tượng theo hướng di chuyển hoặc theo hướng mục tiêu được chọn.
    */
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
    private void SetNewPatrol()
    {
        Vector2 randomPoint;
        do
        {
            randomPoint = (Random.insideUnitCircle * _PatrolRadius) + (Vector2)_centerTransform.position;
        }
        while (randomPoint.magnitude < 2f); // tránh điểm quá gần

        _patrolTarget = new Vector3(randomPoint.x, randomPoint.y, 0);
        //  Debug.Log("New Patrol Target: " + _patrolTarget);
        path.setTarget(_patrolTarget);
    }

    private IEnumerator setDelayPatrol()
    {
        yield return new WaitForSeconds(_timeDelayPatrol);
        SetNewPatrol();
        _delay = null;
    }
    #endregion

    #region TakeDame
    public void TakeDamage()
    {

    }

    #endregion


    #region Die
    public void Die()
    {
        _Die = true;
        //Debug.Log("die rr");
        _MiniMapIcon.SetActive(false);
        _HPCanvas.SetActive(false);
        _OutLine.SetActive(false);

        StartCoroutine(Respawm(10f));
       //Debug.Log("die r");
    }
    #endregion

    #region RunAway Sheep
    private Coroutine _fleeRoutine;

    public void FleeFrom(GameObject attackr)
    {
        if (_animalAI != AnimalClass.Sheep) return;

         if (_fleeRoutine != null)
             StopCoroutine(_fleeRoutine);

        _fleeRoutine = StartCoroutine(FleeRoutine(attackr));
    }

    private IEnumerator FleeRoutine(GameObject attackr)
    {
        float FleeTime = 2f; // nó sẽ tăng tốc chạy trong mấy giây
        float FleeSpeed = _speed * 2;

        float Timer = FleeTime;

        while (Timer > 0 && attackr != null)
        {
            Vector2 FleeDir = (transform.position - attackr.transform.position).normalized;
            _rb.linearVelocity = FleeDir * FleeSpeed;

            Timer -= Time.deltaTime;
            yield return null;
        }

        _rb.linearVelocity = Vector2.zero;
        _fleeRoutine = null;
    }
    #endregion

    #region Respawm
    private IEnumerator Respawm(float delay)
    {

        float timer = delay;
        while (timer > 0)
        {
            Debug.Log("respawm" + timer);
            yield return new WaitForSeconds(1f);
            timer -= 1;
        }


        _Die = false;
        _isDead = false;
        _health = _maxHealth;

        _MiniMapIcon.SetActive(true);
        _HPCanvas.SetActive(true);
        _OutLine.SetActive(true);

        SetNewPatrol();
    }

    #endregion
}

public enum AnimalClass
{
    Bear,
    Snake,
    Sheep,
    Spider
}