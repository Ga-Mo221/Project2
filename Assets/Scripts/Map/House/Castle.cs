using UnityEngine;

public class Castle : MonoBehaviour
{
    public static Castle Instance { get; private set; }

    [SerializeField] private SpriteRenderer _sottingLayer;
    [SerializeField] private Transform _point;

    public GameObject[] _allItems;

    public bool _canFind = true;


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

    private void sort()
    {
        System.Array.Sort(_allItems, (a, b) =>
        {
            float distA = Vector3.Distance(_point.position, a.transform.position);
            float distB = Vector3.Distance(_point.position, b.transform.position);
            return distA.CompareTo(distB); // nhỏ -> lớn
        });
    }
}
