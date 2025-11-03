using System.Collections;
using NaughtyAttributes;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public EnemyType _type;
    private bool IsTNTRed => _type == EnemyType.TNT;


    [Foldout("Stats")]
    public float _maxHealth = 100;
    [Foldout("Stats")]
    public float _currentHealth = 0;
    [Foldout("Stats")]
    public float _speed = 6f;
    [Foldout("Stats")]
    public float _damage = 10f;
    [Foldout("Stats")]
    public float _range = 2.5f;
    [Foldout("Stats")]
    [HideIf(nameof(IsTNTRed))]
    [SerializeField] private float _attackspeed = 2f;
    [Foldout("Stats")]
    [SerializeField] private float _radius = 10f;
    [Foldout("Stats")]
    [SerializeField] private int _coin = 1;

    [Foldout("Die")]
    [SerializeField] private bool _Die = false;
    [Foldout("Die")]
    [SerializeField] private int _typeDie = 0;
    [Foldout("Die")]
    [HideIf(nameof(IsTNTRed))]
    [Range(0, 100)][SerializeField] private int critDropGold = 25;
    [Foldout("Die")]
    [HideIf(nameof(IsTNTRed))]
    [Range(0, 100)][SerializeField] private int critDropMeat = 25;
    [Foldout("Die")]
    [HideIf(nameof(IsTNTRed))]
    [SerializeField] private DropItem MeatDropObj; // respawn thi tat
    [Foldout("Die")]
    [HideIf(nameof(IsTNTRed))]
    [SerializeField] private DropItem GoldDropObj; // respawn thi tat

    [Foldout("Patrol")]
    public bool _inPatrol = true;
    [Foldout("Patrol")]
    [SerializeField] private bool _canPatrol = true;
    [Foldout("Patrol")]
    [SerializeField] private EnemyPatrol _patrol;
    [Foldout("Patrol")]
    [SerializeField] private float _changeTargetPatrolDelay = 1f;

    [Foldout("Status")]
    [SerializeField] private bool _canUpdateHP = true;
    [Foldout("Status")]
    [SerializeField] private bool _IsCreate = false;
    [Foldout("Status")]
    [SerializeField] private bool _canAction = false;
    [Foldout("Status")]
    [SerializeField] private bool _Detec = false;
    [Foldout("Status")]
    public GameObject target;
    [Foldout("Status")]
    [SerializeField] GameObject _currentTarget;

    [Foldout("Componet")]
    [SerializeField] private GameObject _GFX;
    [Foldout("Componet")]
    [SerializeField] private HPBar _HPimg;
    [Foldout("Componet")]
    [SerializeField] private Animator _anim;
    [Foldout("Componet")]
    [SerializeField] private FindPath _path;
    [Foldout("Componet")]
    [SerializeField] private Rigidbody2D _rb;


    [Foldout("Other")]
    [SerializeField] private GameObject _outLine;
    [Foldout("Other")]
    [SerializeField] private GameObject _HPBar;
    [Foldout("Other")]
    [SerializeField] private GameObject _MinimapIcon;
    [Foldout("Other")]
    [SerializeField] private UnitAudio _audio;
    [Foldout("Other")]
    [SerializeField] private IsDetecOBJ _isDectec;

    private Vector3 _originPos;


    void Awake()
    {
        if (_rb == null)
            _rb = GetComponent<Rigidbody2D>();
        _originPos = transform.position;

        if (_isDectec == null)
            _isDectec = transform.parent.GetComponent<IsDetecOBJ>();
    }

    void Start()
    {
        if (_canUpdateHP)
            _currentHealth = _maxHealth;

        _path.setPropety(_speed, _range);
        InvokeRepeating("UpdatePath", 0f, 0.5f);

        InvokeRepeating(nameof(AI), 0f, 0.2f);
    }

    void OnEnable()
    {
        _attackSpawn = null;
        _newPaltro = null;
        _anim.SetBool("Die", _Die);
        if (!IsTNTRed)
            _anim.SetInteger("Type", _typeDie);
    }

    protected virtual void AI()
    {
        flip(target);
        if (getDie()) return;
        target = Find();
        if (target != null)
        {
            setDetect(true);
            attack();
        }

        pantrol();
    }

    protected virtual void Update()
    {
        _HPimg.SetHealth(_currentHealth / _maxHealth);
    }

    #region Create Path
    private void UpdatePath()
    {
        if (_currentTarget != null)
        {
            float dist = Vector2.Distance(transform.position, _currentTarget.transform.position);
            if (dist < 5) _currentTarget = null;
        }

        if (_path._seeker.IsDone())
        {
            if (target != null || _currentTarget != null)
            {
                if (target != null)
                {
                    if (gameObject.activeSelf)
                    {
                        if (target == Castle.Instance.gameObject)
                        {
                            _path.setTarget(Castle.Instance._In_Castle_Pos.position, target);
                        }
                        else if (target.CompareTag("House"))
                        {
                            var house = target.GetComponent<House>();
                            if (house._type == HouseType.Tower)
                                _path.setTarget(house._inTower.transform.position, target);
                            if (house._type == HouseType.Storage)
                                _path.setTarget(house.getInPos(), target);
                        }
                        else
                            _path.setTarget(target.transform.position, target);
                    }
                }
                else
                {
                    if (gameObject.activeSelf)
                    {
                        if (_currentTarget == Castle.Instance.gameObject)
                        {
                            _path.setTarget(Castle.Instance._In_Castle_Pos.position, _currentTarget);
                        }
                        else if (_currentTarget.CompareTag("House"))
                        {
                            var house = _currentTarget.GetComponent<House>();
                            if (house._type == HouseType.Tower)
                                _path.setTarget(house._inTower.transform.position, _currentTarget);
                            if (house._type == HouseType.Storage)
                                _path.setTarget(house.getInPos(), _currentTarget);
                        }
                        else
                            _path.setTarget(_currentTarget.transform.position, _currentTarget);
                    }
                }
            }
            else
                setDetect(false);
            _path.setDetec(_Detec);
            _path.UpdatePath();
        }
    }
    #endregion


    #region Respawn
    public void respawn(Vector3 pos, bool war = true)
    {
        if (_isDectec != null) _isDectec.isChillActive = false;
        _canUpdateHP = true;
        _path.setDie(false);
        _path.setCanMove(true);
        Vector3 SpawnPoint = pos;
        if (!war) SpawnPoint = _originPos;
        transform.parent.position = SpawnPoint;
        Debug.Log($"[{transform.name}] <color=#FF4444>[Respawn]</color> Spawn Point [{transform.position}] Origin Point [{_originPos}] War [{war}]", this);
        setCanPatrol(true);
        if (!IsTNTRed)
        {
            MeatDropObj.gameObject.SetActive(false);
            MeatDropObj.ResetPickUp();
            GoldDropObj.gameObject.SetActive(false);
            GoldDropObj.ResetPickUp();
        }
        _currentHealth = _maxHealth;
        setDie(false);
        _canAction = false;
        _Detec = false;
        _anim.SetBool("Die", false);
    }
    #endregion


    #region Set Target
    public void setTarget(GameObject obj, bool amount = true)
    {
        target = obj;
        _currentTarget = obj;
        setCanPatrol(!amount);
    }
    public void resetCurrentTarget() => _currentTarget = null;
    #endregion


    #region Die
    public void dead()
    {
        setDie(true);
        _outLine.SetActive(false);
        _MinimapIcon.SetActive(false);
        _HPBar.SetActive(false);
        target = null;
        _path.setDie(true);
        _path.setTarget(transform.position, target);

        if (!IsTNTRed)
        {
            int roll = Random.Range(0, 100);
            if (roll >= 100 - critDropGold)
            {
                _typeDie = 1;
                _anim.SetInteger("Type", _typeDie);
            }
            else if (roll >= 100 - critDropGold - critDropMeat)
            {
                _typeDie = 2;
                _anim.SetInteger("Type", _typeDie);
            }
            else
            {
                _typeDie = 3;
                _anim.SetInteger("Type", _typeDie);
            }
        }
        else
        {
            CameraShake.Instance.ShakeCamera();
        }
        if (_type != EnemyType.TNT)
            playDieSound();
        _anim.SetBool("Die", true);
        GameManager.Instance.addCoin(_coin);
    }
    #endregion


    #region Pantrol
    private Coroutine _newPaltro;
    private bool _lastInPatrol = true;
    public void pantrol()
    {
        if (!_canPatrol) return;

        if (_patrol != null && _isDectec != null)
        {
            float dist = Vector2.Distance(transform.position, _patrol.transform.position);
            _inPatrol = dist < _patrol.getRadius();
            if (_inPatrol != _lastInPatrol)
            {
                _lastInPatrol = _inPatrol;
                if (_inPatrol)
                    _isDectec.UpdateChillEffect(_inPatrol);
            }
        }

        if (target != null) return;
        if (_patrol == null && EnemyHouse.Instance != null)
            _patrol = EnemyHouse.Instance.getTargetPatrol(this);

        if (_patrol != null&& _newPaltro == null)
        {
            if (_path.getTargetPos() == _path.getTarget())
                if (gameObject.activeInHierarchy)
                    _newPaltro = StartCoroutine(setTargetPatrol());
        }
    }
    private IEnumerator setTargetPatrol()
    {
        yield return new WaitForSeconds(_changeTargetPatrolDelay);
        Vector3 _point = _patrol.GetRandomPoint();
        Debug.Log($"[{transform.name}] <color=green>Patrol Point</color> [{_point}]", this);
        _path.setTarget(_point, target);
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
        if (PlayerTag.checkTag(hit.tag))
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
        float dist = 0;
        if (target.CompareTag("House"))
        {
            if (target == Castle.Instance.gameObject)
            {
                dist = Vector2.Distance(transform.position, Castle.Instance._In_Castle_Pos.position);
            }
            else
            {
                var house = target.GetComponent<House>();
                if (house._type == HouseType.Tower)
                    dist = Vector2.Distance(house._inTower.transform.position, target.transform.position);
                else if (house._type == HouseType.Storage)
                    dist = Vector2.Distance(house.getInPos(), target.transform.position);
            }
        }
        else dist = Vector2.Distance(transform.position, target.transform.position);
        //Debug.Log($"[{transform.name}] Attack dist = {dist}, range = {_range}");
        if (dist <= _range && _attackSpawn == null)
        {
            //Debug.Log("In Range");
            _canAction = true;
            Debug.Log($"[{transform.name}] <color=#FF4444>[Attack]</color>", this);
            if (gameObject.activeInHierarchy)
                _attackSpawn = StartCoroutine(attackSpawn(_attackspeed));
        }
        else _canAction = false;
    }
    private IEnumerator attackSpawn(float delay)
    {
        _anim.SetTrigger("Attack");
        if (target != null)
        {
            if (target.CompareTag("House") || PlayerTag.checkTag(target.tag))
            {
                GameManager.Instance.War();
            }
        }
        yield return new WaitForSeconds(delay);
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

    // Die
    public void setDie(bool amount) => _Die = amount;
    public bool getDie() => _Die;

    public void setIsCreate(bool amount) => _IsCreate = amount;

    // Patrol
    public void setCanPatrol(bool amount) => _canPatrol = amount;
    public void setPatrol(EnemyPatrol patrol)
        => _patrol = patrol;
    public EnemyPatrol getPatrol() => _patrol;

    // path can move
    public void setCanMove(bool amount) => _path.setCanMove(amount);

    // Type Die
    public void setTypeDie(int value) => _typeDie = value;
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


    #region Play Sound Die
    public void playDieSound()
    {
        _audio.PlayDieSound();
    }
    #endregion
    #region Play Sound Attack
    public void playAttackSoundEnemy()
    {
        _audio.PlayAttackSound();
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
