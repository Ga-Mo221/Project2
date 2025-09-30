using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private EnemyType _type;
    private bool IsTNTRed => _type == EnemyType.TNT;

    
    [Foldout("Stats")]
    public float _maxHealth = 100;
    [Foldout("Stats")]
    public float _currentHealth = 0;
    [Foldout("Stats")]
    [SerializeField] private float _speed = 6f;
    [Foldout("Stats")]
    public float _damage = 10f;
    [Foldout("Stats")]
    [SerializeField] private float _range = 2.5f;
    [Foldout("Stats")]
    [SerializeField] private float _attackspeed = 2f;
    [Foldout("Stats")]
    [SerializeField] private float _radius = 10f;

    [Foldout("Die")]
    [SerializeField] private bool _Die = false;
    [Foldout("Die")]
    [HideIf(nameof(IsTNTRed))]
    [Range(0, 100)][SerializeField] private int critDropGold = 25;
    [Foldout("Die")]
    [HideIf(nameof(IsTNTRed))]
    [Range(0, 100)][SerializeField] private int critDropMeat = 25;
    [Foldout("Die")]
    [HideIf(nameof(IsTNTRed))]
    [SerializeField] private GameObject MeatDropObj; // respawn thi tat
    [Foldout("Die")]
    [HideIf(nameof(IsTNTRed))]
    [SerializeField] private GameObject GoldDropObj; // respawn thi tat

    [Foldout("Patrol")]
    [SerializeField] private bool _canPatrol = true;
    [Foldout("Patrol")]
    [SerializeField] public EnemyPatrol _patrol;
    [Foldout("Patrol")]
    [SerializeField] private float _changeTargetPatrolDelay = 1f;

    [Foldout("Status")]
    [SerializeField] private bool _IsCreate = false;
    [Foldout("Status")]
    [SerializeField] private bool _canAction = false;
    [Foldout("Status")]
    [SerializeField] private bool _Detec = false;
    [Foldout("Status")]
    [SerializeField] public GameObject target;

    [Foldout("Componet")]
    [SerializeField] private FindPath _path;
    [Foldout("Componet")]
    [SerializeField] private GameObject _GFX;
    [Foldout("Componet")]
    [SerializeField] private Image _HPimg;
    [Foldout("Componet")]
    [SerializeField] private Animator _anim;

    [Foldout("Other")]
    [SerializeField] private GameObject _outLine;
    [Foldout("Other")]
    [SerializeField] private GameObject _HPBar;
    [Foldout("Other")]
    [SerializeField] private GameObject _MinimapIcon;

    private Rigidbody2D _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _currentHealth = _maxHealth;
        _path.setPropety(_speed, _range);
        InvokeRepeating("UpdatePath", 0f, 0.5f);
        if (!_IsCreate)
            EnemyHouse.Instance._listEnemy.Add(this);
        else
            EnemyHouse.Instance._listEnemyCreate.Add(this);
    }

    protected virtual void Update()
    {
        flip(target);
        _HPimg.fillAmount = _currentHealth / _maxHealth;
    }

    #region Create Path
    private void UpdatePath()
    {
        if (_path._seeker.IsDone())
        {
            if (target != null)
            {
                _path.setTarget(target.transform.position);
            }
            else
                setDetect(false);
            _path.setDetec(_Detec);
            _path.UpdatePath();
        }
    }
    #endregion


    #region Respawn
    public void respawn(Vector3 pos)
    {
        transform.position = pos;
        setCanPatrol(true);
        if (!IsTNTRed)
        {
            MeatDropObj.SetActive(false);
            GoldDropObj.SetActive(false);
        }
        _currentHealth = _maxHealth;
        setDie(false);
        _canAction = false;
        _Detec = false;
        _anim.SetBool("Die", false);
    }
    #endregion


    #region Set Target
    public void setTarget(GameObject obj)
    {
        target = obj;
        setCanPatrol(false);
    }
    #endregion


    #region Die
    public void dead()
    {
        setDie(true);
        _outLine.SetActive(false);
        _MinimapIcon.SetActive(false);
        _HPBar.SetActive(false);
        target = null;
        _path.setTarget(transform.position);

        if (!IsTNTRed)
        {
            int roll = Random.Range(0, 100);
            if (roll >= 100 - critDropGold)
                _anim.SetInteger("Type", 1);
            else if (roll >= 100 - critDropGold - critDropMeat)
                _anim.SetInteger("Type", 2);
            else
                _anim.SetInteger("Type", 3);
        }
        _anim.SetBool("Die", true);
    }
    #endregion


    #region Pantrol
    private Coroutine _newPaltro;
    public void setPatrol(EnemyPatrol patrol)
        => _patrol = patrol;
    public void pantrol()
    {
        if (!_canPatrol) return;
        if (target != null) return;
        if (_patrol == null)
            _patrol = EnemyHouse.Instance.getTargetPatrol(this);

        if (_patrol != null)
        {
            if (_path.getTargetPos() == _path.getTarget() && _newPaltro == null)
                _newPaltro = StartCoroutine(setTargetPatrol());
        }
    }
    private IEnumerator setTargetPatrol()
    {
        yield return new WaitForSeconds(_changeTargetPatrolDelay);
        Vector3 _point = _patrol.GetRandomPoint();
        _path.setTarget(_point);
        _newPaltro = null;
    }
    #endregion


    #region Find Player or Animal
    public GameObject Find()
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
                if (dist < minDist)
                {
                    minDist = dist;
                    nearest = hit.gameObject;
                }
            }
        }
        return nearest;
    }

    private bool checkTag(Collider2D hit)
    {
        List<string> _tagPlayer = new List<string> { "Warrior", "Archer", "Lancer", "TNT", "Healer" };
        if (_tagPlayer.Contains(hit.tag))
        {
            var playerAI = hit.GetComponent<PlayerAI>();
            if (!playerAI.getDie()) return true;
        }
        if (hit.CompareTag("House"))
        {
            var house = hit.GetComponent<HouseHealth>();
            if (house.getCanDetec()) return true;
        }
        if (hit.CompareTag("Animal"))
        {
            var animalAi = hit.GetComponent<AnimalAI>();
            if (!animalAi.getDie()) return true;
        }

        return false;
    }
    #endregion


    #region Attack
    private Coroutine _attackSpawn;
    public virtual void attack()
    {
        float dist = Vector3.Distance(transform.position, target.transform.position);
        if (dist <= _range)
        {
            _canAction = true;
            if (_attackSpawn == null)
                _attackSpawn = StartCoroutine(attackSpawn(_attackspeed));
        }
        else _canAction = false;
    }
    private IEnumerator attackSpawn(float delay)
    {
        yield return new WaitForSeconds(delay);
        _anim.SetTrigger("Attack");
        _attackSpawn = null;
    }
    #endregion


    #region Flip
    public void flip(GameObject _nearest)
    {
        if (_canAction)
        {
            if (_nearest != null)
            {
                // lật theo hướng của nearest
                float _nearestX = _nearest.transform.position.x;
                float _X = transform.position.x;
                if (_nearestX > _X)
                    _GFX.transform.localScale = new Vector3(1, 1, 1);
                else if (_nearestX < _X)
                    _GFX.transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        else
        {
            // Lật sprite theo hướng di chuyển
            if (Mathf.Abs(_rb.linearVelocity.x) > 0.01f)
            {
                float sx = _rb.linearVelocity.x > 0 ? 1f : -1f;
                _GFX.transform.localScale = new Vector3(sx, 1f, 1f);
            }
        }
    }
    #endregion


    #region set
    public void setDetect(bool amount)
    {
        _Detec = amount;
        if (!amount) setCanAction(false);
    }
    public void setCanAction(bool amount) => _canAction = amount;

    public void setCanPatrol(bool amount) => _canPatrol = amount;

    // Die
    public void setDie(bool amount) => _Die = amount;
    public bool getDie() => _Die;

    public void setIsCreate(bool amount) => _IsCreate = amount;
    #endregion

    #region Draw
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radius);

        // tam danh
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _range);
    }
    #endregion
}

public enum EnemyType
{
    Lancer,
    Minotaur,
    Orc,
    TNT,
    Shaman,
    Fish,
    Gnoll
}
