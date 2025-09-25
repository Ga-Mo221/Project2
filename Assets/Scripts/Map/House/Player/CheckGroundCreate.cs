using NaughtyAttributes;
using UnityEngine;

public class CheckGroundCreate : MonoBehaviour
{
    [SerializeField] private bool Tower = true;
    [SerializeField] private SpriteRenderer _TowerGFX;
    [ShowIf(nameof(Tower))]
    [SerializeField] private SpriteRenderer _ArcherUp_GFX;
    [ShowIf(nameof(Tower))]
    [SerializeField] private GameObject _Button_ArcherUp;
    public Animator _anim;
    [SerializeField] private HouseHealth _houshealth;
    [SerializeField] private Collider2D _groundColider;
    [SerializeField] private GameObject _outline;
    [ShowIf(nameof(Tower))]
    [SerializeField] private GameObject _inPos;

    [HideIf(nameof(Tower))]
    [SerializeField] private GameObject _inPos1;
    [HideIf(nameof(Tower))]
    [SerializeField] private GameObject _inPos2;
    [HideIf(nameof(Tower))]
    [SerializeField] private GameObject _inPos3;

    [SerializeField] private GameObject _light;
    [SerializeField] private GameObject _HPBar;
    [SerializeField] private GameObject _buttonOK;
    [SerializeField] private GameObject _buttonCancel;
    [SerializeField] private GameObject _ingLoad;

    [HideIf(nameof(Tower))]
    [SerializeField] private GameObject _RotationR;
    [HideIf(nameof(Tower))]
    [SerializeField] private GameObject _RotationL;

    [SerializeField] private bool _canCreate = true;
    public bool _see = false;
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
        if (!_inPos && Tower)
            Debug.LogError($"[{transform.parent.name}] [CheckGroundCreate] Chưa gán 'Gameobjet InPos'");
        if (!_inPos1 && !Tower)
            Debug.LogError($"[{transform.parent.name}] [CheckGroundCreate] Chưa gán 'Gameobjet InPos1'");
        if (!_inPos2 && !Tower)
            Debug.LogError($"[{transform.parent.name}] [CheckGroundCreate] Chưa gán 'Gameobjet InPos2'");
        if (!_inPos3 && !Tower)
            Debug.LogError($"[{transform.parent.name}] [CheckGroundCreate] Chưa gán 'Gameobjet InPos3'");
        if (!_outline)
            Debug.LogError($"[{transform.parent.name}] [CheckGroundCreate] Chưa gán 'OutLine'");
        if (!_light)
            Debug.LogError($"[{transform.parent.name}] [CheckGroundCreate] Chưa gán 'Light'");
        if (!_ArcherUp_GFX && Tower)
            Debug.LogError($"[{transform.parent.name}] [CheckGroundCreate] Chưa gán 'ArcherUP'");
        if (!_Button_ArcherUp && Tower)
            Debug.LogError($"[{transform.parent.name}] [CheckGroundCreate] Chưa gán 'Canvas'");
        if (!_RotationR && !Tower)
            Debug.LogError($"[{transform.parent.name}] [CheckGroundCreate] Chưa gán 'Button Rotation Right'");
        if (!_RotationL && !Tower)
            Debug.LogError($"[{transform.parent.name}] [CheckGroundCreate] Chưa gán 'Button Rotation Left'");
        _groundColider.enabled = false;
        _light.SetActive(false);
        _outline.SetActive(false);
        _HPBar.SetActive(false);
        if (Tower)
        {
            _inPos.SetActive(false);
            _Button_ArcherUp.SetActive(false);
        }
        else
        {
            _inPos1.SetActive(false);
            _inPos2.SetActive(false);
            _inPos3.SetActive(false);
        }
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
        if (collision.CompareTag("Buiding")) return;
        _anim.SetBool("Red", true);
        _canCreate = false;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == null) return;
        if (collision.CompareTag("Buiding")) return;
        _anim.SetBool("Red", false);
        _canCreate = true;
    }

    public void Create()
    {
        if (_canCreate && _see)
        {
            int _yOder = 0;
            if (Tower)
            {
                Castle.Instance._wood -= GameManager.Instance.Info._wood_Tower;
                Castle.Instance._rock -= GameManager.Instance.Info._rock_Tower;
                Castle.Instance._gold -= GameManager.Instance.Info._gold_Tower;
                GameManager.Instance.UIupdateReferences();
                _yOder = -(int)(_inPos.transform.position.y * 100) + 10000;
                _ArcherUp_GFX.sortingOrder = _yOder + 1;
            }
            else
            {
                Castle.Instance._wood -= GameManager.Instance.Info._wood_Storage;
                Castle.Instance._rock -= GameManager.Instance.Info._rock_Storage;
                Castle.Instance._gold -= GameManager.Instance.Info._gold_Storage;
                GameManager.Instance.UIupdateReferences();
                _yOder = -(int)(_inPos1.transform.position.y * 100) + 10000;
                _RotationL.SetActive(false);
                _RotationR.SetActive(false);
            }
            _TowerGFX.sortingOrder = _yOder;
            _anim.SetBool("Idle", true);
            _houshealth.setCanDetec(true);
            _groundColider.enabled = true;
            _outline.SetActive(true);
            _HPBar.SetActive(true);
            _buttonOK.SetActive(false);
            _buttonCancel.SetActive(false);
            _ingLoad.SetActive(true);
            if (Tower)
                Castle.Instance._towerList.Add(transform.parent.gameObject);
            else
                Castle.Instance._storageList.Add(transform.parent.gameObject);

            AstarPath.active.Scan();
            GameManager.Instance.setCanBuy(true);
            gameObject.SetActive(false);
        }
    }

    public void Cancle()
    {
        GameManager.Instance.setCanBuy(true);
        Destroy(transform.parent.gameObject);
    }

    public void RotationRight()
    {
        int type = _anim.GetInteger("Type");
        if (type == 1)
            _anim.SetInteger("Type", 3);
        if (type == 2)
            _anim.SetInteger("Type", 1);
        if (type == 3)
            _anim.SetInteger("Type", 2);
    }

    public void RotationLeft()
    {
        int type = _anim.GetInteger("Type");
        if (type == 1)
            _anim.SetInteger("Type", 2);
        if (type == 2)
            _anim.SetInteger("Type", 3);
        if (type == 3)
            _anim.SetInteger("Type", 1);
    }
}
