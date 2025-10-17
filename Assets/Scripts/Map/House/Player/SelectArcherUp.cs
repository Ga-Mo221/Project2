using UnityEngine;

public class SelectArcherUp : MonoBehaviour
{
    [SerializeField] private GameObject _button_Right;
    [SerializeField] private GameObject _button_Center;
    [SerializeField] private GameObject _button_Left;

    private bool _archer_right = false;
    private bool _archer_center = false;
    private bool _archer_Left = false;

    private int _currentLevel = 0;

    void Start()
    {
        _currentLevel = 0;
        _button_Right.SetActive(false);
        _button_Center.SetActive(false);
        _button_Left.SetActive(false);
    }


    void Update()
    {
        if (CastleManager.Instance.Castle != null && CastleManager.Instance.Castle._level != _currentLevel)
        {
            _currentLevel = CastleManager.Instance.Castle._level;
            switch (_currentLevel)
            {
                case 2:
                    _button_Right.SetActive(true);
                    break;
                case 3:
                    _button_Left.SetActive(true);
                    break;
                case 4:
                    _button_Center.SetActive(true);
                    break;
            }
        }
    }


    public void button_Right()
    {
        if (SelectionBox.Instance.chosen.Count == 1
            && CastleManager.Instance.Castle._archer_Right == null)
        {
            if (!SelectionBox.Instance.chosen[0].CompareTag("Archer")) return;
            _archer_right = true;
            var _scrip = SelectionBox.Instance.chosen[0].GetComponent<ArcherGFX>();
            _scrip.setTarget(CastleManager.Instance.Castle._In_Castle_Pos.position, true);
            _scrip._In_Castle = true;
            _scrip._upDirection = UpDirection.Right;
            _scrip._movingToTower = true;
        }
        else if (CastleManager.Instance.Castle._archer_Right != null)
        {
            CastleManager.Instance.Castle._archer_Right_Obj.SetActive(false);
            var _scrip = CastleManager.Instance.Castle._archer_Right.GetComponent<ArcherGFX>();
            _scrip.target = null;
            _scrip.setTargetPos(transform);
            _scrip._In_Castle = false;
            _scrip.setUpTower(false);
            CastleManager.Instance.Castle._archer_Right.SetActive(true);
            _scrip.setIsAI(true);
            CastleManager.Instance.Castle._archer_Right = null;
        }
    }

    public void button_Centter()
    {
        if (SelectionBox.Instance.chosen.Count == 1
        && CastleManager.Instance.Castle._archer_Center == null)
        {
            if (!SelectionBox.Instance.chosen[0].CompareTag("Archer")) return;
            _archer_center = true;
            var _scrip = SelectionBox.Instance.chosen[0].GetComponent<ArcherGFX>();
            _scrip.setTarget(CastleManager.Instance.Castle._In_Castle_Pos.position, true);
            _scrip._In_Castle = true;
            _scrip._upDirection = UpDirection.Center;
            _scrip._movingToTower = true;
        }
        else if (CastleManager.Instance.Castle._archer_Center != null)
        {
            CastleManager.Instance.Castle._archer_Center_Obj.SetActive(false);
            var _scrip = CastleManager.Instance.Castle._archer_Center.GetComponent<ArcherGFX>();
            _scrip.setTargetPos(transform);
            _scrip.target = null;
            _scrip._In_Castle = false;
            _scrip.setUpTower(false);
            CastleManager.Instance.Castle._archer_Center.SetActive(true);
            _scrip.setIsAI(true);
            CastleManager.Instance.Castle._archer_Center = null;
        }
    }

