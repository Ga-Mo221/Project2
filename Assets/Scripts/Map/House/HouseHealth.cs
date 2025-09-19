using UnityEngine;
using UnityEngine.UI;

public class HouseHealth : MonoBehaviour
{
    private Animator _anim;
    [SerializeField] private bool _canDetec = false;
    [SerializeField] private Image _imgLoad;
    [SerializeField] private GameObject _inPos;
    [SerializeField] private GameObject _light;
    [SerializeField] private GameObject _canvas;

    public void setCanDetec(bool amount) => _canDetec = amount;
    public bool getCanDetec() => _canDetec;

    [SerializeField] private int _createTimeSec = 10;
    private int _time = 0;

    void Start()
    {
        _time = _createTimeSec;
        _anim = GetComponent<Animator>();
    }

    public void loadCount()
    {
        if (_time > 0)
        {
            _time--;
            _imgLoad.fillAmount = Mathf.Clamp01((float)_time / _createTimeSec);
        }

        if (_time <= 0)
        {
            _anim.SetBool("Idle", false);
            _inPos.SetActive(true);
            _light.SetActive(true);
            _imgLoad.fillAmount = 0f; // chắc chắn thanh load về 0
            _canvas.SetActive(false);
        }
    }
}
