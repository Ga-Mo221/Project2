using UnityEngine;

public class InTower : MonoBehaviour
{
    public GameObject _ArcherUp;
    public GameObject _ArcherUp_Obj;

    public GameObject _cache;
    [SerializeField] private UnitAudio _audio;

    void Start()
    {
        _ArcherUp_Obj.SetActive(false);
    }


    public void ButtonDown()
    {
        if (SelectionBox.Instance.chosen.Count == 1
        && _ArcherUp == null)
        {
            if (!SelectionBox.Instance.chosen[0].CompareTag("Archer")) return;
            _cache = SelectionBox.Instance.chosen[0];
            var _scrip = _cache.GetComponent<ArcherGFX>();
            _scrip.setTarget(transform.position, true);
            _scrip._In_Castle = true;
            _scrip._upDirection = UpDirection.Tower;
            _scrip._movingToTower = true;
        }
        else if (_ArcherUp != null)
        {
            Out();
        }
    }

    public void Out()
    {
        _ArcherUp_Obj.SetActive(false);
        var _scrip = _ArcherUp.GetComponent<ArcherGFX>();
        _scrip.target = null;
        _scrip.setTargetPos(transform);
        _scrip._In_Castle = false;
        _scrip.setUpTower(false);
        _ArcherUp.SetActive(true);
        _scrip.setIsAI(true);
        _ArcherUp = null;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;
        if (collision.CompareTag("Archer"))
        {
            var _script = collision.GetComponent<ArcherGFX>();
            if (_script._In_Castle)
            {
                if (_script._upDirection == UpDirection.Tower && _cache == collision.gameObject)
                {
                    _ArcherUp = collision.gameObject;
                    _ArcherUp_Obj.SetActive(true);
                    _script.setUpTower(true);
                    collision.gameObject.SetActive(false);
                    _audio.PlayArcherUpSound();
                }
            }
        }
    }
}
