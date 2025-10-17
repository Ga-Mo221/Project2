using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using NaughtyAttributes;
using System.Collections.Generic;

public class PlayerAI : MonoBehaviour
{
    #region Value

    [Header("Class")]
    public UnitType _unitClass;
    private bool IsHealer => _unitClass == UnitType.Healer;
    private bool IsTNT => _unitClass == UnitType.TNT;
    private bool IsHealerOrTNT => _unitClass == UnitType.Healer || _unitClass == UnitType.TNT;

    [Foldout("Stats")]
    public int _level = 1;
    [Foldout("Stats")]
    public float _maxHealth = 100;
    [Foldout("Stats")]
    public float _health; // máu
    [Foldout("Stats")]
    public float _damage = 10; // sát thương
    [Foldout("Stats")]
    public float _maxSpeed = 6f; // tốc độ tối đa
    [Foldout("Stats")]
    public float _range = 1.5f; // tầm đánh
    [HideIf(nameof(IsTNT))]
    [Foldout("Stats")]
    public float _attackSpeedd = 2.5f; // thời gian sau mỗi đồn đánh.
    [Foldout("Stats")]
    public int _slot = 1;
    [Foldout("Stats")]
    public int _createTime_sec = 5;

    [Foldout("AI Find")]
    [SerializeField] private float _radius = 10f; // bán kính phát hiện Items, Enemys, Animals
    [HideIf(nameof(IsHealerOrTNT))]
    [Foldout("AI Find")]
    [SerializeField] private float _radius_farm = 1.5f; // tầm farm
    [HideIf(nameof(IsHealerOrTNT))]
    [Foldout("AI Find")]
    [SerializeField] private float _farmSpeed = 1f; // thời gian sau mỗi đòn farm
    [Foldout("AI Find")]
    public Item _itemScript; // dùng để tắt chọn đối với item.

    [HideIf(nameof(IsHealerOrTNT))]
    [Foldout("Inventory")]
    public int _maxRock = 5;
    [HideIf(nameof(IsHealerOrTNT))]
    [Foldout("Inventory")]
    public int _maxGold = 5;
    [HideIf(nameof(IsHealerOrTNT))]
    [Foldout("Inventory")]
    public int _maxWood = 5;
    [HideIf(nameof(IsHealerOrTNT))]
    [Foldout("Inventory")]
    public int _maxMeat = 5;
    [HideIf(nameof(IsHealerOrTNT))]
    [Foldout("Inventory")]
    public int _rock = 0;
    [HideIf(nameof(IsHealerOrTNT))]
    [Foldout("Inventory")]
    public int _gold = 0;
    [HideIf(nameof(IsHealerOrTNT))]
    [Foldout("Inventory")]
    public int _wood = 0;
    [HideIf(nameof(IsHealerOrTNT))]
    [Foldout("Inventory")]
    public int _meat = 0;

    [Foldout("GFX")]
    [SerializeField] private GameObject _HPCanvas;
    [Foldout("GFX")]
    [SerializeField] private GameObject _OutLine;
    [Foldout("GFX")]
    [SerializeField] private Image _hpBar; // image thanh máu
    [Foldout("GFX")]
    [SerializeField] private GameObject _selet; // phát hiện đã được chọn
    [Foldout("GFX")]
    [SerializeField] private GameObject _GFX; // hình ảnh nhân vật
    [Foldout("GFX")]
    public GameObject _MiniMapIcon;
    [Foldout("GFX")]
    [SerializeField] private GameObject _AI; // hiển thị đang bật tắt AI

    [Foldout("Other")]
    [SerializeField] private UnitAudio _audio;
    [Foldout("Other")]
    [SerializeField] private Rada _rada;
    [Foldout("Other")]
    [Header("Component")]
    public Animator _anim; // animation của đối tượng

    [Foldout("Other")]
    [Header("Effect")]
    [HideIf(nameof(IsHealerOrTNT))]
    public GameObject _healEffect;

