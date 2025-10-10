using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class Castle : MonoBehaviour
{
    public static Castle Instance { get; private set; }

    [Header("Propety")]
    [SerializeField] private bool _canUpdateHP = true;
    public int _level = 1;
    public float _maxHealth = 500;
    public float _currentHealth = 0;
    public int _maxSlot = 50;
    public int _currentSlot = 0;

    [Foldout("Inport")]
    [SerializeField] private SpriteRenderer _sottingLayer;
    [Foldout("Inport")]
    [SerializeField] private Transform _point;
    [Foldout("Inport")]
    [SerializeField] public Transform _In_Castle_Pos;
    [Foldout("Inport")]
    [SerializeField] private BuidingFire _fire;

    public UnitAudio _audio;


    #region Inventory
    [Foldout("Inventory")]
    public int _maxRock = 50;
    [Foldout("Inventory")]
    public int _maxWood = 50;
    [Foldout("Inventory")]
    public int _maxGold = 50;
    [Foldout("Inventory")]
    public int _maxMeat = 50;
    [Foldout("Inventory")]
    public int _rock = 0;
    [Foldout("Inventory")]
    public int _wood = 0;
    [Foldout("Inventory")]
    public int _gold = 0;
    [Foldout("Inventory")]
    public int _meat = 0;
    #endregion


    #region Archer
    [Foldout("Archer_Up")]
    public GameObject _archer_Right;
    [Foldout("Archer_Up")]
    public GameObject _archer_Center;
    [Foldout("Archer_Up")]
    public GameObject _archer_Left;
    [Foldout("Archer_Up")]
    public GameObject _archer_Right_Obj;
    [Foldout("Archer_Up")]
    public GameObject _archer_Center_Obj;
    [Foldout("Archer_Up")]
    public GameObject _archer_Left_Obj;
    #endregion


    #region Key
    [Foldout("Key Down")]
    public bool _Q = false; // warrior
    [Foldout("Key Down")]
    public bool _W = false; // archer
    [Foldout("Key Down")]
    public bool _E = false; // lancer
    [Foldout("Key Down")]
    public bool _A = false; // healer
    [Foldout("Key Down")]
    public bool _S = false; // tnt
    [Foldout("Key Down")]
    public bool _V = false;
    #endregion


    #region List
    [Foldout("All Item")]
    public bool _canFind = true;
    [Foldout("All Item")]
    public GameObject[] _allItems;

    [Foldout("Storage")]
    public Transform _StorgeFolder;
    [Foldout("Storage")]
    public List<GameObject> _storageList;
    [Foldout("Tower")]
    public Transform _TowerFolder;
    [Foldout("Tower")]
    public List<GameObject> _towerList;

    [Foldout("Player List")]
    public Transform _PlayerFolder;
    [Foldout("Player List")]
    public List<PlayerAI> _ListWarrior;
    [Foldout("Player List")]
    public List<PlayerAI> _ListArcher;
    [Foldout("Player List")]
    public List<PlayerAI> _ListLancer;
    [Foldout("Player List")]
    public List<PlayerAI> _ListHealer;
    [Foldout("Player List")]
    public List<PlayerAI> _ListTNT;
    #endregion


    #region Upgrade Reference
    [BoxGroup("LV2")]
    public int _lv2_Wood = 100;
    [BoxGroup("LV2")]
    public int _lv2_Rock = 100;
    [BoxGroup("LV2")]
    public int _lv2_Gold = 100;
    [BoxGroup("LV2")]
    public float _lv2_MaxHealth = 600;
    [BoxGroup("LV2")]
    public int _lv2_MaxSlot = 10;

    [BoxGroup("LV3")]
    public int _lv3_Wood = 100;
    [BoxGroup("LV3")]
    public int _lv3_Rock = 100;
    [BoxGroup("LV3")]
    public int _lv3_Gold = 100;
    [BoxGroup("LV3")]
    public float _lv3_MaxHealth = 600;
    [BoxGroup("LV3")]
    public int _lv3_MaxSlot = 10;

    [BoxGroup("LV4")]
    public int _lv4_Wood = 100;
    [BoxGroup("LV4")]
    public int _lv4_Rock = 100;
    [BoxGroup("LV4")]
    public int _lv4_Gold = 100;
    [BoxGroup("LV4")]
    public float _lv4_MaxHealth = 600;
    [BoxGroup("LV4")]
    public int _lv4_MaxSlot = 10;

    [BoxGroup("LV5")]
    public int _lv5_Wood = 100;
    [BoxGroup("LV5")]
    public int _lv5_Rock = 100;
    [BoxGroup("LV5")]
    public int _lv5_Gold = 100;
    [BoxGroup("LV5")]
    public float _lv5_MaxHealth = 600;
    [BoxGroup("LV5")]
    public int _lv5_MaxSlot = 10;
    #endregion


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }


    #region  Start
    void Start()
    {
        if (!_sottingLayer)
            Debug.LogError("[Castle] Chưa gán 'SpriteRender'");
        if (!_point)
            Debug.LogError("[Castle] Chưa gán 'Transform _point'");
        if (!_In_Castle_Pos)
            Debug.LogError("[Castle] Chưa gán 'Transform _In_Castle_Pos'");
        if (!_archer_Right_Obj)
            Debug.LogError("[Castle] Chưa gán 'GameObject _archer_Right_Obj'");
        if (!_archer_Center_Obj)
            Debug.LogError("[Castle] Chưa gán 'GameObject _archer_Center_Obj'");
        if (!_archer_Left_Obj)
            Debug.LogError("[Castle] Chưa gán 'GameObject _archer_Left_Obj'");

        int _yOder = -(int)(_point.position.y * 100) + 10000;
        _sottingLayer.sortingOrder = _yOder;
        _fire.oder(_yOder);
        _archer_Right_Obj.GetComponent<SpriteRenderer>().sortingOrder = _yOder + 1;
        _archer_Center_Obj.GetComponent<SpriteRenderer>().sortingOrder = _yOder + 1;
        _archer_Left_Obj.GetComponent<SpriteRenderer>().sortingOrder = _yOder + 1;

        _archer_Center_Obj.SetActive(false);
        _archer_Left_Obj.SetActive(false);
        _archer_Right_Obj.SetActive(false);

        _allItems = GameObject.FindGameObjectsWithTag("Item");

        if (_canUpdateHP)
            _currentHealth = _maxHealth;
        GameManager.Instance.UIupdateHPCastle();
    }
    #endregion


    #region  Update
    void Update()
    {
        if (GameManager.Instance.getGameOver()) return;
        buttonEnter();
        buttonTab();
        OpenWindown();
        CloseWindown();
    }
    #endregion


    #region Upgrade
    public void Upgrade()
    {
        _audio.PlayLevelUpSound();
        _level++;
        float health = 0;
        switch (_level)
        {
            case 2:
                health = _lv2_MaxHealth - _maxHealth;
                _maxHealth = _lv2_MaxHealth;
                _currentHealth += health;
                _maxSlot = _lv2_MaxSlot;
                break;
            case 3:
                health = _lv3_MaxHealth - _maxHealth;
                _maxHealth = _lv3_MaxHealth;
                _currentHealth += health;
                _maxSlot = _lv3_MaxSlot;
                break;
            case 4:
                health = _lv4_MaxHealth - _maxHealth;
                _maxHealth = _lv4_MaxHealth;
                _currentHealth += health;
                _maxSlot = _lv4_MaxSlot;
                break;
            case 5:
                health = _lv5_MaxHealth - _maxHealth;
                _maxHealth = _lv5_MaxHealth;
                _currentHealth += health;
                _maxSlot = _lv5_MaxSlot;
                break;
        }
    }
    #endregion


    #region Check GameOver
    public void CheckGameOver()
    {
        // Nếu tất cả quân đã chết
        if (AreAllUnitsDead(_level))
        {
            // Và không đủ tài nguyên tạo bất kỳ loại nào
            if (NoResourcesToCreateAny(_level))
            {
                GameManager.Instance.setWin(false);
                GameManager.Instance.setGameOver(true);
            }
        }
    }

    private IEnumerator Over()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        if (GameManager.Instance.TutorialWar && !TutorialSetUp.Instance._Win)
        {
            TutorialSetUp.Instance.GameLose();
        }
        else
        {
            
        }
    }

    // Check từng list class có còn lính hay không
    private bool AreAllUnitsDead(int level)
    {
        var unitLists = new List<List<PlayerAI>> { _ListWarrior };
        if (level >= 2) unitLists.Add(_ListArcher);
        if (level >= 3) unitLists.Add(_ListLancer);
        if (level >= 4) unitLists.Add(_ListTNT);
        if (level >= 5) unitLists.Add(_ListHealer);

        foreach (var list in unitLists)
        {
            if (!IsListDead(list))
                return false;
        }

        return true;
    }


    // kiểm tra còn lính hay không
    private bool IsListDead(List<PlayerAI> list)
    {
        // Nếu còn bất kỳ lính nào sống, chưa tạo xong, hoặc đang lên tháp → chưa "dead"
        foreach (var p in list)
        {
            if (p.gameObject.activeSelf || p.getCreating() || p.getUpTower())
                return false;
        }
        return true;
    }

    // kiểm tra đủ tài nguyên để tạo lính mới hay không
    private bool NoResourcesToCreateAny(int level)
    {
        // Mô tả điều kiện tài nguyên cho từng class
        bool noWarrior = checkReferences(_wood, GameManager.Instance.Info._wood_Warrior);
        bool noArcher = checkReferences(_wood, GameManager.Instance.Info._wood_Archer)
                        && checkReferences(_rock, GameManager.Instance.Info._rock_Archer);
        bool noLancer = checkReferences(_wood, GameManager.Instance.Info._wood_Lancer)
                        && checkReferences(_rock, GameManager.Instance.Info._rock_Lancer)
                        && checkReferences(_meat, GameManager.Instance.Info._meat_Lancer);
        bool noTNT = checkReferences(_rock, GameManager.Instance.Info._rock_TNT)
                        && checkReferences(_meat, GameManager.Instance.Info._meat_TNT)
                        && checkReferences(_gold, GameManager.Instance.Info._gold_TNT);
        bool noHealer = checkReferences(_wood, GameManager.Instance.Info._wood_Healer)
                        && checkReferences(_rock, GameManager.Instance.Info._rock_Healer)
                        && checkReferences(_meat, GameManager.Instance.Info._meat_Healer)
                        && checkReferences(_gold, GameManager.Instance.Info._gold_Healer);

        bool result = level switch
        {
            1 => noWarrior,
            2 => noWarrior && noArcher,
            3 => noWarrior && noArcher && noLancer,
            4 => noWarrior && noArcher && noLancer && noTNT,
            5 => noWarrior && noArcher && noLancer && noTNT && noHealer,
            _ => false
        };

        // Gắn thông báo cụ thể
        if (result)
        {
            string msg = level switch
            {
                1 => "Không đủ tài nguyên để tạo lính Warrior.",
                2 => !noWarrior ? "Không đủ tài nguyên để tạo lính Warrior." :
                    "Không đủ tài nguyên để tạo lính Archer.",
                3 => !noWarrior ? "Không đủ tài nguyên để tạo lính Warrior." :
                    !noArcher ? "Không đủ tài nguyên để tạo lính Archer." :
                    "Không đủ tài nguyên để tạo lính Lancer.",
                4 => !noWarrior ? "Không đủ tài nguyên để tạo lính Warrior." :
                    !noArcher ? "Không đủ tài nguyên để tạo lính Archer." :
                    !noLancer ? "Không đủ tài nguyên để tạo lính Lancer." :
                    "Không đủ tài nguyên để tạo lính TNT.",
                5 => !noWarrior ? "Không đủ tài nguyên để tạo lính Warrior." :
                    !noArcher ? "Không đủ tài nguyên để tạo lính Archer." :
                    !noLancer ? "Không đủ tài nguyên để tạo lính Lancer." :
                    !noTNT ? "Không đủ tài nguyên để tạo lính TNT." :
                    "Không đủ tài nguyên để tạo lính Healer.",
                _ => "Không đủ tài nguyên để tạo lính."
            };

            GameManager.Instance._contentGameOver = msg;
        }

        return result;
    }

    private bool checkReferences(int currentValue, int createValue)
    {
        // Trả về true nếu KHÔNG đủ tài nguyên để tạo
        return currentValue < createValue;
    }
    #endregion



    #region Button Enter
    private void buttonEnter()
    {
        // Q
        if (Input.GetKeyDown(KeyCode.Q))
            _Q = true;
        if (Input.GetKeyUp(KeyCode.Q))
            _Q = false;

        // W
        if (Input.GetKeyDown(KeyCode.W))
            _W = true;
        if (Input.GetKeyUp(KeyCode.W))
            _W = false;

        // E
        if (Input.GetKeyDown(KeyCode.E))
            _E = true;
        if (Input.GetKeyUp(KeyCode.E))
            _E = false;

        // A
        if (Input.GetKeyDown(KeyCode.A))
            _A = true;
        if (Input.GetKeyUp(KeyCode.A))
            _A = false;

        // S
        if (Input.GetKeyDown(KeyCode.S))
            _S = true;
        if (Input.GetKeyUp(KeyCode.S))
            _S = false;

        // V
        if (Input.GetKeyDown(KeyCode.V))
            _V = true;
        if (Input.GetKeyUp(KeyCode.V))
            _V = false;
    }
    #endregion


    #region  Button Tab
    private void buttonTab()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            GameManager.Instance.UIOpenBuidingPanel();
        }
    }
    #endregion


    #region Open Windown
    private void OpenWindown()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            GameManager.Instance.UIopenShop();
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            GameManager.Instance.UIopenUpgradePanel();
        }
    }
    #endregion


    #region Close Windown
    private void CloseWindown()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.Instance.getShopOpen())
            {
                GameManager.Instance.UIcloseShop();
            }
            if (GameManager.Instance.getUpgradeOpen())
            {
                GameManager.Instance.UIcloseUpgradePanel();
            }
        }
    }
    #endregion
}
