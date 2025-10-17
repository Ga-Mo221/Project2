using System.Collections;
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

    [Foldout("Other")]
    [SerializeField] private BuidingFire _fire;
    [Foldout("Other")]
    [HideIf(nameof(IsCastle))]
    [SerializeField] private Image _imgLoad;
    [Foldout("Other")]
    [ShowIf(nameof(IsTower))]
    [SerializeField] private GameObject _inPos;
    [Foldout("Other")]
    [ShowIf(nameof(IsStorage))]
    [SerializeField] private GameObject _inPos1;
    [Foldout("Other")]
    [ShowIf(nameof(IsStorage))]
    [SerializeField] private GameObject _inPos2;
    [Foldout("Other")]
    [ShowIf(nameof(IsStorage))]
    [SerializeField] private GameObject _inPos3;
    [Foldout("Other")]
    [HideIf(nameof(IsCastle))]
    [SerializeField] private Rada _rada;
    [HideIf(nameof(IsCastle))]
    public House _house;


    [Foldout("Other")]
    [ShowIf(nameof(IsTower))]
    [SerializeField] private GameObject _ButtonArcherUP; // canvas
    [Foldout("Other")]
    [HideIf(nameof(IsCastle))]
    [SerializeField] private GameObject _light;
    [Foldout("Other")]
    [HideIf(nameof(IsCastle))]
    [SerializeField] private GameObject _canvas; // button x and v
    [Foldout("Other")]
    [HideIf(nameof(IsCastle))]
    [SerializeField] private GameObject _canCreate;
    [Foldout("Other")]
    [HideIf(nameof(IsCastle))]
    [SerializeField] private GameObject _HPcanvas;

    [Foldout("Other")]
    [SerializeField] private OutLine _outLine;



    public void setCanDetec(bool amount) => _canDetec = amount;
    public bool getCanDetec() => _canDetec;

    [HideIf(nameof(IsCastle))]
    [SerializeField] private int _time = 0;

    void Start()
    {
        if (!IsCastle)
        {
            if (_house == null)
                _house = GetComponent<House>();
            _rada.setOn(false);
            _time = _house._createTimeSec;
        }
        else _canDetec = true;
        _anim = GetComponent<Animator>();
        if (_HPcanvas == null && !IsCastle)
            Debug.LogError($"[{transform.name}] [HouseHealth] Chưa gán 'GameObject HPbar'!");
        else if (!IsCastle)
            _HPcanvas.SetActive(false);

        InvokeRepeating(nameof(fireDie), 1f, 1f);
    }


    #region Load Count Create Buiding
    public void loadCount()
    {
        if (_time > 0)
        {
            _time--;
            _imgLoad.fillAmount = Mathf.Clamp01((float)_time / _house._createTimeSec);
        }

        if (_time <= 0)
        {
            _anim.SetBool("Idle", false);
            _rada.setOn(true);
            _light.SetActive(true);
            _imgLoad.fillAmount = 0f; // chắc chắn thanh load về 0
            _canvas.SetActive(false);
            _canCreate.SetActive(false);
            _house._audio.StopCreatingSound();
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

                CastleManager.Instance.Castle._maxWood += _house._wood;
                CastleManager.Instance.Castle._maxRock += _house._rock;
                CastleManager.Instance.Castle._maxMeat += _house._meat;
                CastleManager.Instance.Castle._maxGold += _house._gold;

                _house._new = false;
                GameManager.Instance.UIupdateReferences();
            }
        }
    }
    #endregion


    #region  Take Damage
    public void takeDamage(float damage)
    {
        GameManager.Instance.onDefen();
        if (!IsCastle)
        {
            _house._currentHealth -= damage;
            _house.updateHP();
            if (_house._currentHealth <= 0)
            {
                _house.setDie(true);
                _rada.setDie(true);
                _house._currentHealth = 0;
                CameraShake.Instance.ShakeCamera(0.5f, 0.3f);
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
                    _canvas.SetActive(false);
                }
                if (IsStorage)
                {
                    CastleManager.Instance.Castle._maxWood -= _house._wood;
                    CastleManager.Instance.Castle._maxRock -= _house._rock;
                    CastleManager.Instance.Castle._maxMeat -= _house._meat;
                    CastleManager.Instance.Castle._maxGold -= _house._gold;

                    int wood = CastleManager.Instance.Castle._wood / (CastleManager.Instance.Castle._storageList.Count + 1);
                    int rock = CastleManager.Instance.Castle._rock / (CastleManager.Instance.Castle._storageList.Count + 1);
                    int meat = CastleManager.Instance.Castle._meat / (CastleManager.Instance.Castle._storageList.Count + 1);
                    int gold = CastleManager.Instance.Castle._gold / (CastleManager.Instance.Castle._storageList.Count + 1);

                    if (wood > _house._wood) wood = _house._wood;
                    if (rock > _house._rock) rock = _house._rock;
                    if (meat > _house._meat) meat = _house._meat;
                    if (gold > _house._gold) gold = _house._gold;

                    CastleManager.Instance.Castle._wood -= wood;
                    CastleManager.Instance.Castle._rock -= rock;
                    CastleManager.Instance.Castle._meat -= meat;
                    CastleManager.Instance.Castle._gold -= gold;

                    GameManager.Instance.UIupdateReferences();
                }
                _light.SetActive(false);
                _HPcanvas.SetActive(false);
                _house._audio.PlayDieSound();
                _outLine.Out();
            }

            _HPcanvas.SetActive(true);
            if (_hideHP != null)
                StopCoroutine(_hideHP);
            _hideHP = StartCoroutine(hideHP());
        }
        else
        {
            CastleManager.Instance.Castle._currentHealth -= damage;
            GameManager.Instance.UIupdateHPCastle();
            if (CastleManager.Instance.Castle._currentHealth <= 0)
            {
                CameraShake.Instance.ShakeCamera(1f, 0.5f);
                _anim.SetBool("Die", true);
                setCanDetec(false);
                StartCoroutine(GameOver());
                _outLine.Out();
                CastleManager.Instance.Castle._audio.PlayDieSound();
                CastleManager.Instance.Castle.Dead();
            }
        }
    }
    #endregion


    private Coroutine _hideHP;
    private IEnumerator hideHP()
    {
        if (!_canDetec)
            _HPcanvas.SetActive(false);
        yield return new WaitForSeconds(5.5f);
        _HPcanvas.SetActive(false);
    }


    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(GameManager.Instance._displayGameOverTime);
        if (GameManager.Instance.TutorialWar && !TutorialSetUp.Instance._Win)
        {
            TutorialSetUp.Instance.GameLose();
        }
        else
        {
            GameManager.Instance.setWin(false);
            GameManager.Instance.setGameOver(true);
        }
    }


    #region Display Fire Die
    private void fireDie()
    {
        float currentHealth;
        float maxHealth;
        if (!_canDetec)
        {
            _fire.gameObject.SetActive(false);
            return;
        }
        if (!IsCastle)
        {
            currentHealth = _house._currentHealth;
            maxHealth = _house._maxHealth;
        }
        else if (CastleManager.Instance.Castle != null)
        {
            currentHealth = CastleManager.Instance.Castle._currentHealth;
            maxHealth = CastleManager.Instance.Castle._maxHealth;
        }
        else
        {
            currentHealth = 100;
            maxHealth = 100;
        }


        if (currentHealth / maxHealth < 0.3)
        {
            _fire.gameObject.SetActive(true);
            if (!IsCastle)
                _house._audio.PlayFireSound();
            else
                CastleManager.Instance.Castle._audio.PlayFireSound();
        }
        else
        {
            _fire.gameObject.SetActive(false);
            if (!IsCastle)
                _house._audio.StopFireSound();
            else
                CastleManager.Instance.Castle._audio.StopFireSound();
        }
    }
    #endregion

    public void PlayCreatingSound() => _house._audio.PlayCreatingSound();
}