    [Header("Target")]
    [SerializeField] private FindPath path;
    public GameObject target;
    public GameObject cacheTarget;

    // bool
    [Foldout("Status")]
    [SerializeField] private bool _canUpdateHP = true;
    [Foldout("Status")]
    [SerializeField] private bool _creating = false;
    [Foldout("Status")]
    [SerializeField] private bool _UpTower = false;
    [Foldout("Status")]
    [SerializeField] private bool _Die = false;
    [Foldout("Status")]
    [SerializeField] private bool _detect = false; // phát hiện kẻ địch
    [Foldout("Status")]
    [SerializeField] private bool _isLock = false; // khóa lại không cho về và tìm item dựa vào thành chính.
    [Foldout("Status")]
    [SerializeField] private bool _isAI = true; // để đối tượng được lựa chọn mục tiêu nhắm đến
    [Foldout("Status")]
    [SerializeField] private bool _isTarget = false; // có đang hướng đến mục tiêu nào hay không
    [ShowIf(nameof(IsHealer))]
    [Foldout("Status")]
    [SerializeField] private bool foundLowHealth = false;
    [HideIf(nameof(IsTNT))]
    [Foldout("Status")]
    public int _attackCount = 0;
    [HideIf(nameof(IsHealerOrTNT))]
    [Foldout("Status")]
    public float _healPlus = 0;
    [HideIf(nameof(IsHealerOrTNT))]
    [Foldout("Status")]
    public bool _AOEHeal = false;
    [Foldout("Status")]
    public bool _canAction = false;
    [Foldout("Status")]
    public bool _movingToTower = false;


    [Foldout("Priority")]
    [SerializeField] bool _isUnderAttack = false;
    private Coroutine _cor_UnderAttack;
    [Foldout("Priority")]
    [SerializeField] bool _isAttacking = false;
    private Coroutine _cor_Attacking;
    [Foldout("Priority")]
    [SerializeField] bool _isHarvesting = false;
    private Coroutine _cor_Harvesting;
    [Foldout("Priority")]
    [SerializeField] bool _isIdle = false;
    [Foldout("Priority")]
    [SerializeField] bool _isNewlySpawned = true;
    private Coroutine _cor_NewlySpawned;


    // float
    private float _repathRate = 0.5f; // thời gian lặp lại tiềm đường

    private Rigidbody2D _rb;

    private Collider2D[] hits;

    #endregion



    protected virtual void Start()
    {
        if (_canUpdateHP)
            _health = _maxHealth;

        if (!_anim)
            Debug.LogError($"[{gameObject.name}] [PlayerAI] Chưa Gán 'Animator'");
        if (!_MiniMapIcon)
            Debug.LogError($"[{gameObject.name}] [PlayerAI] Chưa Gán 'GFX'");
        if (!_GFX)
            Debug.LogError($"[{gameObject.name}] [PlayerAI] Chưa Gán 'MinimapIcon'");

        path.setPropety(_maxSpeed, _range);

        _rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, _repathRate);
        InvokeRepeating(nameof(Ai), 0.3f, 0.3f);

        if (_cor_NewlySpawned != null)
            StopCoroutine(_cor_NewlySpawned);

