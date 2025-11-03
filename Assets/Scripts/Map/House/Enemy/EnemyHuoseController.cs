using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class EnemyHuoseController : MonoBehaviour
{
    [SerializeField] private HouseType _type;
    private bool IsTower => _type == HouseType.Tower;
    private bool IsStorage => _type == HouseType.Storage;
    private bool IsCastle => _type == HouseType.Castle;

    [Header("Stats")]
    [SerializeField] private bool _canUpdateHP = true;
    [SerializeField] private bool _Die = false;
    [SerializeField] private bool _Detec = false;
    public float _maxHealth = 100f;
    public float _currentHealth = 0f;
    [SerializeField] private int _coin = 10;

    [Header("Assitst")]
    [HideIf(nameof(IsTower))]
    [SerializeField] private float _alarmRadius = 20f; // khoảng cách báo động
    [HideIf(nameof(IsTower))]
    [SerializeField] private int _playerCountAssist  = 5; // báo động khi có trên 5 kẻ địch lại gần;
    [HideIf(nameof(IsTower))]
    public float _warningTime = 10f;

    [ShowIf(nameof(IsStorage))]
    [SerializeField] private float _assistRadius = 50f; // khoảng cách lính trong phạm vi được gọi đến

    [Header("Storage")]
    [ShowIf(nameof(IsStorage))]
    public int _wood = 10;
    [ShowIf(nameof(IsStorage))]
    public int _rock = 10;
    [ShowIf(nameof(IsStorage))]
    public int _meat = 10;
    [ShowIf(nameof(IsStorage))]
    public int _gold = 10;

    [Header("GnollUp")]
    [ShowIf(nameof(IsTower))]
    public GameObject _GnollUp;
    [ShowIf(nameof(IsTower))]
    [SerializeField] private GameObject _GnollPrefab;

    [Header("Other")]
    [SerializeField] private GameObject _HpBar;
    [SerializeField] private GameObject _Icon;
    [SerializeField] private OutLine _outLine;
    public UnitAudio _audio;
    public Display _display;

    private List<EnemyAI> _enemyAssitList = new List<EnemyAI>();

    void Start()
    {
        if (IsStorage)
            EnemyHouse.Instance._listSpawnPoint.Add(transform);

        if (_canUpdateHP)
            _currentHealth = _maxHealth;
            
        if (IsTower && _GnollUp != null && _GnollPrefab != null)
        {
            float damage = _GnollPrefab.GetComponent<GnollGFX>()._damage;
            _GnollUp.GetComponent<GnollUpGFX>().setDamage(damage);
        }
    }

    void Update()
    {
        if (IsTower) return;
        if (_Die) return;
        callReinforcement();
    }

    public void updateSpriteOder(int oder)
    {
        if (!IsTower) return;
        _GnollUp.GetComponent<SpriteRenderer>().sortingOrder = oder + 1;
    }

    public void gnollCreate(Transform amount)
    {
        if (!IsTower) return;
        _GnollUp.SetActive(false);
        GameObject obj = Instantiate(_GnollPrefab, transform.position, Quaternion.identity, amount);
        var enemy = obj.GetComponent<EnemyAI>();
        enemy.setIsCreate(true);
        enemy.setCanPatrol(false);
    }

    public void die()
    {
        _Die = true;
        _HpBar.SetActive(false);
        _Icon.SetActive(false);
        _outLine.Out();
        CameraShake.Instance.ShakeCamera(1f, 0.5f);
        if (IsStorage)
        {
            Castle.Instance._wood += _wood;
            Castle.Instance._rock += _rock;
            Castle.Instance._meat += _meat;
            Castle.Instance._gold += _gold;
            GameManager.Instance.UIupdateReferences();
        }
        if (IsCastle)
        {
            StartCoroutine(GameOver());
        }
        GameManager.Instance.addCoin(_coin);
    }


    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(GameManager.Instance._displayGameOverTime);
        GameManager.Instance.setWin(true);
        GameManager.Instance.setGameOver(true);
    }


    private Coroutine _call;
    private void callReinforcement()
    {
        int _playerCount = 0;
        Collider2D[] Hits = Physics2D.OverlapCircleAll(transform.position, _alarmRadius);
        foreach (var hit in Hits)
        {
            if (PlayerTag.checkTag(hit.tag))
            {
                var playerAI = hit.GetComponent<PlayerAI>();
                if (!playerAI.getDie())
                    _playerCount++;
            }
        }

        if (_playerCount >= _playerCountAssist)
        {
            _Detec = true;
            if (_call == null)
                _call = StartCoroutine(Call());
        }
        else
        {
            _Detec = false;
            _call = null;
            resetCurrenTargetEnemy();
        }
    }


    private IEnumerator Call()
    {
        Debug.Log("Bao Dong Do");
        _audio.PlayWarningSound();
        GameManager.Instance.UIonWarning(this);
        yield return new WaitForSeconds(_warningTime);
        if (_Detec)
        {
            if (!IsCastle)
            {
                foreach (var hit in EnemyHouse.Instance._listPatrol)
                {
                    float dist = Vector2.Distance(transform.position, hit.transform.position);
                    if (dist > _assistRadius) continue;
                    var enemyPatrol = hit.GetComponent<EnemyPatrol>();
                    int count = 0;
                    if (enemyPatrol != null)
                        foreach (var enemy in EnemyHouse.Instance._listEnemy)
                            if (enemy.getPatrol() == enemyPatrol && count < 3)
                            {
                                count++;
                                enemy.transform.parent.GetComponent<FindPath>().enabled = true;
                                enemy.gameObject.SetActive(true);
                                enemy.setTarget(gameObject, false);
                                _enemyAssitList.Add(enemy);
                                Debug.Log($"<color=#FF4444>[Warning]</color> [{enemy.transform.name}] đã điều động đến [{transform.name}]", enemy);
                            }
                }
            }
            else
                foreach (var enemy in EnemyHouse.Instance._listEnemy)
                    enemy.setTarget(gameObject);
        }
    }

    private void resetCurrenTargetEnemy()
    {
        foreach (var enemy in _enemyAssitList)
        {
            enemy.resetCurrentTarget();
        }
        _enemyAssitList = new List<EnemyAI>();
    }


    public bool getDie() => _Die;


    private void OnDrawGizmosSelected()
    {
        // alarm
        if (!IsTower)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _alarmRadius);
        }

        // assit
        if (IsCastle || IsTower) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _assistRadius);
    }
}