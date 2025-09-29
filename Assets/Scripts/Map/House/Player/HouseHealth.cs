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
    
    [HideIf(nameof(IsCastle))]
    [SerializeField] private Image _imgLoad;
    [ShowIf(nameof(IsTower))]
    [SerializeField] private GameObject _inPos;
    [ShowIf(nameof(IsStorage))]
    [SerializeField] private GameObject _inPos1;
    [ShowIf(nameof(IsStorage))]
    [SerializeField] private GameObject _inPos2;
    [ShowIf(nameof(IsStorage))]
    [SerializeField] private GameObject _inPos3;
    [HideIf(nameof(IsCastle))]
    [SerializeField] private Rada _rada;
    [ShowIf(nameof(IsStorage))]
    private House _house;


    [ShowIf(nameof(IsTower))]
    [SerializeField] private GameObject _ButtonArcherUP; // canvas
    [HideIf(nameof(IsCastle))]
    [SerializeField] private GameObject _light;
    [HideIf(nameof(IsCastle))]
    [SerializeField] private GameObject _canvas; // button x and v
    [HideIf(nameof(IsCastle))]
    [SerializeField] private GameObject _canCreate;
    [HideIf(nameof(IsCastle))]
    [SerializeField] private GameObject _HPcanvas;



    public void setCanDetec(bool amount) => _canDetec = amount;
    public bool getCanDetec() => _canDetec;

    [HideIf(nameof(IsCastle))]
    [SerializeField] private int _createTimeSec = 10;
    private int _time = 0;

    void Start()
    {
        if (!IsCastle)
        {
            _house = GetComponent<House>();
            _rada.setOn(false);
            _time = _createTimeSec;
        }
        else _canDetec = true;
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
            _canCreate.SetActive(false);
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

    public void takeDamage(float damage)
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
                    _ButtonArcherUP.SetActive(false);
                }
                if (IsStorage)
                {
                    Castle.Instance._maxWood -= _house._wood;
                    Castle.Instance._maxRock -= _house._rock;
                    Castle.Instance._maxMeat -= _house._meat;
                    Castle.Instance._maxGold -= _house._gold;

                    int wood = Castle.Instance._wood / (Castle.Instance._storageList.Count + 1);
                    int rock = Castle.Instance._rock / (Castle.Instance._storageList.Count + 1);
                    int meat = Castle.Instance._meat / (Castle.Instance._storageList.Count + 1);
                    int gold = Castle.Instance._gold / (Castle.Instance._storageList.Count + 1);

                    if (wood > _house._wood) wood = _house._wood;
                    if (rock > _house._rock) rock = _house._rock;
                    if (meat > _house._meat) meat = _house._meat;
                    if (gold > _house._gold) gold = _house._gold;
                    
                    Castle.Instance._wood -= wood;
                    Castle.Instance._rock -= rock;
                    Castle.Instance._meat -= meat;
                    Castle.Instance._gold -= gold;

                    GameManager.Instance.UIupdateReferences();
                }
                _light.SetActive(false);
                _HPcanvas.SetActive(false);
            }
        }
        else
        {
            Castle.Instance._currentHealth -= damage;
            GameManager.Instance.UIupdateHPCastle();
            if (Castle.Instance._currentHealth <= 0)
            {
                _anim.SetBool("Die", true);
                setCanDetec(false);
            }
        }
    }
}
