using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class Castle : MonoBehaviour
{
    public static Castle Instance { get; private set; }

    [Header("Propety")]
    [SerializeField] public int _level = 1;

    [SerializeField] private SpriteRenderer _sottingLayer;
    [SerializeField] private Transform _point;
    [SerializeField] public Transform _In_Castle_Pos;

    [Foldout("All Item")]
    public bool _canFind = true;
    public GameObject[] _allItems;

    [Header("Storage")]
    public GameObject _inventory;

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
        _archer_Right_Obj.GetComponent<SpriteRenderer>().sortingOrder = _yOder+1;
        _archer_Center_Obj.GetComponent<SpriteRenderer>().sortingOrder = _yOder+1;
        _archer_Left_Obj.GetComponent<SpriteRenderer>().sortingOrder = _yOder+1;
        
        _archer_Center_Obj.SetActive(false);
        _archer_Left_Obj.SetActive(false);
        _archer_Right_Obj.SetActive(false);

        _allItems = GameObject.FindGameObjectsWithTag("Item");
        sort();
    }

    void Update()
    {
        buttonEnter();
    }

    private void sort()
    {
        System.Array.Sort(_allItems, (a, b) =>
        {
            float distA = Vector3.Distance(_point.position, a.transform.position);
            float distB = Vector3.Distance(_point.position, b.transform.position);
            return distA.CompareTo(distB); // nhỏ -> lớn
        });
    }

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
