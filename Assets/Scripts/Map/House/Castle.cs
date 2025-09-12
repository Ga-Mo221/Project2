using UnityEngine;

public class Castle : MonoBehaviour
{
    public static Castle Instance { get; private set; }

    [SerializeField] private SpriteRenderer _sottingLayer;
    [SerializeField] private Transform _point;

    public GameObject[] _allItems;
    public GameObject _inventory;

    public bool _canFind = true;
    public bool _Q = false;
    public bool _W = false;
    public bool _E = false;
    public bool _A = false;
    public bool _S = false;
    public bool _V = false;


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
        _sottingLayer.sortingOrder = -(int)(_point.position.y * 100);

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
