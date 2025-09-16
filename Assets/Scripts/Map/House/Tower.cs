using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _TowerGFX;
    [SerializeField] private GameObject _ArcherUp;
    [SerializeField] private GameObject _ArcherUp_Obj;

    void Start()
    {
        int _yOder = -(int)(transform.position.y * 100);
        _ArcherUp_Obj.GetComponent<SpriteRenderer>().sortingOrder = _yOder + 1;
        _TowerGFX.sortingOrder = _yOder;

        _ArcherUp_Obj.SetActive(false);
    }


    public void ButtonDown()
    {
        if (SelectionBox.Instance.chosen.Count == 1
        && _ArcherUp == null)
        {
            if (!SelectionBox.Instance.chosen[0].CompareTag("Archer")) return;
            var _scrip = SelectionBox.Instance.chosen[0].GetComponent<ArcherGFX>();
            _scrip.setTarget(transform.position, true);
            _scrip._In_Castle = true;
            _scrip._upDirection = UpDirection.Tower;
        }
        else if (_ArcherUp != null)
        {
            _ArcherUp_Obj.SetActive(false);
            var _scrip = _ArcherUp.GetComponent<ArcherGFX>();
            _scrip.setIsAI(false);
            _scrip.target = null;
            _scrip._In_Castle = false;
            _ArcherUp.SetActive(true);
            _scrip.setIsAI(true);
            _ArcherUp = null;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;
        if (collision.CompareTag("Archer"))
        {
            var _script = collision.GetComponent<ArcherGFX>();
            if (_script._In_Castle)
            {
                if (_script._upDirection == UpDirection.Tower)
                {
                    _ArcherUp = collision.gameObject;
                    _ArcherUp_Obj.SetActive(true);
                }

                collision.gameObject.SetActive(false);
            }
        }
    }
}
