using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyPanel : MonoBehaviour
{
    [SerializeField] private UnitInventory _type;
    [SerializeField] private TextMeshProUGUI _txt_price;
    [SerializeField] private int _price;
    [SerializeField] private int ID;
    [SerializeField] private Button _buttonYes;

    [SerializeField] private Image _Show;
    [SerializeField] private InventoryUnitSprite _sprite;

    [SerializeField] private GameObject _buy;
    [SerializeField] private GameObject _buyPanel;

    [SerializeField] private bool _Buy = false;
    [SerializeField] private bool _onDis = false;

    [SerializeField] private LoadInventoryUnit _load;

    private Animator _anim;

    void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0) && _Buy && _onDis && !CursorManager.Instance.ChoseUI)
        {
            _anim.SetTrigger("exit");
        }
    }

    public void set(UnitInventory type, int price, int id)
    {
        _buy.SetActive(true);
        _type = type;
        _price = price;
        ID = id;
        _txt_price.text = $"-{_price}";
        if (_price <= SettingManager.Instance._gameSettings._coin)
            _buttonYes.interactable = true;
        else _buttonYes.interactable = false;
    }

    public void Buy()
    {
        _Buy = true;
        _anim.SetTrigger("exit");
    }

    public void checkBuy()
    {
        if (!_Buy)
        {
            gameObject.SetActive(false);
            return;
        }

        SettingManager.Instance._gameSettings._coin -= _price;
        MainMenu.Instance.UpdateCoin();

        switch (_type)
        {
            case UnitInventory.Castle:
                _Show.sprite = _sprite.GetCastleShowSprite(ID);
                SettingManager.Instance._gameSettings._listCastle.Add(ID);
                break;
            case UnitInventory.Tower:
                _Show.sprite = _sprite.GetTowerShowSprite(ID);
                SettingManager.Instance._gameSettings._listTower.Add(ID);
                break;
            case UnitInventory.Storage:
                _Show.sprite = _sprite.GetStorageShowSprite(ID);
                SettingManager.Instance._gameSettings._listStorage.Add(ID);
                break;
            case UnitInventory.Warrior:
                _Show.sprite = _sprite.GetWarriorShowSprite(ID);
                SettingManager.Instance._gameSettings._listWarrior.Add(ID);
                break;
            case UnitInventory.Archer:
                _Show.sprite = _sprite.GetArcherShowSprite(ID);
                SettingManager.Instance._gameSettings._listArcher.Add(ID);
                break;
            case UnitInventory.Lancer:
                _Show.sprite = _sprite.GetLancerShowSprite(ID);
                SettingManager.Instance._gameSettings._listLancer.Add(ID);
                break;
            case UnitInventory.Healer:
                _Show.sprite = _sprite.GetHealerShowSprite(ID);
                SettingManager.Instance._gameSettings._listHealer.Add(ID);
                break;
            case UnitInventory.TNT:
                _Show.sprite = _sprite.GetTNTShowSprite(ID);
                SettingManager.Instance._gameSettings._listTNT.Add(ID);
                break;
        }

        SettingManager.Instance.Save();
        _load.setUnit(_type);
    }

    public void _out() => _anim.SetTrigger("exit");

    public void onDis()
    {
        StartCoroutine(onDisCor());
    }
    
    private IEnumerator onDisCor()
    {
        yield return new WaitForSeconds(1.5f);
        _onDis = true;
    }

    public void setActive()
    {
        _Buy = false;
        _onDis = false;
        _buyPanel.SetActive(false);
        gameObject.SetActive(false);
    }
}
