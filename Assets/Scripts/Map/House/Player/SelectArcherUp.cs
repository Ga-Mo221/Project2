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
        if (Castle.Instance._level != _currentLevel)
        {
            _currentLevel = Castle.Instance._level;
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
            && Castle.Instance._archer_Right == null)
        {
            if (!SelectionBox.Instance.chosen[0].CompareTag("Archer")) return;
            _archer_right = true;
            var _scrip = SelectionBox.Instance.chosen[0].GetComponent<ArcherGFX>();
            _scrip.setTarget(Castle.Instance._In_Castle_Pos.position, true);
            _scrip._In_Castle = true;
            _scrip._upDirection = UpDirection.Right;
        }
        else if (Castle.Instance._archer_Right != null)
        {
            Castle.Instance._archer_Right_Obj.SetActive(false);
            var _scrip = Castle.Instance._archer_Right.GetComponent<ArcherGFX>();
            _scrip.target = null;
            _scrip.setTargetPos(transform);
            _scrip._In_Castle = false;
            Castle.Instance._archer_Right.SetActive(true);
            _scrip.setIsAI(true);
            Castle.Instance._archer_Right = null;
        }
    }

    public void button_Centter()
    {
        if (SelectionBox.Instance.chosen.Count == 1
        && Castle.Instance._archer_Center == null)
        {
            if (!SelectionBox.Instance.chosen[0].CompareTag("Archer")) return;
            _archer_center = true;
            var _scrip = SelectionBox.Instance.chosen[0].GetComponent<ArcherGFX>();
            _scrip.setTarget(Castle.Instance._In_Castle_Pos.position, true);
            _scrip._In_Castle = true;
            _scrip._upDirection = UpDirection.Center;
        }
        else if (Castle.Instance._archer_Center != null)
        {
            Castle.Instance._archer_Center_Obj.SetActive(false);
            var _scrip = Castle.Instance._archer_Center.GetComponent<ArcherGFX>();
            _scrip.setTargetPos(transform);
            _scrip.target = null;
            _scrip._In_Castle = false;
            Castle.Instance._archer_Center.SetActive(true);
            _scrip.setIsAI(true);
            Castle.Instance._archer_Center = null;
        }
    }

    public void button_Left()
    {
        if (SelectionBox.Instance.chosen.Count == 1
        && Castle.Instance._archer_Left == null)
        {
            if (!SelectionBox.Instance.chosen[0].CompareTag("Archer")) return;
            _archer_Left = true;
            var _scrip = SelectionBox.Instance.chosen[0].GetComponent<ArcherGFX>();
            _scrip.setTarget(Castle.Instance._In_Castle_Pos.position, true);
            _scrip._In_Castle = true;
            _scrip._upDirection = UpDirection.Left;
        }
        else if (Castle.Instance._archer_Left != null)
        {
            Castle.Instance._archer_Left_Obj.SetActive(false);
            var _scrip = Castle.Instance._archer_Left.GetComponent<ArcherGFX>();
            _scrip.setTargetPos(transform);
            _scrip.target = null;
            _scrip._In_Castle = false;
            Castle.Instance._archer_Left.SetActive(true);
            _scrip.setIsAI(true);
            Castle.Instance._archer_Left = null;
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
            var _script = collision.GetComponent<ArcherGFX>();
            if (_script._In_Castle)
            {
                if (_archer_right && _script._upDirection == UpDirection.Right)
                {
                    Castle.Instance._archer_Right = collision.gameObject;
                    Castle.Instance._archer_Right_Obj.SetActive(true);
                }
                else if (_archer_center && _script._upDirection == UpDirection.Center)
                {
                    Castle.Instance._archer_Center = collision.gameObject;
                    Castle.Instance._archer_Center_Obj.SetActive(true);
                }
                else if (_archer_Left && _script._upDirection == UpDirection.Left)
                {
                    Castle.Instance._archer_Left = collision.gameObject;
                    Castle.Instance._archer_Left_Obj.SetActive(true);
                }
                collision.gameObject.SetActive(false);
            }
        }
    }
}
