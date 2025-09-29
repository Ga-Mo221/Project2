using NaughtyAttributes;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

    public int _currentDay = 1;
    public bool _night = false;
    public float _4Hours_Sec = 120;
    public int _timeRTS = 0;
    public Vector2 _playTime = Vector2.zero;

    [SerializeField] private bool _canBuy = true;

    [SerializeField] private GameUI _ui;
    [SerializeField] public Upgrade Info;


    [Foldout("Prefab")]
    public GameObject _warriorPrefab;
    [Foldout("Prefab")]
    public GameObject _ArcherPrefab;
    [Foldout("Prefab")]
    public GameObject _LancerPrefab;
    [Foldout("Prefab")]
    public GameObject _TNTPrefab;
    [Foldout("Prefab")]
    public GameObject _HealerPrefab;
    [Foldout("Prefab")]
    public GameObject _createPrefab;
    [Foldout("Prefab")]
    public GameObject _TowerPrefab;
    [Foldout("Prefab")]
    public GameObject _StoragePrefab;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        UIupdateReferences();
        UIupdatePlayerValue();
    }

    public void UIupdateReferences()
        => _ui.updateReferent();

    public void UIupdatePlayerValue()
        => _ui.updatePlayerValue();

    public void UIloadPlayer()
        => _ui.loadPlayer();

    public void UIsetActiveButtonUpgrade(bool amount)
        => _ui.setActiveButtonUpgrade(amount);

    public void setCanBuy(bool amount) => _canBuy = amount;
    public bool getCanBuy() => _canBuy;

    public void UIcheckButtonBuyBuiding()
        => _ui.checkButtonBuyTowerAndStorage();

    public void UIupdateHPCastle()
        => _ui.updateHP();
}
