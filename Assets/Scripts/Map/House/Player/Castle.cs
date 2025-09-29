using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class Castle : MonoBehaviour
{
    public static Castle Instance { get; private set; }

    [Header("Propety")]
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
    public bool _Q = false;
    [Foldout("Key Down")]
    public bool _W = false;
    [Foldout("Key Down")]
    public bool _E = false;
    [Foldout("Key Down")]
    public bool _A = false;
    [Foldout("Key Down")]
    public bool _S = false;
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
        _archer_Right_Obj.GetComponent<SpriteRenderer>().sortingOrder = _yOder + 1;
        _archer_Center_Obj.GetComponent<SpriteRenderer>().sortingOrder = _yOder + 1;
        _archer_Left_Obj.GetComponent<SpriteRenderer>().sortingOrder = _yOder + 1;

        _archer_Center_Obj.SetActive(false);
        _archer_Left_Obj.SetActive(false);
        _archer_Right_Obj.SetActive(false);

        _allItems = GameObject.FindGameObjectsWithTag("Item");

        _currentHealth = _maxHealth;
    }

    void Update()
    {
        buttonEnter();
    }

    #region Upgrade
    public void Upgrade()
    {
        _level++;
        switch (_level)
        {
            case 2:
                _maxHealth = _lv2_MaxHealth;
                _maxSlot = _lv2_MaxSlot;
                break;
            case 3:
                _maxHealth = _lv3_MaxHealth;
                _maxSlot = _lv3_MaxSlot;
                break;
            case 4:
                _maxHealth = _lv4_MaxHealth;
                _maxSlot = _lv4_MaxSlot;
                break;
            case 5:
                _maxHealth = _lv5_MaxHealth;
                _maxSlot = _lv5_MaxSlot;
                break;
        }
    }
    #endregion


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
}
