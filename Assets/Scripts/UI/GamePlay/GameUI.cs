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
    [SerializeField] private TextMeshProUGUI _Day;
    [Foldout("Mini Map")]
    [SerializeField] private TextMeshProUGUI _Time_RTS;
    [Foldout("Mini Map")]
    [SerializeField] private GameObject _Sun;
    [Foldout("Mini Map")]
    [SerializeField] private GameObject _Night;
    private float _timeAccumulator = 0f; // tích lũy thời gian để tăng giờ RTS
    private int _hours = 8; // bắt đầu 8h sáng
    #endregion


    #region HP Bar
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
    #endregion


    #region Buiding
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


    #region Upgrade
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


    [SerializeField] private List<GameObject> _createList = new List<GameObject>();
    [SerializeField] private List<GameObject> _PlayerCreate = new List<GameObject>();
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
        updateBuidingReference();
    }


    #region Update
    void Update()
    {
        // Tăng thời gian chơi
        _playTime += Time.deltaTime;
        _timeAccumulator += Time.deltaTime;

        // Cập nhật PlayTime hiển thị 00:00
        int playMinutes = Mathf.FloorToInt(_playTime / 60f);
        int playSeconds = Mathf.FloorToInt(_playTime % 60f);
        _Time.text = string.Format("{0:00}:{1:00}", playMinutes, playSeconds);


        GameManager.Instance._playTime = new Vector2(playMinutes, playSeconds);
        // Mỗi 120 giây playtime = tăng 4 giờ RTS
        if (_timeAccumulator >= GameManager.Instance._4Hours_Sec)
        {
            _hours += 4;
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
        }
        if (_hours == 8)
        {
            _Night.SetActive(false);
            _Sun.SetActive(true);
        }

        // Kiểm tra Night
        if (_hours >= 0 && _hours < 8)
            GameManager.Instance._night = true;
        else
            GameManager.Instance._night = false;

        // Hiển thị Day và Time RTS
        _Day.text = "Day " + GameManager.Instance._currentDay;
        _Time_RTS.text = string.Format("{0:00}:{1:00}", _hours, 0);
    }
    #endregion


    #region Update HP Castle
    public void updateHP()
    {
        float currentHealt = Castle.Instance._currentHealth;
        float maxHealth = Castle.Instance._maxHealth;
        _HPBar.fillAmount = currentHealt / maxHealth;
        _HP_Text.text = currentHealt + " / " + maxHealth;
    }
    #endregion


    #region Update References
    public void updateReferent()
    {
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
    public void updatePlayerValue()
    {
        int WarriorVulue = Castle.Instance._ListWarrior.Count;
        int ArcherVulue = Castle.Instance._ListArcher.Count;
        int LancerVulue = Castle.Instance._ListLancer.Count;
        int HealerVulue = Castle.Instance._ListHealer.Count;
        int TNTVulue = Castle.Instance._ListTNT.Count;

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
        Castle.Instance._currentSlot = 0;
        foreach (var p in Castle.Instance._ListWarrior)
        {
            if (p.gameObject.activeSelf || p.getCreating())
                Castle.Instance._currentSlot += p._slot;
        }
        foreach (var p in Castle.Instance._ListArcher)
        {
            if (p.gameObject.activeSelf || p.getCreating())
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
    }
    #endregion


    #region Close Shop
    public void closeShop()
    {
        _gr.alpha = 0;
        _gr.interactable = false;
        _gr.blocksRaycasts = false;
    }
    #endregion


    #region Check Level
    private void CheckLevel()
    {
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
        Castle.Instance._wood -= GameManager.Instance.Info._wood_Warrior;
        updateReferent();
        // tao player
        GameObject _obj = null;
        PlayerAI _scripPlayer = null;
        bool _isHas = false;
        foreach (var player in Castle.Instance._ListWarrior)
        {
            if (!player.getCreating() && !player.gameObject.activeSelf)
            {
                _obj = player.gameObject;
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
            if (_on && _obj && _scripCreate._unitClass == _scripPlayer._unitClass)
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
        Castle.Instance._wood -= GameManager.Instance.Info._wood_Archer;
        Castle.Instance._rock -= GameManager.Instance.Info._rock_Archer;
        updateReferent();
        // tao player
        GameObject _obj = null;
        PlayerAI _scripPlayer = null;
        bool _isHas = false;
        foreach (var player in Castle.Instance._ListArcher)
        {
            if (!player.getCreating() && !player.gameObject.activeSelf)
            {
                _obj = player.gameObject;
                player.setCreating(true);
                _isHas = true;
                break;
            }
        }
        if (!_isHas)
        {
            _obj = Instantiate(GameManager.Instance._ArcherPrefab, Castle.Instance._In_Castle_Pos.position, Quaternion.identity, Castle.Instance._PlayerFolder);
            _obj.SetActive(false);
            _scripPlayer = _obj.GetComponent<PlayerAI>();
            _scripPlayer.setCreating(true);
            Castle.Instance._ListArcher.Add(_scripPlayer);
        }
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
            if (_on && _obj && _scripCreate._unitClass == _scripPlayer._unitClass)
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
            if (!player.getCreating() && !player.gameObject.activeSelf)
            {
                _obj = player.gameObject;
                player.setCreating(true);
                _isHas = true;
                break;
            }
        }
        if (!_isHas)
        {
            _obj = Instantiate(GameManager.Instance._LancerPrefab, Castle.Instance._In_Castle_Pos.position, Quaternion.identity, Castle.Instance._PlayerFolder);
            _obj.SetActive(false);
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
            if (_on && _obj && _scripCreate._unitClass == _scripPlayer._unitClass)
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
            if (!player.getCreating() && !player.gameObject.activeSelf)
            {
                _obj = player.gameObject;
                player.setCreating(true);
                _isHas = true;
                break;
            }
        }
        if (!_isHas)
        {
            _obj = Instantiate(GameManager.Instance._TNTPrefab, Castle.Instance._In_Castle_Pos.position, Quaternion.identity, Castle.Instance._PlayerFolder);
            _obj.SetActive(false);
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
            if (_on && _obj && _scripCreate._unitClass == _scripPlayer._unitClass)
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
            if (!player.getCreating() && !player.gameObject.activeSelf)
            {
                _obj = player.gameObject;
                player.setCreating(true);
                _isHas = true;
                break;
            }
        }
        if (!_isHas)
        {
            _obj = Instantiate(GameManager.Instance._HealerPrefab, Castle.Instance._In_Castle_Pos.position, Quaternion.identity, Castle.Instance._PlayerFolder);
            _obj.SetActive(false);
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
            if (_on && _obj && _scripCreate._unitClass == _scripPlayer._unitClass)
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
                if (obj.gameObject.activeSelf)
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


    #region Buy Tower
    public void CreateTower()
    {
        GameObject obj = Instantiate(GameManager.Instance._TowerPrefab, wordSpace(), Quaternion.identity, Castle.Instance._TowerFolder);
        obj.GetComponent<House>().setLevel(Castle.Instance._level);
        _buiding_hide.move();
        GameManager.Instance.setCanBuy(false);
    }
    #endregion


    private Vector3 wordSpace()
    {
        // Lấy vị trí giữa màn hình
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0f);
        // Vì ScreenToWorldPoint cần z theo khoảng cách tới camera
        // nên phải truyền vào giá trị z (thường là khoảng cách từ camera đến mặt phẳng bạn muốn tính)
        screenCenter.z = Mathf.Abs(Camera.main.transform.position.z);
        // Chuyển sang tọa độ world
        return Camera.main.ScreenToWorldPoint(screenCenter);
    }


    #region Buy Storage
    public void CreateStorage()
    {
        GameObject obj = Instantiate(GameManager.Instance._StoragePrefab, wordSpace(), Quaternion.identity, Castle.Instance._StorgeFolder);
        obj.GetComponent<House>().setLevel(Castle.Instance._level);
        _buiding_hide.move();
        GameManager.Instance.setCanBuy(false);
    }
    #endregion


    #region Set Active Button Upgrade
    public void setActiveButtonUpgrade(bool amount)
    {
        if (Castle.Instance._level >= 5) return;
        bool _on = _ButtonUpgrade.activeSelf;
        if (!CursorManager.Instance.ChoseUI && _on && !amount)
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
    private void updateInfoUpgrade()
    {
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

                _PanelUpgrade_Level.text = "Lv." + Castle.Instance._level;
                _PanelUpgrade_LevelUpgrade.text = "Lv." + Castle.Instance._level + " -> Lv." + (Castle.Instance._level + 1);
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

                _PanelUpgrade_Level.text = "Lv." + Castle.Instance._level;
                _PanelUpgrade_LevelUpgrade.text = "Lv." + Castle.Instance._level + " -> Lv." + (Castle.Instance._level + 1);
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

                _PanelUpgrade_Level.text = "Lv." + Castle.Instance._level;
                _PanelUpgrade_LevelUpgrade.text = "Lv." + Castle.Instance._level + " -> Lv." + (Castle.Instance._level + 1);
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

                _PanelUpgrade_Level.text = "Lv." + Castle.Instance._level;
                _PanelUpgrade_LevelUpgrade.text = "Lv." + Castle.Instance._level + " -> Lv." + (Castle.Instance._level + 1);
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
            _PanelUpgrade_Level.text = "Lv." + Castle.Instance._level;
            _PanelUpgrade_LevelUpgrade.text = "Lv." + Castle.Instance._level;
            _PanelUpgrade_Health.text = Castle.Instance._lv5_MaxHealth.ToString();
            _PanelUpgrade_Slot.text = Castle.Instance._lv5_MaxSlot.ToString();
            _PanelButtonUpgrade.interactable = false;
        }
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
}
