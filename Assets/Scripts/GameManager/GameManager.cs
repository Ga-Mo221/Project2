using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

    public int _currentDay = 1;
    public bool _night = false;
    public float _2Hours_Sec = 120;
    public int _startTimeRTS = 8;
    public int _timeRTS = 0;
    public Vector2 _playTime = Vector2.zero;

    [SerializeField] private int _coint = 0;

    [Foldout("Status")]
    public bool Tutorial = false;
    public bool TutorialWar = false;
    [Foldout("Status")]
    [SerializeField] private bool _canBuy = true;
    [Foldout("Status")]
    [Header("Game Play")]
    [SerializeField] private bool _GameOver = false;
    [Foldout("Status")]
    [SerializeField] private bool _Win = false;
    [Foldout("Status")]
    public float _displayGameOverTime = 1.5f;
    [Foldout("Status")]
    [Header("Windowns")]
    [SerializeField] private bool _canOpenWindown = true;
    [Foldout("Status")]
    [SerializeField] private bool _shopOpen = false;
    [Foldout("Status")]
    [SerializeField] private bool _upgradeOpen = false;
    [Foldout("Status")]
    public bool _canNewDay = false; // dùng để bật tắt new day

    [Foldout("Game Detail Player")]
    public int _warriorValue = 0;
    [Foldout("Game Detail Player")]
    public int _archerValue = 0;
    [Foldout("Game Detail Player")]
    public int _lancerValue = 0;
    [Foldout("Game Detail Player")]
    public int _healerValue = 0;
    [Foldout("Game Detail Player")]
    public int _tntValue = 0;

    [Foldout("Game Detail References")]
    public int _wood = 0;
    [Foldout("Game Detail References")]
    public int _rock = 0;
    [Foldout("Game Detail References")]
    public int _meat = 0;
    [Foldout("Game Detail References")]
    public int _gold = 0;

    [Foldout("Component")]
    [SerializeField] private GameUI _ui;
    [Foldout("Component")]
    [SerializeField] private DisplayPlayerDie _displayPlayerDie;
    [Foldout("Component")]
    public Upgrade Info;
    [Foldout("Component")]
    public SelectBoxItem _selectBox;

    [Foldout("Other")]
    [SerializeField] private GameObject _defen;
    [Foldout("Other")]
    [SerializeField] private GameObject _GameUI_Obj;
    [Foldout("Other")]
    [SerializeField] private MusicGameplay _audio;
    [Foldout("Other")]
    [SerializeField] private GetMainCam _UIgame_setupCamera;
    [Foldout("Other")]
    [SerializeField] private GetMainCam _UIselectBox_setupCamera;
    [Foldout("Other")]
    public AutoTrain _autoTrain;
    [Foldout("Other")]
    public Camera _cameraMiniMap;
    private GameOver _gameOver;



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
    [Foldout("Prefab")]
    public GameObject _enemy_LancerPrefab;
    [Foldout("Prefab")]
    public GameObject _enemy_OrcPrefab;
    [Foldout("Prefab")]
    public GameObject _enemy_GnollPrefab;
    [Foldout("Prefab")]
    public GameObject _enemy_FishPrefab;
    [Foldout("Prefab")]
    public GameObject _enemy_TNTRedPrefab;
    [Foldout("Prefab")]
    public GameObject _enemy_MinotaurPrefab;
    [Foldout("Prefab")]
    public GameObject _enemy_ShamanPrefab;
    [Foldout("Prefab")]
    public GameObject _healing_Buff_Brefab;
    [Foldout("Prefab")]
    public GameObject _farm_Buff_Brefab;
    [Foldout("Prefab")]
    public GameObject _fury_Buff_Brefab;

    public event Action On_RenderMap;
    public event Action On_onLight;
    public event Action On_offLight;
    public event Action On_onNight;
    public event Action On_offNight;

    private bool _changeLight = false;
    private bool _changeNight = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _gameOver = GetComponent<GameOver>();
    }

    void Start()
    {
        UIupdateReferences();

        if (gameObject.activeInHierarchy)
            StartCoroutine(setupcamera());
    }

    private IEnumerator setupcamera()
    {
        yield return new WaitForSeconds(5f);
        _UIgame_setupCamera.setupCamera();
        if (_UIselectBox_setupCamera != null)
            _UIselectBox_setupCamera.setupCamera();
    }

    void Update()
    {
        if (_timeRTS == 16 && !_changeNight)
        {
            _changeNight = !_changeNight;
            On_onNight?.Invoke();
        }
        else if (_timeRTS == 18 && !_changeLight)
        {
            _changeLight = !_changeLight;
            On_onLight?.Invoke();
        }
        else if (_timeRTS == 4 && _changeNight)
        {
            _changeNight = !_changeNight;
            On_offNight?.Invoke();
        }
        else if (_timeRTS == 6 && _changeLight)
        {
            _changeLight = !_changeLight;
            On_offLight?.Invoke();
        }
    }

    public void renderMap()
    {
        On_RenderMap?.Invoke();
        UIupdateReferences();
    }

    public void setHourRTS(int house)
        => _ui.setHouseRTS(house);

    public void createWarrior()
        => _ui.createWarrior();

    public void createArcher()
        => _ui.createArcher();

    public void createLancer()
        => _ui.createLancer();

    public void createTNT()
        => _ui.createTNT();

    public void createHealer()
        => _ui.createHealer();

    public void UIupdateReferences()
        => _ui.updateReferent();

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

    public void UIopenShop()
        => _ui.openShop();

    public void UIcloseShop()
        => _ui.closeShop();

    public void UIopenUpgradePanel()
        => _ui.openPanelUpgrade();

    public void UIcloseUpgradePanel()
        => _ui.closePanelUpgrade();

    public void UIupdateInfoUpgrade()
        => _ui.updateInfoUpgrade();

    public void UIupdateCreateUnitButton()
        => _ui.CheckLevel();

    public void addCoin(int value)
        => _coint += value;
    public int getCoin() => _coint;

    private Coroutine _offDefen;
    public void onDefen()
    {
        _defen.SetActive(true);
        if (_offDefen != null)
            StopCoroutine(_offDefen);
        _offDefen = StartCoroutine(offDefen());
    }
    private IEnumerator offDefen()
    {
        yield return new WaitForSeconds(5.3333333f);
        _defen.SetActive(false);
    }

    public void UIOpenBuidingPanel()
        => _ui.openPanelBuiding();


    public void setGameOver(bool amount)
    {
        _GameOver = amount;
        _gameOver.display();
    }
    public bool getGameOver() => _GameOver;

    public void setWin(bool amount) => _Win = amount;
    public bool getWin() => _Win;

    public bool setOpenShop()
    {
        if (_canOpenWindown)
        {
            if (Tutorial && (TutorialSetUp.Instance.ID == 2 || TutorialSetUp.Instance.ID == 3 || TutorialSetUp.Instance.ID == 4)) return false;
            _canOpenWindown = false;
            _shopOpen = true;
            return true;
        }
        return false;
    }

    public bool getCanOpenWindown() => _canOpenWindown;

    public void setCloseShop()
    {
        if (Tutorial && TutorialSetUp.Instance.ID == 5) return;
        _canOpenWindown = true;
        _shopOpen = false;
    }

    public bool getShopOpen() => _shopOpen;

    public bool setOpenUpgrade()
    {
        if (_canOpenWindown)
        {
            if (Tutorial && (TutorialSetUp.Instance.ID == 2
            || TutorialSetUp.Instance.ID == 3
            || TutorialSetUp.Instance.ID == 4
            || TutorialSetUp.Instance.ID == 5)) return false;
            _canOpenWindown = false;
            _upgradeOpen = true;
            return true;
        }
        return false;
    }

    public void setCloseUpgrade()
    {
        _canOpenWindown = true;
        _upgradeOpen = false;
    }

    public void setCanOpenWindowan(bool amount) => _canOpenWindown = amount;

    public bool getUpgradeOpen() => _upgradeOpen;


    public void UIonEnemyRespawn(bool war)
        => _ui.onEnemyRespawn(war);


    public void UIonWarning(EnemyHuoseController house)
        => _ui.onWarning(house);


    public void UIPlayerDie(UnitType type)
        => _displayPlayerDie.Add(type);


    public void setActiveGameUI(bool amount)
        => _GameUI_Obj.SetActive(amount);

    public void War()
        => _audio.PlayWarSound();

    public void StopSoundMusic()
        => _audio.StopAudio();
} 
