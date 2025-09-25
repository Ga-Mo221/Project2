using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public enum HouseType
{
    Tower,
    Storage,
    Castle
}

public class HouseHealth : MonoBehaviour
{
    [SerializeField] private HouseType _type;
    private bool IsTower => _type == HouseType.Tower;
    private bool IsStorage => _type == HouseType.Storage;
    private bool IsCastle => _type == HouseType.Castle;
    private Animator _anim;
    [SerializeField] private bool _canDetec = false;
    [ShowIf(nameof(IsTower))]
    [SerializeField] private GameObject _ButtonArcherUP;
    [SerializeField] private Image _imgLoad;
    [ShowIf(nameof(IsTower))]
    [SerializeField] private GameObject _inPos;
    [ShowIf(nameof(IsStorage))]
    [SerializeField] private GameObject _inPos1;
    [ShowIf(nameof(IsStorage))]
    [SerializeField] private GameObject _inPos2;
    [ShowIf(nameof(IsStorage))]
    [SerializeField] private GameObject _inPos3;
    [SerializeField] private GameObject _light;
    [SerializeField] private GameObject _canvas;
    [SerializeField] private Rada _rada;
    [ShowIf(nameof(IsStorage))]
    private House _house;

    public void setCanDetec(bool amount) => _canDetec = amount;
    public bool getCanDetec() => _canDetec;

    [SerializeField] private int _createTimeSec = 10;
    private int _time = 0;

    void Start()
    {
        _house = GetComponent<House>();
        _rada.setOn(false);
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
            _rada.setOn(true);
            _light.SetActive(true);
            _imgLoad.fillAmount = 0f; // chắc chắn thanh load về 0
            _canvas.SetActive(false);
            if (IsTower)
            {
                _ButtonArcherUP.SetActive(true);
                _inPos.SetActive(true);
            }
            if (IsStorage)
            {
                int type = _anim.GetInteger("Type");
                if (type == 1)
                    _inPos1.SetActive(true);
                if (type == 2)
                    _inPos2.SetActive(true);
                if (type == 3)
                    _inPos3.SetActive(true);
                _house.setActive(true);

                Castle.Instance._maxWood += _house._wood;
                Castle.Instance._maxRock += _house._rock;
                Castle.Instance._maxMeat += _house._meat;
                Castle.Instance._maxGold += _house._gold;

                _house._new = false;
                GameManager.Instance.UIupdateReferences();
            }
        }
    }

    public void takeDamage(int damage)
    {
        if (!IsCastle)
        {
            _house._currentHealth -= damage;
            _house.updateHP();
            if (_house._currentHealth <= 0)
            {
                _anim.SetBool("Die", true);
                setCanDetec(false);
                if (IsTower)
                {
                    _house._inTower._ArcherUp_Obj.SetActive(false);
                    if (_house._inTower._ArcherUp != null)
                    {
                        _house._inTower.Out();
                    }
                }
            }
        }
    }
}