        if (gameObject.activeInHierarchy)
            _cor_NewlySpawned = StartCoroutine(reset_isNewlySpawned());
    }


    private bool IsDie = false;
    public virtual void Ai()
    {
        if (_Die && !IsDie)
        {
            IsDie = true;
            Dead();
            return;
        }
    }


    #region Update
    protected virtual void Update()
    {
        if (_Die) return;
        hits = null;
        hits = Physics2D.OverlapCircleAll(transform.position, _radius);
        setHPBar();
        flip(target, _canAction);
    }
    #endregion


    #region Up Level
    public void upLevel(int leve)
    {
        int level = 0;
        for (int i = _level; i < leve; i++)
        {
            level++;
            if (!IsTNT)
            {
                _health += GameManager.Instance.Info._playerHealthBounus;
                _maxHealth += GameManager.Instance.Info._playerHealthBounus;
            }
            else
            {
                _health -= GameManager.Instance.Info._playerHealthBounus;
                _maxHealth -= GameManager.Instance.Info._playerHealthBounus;
                _damage += 47;
            }
            _damage += GameManager.Instance.Info._damageBounus;
        }
        _level = leve;
        if (level != 0)
            _audio.PlayLevelUpSound();
    }
    #endregion


    #region New Status
    private void newStatus()
    {
        Debug.Log("New State");
        _canUpdateHP = true;
        _level = 1;
        _health = _maxHealth;
        _rock = 0;
        _gold = 0;
        _meat = 0;
        _wood = 0;
        path.setReachedEndOfPath(false);
        _detect = false;
        _isLock = false;
        _isAI = true;
        _isTarget = false;
        foundLowHealth = false;
        _attackCount = 0;
        target = null;
        _healPlus = 0;
        _AOEHeal = false;
        setDie(false);
        setIsTarget(false);
        path.setCurrentWaypoint(0);
    }
    #endregion


    #region Respawn 
    /*
    Dùng để respawn lại đối tượng
    */
    public void respawn(Transform pos)
    {
        newStatus();
        IsDie = false;
        path.setDie(false);
        path.setCanMove(true);
        _MiniMapIcon.SetActive(true);
        _OutLine.SetActive(true);
        _GFX.SetActive(true);
        _HPCanvas.SetActive(true);
        if (!IsHealerOrTNT)
            _healEffect.SetActive(false);
        _selet.SetActive(false);
        _rada.setDie(false);

        if (_cor_NewlySpawned != null)
            StopCoroutine(_cor_NewlySpawned);

        if (gameObject.activeInHierarchy)
            _cor_NewlySpawned = StartCoroutine(reset_isNewlySpawned());
        transform.position = pos.position;
        Debug.Log("respawn");
    }
    #endregion


    #region Dead
    public void Dead()
    {
        _rada.setDie(true);
        path.setDie(true);
        setTarget(transform.position, true);
        resetItemSelect();
        _MiniMapIcon.SetActive(false);
        _OutLine.SetActive(false);
        _HPCanvas.SetActive(false);
        if (!IsHealerOrTNT)
            _healEffect.SetActive(false);
        _selet.SetActive(false);
        setDie(true);
        _farmCoroutine = null;
        _anim.SetTrigger("Die");
        GameManager.Instance.UIPlayerDie(_unitClass);
        Castle.Instance.CheckGameOver();
    }
    public virtual void setActive() => gameObject.SetActive(false);
    #endregion


    #region SetUp Folow
    /*
    Dành cho tất cả các UnitType.
    Dùng để reset Detect hoặc folow theo các mục tiêu có thể chuyển động.
    */
    public void setupFolow(GameObject _nearest)
    {
        if (_nearest == null || _nearest.tag == "Item")
            setDetect(false);
    }
    #endregion


    #region Create Path
    private void UpdatePath()
    {
        //if (_Die) return;
        if (path._seeker.IsDone())
        {
            if (target != null)
            {
                path.setTarget(target.transform.position, target);
            }
            else
                setDetect(false);
            path.UpdatePath();
        }
    }
    #endregion


    #region Set Target
    /*
    Dành cho tất cả các UnitType
    Dùng để gán cho mục tiêu một tọa độ để duy chuyển tới.
    với 2 tham số là Vector3 là địa chỉ đến.
    bool controller: 
        + nếu bạn điều khiển thì là true.
        + nếu do Ai điều khiển thì là false.
    */
    public void setTargetPos(Transform targetPos)
    {
        path.setTargetPos(targetPos.position);
    }
    public virtual void setTarget(Vector3 pos, bool controller, bool farm = false)
    {
        if (_unitClass == UnitType.Archer)
            _movingToTower = false;
        resetItemSelect(farm);
        if (controller)
        {
            setIsAI(false);
            setIsTarget(false);
            target = null;
        }
        else setIsTarget(true);
        path.setTarget(pos, target);
        _canAction = false;
    }
    public Vector3 getTarget() => path.getTarget();
    public void resetItemSelect(bool farm = false)
    {
        if (_itemScript != null)
        {
            if (_itemScript._type != ItemType.Gold)
            {
                _itemScript.removeSelect(transform.name);
            }
            else
            {
                bool see = false;
                foreach (var hit in _itemScript._Farmlist)
                {
                    if (hit == this)
                        see = true;
                }
                if (see)
                    _itemScript._Farmlist.Remove(this);
            }
            _itemScript = null;
        }

        if (!farm)
            cacheTarget = null;
    }
    #endregion


    #region Move To Target
    public bool moveToTarget(GameObject _nearest, bool farm = false)
    {
        if (_nearest != null)
        {
            setTarget(_nearest.transform.position, false, farm);
            return true;
        }
        else return false;
    }
    #endregion


    #region HP Bar Sprite
    private void setHPBar()
    {
        _hpBar.fillAmount = _health / _maxHealth;
        _AI.SetActive(!_isAI);
    }
    #endregion


    #region Attack
    /*
    Dành cho UnitType không phải là TNT và Healer.
    Đánh thú rừng, kẻ địch, thành của kẻ địch
    */
    private Coroutine _attackSpeed;
    protected virtual PlayerAI attack(GameObject _nearest)
    {
        if (_UpTower) return null;
        // Kiểm tra _nearest có null không, có tag "Item" không, và có nằm trong vùng farm không
        if (_nearest != null && _nearest.CompareTag("Enemy"))
        {
            float dist = Vector2.Distance(transform.position, _nearest.transform.position);
            if (dist <= _range)
            {
                var enemy = _nearest.GetComponent<EnemyAI>();
                if (!enemy.getDie())
                {
                    _canAction = true;
                    if (_attackSpeed == null)
                        if (gameObject.activeInHierarchy)
                            _attackSpeed = StartCoroutine(attackSpeed());
                }
            }
            else
                _canAction = false;
        }
        if (_nearest != null && _nearest.CompareTag("Animal"))
        {
            float dist = Vector2.Distance(transform.position, _nearest.transform.position);
            if (dist <= _range)
            {
                var animal = _nearest.GetComponent<AnimalAI>();
                if (!animal.getDie())
                {
                    _canAction = true;
                    if (_attackSpeed == null)
                        if (gameObject.activeInHierarchy)
                            _attackSpeed = StartCoroutine(attackSpeed());
                }
            }
            else
                _canAction = false;
        }
        if (_nearest != null && _nearest.CompareTag("EnemyHouse"))
        {
            float dist = Vector2.Distance(transform.position, _nearest.transform.position);
            if (dist <= _range)
            {
                var house = _nearest.GetComponent<EnemyHouseHealth>();
                if (!house._Die)
                {
                    _canAction = true;
                    if (_attackSpeed == null)
                        _attackSpeed = StartCoroutine(attackSpeed());
                }
            }
            else
                _canAction = false;
        }
        return null;
    }
    private IEnumerator attackSpeed()
    {
        _anim.SetTrigger("attack");

        if (_cor_Attacking != null)
            StopCoroutine(_cor_Attacking);

        if (gameObject.activeInHierarchy)
            _cor_Attacking = StartCoroutine(reset_isAttacking());

        yield return new WaitForSeconds(_attackSpeedd);
        _attackSpeed = null;
    }
    #endregion


    #region Farm
    /*
    Dành Cho UnitType không phải là TNT và Healer.
    dùng để farm các tài nguyên kế bên.
    */
    private Coroutine _farmCoroutine;
    public void farm(GameObject _nearest)
    {
        // Kiểm tra _nearest có null không, có tag "Item" không, và có nằm trong vùng farm không
        if (_nearest != null && _nearest.CompareTag("Item") && !_Die && !_UpTower)
        {
            float dist = Vector2.Distance(transform.position, _nearest.transform.position);
            if (dist <= _radius_farm)
            {
                _canAction = true;
                var _script = _nearest.GetComponent<Item>();
                if (_script != null && _farmCoroutine == null)
                {
                    _farmCoroutine = StartCoroutine(farmTrigger(_script));
                }
            }
            else
                _canAction = false;
        }
    }
    private IEnumerator farmTrigger(Item _script)
    {
        ItemType type = _script._type;
        if (_script._stack > 0)
        {
            if (type == ItemType.Tree && _script.checkSelect(this))
            {
                if (_wood < _maxWood)
                {
                    _anim.SetInteger("TypeFarm", 1);
                    _anim.SetTrigger("attack");

                    if (_cor_Harvesting != null)
                        StopCoroutine(_cor_Harvesting);

                    if (gameObject.activeInHierarchy)
                        _cor_Harvesting = StartCoroutine(reset_isHarvesting());
                }
            }
            else if (type == ItemType.Rock && _script.checkSelect(this))
            {
                if (_rock < _maxRock)
                {
                    _anim.SetInteger("TypeFarm", 2);
                    _anim.SetTrigger("attack");

                    if (_cor_Harvesting != null)
                        StopCoroutine(_cor_Harvesting);

                    if (gameObject.activeInHierarchy)
                        _cor_Harvesting = StartCoroutine(reset_isHarvesting());
                }
            }
            else if (type == ItemType.Gold && _script._value > 0)
            {
                if (_gold < _maxGold)
                {
                    _anim.SetInteger("TypeFarm", 2);
                    _anim.SetTrigger("attack");

                    if (_cor_Harvesting != null)
                        StopCoroutine(_cor_Harvesting);

                    if (gameObject.activeInHierarchy)
                        _cor_Harvesting = StartCoroutine(reset_isHarvesting());
                }
            }
        }
        else if (_script._stack <= 0 && _script.checkSelect(this))
        {
            setIsTarget(false);
            if (_script._type != ItemType.Gold)
                _script.removeSelect(transform.name);
        }
        yield return new WaitForSeconds(_farmSpeed);
        _farmCoroutine = null;
    }
    #endregion


    #region Find Item
    public GameObject findItems()
    {
        if (_UpTower) return null;
        GameObject nearest = null;
        if (Castle.Instance._canFind)
        {
            float minDist = Mathf.Infinity;
            if (hits == null) return null;
            Castle.Instance._canFind = false;
            foreach (var hit in Castle.Instance._allItems)
            {
                if (hit == null) continue;
                if (hit.CompareTag("Item"))
                {
                    float dist = Vector2.Distance(transform.position, hit.transform.position);
                    var _script = hit.gameObject.GetComponent<Item>();
                    if (_script._type == ItemType.Gold)
                    {
                        if (_script._detec && _script._value > 0 && _gold < _maxGold)
                        {
                            if (_script._Farmlist.Count < _script._maxFarmers && _script._detec)
                            {
                                if (dist < minDist) // chỉ chọn nếu gần hơn
                                {
                                    minDist = dist;
                                    nearest = hit.gameObject;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (_script.checkSelectNull() && _script._stack > 0 && _script._detec)
                        {
                            if (_script._type == ItemType.Tree && _wood < _maxWood)
                            {
                                if (dist < minDist)
                                {
                                    minDist = dist;
                                    nearest = hit.gameObject;
                                }
                            }
                            else if (_script._type == ItemType.Rock && _rock < _maxRock)
                            {
                                if (dist < minDist)
                                {
                                    minDist = dist;
                                    nearest = hit.gameObject;
                                }
                            }
                        }
                    }
                }
            }
        }
        // if (nearest != null)
        //     Debug.Log(transform.name + " || " + nearest.transform.parent.name + " || " + Vector3.Distance(transform.position, nearest.transform.position));
        Castle.Instance._canFind = true;
        return nearest;
    }
    #endregion


    #region Find Artor
    private GameObject FindNearest(string tag, System.Func<GameObject, bool> isValid)
    {
        GameObject nearest = null;
        float minDist = Mathf.Infinity;

        if (hits == null) return null;

        foreach (var hit in hits)
        {
            if (hit == null) continue;
            if (!hit.CompareTag(tag)) continue;

            if (isValid(hit.gameObject))
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

    public GameObject findAnimals()
    {
        return FindNearest("Animal", go => !go.GetComponent<AnimalAI>().getDie());
    }

    public GameObject findEnemys()
    {
        return FindNearest("Enemy", go => !go.GetComponent<EnemyAI>().getDie());
    }

    public GameObject findEnemyHouse()
    {
        return FindNearest("EnemyHouse", go => !go.GetComponent<EnemyHouseHealth>()._Die);
    }
    #endregion



    #region Find Players
    public GameObject findPlayers()
    {
        GameObject nearest = null;
        float minDist = Mathf.Infinity;
        if (hits == null) return null;
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Warrior") || hit.CompareTag("Archer") || hit.CompareTag("Lancer"))
            {
                var playerAI = hit.GetComponent<PlayerAI>();
                if (playerAI._Die == true) return null;

                float health = playerAI._health;
                float maxhealth = playerAI._maxHealth;
                float dist = Vector3.Distance(transform.position, hit.transform.position);

                // Nếu tìm thấy người máu thấp
                if (health < maxhealth)
                {
                    if (!foundLowHealth || dist < minDist) // lần đầu thấy hoặc gần hơn
                    {
                        foundLowHealth = true;
                        minDist = dist;
                        nearest = hit.gameObject;
                    }
                }
                else if (!foundLowHealth) // chỉ xét máu đầy khi chưa tìm thấy ai máu thấp
                {
                    if (dist < minDist)
                    {
                        minDist = dist;
                        nearest = hit.gameObject;
                    }
                }
            }
        }
        return nearest;
    }
    #endregion

    #region Stop Here
    public void StopHere()
    {
        path.setTarget(transform.position, gameObject);
    }
    #endregion


    #region Go To Home
    public void goToHome(GameObject nearest)
    {
        if (nearest == null && cacheTarget == null && checkInventory())
        {
            setIsTarget(false);
            float minDist = Vector3.Distance(transform.position, Castle.Instance._In_Castle_Pos.position);
            Vector3 _pos = Castle.Instance._In_Castle_Pos.position;
            if (Castle.Instance._storageList.Count > 0)
            {
                foreach (var i in Castle.Instance._storageList)
                {
                    var _storage = i.GetComponent<Storage>();
                    if (_storage.getActive() && !_storage.getDie())
                    {
                        float dist = Vector3.Distance(transform.position, i.transform.position);
                        if (dist < minDist)
                        {
                            minDist = dist;
                            _pos = _storage.getInPos();
                        }
                    }
                }
            }
            setTarget(_pos, false);
        }
    }
    #endregion


    private bool checkInventory()
    {
        if (_wood > 0) return true;
        if (_rock > 0) return true;
        if (_meat > 0) return true;
        if (_gold > 0) return true;
        return false;
    }


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
        return _nearest;
    }
    #endregion

    #region Check and Add Castle
    public void addCastle(List<PlayerAI> listplayer)
    {
        foreach (var p in listplayer)
        {
            if (p == this)
                return;
        }
        listplayer.Add(this);
    }
    #endregion


    #region  Get Player Priority
    public float GetPlayerPriority()
    {
        float score = 0;

        if (_isUnderAttack) score += 10;
        if (_isAttacking) score += 8;
        if (_isHarvesting) score += 4;
        if (_isIdle) score -= 3;
        if (_isNewlySpawned) score += 6;
        return score;
    }
    #endregion


    #region Draw
    protected virtual void OnDrawGizmosSelected()
    {
        // find
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _radius);

        // farm
        if (!IsHealerOrTNT)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _radius_farm);
        }

        // attack
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _range);
    }
    #endregion



    #region Set funsiton
    // Target
    public void setIsTarget(bool amount)
    {
        _isTarget = amount;
    }
    public bool getIsTarget() => _isTarget;

    // AI
    public void setIsAI(bool amount) => _isAI = amount;
    public bool getIsAI() => _isAI;

    // Lock
    public void setIsLock(bool amount) => _isLock = amount;
    public bool getIsLock() => _isLock;

    // Dectec Enemy
    public void setDetect(bool amount)
    {
        _detect = amount;
        path.setDetec(amount);
        _anim.SetBool("Detectenemy", amount);
    }
    public bool getDetect() => _detect;

    // Selected
    public void isSetSelected(bool amount)
        => _selet.SetActive(amount);

    // Die
    public void setDie(bool amount)
    {
        _Die = amount;
    }
    public bool getDie() => _Die;

    // Creating
    public void setCreating(bool amount)
        => _creating = amount;
    public bool getCreating() => _creating;

    // path can move
    public void setCanMove(bool amount) => path.setCanMove(amount);


    // Archer
    public void setUpTower(bool amount) => _UpTower = amount;
    public bool getUpTower() => _UpTower;

    public virtual int getOderInLayer() => 0;
    #endregion


    #region IEnumerator
    private IEnumerator reset_isNewlySpawned()
    {
        if (_cor_Attacking != null)
            StopCoroutine(_cor_Attacking);
        if (_cor_Harvesting != null)
            StopCoroutine(_cor_Harvesting);
        if (_cor_UnderAttack != null)
            StopCoroutine(_cor_UnderAttack);


        _isNewlySpawned = true;
        yield return new WaitForSeconds(3f);
        _isNewlySpawned = false;
        _isIdle = true;
        _isUnderAttack = false;
        _isAttacking = false;
        _isHarvesting = false;
    }

    private IEnumerator reset_isUnderAttack()
    {
        if (_cor_NewlySpawned != null)
            StopCoroutine(_cor_NewlySpawned);
        if (_cor_Harvesting != null)
            StopCoroutine(_cor_Harvesting);


        _isNewlySpawned = false;
        _isIdle = false;
        _isUnderAttack = true;
        _isHarvesting = false;
        yield return new WaitForSeconds(3f);
        _isUnderAttack = false;
        _isIdle = true;
    }
    
    public void resetUnderAttack()
    {
        if (_cor_UnderAttack != null)
            StopCoroutine(_cor_UnderAttack);

        if (gameObject.activeInHierarchy)
            _cor_UnderAttack = StartCoroutine(reset_isUnderAttack());
    }

    private IEnumerator reset_isAttacking()
    {
        if (_cor_NewlySpawned != null)
            StopCoroutine(_cor_NewlySpawned);
        if (_cor_Harvesting != null)
            StopCoroutine(_cor_Harvesting);


        _isNewlySpawned = false;
        _isIdle = false;
        _isAttacking = true;
        _isHarvesting = false;
        yield return new WaitForSeconds(3f);
        _isAttacking = false;
        _isIdle = true;
    }

    public void resetAttacking()
    {
        if (_cor_Attacking != null)
            StopCoroutine(_cor_Attacking);

        if (gameObject.activeInHierarchy)
            _cor_Attacking = StartCoroutine(reset_isAttacking());
    }

    private IEnumerator reset_isHarvesting()
    {
        if (_cor_NewlySpawned != null)
            StopCoroutine(_cor_NewlySpawned);
        if (_cor_Attacking != null)
            StopCoroutine(_cor_Attacking);
        if (_cor_UnderAttack != null)
            StopCoroutine(_cor_UnderAttack);


        _isNewlySpawned = false;
        _isIdle = false;
        _isAttacking = false;
        _isUnderAttack = false;
        _isHarvesting = true;
        yield return new WaitForSeconds(3f);
        _isHarvesting = false;
        _isIdle = true;
    }
    #endregion
}

public enum UnitType
{
    Warrior,
    Archer,
    Lancer,
    Healer,
    TNT
}