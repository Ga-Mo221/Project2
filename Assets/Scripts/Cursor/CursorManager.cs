using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance { get; private set; }

    [SerializeField] private Texture2D _normal;
    [SerializeField] private Texture2D _select;
    public Vector2 _hotPot = new Vector2(32,32);

    public bool ChoseUI = false;
    public bool Select = false;

    public GameObject _hoverGameobject;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SetNormalCursor();
    }

    public void SetNormalCursor()
    {
        Cursor.SetCursor(_normal, _hotPot, CursorMode.Auto);
        Select = false;
        _hoverGameobject = null;
    }

    public void SetSelectCursor(GameObject obj)
    {
        Cursor.SetCursor(_select, _hotPot, CursorMode.Auto);
        Select = true;
        _hoverGameobject = obj;
    }
}
