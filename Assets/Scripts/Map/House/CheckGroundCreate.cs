using UnityEngine;

public class CheckGroundCreate : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _TowerGFX;
    [SerializeField] private SpriteRenderer _ArcherUp_GFX;
    [SerializeField] private Animator _anim;
    [SerializeField] private HouseHealth _houshealth;
    [SerializeField] private Collider2D _groundColider;
    [SerializeField] private GameObject _outline;
    [SerializeField] private GameObject _inPos;
    [SerializeField] private GameObject _light;
    [SerializeField] private GameObject _HPBar;
    [SerializeField] private bool _canCreate = true;
    [SerializeField] private GameObject _buttonOK;
    [SerializeField] private GameObject _buttonCancel;
    [SerializeField] private GameObject _ingLoad;

    private Camera _cam;
    private GameObject _selectedObj;
    private Vector3 _offset;

    void Start()
    {
        _cam = Camera.main;
        if (!_anim)
            Debug.LogError($"[{transform.parent.name}] [CheckGroundCreate] Chưa gán 'Animator'");
        if (!_houshealth)
            Debug.LogError($"[{transform.parent.name}] [CheckGroundCreate] Chưa gán 'HouseHealth'");
        if (!_groundColider)
            Debug.LogError($"[{transform.parent.name}] [CheckGroundCreate] Chưa gán 'Collider'");
        if (!_inPos)
            Debug.LogError($"[{transform.parent.name}] [CheckGroundCreate] Chưa gán 'Gameobjet'");
        if (!_outline)
            Debug.LogError($"[{transform.parent.name}] [CheckGroundCreate] Chưa gán 'OutLine'");
        if (!_light)
            Debug.LogError($"[{transform.parent.name}] [CheckGroundCreate] Chưa gán 'Light'");
        _groundColider.enabled = false;
        _inPos.SetActive(false);
        _light.SetActive(false);
        _outline.SetActive(false);
        _HPBar.SetActive(false);
    }

    void Update()
    {
        // Nhấn chuột trái
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.collider.name == transform.name)
                {
                    _selectedObj = transform.parent.gameObject;
                    // Tính offset giữa vị trí object và chuột
                    Vector3 objPos = _selectedObj.transform.position;
                    _offset = objPos - (Vector3)mousePos;
                }
            }
        }

        // Giữ chuột trái
        if (Input.GetMouseButton(0) && _selectedObj != null)
        {
            Vector3 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
            _selectedObj.transform.position =
            new Vector3(mousePos.x + _offset.x, mousePos.y + _offset.y, _selectedObj.transform.position.z);
        }

        // Thả chuột
        if (Input.GetMouseButtonUp(0))
        {
            _selectedObj = null;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;
        _anim.SetBool("Red", true);
        _canCreate = false;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == null) return;
        _anim.SetBool("Red", false);
        _canCreate = true;
    }

    public void Create()
    {
        if (_canCreate)
        {
            int _yOder = -(int)(_inPos.transform.position.y * 100) + 10000;
            _TowerGFX.sortingOrder = _yOder;
            _ArcherUp_GFX.sortingOrder = _yOder + 1;
            _anim.SetBool("Idle", true);
            _houshealth.setCanDetec(true);
            _groundColider.enabled = true;
            _outline.SetActive(true);
            _HPBar.SetActive(true);
            _buttonOK.SetActive(false);
            _buttonCancel.SetActive(false);
            _ingLoad.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    public void Cancle()
        => Destroy(transform.parent.gameObject);
}