    public void button_Left()
    {
        if (SelectionBox.Instance.chosen.Count == 1
        && CastleManager.Instance.Castle._archer_Left == null)
        {
            if (!SelectionBox.Instance.chosen[0].CompareTag("Archer")) return;
            _archer_Left = true;
            var _scrip = SelectionBox.Instance.chosen[0].GetComponent<ArcherGFX>();
            _scrip.setTarget(CastleManager.Instance.Castle._In_Castle_Pos.position, true);
            _scrip._In_Castle = true;
            _scrip._upDirection = UpDirection.Left;
            _scrip._movingToTower = true;
        }
        else if (CastleManager.Instance.Castle._archer_Left != null)
        {
            CastleManager.Instance.Castle._archer_Left_Obj.SetActive(false);
            var _scrip = CastleManager.Instance.Castle._archer_Left.GetComponent<ArcherGFX>();
            _scrip.setTargetPos(transform);
            _scrip.target = null;
            _scrip._In_Castle = false;
            _scrip.setUpTower(false);
            CastleManager.Instance.Castle._archer_Left.SetActive(true);
            _scrip.setIsAI(true);
            CastleManager.Instance.Castle._archer_Left = null;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Aply(collision);
    }

    private void Aply(Collider2D collision)
    {
        if (collision == null) return;
        if (collision.CompareTag("Archer"))
        {
            bool ok = false;
            var _script = collision.GetComponent<ArcherGFX>();
            if (_script._In_Castle)
            {
                if (_archer_right && _script._upDirection == UpDirection.Right)
                {
                    CastleManager.Instance.Castle._archer_Right = collision.gameObject;
                    CastleManager.Instance.Castle._archer_Right_Obj.SetActive(true);
                    ok = true;
                }
                else if (_archer_center && _script._upDirection == UpDirection.Center)
                {
                    CastleManager.Instance.Castle._archer_Center = collision.gameObject;
                    CastleManager.Instance.Castle._archer_Center_Obj.SetActive(true);
                    ok = true;
                }
                else if (_archer_Left && _script._upDirection == UpDirection.Left)
                {
                    CastleManager.Instance.Castle._archer_Left = collision.gameObject;
                    CastleManager.Instance.Castle._archer_Left_Obj.SetActive(true);
                    ok = true;
                }
                if (ok)
                {
                    _script.setUpTower(true);
                    collision.gameObject.SetActive(false);
                    CastleManager.Instance.Castle._audio.PlayArcherUpSound();
                }
            }
        }
    }


    public void Out()
    {
        // right
        CastleManager.Instance.Castle._archer_Right_Obj.SetActive(false);
        if (CastleManager.Instance.Castle._archer_Right != null)
        {
            var _scrip = CastleManager.Instance.Castle._archer_Right.GetComponent<ArcherGFX>();
            if (_scrip != null)
            {
                _scrip.target = null;
                _scrip.setTargetPos(transform);
                _scrip._In_Castle = false;
                _scrip.setUpTower(false);
                CastleManager.Instance.Castle._archer_Right.SetActive(true);
                _scrip.setIsAI(true);
                CastleManager.Instance.Castle._archer_Right = null;
            }
        }

        // center
        CastleManager.Instance.Castle._archer_Center_Obj.SetActive(false);
        if (CastleManager.Instance.Castle._archer_Center != null)
        {
            var _scrip = CastleManager.Instance.Castle._archer_Center.GetComponent<ArcherGFX>();
            if (_scrip  != null)
            {
                _scrip.setTargetPos(transform);
                _scrip.target = null;
                _scrip._In_Castle = false;
                _scrip.setUpTower(false);
                CastleManager.Instance.Castle._archer_Center.SetActive(true);
                _scrip.setIsAI(true);
                CastleManager.Instance.Castle._archer_Center = null;
            }
        }

        // left
        CastleManager.Instance.Castle._archer_Left_Obj.SetActive(false);
        if (CastleManager.Instance.Castle._archer_Left == null) return;
        var _script = CastleManager.Instance.Castle._archer_Left.GetComponent<ArcherGFX>();
        if (_script != null)
        {
            _script.setTargetPos(transform);
            _script.target = null;
            _script._In_Castle = false;
            _script.setUpTower(false);
            CastleManager.Instance.Castle._archer_Left.SetActive(true);
            _script.setIsAI(true);
            CastleManager.Instance.Castle._archer_Left = null;
        }
    }
}
