using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    #region TakBar
    [Foldout("TakBar")]
    // Time
    [SerializeField] private TextMeshProUGUI _Time;
    private float _playTime = 0;
    // references
    [Foldout("TakBar")]
    [SerializeField] private GameObject _Takbar_Obj;
    [Foldout("TakBar")]
    [SerializeField] private TextMeshProUGUI _currentwood;
    [Foldout("TakBar")]
    [SerializeField] private TextMeshProUGUI _maxWood;
    [Foldout("TakBar")]
    [SerializeField] private TextMeshProUGUI _currentrock;
    [Foldout("TakBar")]
    [SerializeField] private TextMeshProUGUI _maxRock;
    [Foldout("TakBar")]
    [SerializeField] private TextMeshProUGUI _currentmeat;
    [Foldout("TakBar")]
    [SerializeField] private TextMeshProUGUI _maxMeat;
    [Foldout("TakBar")]
    [SerializeField] private TextMeshProUGUI _currentgold;
    [Foldout("TakBar")]
    [SerializeField] private TextMeshProUGUI _maxGold;
    #endregion


    #region Mini Map
    [Foldout("Mini Map")]
    [SerializeField] private GameObject _MiniMap_Obj;
    [Foldout("Mini Map")]
    [SerializeField] private TextMeshProUGUI _Day;
    [Foldout("Mini Map")]
    [SerializeField] private TextMeshProUGUI _Time_RTS;
    [Foldout("Mini Map")]
    [SerializeField] private GameObject _Sun;
    [Foldout("Mini Map")]
    [SerializeField] private GameObject _Night;
    private float _timeAccumulator = 0f; // tích lũy thời gian để tăng giờ RTS
    private int _hours = 0; // bắt đầu 8h sáng

    // aotu train
    //private bool _isAutoTraining = false;
    #endregion


    #region HP Bar
    [Foldout("HP Bar")]
    [SerializeField] private GameObject _HPBar_Obj;
    [Foldout("HP Bar")]
    [SerializeField] private TextMeshProUGUI _HP_Text;
    [Foldout("HP Bar")]
    [SerializeField] private Image _HPBar;
    [Foldout("HP Bar")]
    [SerializeField] private TextMeshProUGUI _HP_Warrior_Value;
    [Foldout("HP Bar")]
    [SerializeField] private TextMeshProUGUI _HP_Archer_Value;
    [Foldout("HP Bar")]
    [SerializeField] private TextMeshProUGUI _HP_Lancer_Value;
    [Foldout("HP Bar")]
    [SerializeField] private TextMeshProUGUI _HP_Healer_Value;
    [Foldout("HP Bar")]
    [SerializeField] private TextMeshProUGUI _HP_TNT_Value;
    #endregion

    #region Buff
    [Foldout("Buff")]
    [SerializeField] private GameObject _Buff;
    #endregion


    #region Shop
    [Foldout("Shop")]
    [SerializeField] private CanvasGroup _gr;
    [Foldout("Shop")]
    // Warrior
    [SerializeField] private TextMeshProUGUI _wood_New_Create_W;
    // Archer
    [Foldout("Shop")]
    [SerializeField] private TextMeshProUGUI _wood_New_Create_A;
    [Foldout("Shop")]
    [SerializeField] private TextMeshProUGUI _rock_New_Create_A;
    // Lancer
    [Foldout("Shop")]
    [SerializeField] private TextMeshProUGUI _wood_New_Create_L;
    [Foldout("Shop")]
    [SerializeField] private TextMeshProUGUI _rock_New_Create_L;
    [Foldout("Shop")]
    [SerializeField] private TextMeshProUGUI _meat_New_Create_L;
    // TNT
    [Foldout("Shop")]
    [SerializeField] private TextMeshProUGUI _rock_New_Create_T;
    [Foldout("Shop")]
    [SerializeField] private TextMeshProUGUI _gold_New_Create_T;
    [Foldout("Shop")]
    [SerializeField] private TextMeshProUGUI _meat_New_Create_T;
    // Healer
    [Foldout("Shop")]
    [SerializeField] private TextMeshProUGUI _wood_New_Create_H;
    [Foldout("Shop")]
    [SerializeField] private TextMeshProUGUI _rock_New_Create_H;
    [Foldout("Shop")]
    [SerializeField] private TextMeshProUGUI _meat_New_Create_H;
    [Foldout("Shop")]
    [SerializeField] private TextMeshProUGUI _gold_New_Create_H;
    // Slot
    [Foldout("Shop")]
    [SerializeField] private TextMeshProUGUI _currentslot;
    [Foldout("Shop")]
    [SerializeField] private TextMeshProUGUI _maxSlot;
    [Foldout("Shop")]
    [SerializeField] private Button _WarriorButton;
    [Foldout("Shop")]
    [SerializeField] private Button _ArcherButton;
    [Foldout("Shop")]
    [SerializeField] private Button _LancerButton;
    [Foldout("Shop")]
    [SerializeField] private Button _TNTButton;
    [Foldout("Shop")]
    [SerializeField] private Button _HealerButton;
    [Foldout("Shop")]
    [SerializeField] private Transform _loadingPos;

    [Foldout("Shop")]
    [SerializeField] private TextMeshProUGUI _warriorSlot;
    [Foldout("Shop")]
    [SerializeField] private TextMeshProUGUI _archerSlot;
    [Foldout("Shop")]
    [SerializeField] private TextMeshProUGUI _lancerSlot;
    [Foldout("Shop")]
    [SerializeField] private TextMeshProUGUI _tntSlot;
    [Foldout("Shop")]
    [SerializeField] private TextMeshProUGUI _healerSlot;
    [Foldout("Shop")]
    [SerializeField] private List<GameObject> _createList = new List<GameObject>();
    [Foldout("Shop")]
    [SerializeField] private List<GameObject> _PlayerCreate = new List<GameObject>();
    #endregion


    #region Buiding
    [Foldout("Buiding")]
    [SerializeField] private GameObject _Buiding_Obj;
    [Foldout("Buiding")]
    [SerializeField] private HideUI _buiding_hide;
    [Foldout("Buiding")]
    [SerializeField] private Button _buttonTower;
    [Foldout("Buiding")]
    [SerializeField] private TextMeshProUGUI _wood_Tower;
    [Foldout("Buiding")]
    [SerializeField] private TextMeshProUGUI _rock_Tower;
    [Foldout("Buiding")]
    [SerializeField] private TextMeshProUGUI _gold_Tower;
    [Foldout("Buiding")]
    [SerializeField] private Button _buttonStorage;
    [Foldout("Buiding")]
    [SerializeField] private TextMeshProUGUI _wood_Storage;
    [Foldout("Buiding")]
    [SerializeField] private TextMeshProUGUI _rock_Storage;
    [Foldout("Buiding")]
    [SerializeField] private TextMeshProUGUI _gold_Storage;
    #endregion

    #region Tutorial
    [Foldout("Tutorial")]
    [SerializeField] private GameObject _Tutorial_Obj;
    #endregion


    #region Upgrade
    [Foldout("Upgrade")]
    [SerializeField] private GameObject _UpgradePanel;
    [Foldout("Upgrade")]
    [SerializeField] private GameObject _ButtonUpgrade;
    [Foldout("Upgrade")]
    [SerializeField] private TextMeshProUGUI _buttonUpgrade_Wood;
    [Foldout("Upgrade")]
    [SerializeField] private TextMeshProUGUI _buttonUpgrade_Rock;
    [Foldout("Upgrade")]
    [SerializeField] private TextMeshProUGUI _buttonUpgrade_Gold;
    [Foldout("Upgrade")]
    [SerializeField] private TextMeshProUGUI _PanelUpgrade_Level;
    [Foldout("Upgrade")]
    [SerializeField] private TextMeshProUGUI _PanelUpgrade_LevelUpgrade;
    [Foldout("Upgrade")]
    [SerializeField] private TextMeshProUGUI _PanelUpgrade_Slot;
    [Foldout("Upgrade")]
    [SerializeField] private TextMeshProUGUI _PanelUpgrade_Health;
    [Foldout("Upgrade")]
    [SerializeField] private TextMeshProUGUI _PanelUpgrade_Wood;
    [Foldout("Upgrade")]
    [SerializeField] private TextMeshProUGUI _PanelUpgrade_Rock;
    [Foldout("Upgrade")]
    [SerializeField] private TextMeshProUGUI _PanelUpgrade_Gold;
    [Foldout("Upgrade")]
    [SerializeField] private Button _PanelButtonUpgrade;
    #endregion


    #region NewDay
    [Foldout("New Day")]
    [SerializeField] private GameObject _newday;
    [Foldout("New Day")]
    [SerializeField] private TextMeshProUGUI _textNewDay;
    #endregion


    #region Enemy Respawn
    [Foldout("Enemy Respawn")]
    [SerializeField] private GameObject _enemyRespawn;
    [Foldout("Enemy Respawn")]
    [SerializeField] private TextMeshProUGUI _textEnemyRespawn;
    #endregion


    #region Warning
    [Foldout("Warning")]
    [SerializeField] private GameObject _warning;
    [Foldout("Warning")]
    [SerializeField] private TextMeshProUGUI _textWarning2;
    #endregion

    [Foldout("Exit Game")]
    [SerializeField] private GameObject _exitgameimg;

    [Foldout("In Game")]
    [SerializeField] private GameObject _inGame;

    
    private Dictionary<string, Button> _unitButtons;


    private void Awake()
    {
        _unitButtons = new Dictionary<string, Button>()
        {
            { "Warrior", _WarriorButton },
            { "Archer", _ArcherButton },
            { "Lancer", _LancerButton },
            { "TNT", _TNTButton },
            { "Healer", _HealerButton },
        };
    }

    void Start()
    {
        if (!GameManager.Instance.Tutorial)
        {
            _inGame.SetActive(true);
            StartCoroutine(startGameDelay());
        }
        _hours = GameManager.Instance._startTimeRTS;
        updateBuidingReference();
        updateHP();

        InvokeRepeating(nameof(updatePlayerValue), 0f, 1f);
        //InvokeRepeating(nameof(autoTrain), 0f, 3f);
    }

    private IEnumerator startGameDelay()
    {
        yield return new WaitForSeconds(2f);
        if (LoadingSceneController.Instance != null)
        {
            LoadingSceneController.Instance.NotifyGameReadyComplete();
        }
    }

    #region Update
    void Update()
    {
        if (GameManager.Instance == null) return;
        string key = "";
        string txt = "";
        // Tăng thời gian chơi
        if (GameManager.Instance != null && !GameManager.Instance.Tutorial)
            _timeAccumulator += Time.deltaTime;

        _playTime += Time.deltaTime;

        // Cập nhật PlayTime hiển thị 00:00
        int playMinutes = Mathf.FloorToInt(_playTime / 60f);
        int playSeconds = Mathf.FloorToInt(_playTime % 60f);
        _Time.text = string.Format("{0:00}:{1:00}", playMinutes, playSeconds);

        
        GameManager.Instance._playTime = new Vector2(playMinutes, playSeconds);
        // Mỗi 120 giây playtime = tăng 4 giờ RTS
        if (_timeAccumulator >= GameManager.Instance._2Hours_Sec)
        {
            _hours += 2;
            _timeAccumulator = 0f;
        }

        // Nếu quá 24h → sang ngày mới
        if (_hours >= 24)
        {
            _hours = 0;
            GameManager.Instance._currentDay++;
        }
        GameManager.Instance._timeRTS = _hours;
        if (_hours == 20)
        {
            _Night.SetActive(true);
            _Sun.SetActive(false);
            if (GameManager.Instance._canNewDay)
                GameManager.Instance._canNewDay = false;
        }
        if (_hours == 8)
        {
            _Night.SetActive(false);
            _Sun.SetActive(true);
            if (!GameManager.Instance._canNewDay)
            {
                key = "ui.NewDay.Day";
                txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";

                GameManager.Instance._canNewDay = true;
                _textNewDay.text = txt + GameManager.Instance._currentDay;
                if (gameObject.activeInHierarchy)
                    StartCoroutine(displayNewDay());
            }
        }

        // Kiểm tra Night
        if (_hours >= 0 && _hours < 8)
            GameManager.Instance._night = true;
        else
            GameManager.Instance._night = false;

        // Hiển thị Day và Time RTS
        key = "ui.minimap.Day";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        _Day.text = txt + GameManager.Instance._currentDay;
        _Time_RTS.text = string.Format("{0:00}:{1:00}", _hours, 0);
    }
    #endregion


    private IEnumerator displayNewDay()
    {
        yield return new WaitForSeconds(5f);
        _newday.SetActive(true);
    }


    #region Set House
    public void setHouseRTS(int hours)
        => _hours = hours;
    #endregion


    #region On All WinDown
    public void OnAllWinDown()
    {
        _Takbar_Obj.SetActive(true);
        _MiniMap_Obj.SetActive(true);
        _Buiding_Obj.SetActive(true);
        _Tutorial_Obj.SetActive(true);
        _HPBar_Obj.SetActive(true);
        _Buff.SetActive(true);
        GameManager.Instance.setCanOpenWindowan(true);
    }
    #endregion

    #region Off All WinDown
    public void OffAllWinDown()
    {
        closePanelUpgrade();
        closeShop();
        setActiveButtonUpgrade(false);
        GameManager.Instance._selectBox.gameObject.SetActive(false);
        _Takbar_Obj.SetActive(false);
        _MiniMap_Obj.SetActive(false);
        _Buiding_Obj.SetActive(false);
        _Tutorial_Obj.SetActive(false);
        _Buff.SetActive(false);
        _HPBar_Obj.SetActive(false);
        GameManager.Instance.setCanOpenWindowan(false);
    }
    #endregion


    #region Update HP Castle
    public void updateHP()
    {
        if (Castle.Instance == null) return;
        float currentHealt = Castle.Instance._currentHealth;
        float maxHealth = Castle.Instance._maxHealth;
        _HPBar.fillAmount = currentHealt / maxHealth;
        _HP_Text.text = currentHealt + " / " + maxHealth;
    }
    #endregion


    #region Update References
    public void updateReferent()
    {
        if (Castle.Instance == null) return;
        _currentwood.text = Castle.Instance._wood.ToString();
        _currentrock.text = Castle.Instance._rock.ToString();
        _currentmeat.text = Castle.Instance._meat.ToString();
        _currentgold.text = Castle.Instance._gold.ToString();
        _maxWood.text = " /" + Castle.Instance._maxWood.ToString();
        _maxRock.text = " /" + Castle.Instance._maxRock.ToString();
        _maxMeat.text = " /" + Castle.Instance._maxMeat.ToString();
        _maxGold.text = " /" + Castle.Instance._maxGold.ToString();
    }
    #endregion


    #region Update Player Value
    private void updatePlayerValue()
    {
        int WarriorVulue = 0;
        int ArcherVulue = 0;
        int LancerVulue = 0;
        int HealerVulue = 0;
        int TNTVulue = 0;

        if (Castle.Instance != null)
        {
            foreach (var p in Castle.Instance._ListWarrior)
            {
                if (p.gameObject.activeSelf)
                    WarriorVulue++;
            }
            foreach (var p in Castle.Instance._ListArcher)
            {
                if (p.gameObject.activeSelf || p.getUpTower())
                    ArcherVulue++;
            }
            foreach (var p in Castle.Instance._ListLancer)
            {
                if (p.gameObject.activeSelf)
                    LancerVulue++;
            }
            foreach (var p in Castle.Instance._ListHealer)
            {
                if (p.gameObject.activeSelf)
                    HealerVulue++;
            }
            foreach (var p in Castle.Instance._ListTNT)
            {
                if (p.gameObject.activeSelf)
                    TNTVulue++;
            }
        }

        _HP_Warrior_Value.text = WarriorVulue.ToString();
        _HP_Archer_Value.text = ArcherVulue.ToString();
        _HP_Lancer_Value.text = LancerVulue.ToString();
        _HP_Healer_Value.text = HealerVulue.ToString();
        _HP_TNT_Value.text = TNTVulue.ToString();

        updateSlot();
    }
    #endregion


    #region Update Slot
    private void updateSlot()
    {
        if (Castle.Instance == null) return;
        Castle.Instance._currentSlot = 0;
        foreach (var p in Castle.Instance._ListWarrior)
        {
            if (p.gameObject.activeSelf || p.getCreating())
                Castle.Instance._currentSlot += p._slot;
        }
        foreach (var p in Castle.Instance._ListArcher)
        {
            if (p.gameObject.activeSelf || p.getCreating() || p.getUpTower())
                Castle.Instance._currentSlot += p._slot;
        }
        foreach (var p in Castle.Instance._ListLancer)
        {
            if (p.gameObject.activeSelf || p.getCreating())
                Castle.Instance._currentSlot += p._slot;
        }
        foreach (var p in Castle.Instance._ListHealer)
        {
            if (p.gameObject.activeSelf || p.getCreating())
                Castle.Instance._currentSlot += p._slot;
        }
        foreach (var p in Castle.Instance._ListTNT)
        {
            if (p.gameObject.activeSelf || p.getCreating())
                Castle.Instance._currentSlot += p._slot;
        }

        _currentslot.text = Castle.Instance._currentSlot.ToString();
        _maxSlot.text = Castle.Instance._maxSlot.ToString();
    }
    #endregion


    #region Open Shop
    public void openShop()
    {
        if (GameManager.Instance.Tutorial && TutorialSetUp.Instance.ID == 5)
            TutorialSetUp.Instance.TutorialCreateArcher();
        if (!GameManager.Instance.setOpenShop()) return;
        GameManager.Instance._selectBox.gameObject.SetActive(false);
        _gr.alpha = 1;
        _gr.interactable = true;
        _gr.blocksRaycasts = true;


        // warrior
        _wood_New_Create_W.text = GameManager.Instance.Info._wood_Warrior.ToString();
        // Archer
        _wood_New_Create_A.text = GameManager.Instance.Info._wood_Archer.ToString();
        _rock_New_Create_A.text = GameManager.Instance.Info._rock_Archer.ToString();
        // Lancer
        _wood_New_Create_L.text = GameManager.Instance.Info._wood_Lancer.ToString();
        _rock_New_Create_L.text = GameManager.Instance.Info._rock_Lancer.ToString();
        _meat_New_Create_L.text = GameManager.Instance.Info._meat_Lancer.ToString();
        // TNT
        _rock_New_Create_T.text = GameManager.Instance.Info._rock_TNT.ToString();
        _meat_New_Create_T.text = GameManager.Instance.Info._meat_TNT.ToString();
        _gold_New_Create_T.text = GameManager.Instance.Info._gold_TNT.ToString();
        // Healer
        _wood_New_Create_H.text = GameManager.Instance.Info._wood_Healer.ToString();
        _rock_New_Create_H.text = GameManager.Instance.Info._rock_Healer.ToString();
        _meat_New_Create_H.text = GameManager.Instance.Info._meat_Healer.ToString();
        _gold_New_Create_H.text = GameManager.Instance.Info._gold_Healer.ToString();

        updateSlot();
        CheckLevel();
        loadPlayer();
        GameManager.Instance._autoTrain.changemode(false);
    }
    #endregion


    #region Close Shop
    public void closeShop()
    {
        if (GameManager.Instance.Tutorial && TutorialSetUp.Instance.ID == 5) return;
        if (GameManager.Instance.Tutorial && TutorialSetUp.Instance.ID == 2 && !TutorialSetUp.Instance._openNhatKy) return;
        if (GameManager.Instance.Tutorial && TutorialSetUp.Instance.ID == 2)
            TutorialSetUp.Instance.TutorialOpenButtonUpgrade();
        GameManager.Instance.setCloseShop();
        _gr.alpha = 0;
        _gr.interactable = false;
        _gr.blocksRaycasts = false;
    }
    #endregion


    #region Open Panel Upgrade
    public void openPanelUpgrade()
    {
        if (!GameManager.Instance.setOpenUpgrade()) return;
        if (GameManager.Instance.Tutorial && TutorialSetUp.Instance.ID == 6)
            TutorialSetUp.Instance.TutorialUpgradeAndReferencesAndSlot();
        GameManager.Instance._selectBox.gameObject.SetActive(false);
        updateInfoUpgrade();
        _UpgradePanel.SetActive(true);
        GameManager.Instance._autoTrain.changemode(false);
    }
    #endregion


    #region Close Panel Upgrade
    public void closePanelUpgrade()
    {
        if (GameManager.Instance.Tutorial && TutorialSetUp.Instance.ID < 7) return;
        if (GameManager.Instance.Tutorial && TutorialSetUp.Instance.ID == 8)
            TutorialSetUp.Instance.TutorialArcherInCastle();
        GameManager.Instance.setCloseUpgrade();
        _UpgradePanel.SetActive(false);
    }
    #endregion


    #region Open Setting
    public void OpenSetting()
    {
        if (SettingManager.Instance == null) return;
        OffAllWinDown();
        SettingManager.Instance.OpenSetting();
    }
    #endregion

    #region Close Setting
    private void OnEnable()
    {
        if (SettingManager.Instance != null)
        {
            SettingManager.Instance._onCloseSetting += CloseSetting;
            SettingManager.Instance._onExitGame += exitGame;
        }
    }

    private void OnDisable()
    {
        // 4. Hủy đăng ký (unsubscribe)
        if (SettingManager.Instance != null)
        {
            SettingManager.Instance._onCloseSetting -= CloseSetting;
            SettingManager.Instance._onExitGame -= exitGame;
        }
    }

    private void exitGame()
    {
        _exitgameimg.SetActive(true);
    }

    private void CloseSetting()
    {
        OnAllWinDown();
    }
    #endregion


    #region Check Level
    public void CheckLevel()
    {
        if (Castle.Instance == null) return;

        int level = Castle.Instance._level;

        // Reset hết nút
        foreach (var kv in _unitButtons)
            kv.Value.interactable = false;

        // Luôn mở cho Warrior
        int slot = GameManager.Instance._warriorPrefab.GetComponent<PlayerAI>()._slot;
        _warriorSlot.text = slot.ToString();
        _unitButtons["Warrior"].interactable = CanCreate(GameManager.Instance.Info._wood_Warrior, Castle.Instance._wood)
            && Castle.Instance._maxSlot - Castle.Instance._currentSlot >= slot;

        if (level >= 2)
        {
            int sl = GameManager.Instance._ArcherPrefab.GetComponent<PlayerAI>()._slot;
            _archerSlot.text = sl.ToString();
            _unitButtons["Archer"].interactable =
                CanCreate(GameManager.Instance.Info._wood_Archer, Castle.Instance._wood) &&
                CanCreate(GameManager.Instance.Info._rock_Archer, Castle.Instance._rock) &&
                Castle.Instance._maxSlot - Castle.Instance._currentSlot >= sl;
        }

        if (level >= 3)
        {
            int sl = GameManager.Instance._LancerPrefab.GetComponent<PlayerAI>()._slot;
            _lancerSlot.text = sl.ToString();
            _unitButtons["Lancer"].interactable =
                CanCreate(GameManager.Instance.Info._wood_Lancer, Castle.Instance._wood) &&
                CanCreate(GameManager.Instance.Info._rock_Lancer, Castle.Instance._rock) &&
                CanCreate(GameManager.Instance.Info._meat_Lancer, Castle.Instance._meat) &&
                Castle.Instance._maxSlot - Castle.Instance._currentSlot >= sl;
        }

        if (level >= 4)
        {
            int sl = GameManager.Instance._TNTPrefab.GetComponent<PlayerAI>()._slot;
            _tntSlot.text = sl.ToString();
            _unitButtons["TNT"].interactable =
                CanCreate(GameManager.Instance.Info._rock_TNT, Castle.Instance._rock) &&
                CanCreate(GameManager.Instance.Info._meat_TNT, Castle.Instance._meat) &&
                CanCreate(GameManager.Instance.Info._gold_TNT, Castle.Instance._gold) &&
                Castle.Instance._maxSlot - Castle.Instance._currentSlot >= sl;
        }

        if (level >= 5)
        {
            int sl = GameManager.Instance._HealerPrefab.GetComponent<PlayerAI>()._slot;
            _healerSlot.text = sl.ToString();
            _unitButtons["Healer"].interactable =
                CanCreate(GameManager.Instance.Info._wood_Healer, Castle.Instance._wood) &&
                CanCreate(GameManager.Instance.Info._rock_Healer, Castle.Instance._rock) &&
                CanCreate(GameManager.Instance.Info._meat_Healer, Castle.Instance._meat) &&
                CanCreate(GameManager.Instance.Info._gold_Healer, Castle.Instance._gold) &&
                Castle.Instance._maxSlot - Castle.Instance._currentSlot >= sl;
        }
    }
    private bool CanCreate(int cost, int currentValue)
    {
        return currentValue >= cost; // sửa >= thay vì >
    }
    #endregion


    #region Warrior
    public void createWarrior()
    {
        if (Castle.Instance == null) return;
        if (GameManager.Instance.Tutorial) return;

        Castle.Instance._wood -= GameManager.Instance.Info._wood_Warrior;
        updateReferent();
        // tao player
        GameObject _obj = null;
        PlayerAI _scripPlayer = null;
        bool _isHas = false;
        foreach (var player in Castle.Instance._ListWarrior)
        {
            if (!player.getCreating() && !player.gameObject.activeSelf && player.getDie())
            {
                _obj = player.gameObject;
                _scripPlayer = player;
                player.setCreating(true);
                player.respawn(Castle.Instance._In_Castle_Pos);
                _isHas = true;
                break;
            }
        }
        if (!_isHas)
        {
            _obj = Instantiate(GameManager.Instance._warriorPrefab, Castle.Instance._In_Castle_Pos.position, Quaternion.identity, Castle.Instance._PlayerFolder);
            _obj.SetActive(false);
            _obj.name = $"Warrior {Castle.Instance._ListWarrior.Count}";
            _scripPlayer = _obj.GetComponent<PlayerAI>();
            _scripPlayer.setCreating(true);
            Castle.Instance._ListWarrior.Add(_scripPlayer);
        }
        if (_scripPlayer != null)
            _scripPlayer.upLevel(Castle.Instance._level);

        // check
        updatePlayerValue();
        CheckLevel();

        // kiểm tra đã có load chưa
        bool _has = false;
        foreach (var _objcreate in _createList)
        {
            var _scripCreate = _objcreate.GetComponent<LoadCreate>();
            bool _on = _objcreate.activeSelf;
            if (_on && _obj && _scripCreate != null && _scripPlayer != null && _scripCreate._unitClass == _scripPlayer._unitClass)
            {
                _scripCreate.add(_obj);
                _has = true;
            }
        }
        if (!_has)
        {
            foreach (var _objcreate in _createList)
            {
                var _scripCreate = _objcreate.GetComponent<LoadCreate>();
                bool _on = _objcreate.activeSelf;
                if (!_on)
                {
                    _scripCreate.resetValue();
                    _scripCreate.add(_obj);
                    _objcreate.SetActive(true);
                    return;
                }
            }
        }
    }
    #endregion


    #region Archer
    public void createArcher()
    {
        if (Castle.Instance == null) return;
        if (GameManager.Instance.Tutorial && TutorialSetUp.Instance._CreateArcher) return;
        if (GameManager.Instance.Tutorial && TutorialSetUp.Instance.ID == 5)
            TutorialSetUp.Instance.TutorialCreateTimeAddSlotPlayer();
        Castle.Instance._wood -= GameManager.Instance.Info._wood_Archer;
        Castle.Instance._rock -= GameManager.Instance.Info._rock_Archer;
        updateReferent();
        // tao player
        GameObject _obj = null;
        PlayerAI _scripPlayer = null;
        bool _isHas = false;
        foreach (var player in Castle.Instance._ListArcher)
        {
            if (!player.getCreating() && !player.gameObject.activeSelf && player.getDie())
            {
                _obj = player.gameObject;
                _scripPlayer = player;
                player.setCreating(true);
                player.respawn(Castle.Instance._In_Castle_Pos);
                _isHas = true;
                break;
            }
        }
        if (!_isHas)
        {
            _obj = Instantiate(GameManager.Instance._ArcherPrefab, Castle.Instance._In_Castle_Pos.position, Quaternion.identity, Castle.Instance._PlayerFolder);
            _obj.SetActive(false);
            _obj.name = $"Archer {Castle.Instance._ListWarrior.Count}";
            _scripPlayer = _obj.GetComponent<PlayerAI>();
            _scripPlayer.setCreating(true);
            Castle.Instance._ListArcher.Add(_scripPlayer);
        }
        if (_scripPlayer != null)
            _scripPlayer.upLevel(Castle.Instance._level - 1);
        // check
        updatePlayerValue();
        CheckLevel();

        // kiểm tra đã có load chưa
        bool _has = false;
        foreach (var _objcreate in _createList)
        {
            var _scripCreate = _objcreate.GetComponent<LoadCreate>();
            bool _on = _objcreate.activeSelf;
            if (_on && _obj && _scripCreate != null && _scripPlayer != null && _scripCreate._unitClass == _scripPlayer._unitClass)
            {
                _scripCreate.add(_obj);
                _has = true;
            }
        }
        if (!_has)
        {
            foreach (var _objcreate in _createList)
            {
                var _scripCreate = _objcreate.GetComponent<LoadCreate>();
                bool _on = _objcreate.activeSelf;
                if (!_on)
                {
                    _scripCreate.resetValue();
                    _scripCreate.add(_obj);
                    _objcreate.SetActive(true);
                    return;
                }
            }
        }
    }
    #endregion


    #region Lancer
    public void createLancer()
    {
        if (Castle.Instance == null) return;
        Castle.Instance._wood -= GameManager.Instance.Info._wood_Lancer;
        Castle.Instance._rock -= GameManager.Instance.Info._rock_Lancer;
        Castle.Instance._meat -= GameManager.Instance.Info._meat_Lancer;
        updateReferent();
        // tao player
        GameObject _obj = null;
        PlayerAI _scripPlayer = null;
        bool _isHas = false;
        foreach (var player in Castle.Instance._ListLancer)
        {
            if (!player.getCreating() && !player.gameObject.activeSelf && player.getDie())
            {
                _obj = player.gameObject;
                _scripPlayer = player;
                player.setCreating(true);
                player.respawn(Castle.Instance._In_Castle_Pos);
                _isHas = true;
                break;
            }
        }
        if (!_isHas)
        {
            _obj = Instantiate(GameManager.Instance._LancerPrefab, Castle.Instance._In_Castle_Pos.position, Quaternion.identity, Castle.Instance._PlayerFolder);
            _obj.SetActive(false);
            _obj.name = $"Lancer {Castle.Instance._ListWarrior.Count}";
            _scripPlayer = _obj.GetComponent<PlayerAI>();
            _scripPlayer.setCreating(true);
            Castle.Instance._ListLancer.Add(_scripPlayer);
        }
        _scripPlayer.upLevel(Castle.Instance._level - 2);

        // check
        updatePlayerValue();
        CheckLevel();

        // kiểm tra đã có load chưa
        bool _has = false;
        foreach (var _objcreate in _createList)
        {
            var _scripCreate = _objcreate.GetComponent<LoadCreate>();
            bool _on = _objcreate.activeSelf;
            if (_on && _obj && _scripCreate != null && _scripPlayer != null && _scripCreate._unitClass == _scripPlayer._unitClass)
            {
                _scripCreate.add(_obj);
                _has = true;
            }
        }
        if (!_has)
        {
            foreach (var _objcreate in _createList)
            {
                var _scripCreate = _objcreate.GetComponent<LoadCreate>();
                bool _on = _objcreate.activeSelf;
                if (!_on)
                {
                    _scripCreate.resetValue();
                    _scripCreate.add(_obj);
                    _objcreate.SetActive(true);
                    return;
                }
            }
        }
    }
    #endregion


    #region TNT
    public void createTNT()
    {
        if (Castle.Instance == null) return;
        Castle.Instance._rock -= GameManager.Instance.Info._rock_TNT;
        Castle.Instance._meat -= GameManager.Instance.Info._meat_TNT;
        Castle.Instance._gold -= GameManager.Instance.Info._gold_TNT;
        updateReferent();
        // tao player
        GameObject _obj = null;
        PlayerAI _scripPlayer = null;
        bool _isHas = false;
        foreach (var player in Castle.Instance._ListTNT)
        {
            if (!player.getCreating() && !player.gameObject.activeSelf && player.getDie())
            {
                _obj = player.gameObject;
                _scripPlayer = player;
                player.setCreating(true);
                player.respawn(Castle.Instance._In_Castle_Pos);
                _isHas = true;
                break;
            }
        }
        if (!_isHas)
        {
            _obj = Instantiate(GameManager.Instance._TNTPrefab, Castle.Instance._In_Castle_Pos.position, Quaternion.identity, Castle.Instance._PlayerFolder);
            _obj.SetActive(false);
            _obj.name = $"TNT {Castle.Instance._ListWarrior.Count}";
            _scripPlayer = _obj.GetComponent<PlayerAI>();
            _scripPlayer.setCreating(true);
            Castle.Instance._ListTNT.Add(_scripPlayer);
        }
        _scripPlayer.upLevel(Castle.Instance._level - 3);

        // check
        updatePlayerValue();
        CheckLevel();

        // kiểm tra đã có load chưa
        bool _has = false;
        foreach (var _objcreate in _createList)
        {
            var _scripCreate = _objcreate.GetComponent<LoadCreate>();
            bool _on = _objcreate.activeSelf;
            if (_on && _obj && _scripCreate != null && _scripPlayer != null && _scripCreate._unitClass == _scripPlayer._unitClass)
            {
                _scripCreate.add(_obj);
                _has = true;
            }
        }
        if (!_has)
        {
            foreach (var _objcreate in _createList)
            {
                var _scripCreate = _objcreate.GetComponent<LoadCreate>();
                bool _on = _objcreate.activeSelf;
                if (!_on)
                {
                    _scripCreate.resetValue();
                    _scripCreate.add(_obj);
                    _objcreate.SetActive(true);
                    return;
                }
            }
        }
    }
    #endregion


    #region Healer
    public void createHealer()
    {
        if (Castle.Instance == null) return;
        Castle.Instance._wood -= GameManager.Instance.Info._wood_Healer;
        Castle.Instance._rock -= GameManager.Instance.Info._rock_Healer;
        Castle.Instance._meat -= GameManager.Instance.Info._meat_Healer;
        Castle.Instance._gold -= GameManager.Instance.Info._gold_Healer;
        updateReferent();
        // tao player
        GameObject _obj = null;
        PlayerAI _scripPlayer = null;
        bool _isHas = false;
        foreach (var player in Castle.Instance._ListHealer)
        {
            if (!player.getCreating() && !player.gameObject.activeSelf && player.getDie())
            {
                _obj = player.gameObject;
                _scripPlayer = player;
                player.setCreating(true);
                player.respawn(Castle.Instance._In_Castle_Pos);
                _isHas = true;
                break;
            }
        }
        if (!_isHas)
        {
            _obj = Instantiate(GameManager.Instance._HealerPrefab, Castle.Instance._In_Castle_Pos.position, Quaternion.identity, Castle.Instance._PlayerFolder);
            _obj.SetActive(false);
            _obj.name = $"Healer {Castle.Instance._ListWarrior.Count}";
            _scripPlayer = _obj.GetComponent<PlayerAI>();
            _scripPlayer.setCreating(true);
            Castle.Instance._ListHealer.Add(_scripPlayer);
        }

        // check
        updatePlayerValue();
        CheckLevel();

        // kiểm tra đã có load chưa
        bool _has = false;
        foreach (var _objcreate in _createList)
        {
            var _scripCreate = _objcreate.GetComponent<LoadCreate>();
            bool _on = _objcreate.activeSelf;
            if (_on && _obj && _scripCreate != null && _scripPlayer != null && _scripCreate._unitClass == _scripPlayer._unitClass)
            {
                _scripCreate.add(_obj);
                _has = true;
            }
        }
        if (!_has)
        {
            foreach (var _objcreate in _createList)
            {
                var _scripCreate = _objcreate.GetComponent<LoadCreate>();
                bool _on = _objcreate.activeSelf;
                if (!_on)
                {
                    _scripCreate.resetValue();
                    _scripCreate.add(_obj);
                    _objcreate.SetActive(true);
                    return;
                }
            }
        }
    }
    #endregion


    #region Load Player
    public void loadPlayer()
    {
        if (Castle.Instance == null) return;

        bool foundW = false;
        LoadCreate scripW = null;
        bool foundA = false;
        LoadCreate scripA = null;
        bool foundL = false;
        LoadCreate scripL = null;
        bool foundT = false;
        LoadCreate scripT = null;
        bool foundH = false;
        LoadCreate scripH = null;


        // Tìm một Warrior đang active trước
        foreach (var hit in _PlayerCreate)
        {
            if (hit.activeSelf)
            {
                var scripCreate = hit.GetComponent<LoadCreate>();
                if (scripCreate._unitClass == UnitType.Warrior)
                {
                    foundW = true;
                    scripW = scripCreate;
                }
                if (scripCreate._unitClass == UnitType.Archer)
                {
                    foundA = true;
                    scripA = scripCreate;
                }
                if (scripCreate._unitClass == UnitType.Lancer)
                {
                    foundL = true;
                    scripL = scripCreate;
                }
                if (scripCreate._unitClass == UnitType.TNT)
                {
                    foundT = true;
                    scripT = scripCreate;
                }
                if (scripCreate._unitClass == UnitType.Healer)
                {
                    foundH = true;
                    scripH = scripCreate;
                }
            }
        }

        // Nếu không có, bật slot trống
        if (!foundW)
        {
            foreach (var hit in _PlayerCreate)
            {
                if (!hit.activeSelf)
                {
                    if (Castle.Instance._ListWarrior.Count > 0)
                        hit.SetActive(true);
                    scripW = hit.GetComponent<LoadCreate>();
                    foundW = true;
                    break;
                }
            }
        }
        if (!foundA)
        {
            foreach (var hit in _PlayerCreate)
            {
                if (!hit.activeSelf)
                {
                    if (Castle.Instance._ListArcher.Count > 0)
                        hit.SetActive(true);
                    scripA = hit.GetComponent<LoadCreate>();
                    foundA = true;
                    break;
                }
            }
        }
        if (!foundL)
        {
            foreach (var hit in _PlayerCreate)
            {
                if (!hit.activeSelf)
                {
                    if (Castle.Instance._ListLancer.Count > 0)
                        hit.SetActive(true);
                    scripL = hit.GetComponent<LoadCreate>();
                    foundL = true;
                    break;
                }
            }
        }
        if (!foundT)
        {
            foreach (var hit in _PlayerCreate)
            {
                if (!hit.activeSelf)
                {
                    if (Castle.Instance._ListTNT.Count > 0)
                        hit.SetActive(true);
                    scripT = hit.GetComponent<LoadCreate>();
                    foundT = true;
                    break;
                }
            }
        }
        if (!foundH)
        {
            foreach (var hit in _PlayerCreate)
            {
                if (!hit.activeSelf)
                {
                    if (Castle.Instance._ListHealer.Count > 0)
                        hit.SetActive(true);
                    scripH = hit.GetComponent<LoadCreate>();
                    foundH = true;
                    break;
                }
            }
        }

        // Gắn dữ liệu cho tất cả obj Warrior
        if (foundW && scripW != null)
        {
            scripW.resetValue();
            foreach (var obj in Castle.Instance._ListWarrior)
            {
                if (obj.gameObject.activeSelf)
                    scripW.addValue(obj.gameObject);
            }
        }
        if (foundA && scripA != null)
        {
            scripA.resetValue();
            foreach (var obj in Castle.Instance._ListArcher)
            {
                if (obj.gameObject.activeSelf || obj.getUpTower())
                    scripA.addValue(obj.gameObject);
            }
        }
        if (foundL && scripL != null)
        {
            scripL.resetValue();
            foreach (var obj in Castle.Instance._ListLancer)
            {
                if (obj.gameObject.activeSelf)
                    scripL.addValue(obj.gameObject);
            }
        }
        if (foundT && scripT != null)
        {
            scripT.resetValue();
            foreach (var obj in Castle.Instance._ListTNT)
            {
                if (obj.gameObject.activeSelf)
                    scripT.addValue(obj.gameObject);
            }
        }
        if (foundH && scripH != null)
        {
            scripH.resetValue();
            foreach (var obj in Castle.Instance._ListHealer)
            {
                if (obj.gameObject.activeSelf)
                    scripH.addValue(obj.gameObject);
            }
        }
    }
    #endregion


    #region Open Panel Buiding
    public void openPanelBuiding()
        => _buiding_hide.move();
    #endregion


    #region Buy Tower
    public void CreateTower()
    {
        if (Castle.Instance == null) return;

        if (GameManager.Instance.Tutorial && TutorialSetUp.Instance.ID == 4)
            TutorialSetUp.Instance.TutorialDropTower();
        GameObject obj = Instantiate(GameManager.Instance._TowerPrefab, wordSpace(), Quaternion.identity, Castle.Instance._TowerFolder);
        obj.GetComponent<House>().setLevel(Castle.Instance._level);
        openPanelBuiding();
        GameManager.Instance.setCanBuy(false);
    }
    #endregion


    private Vector3 wordSpace()
    {
        // canvas render mode Screen Space - Camera
        // Lấy vị trí giữa màn hình
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0f);
        // Vì ScreenToWorldPoint cần z theo khoảng cách tới camera
        // nên phải truyền vào giá trị z (thường là khoảng cách từ camera đến mặt phẳng bạn muốn tính)
        screenCenter.z = 0;
        // Chuyển sang tọa độ world
        Vector3 worldPosition = CameraInfo.Instance.cameraMain.ScreenToWorldPoint(screenCenter);
        worldPosition.z = 0f; // Đặt z về 0 nếu cần thiết
        return worldPosition;
    }


    #region Buy Storage
    public void CreateStorage()
    {
        if (GameManager.Instance.Tutorial && TutorialSetUp.Instance.ID == 4) return;

        if (Castle.Instance == null) return;

        GameObject obj = Instantiate(GameManager.Instance._StoragePrefab, wordSpace(), Quaternion.identity, Castle.Instance._StorgeFolder);
        obj.GetComponent<House>().setLevel(Castle.Instance._level);
        openPanelBuiding();
        GameManager.Instance.setCanBuy(false);
    }
    #endregion


    #region Set Active Button Upgrade
    public void setActiveButtonUpgrade(bool amount)
    {

        if (Castle.Instance == null) return;
        if (Castle.Instance._level >= 5) return;
        bool _on = _ButtonUpgrade.activeSelf;
        if (CursorManager.Instance != null && !CursorManager.Instance.ChoseUI && _on && !amount)
        {
            _ButtonUpgrade.SetActive(amount);
            return;
        }

        _ButtonUpgrade.SetActive(amount);
        if (!amount) return;
        // update value
        updateInfoUpgrade();
    }
    #endregion


    #region Update Reference Upgrade
    public void updateInfoUpgrade()
    {
        string key = "ui.Level";
        string txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";

        switch (Castle.Instance._level)
        {
            case 1:
                // OOkiButton
                _buttonUpgrade_Wood.text = Castle.Instance._lv2_Wood.ToString();
                _buttonUpgrade_Rock.text = Castle.Instance._lv2_Rock.ToString();
                _buttonUpgrade_Gold.text = Castle.Instance._lv2_Gold.ToString();

                // Panel
                _PanelUpgrade_Wood.text = Castle.Instance._lv2_Wood.ToString();
                _PanelUpgrade_Rock.text = Castle.Instance._lv2_Rock.ToString();
                _PanelUpgrade_Gold.text = Castle.Instance._lv2_Gold.ToString();

                _PanelUpgrade_Level.text = txt + Castle.Instance._level;
                _PanelUpgrade_LevelUpgrade.text = txt + Castle.Instance._level + " -> " + txt + (Castle.Instance._level + 1);
                _PanelUpgrade_Health.text = Castle.Instance._maxHealth + " -> " + Castle.Instance._lv2_MaxHealth;
                _PanelUpgrade_Slot.text = Castle.Instance._maxSlot + " -> " + Castle.Instance._lv2_MaxSlot;

                if (Castle.Instance._wood >= Castle.Instance._lv2_Wood)
                    if (Castle.Instance._rock >= Castle.Instance._lv2_Rock)
                        if (Castle.Instance._gold >= Castle.Instance._lv2_Gold)
                            _PanelButtonUpgrade.interactable = true;
                        else _PanelButtonUpgrade.interactable = false;
                    else _PanelButtonUpgrade.interactable = false;
                else _PanelButtonUpgrade.interactable = false;
                break;
            case 2:
                // OOkiButton
                _buttonUpgrade_Wood.text = Castle.Instance._lv3_Wood.ToString();
                _buttonUpgrade_Rock.text = Castle.Instance._lv3_Rock.ToString();
                _buttonUpgrade_Gold.text = Castle.Instance._lv3_Gold.ToString();

                // Panel
                _PanelUpgrade_Wood.text = Castle.Instance._lv3_Wood.ToString();
                _PanelUpgrade_Rock.text = Castle.Instance._lv3_Rock.ToString();
                _PanelUpgrade_Gold.text = Castle.Instance._lv3_Gold.ToString();

                _PanelUpgrade_Level.text = txt + Castle.Instance._level;
                _PanelUpgrade_LevelUpgrade.text = txt + Castle.Instance._level + " -> " + txt + (Castle.Instance._level + 1);
                _PanelUpgrade_Health.text = Castle.Instance._maxHealth + " -> " + Castle.Instance._lv3_MaxHealth;
                _PanelUpgrade_Slot.text = Castle.Instance._maxSlot + " -> " + Castle.Instance._lv3_MaxSlot;

                if (Castle.Instance._wood >= Castle.Instance._lv3_Wood)
                    if (Castle.Instance._rock >= Castle.Instance._lv3_Rock)
                        if (Castle.Instance._gold >= Castle.Instance._lv3_Gold)
                            _PanelButtonUpgrade.interactable = true;
                        else _PanelButtonUpgrade.interactable = false;
                    else _PanelButtonUpgrade.interactable = false;
                else _PanelButtonUpgrade.interactable = false;
                break;
            case 3:
                // OOkiButton
                _buttonUpgrade_Wood.text = Castle.Instance._lv4_Wood.ToString();
                _buttonUpgrade_Rock.text = Castle.Instance._lv4_Rock.ToString();
                _buttonUpgrade_Gold.text = Castle.Instance._lv4_Gold.ToString();

                // Panel
                _PanelUpgrade_Wood.text = Castle.Instance._lv4_Wood.ToString();
                _PanelUpgrade_Rock.text = Castle.Instance._lv4_Rock.ToString();
                _PanelUpgrade_Gold.text = Castle.Instance._lv4_Gold.ToString();

                _PanelUpgrade_Level.text = txt + Castle.Instance._level;
                _PanelUpgrade_LevelUpgrade.text = txt + Castle.Instance._level + " -> " + txt + (Castle.Instance._level + 1);
                _PanelUpgrade_Health.text = Castle.Instance._maxHealth + " -> " + Castle.Instance._lv4_MaxHealth;
                _PanelUpgrade_Slot.text = Castle.Instance._maxSlot + " -> " + Castle.Instance._lv4_MaxSlot;

                if (Castle.Instance._wood >= Castle.Instance._lv4_Wood)
                    if (Castle.Instance._rock >= Castle.Instance._lv4_Rock)
                        if (Castle.Instance._gold >= Castle.Instance._lv4_Gold)
                            _PanelButtonUpgrade.interactable = true;
                        else _PanelButtonUpgrade.interactable = false;
                    else _PanelButtonUpgrade.interactable = false;
                else _PanelButtonUpgrade.interactable = false;
                break;
            case 4:
                // OOkiButton
                _buttonUpgrade_Wood.text = Castle.Instance._lv5_Wood.ToString();
                _buttonUpgrade_Rock.text = Castle.Instance._lv5_Rock.ToString();
                _buttonUpgrade_Gold.text = Castle.Instance._lv5_Gold.ToString();

                // Panel
                _PanelUpgrade_Wood.text = Castle.Instance._lv5_Wood.ToString();
                _PanelUpgrade_Rock.text = Castle.Instance._lv5_Rock.ToString();
                _PanelUpgrade_Gold.text = Castle.Instance._lv5_Gold.ToString();

                _PanelUpgrade_Level.text = txt + Castle.Instance._level;
                _PanelUpgrade_LevelUpgrade.text = txt + Castle.Instance._level + " -> " + txt + (Castle.Instance._level + 1);
                _PanelUpgrade_Health.text = Castle.Instance._maxHealth + " -> " + Castle.Instance._lv5_MaxHealth;
                _PanelUpgrade_Slot.text = Castle.Instance._maxSlot + " -> " + Castle.Instance._lv5_MaxSlot;

                if (Castle.Instance._wood >= Castle.Instance._lv5_Wood)
                    if (Castle.Instance._rock >= Castle.Instance._lv5_Rock)
                        if (Castle.Instance._gold >= Castle.Instance._lv5_Gold)
                            _PanelButtonUpgrade.interactable = true;
                        else _PanelButtonUpgrade.interactable = false;
                    else _PanelButtonUpgrade.interactable = false;
                else _PanelButtonUpgrade.interactable = false;
                break;
        }
    }
    #endregion


    #region Button Upgrade Click
    public void Upgrade()
    {
        if (GameManager.Instance.Tutorial && TutorialSetUp.Instance.ID < 7) return;
        if (GameManager.Instance.Tutorial && TutorialSetUp.Instance.ID == 7)
            TutorialSetUp.Instance.TutorialClosePanelUpgrade();
        // trừ tài nguyên
        switch (Castle.Instance._level)
        {
            case 1:
                Castle.Instance._wood -= Castle.Instance._lv2_Wood;
                Castle.Instance._rock -= Castle.Instance._lv2_Rock;
                Castle.Instance._gold -= Castle.Instance._lv2_Gold;
                break;
            case 2:
                Castle.Instance._wood -= Castle.Instance._lv3_Wood;
                Castle.Instance._rock -= Castle.Instance._lv3_Rock;
                Castle.Instance._gold -= Castle.Instance._lv3_Gold;
                break;
            case 3:
                Castle.Instance._wood -= Castle.Instance._lv4_Wood;
                Castle.Instance._rock -= Castle.Instance._lv4_Rock;
                Castle.Instance._gold -= Castle.Instance._lv4_Gold;
                break;
            case 4:
                Castle.Instance._wood -= Castle.Instance._lv5_Wood;
                Castle.Instance._rock -= Castle.Instance._lv5_Rock;
                Castle.Instance._gold -= Castle.Instance._lv5_Gold;
                break;
        }

        // upgrade
        Castle.Instance.Upgrade();
        updateInfoUpgrade();
        // reference Srorage
        GameManager.Instance.Info._wood_Storage += GameManager.Instance.Info._buidingReferenceBounus;
        GameManager.Instance.Info._rock_Storage += GameManager.Instance.Info._buidingReferenceBounus;
        GameManager.Instance.Info._gold_Storage += GameManager.Instance.Info._buidingReferenceBounus;
        // reference Tower
        GameManager.Instance.Info._wood_Tower += GameManager.Instance.Info._buidingReferenceBounus;
        GameManager.Instance.Info._rock_Tower += GameManager.Instance.Info._buidingReferenceBounus;
        GameManager.Instance.Info._gold_Tower += GameManager.Instance.Info._buidingReferenceBounus;

        foreach (var tower in Castle.Instance._towerList)
            tower.GetComponent<House>().setLevel(Castle.Instance._level);
        foreach (var storage in Castle.Instance._storageList)
            storage.GetComponent<House>().setLevel(Castle.Instance._level);
        Castle.Instance._archer_Left_Obj.GetComponent<ArcherUP>().setDamage(GameManager.Instance.Info._damageBounus);
        Castle.Instance._archer_Center_Obj.GetComponent<ArcherUP>().setDamage(GameManager.Instance.Info._damageBounus);
        Castle.Instance._archer_Right_Obj.GetComponent<ArcherUP>().setDamage(GameManager.Instance.Info._damageBounus);

        int level = Castle.Instance._level;
        if (level > 1)
        {
            GameManager.Instance.Info.upgradeWarrior();
            foreach (var player in Castle.Instance._ListWarrior)
                player.upLevel(level);
        }
        if (level > 2)
        {
            GameManager.Instance.Info.upgradeArcher();
            foreach (var player in Castle.Instance._ListArcher)
                player.upLevel(level - 1);
        }
        if (level > 3)
        {
            GameManager.Instance.Info.upgradeLancer();
            foreach (var player in Castle.Instance._ListLancer)
                player.upLevel(level - 2);
        }
        if (level > 4)
        {
            GameManager.Instance.Info.upgradeTNT();
            foreach (var player in Castle.Instance._ListTNT)
                player.upLevel(level - 3);
        }

        updateReferent();
        if (level == 5)
        {
            string key = "ui.Level";
            string txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";

            _PanelUpgrade_Level.text = txt + Castle.Instance._level;
            _PanelUpgrade_LevelUpgrade.text = txt + Castle.Instance._level;
            _PanelUpgrade_Health.text = Castle.Instance._lv5_MaxHealth.ToString();
            _PanelUpgrade_Slot.text = Castle.Instance._lv5_MaxSlot.ToString();
            _PanelButtonUpgrade.interactable = false;
        }

        updateHP();
    }
    #endregion


    #region Update Buiding
    private void updateBuidingReference()
    {
        _wood_Tower.text = GameManager.Instance.Info._wood_Tower.ToString();
        _rock_Tower.text = GameManager.Instance.Info._rock_Tower.ToString();
        _gold_Tower.text = GameManager.Instance.Info._gold_Tower.ToString();

        _wood_Storage.text = GameManager.Instance.Info._wood_Storage.ToString();
        _rock_Storage.text = GameManager.Instance.Info._rock_Storage.ToString();
        _gold_Storage.text = GameManager.Instance.Info._gold_Storage.ToString();
    }

    public void checkButtonBuyTowerAndStorage()
    {
        updateBuidingReference();

        if (Castle.Instance == null) return;

        if (GameManager.Instance.Info._wood_Tower <= Castle.Instance._wood)
            if (GameManager.Instance.Info._rock_Tower <= Castle.Instance._rock)
                if (GameManager.Instance.Info._gold_Tower <= Castle.Instance._gold)
                    _buttonTower.interactable = true;
                else _buttonTower.interactable = false;
            else _buttonTower.interactable = false;
        else _buttonTower.interactable = false;

        if (GameManager.Instance.Info._wood_Storage <= Castle.Instance._wood)
            if (GameManager.Instance.Info._rock_Storage <= Castle.Instance._rock)
                if (GameManager.Instance.Info._gold_Storage <= Castle.Instance._gold)
                    _buttonStorage.interactable = true;
                else _buttonStorage.interactable = false;
            else _buttonStorage.interactable = false;
        else _buttonStorage.interactable = false;
    }
    #endregion


    #region Enemy Respawn
    public void onEnemyRespawn(bool war)
    {
        string content;
        Color color;
        if (war)
        {
            string key = "ui.EnemySpawn.TitleWar";
            string txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
            content = txt;
            color = Color.red;
        }
        else
        {
            string key = "ui.EnemySpawn.Title";
            string txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
            content = txt;
            color = Color.white;
        }
        _textEnemyRespawn.text = content;
        _textEnemyRespawn.color = color;
        _enemyRespawn.SetActive(true);
    }


    public void onWarning(EnemyHuoseController house)
    {
        string key = "ui.Warning.TitleWarning";
        string txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";

        _textWarning2.text = $"{(int)house._warningTime} {txt}";
        _warning.SetActive(true);
    }
    #endregion


    #region Open Tutorial_Viedo
    public void openTutorialVideo()
    {
        if (GameManager.Instance.Tutorial
        && TutorialSetUp.Instance.ID == 2
        && !TutorialSetUp.Instance._openNhatKy)
            TutorialSetUp.Instance.Tutorial_Select_Unit();

        // load ngon ngu o day
    }
    #endregion


    #region Close Tutorial_Video
    public void closeTutorialVideo()
    {
        if (GameManager.Instance.Tutorial
        && TutorialSetUp.Instance.ID == 2
        && !TutorialSetUp.Instance._TutorialNhatKy)
            TutorialSetUp.Instance.TutorialBuilding();
    }
    #endregion


    // #region Auto Train
    // private void autoTrain()
    // {
    //     if (_isAutoTraining) return;
    //     if (!GameManager.Instance.getAutoTrain()) return;
    //     if (Castle.Instance == null) return;
    //     if (GameManager.Instance.Tutorial) return;

    //     _isAutoTraining = true;

    //     try
    //     {
    //         int castleLevel = Castle.Instance._level;
    //         int maxSlot = Castle.Instance._maxSlot;
    //         int currentSlot = Castle.Instance._currentSlot;
    //         int totalSlotAvailable = maxSlot - currentSlot;

    //         if (totalSlotAvailable <= 0) return;


    //         Dictionary<int, float[]> levelRatio = new Dictionary<int, float[]>
    //         {
    //             {1, new float[]{1f, 0f, 0f, 0f, 0f}},
    //             {2, new float[]{0.7f, 0.3f, 0f, 0f, 0f}},
    //             {3, new float[]{0.5f, 0.3f, 0.2f, 0f, 0f}},
    //             {4, new float[]{0.4f, 0.25f, 0.2f, 0.15f, 0f}},
    //             {5, new float[]{0.35f, 0.25f, 0.2f, 0.1f, 0.1f}}
    //         };

    //         float[] ratios = levelRatio[Mathf.Clamp(castleLevel, 1, 5)];

    //         var units = new List<(string name, int unlockLevel, int slot, Button btn, Action create)>
    //         {
    //             ("Warrior", 1, GameManager.Instance._warriorPrefab.GetComponent<PlayerAI>()._slot, _WarriorButton, createWarrior),
    //             ("Archer", 2, GameManager.Instance._ArcherPrefab.GetComponent<PlayerAI>()._slot, _ArcherButton, createArcher),
    //             ("Lancer", 3, GameManager.Instance._LancerPrefab.GetComponent<PlayerAI>()._slot, _LancerButton, createLancer),
    //             ("Healer", 4, GameManager.Instance._HealerPrefab.GetComponent<PlayerAI>()._slot, _HealerButton, createHealer),
    //             ("TNT", 5, GameManager.Instance._TNTPrefab.GetComponent<PlayerAI>()._slot, _TNTButton, createTNT)
    //         };

    //         units.Sort((a, b) => a.slot.CompareTo(b.slot));

    //         var trainPlan = new List<(int index, int count, int slot)>();
            
    //         for (int i = 0; i < units.Count; i++)
    //         {
    //             var u = units[i];
    //             if (castleLevel < u.unlockLevel) continue;
    //             if (u.btn == null || !u.btn.interactable) continue;

    //             int targetSlot = Mathf.FloorToInt(totalSlotAvailable * ratios[i]);
    //             int count = targetSlot / u.slot;
                
    //             if (count > 0)
    //             {
    //                 trainPlan.Add((i, count, u.slot));
    //             }
    //         }

    //         if (trainPlan.Count == 0) return;

    //         foreach (var (index, targetCount, unitSlot) in trainPlan)
    //         {
    //             var u = units[index];
                
    //             for (int j = 0; j < targetCount; j++)
    //             {
    //                 currentSlot = Castle.Instance._currentSlot;
                    
    //                 if (currentSlot + unitSlot > maxSlot) return;
    //                 if (u.btn == null || !u.btn.interactable) break;
                    
    //                 u.create.Invoke();
    //                 // KHÔNG gọi updateSlot() và CheckLevel() ở đây nữa!
    //             }
    //         }
    //     }
    //     finally
    //     {
    //         _isAutoTraining = false;
    //     }
    // }
    // #endregion
}
