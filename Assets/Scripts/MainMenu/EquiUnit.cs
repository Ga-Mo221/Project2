using UnityEngine;

public class EquiUnit : MonoBehaviour
{
    [SerializeField] private UnitInventory _type;
    [SerializeField] private int ID = 1;
    [SerializeField] private bool isLock = false;
    [SerializeField] private LoadInventoryUnit _load;
    [SerializeField] private int price;

    [SerializeField] private BuyPanel _buyPanel;

    public void setButton(UnitInventory type, int id, bool _lock, LoadInventoryUnit load, int _price)
    {
        _type = type;
        ID = id;
        isLock = _lock;
        _load = load;
        price = _price;
    }


    public void click()
    {
        switch (_type)
        {
            case UnitInventory.Castle:
                if (isLock)
                {
                    if (SettingManager.Instance._gameSettings._currentCastle != ID)
                    {
                        SettingManager.Instance._gameSettings._currentCastle = ID;
                        _load.loadCastle();
                        SettingManager.Instance.Save();
                    }
                }
                else
                {
                    // mua
                    _buyPanel.set(UnitInventory.Castle, price, ID);
                    _buyPanel.gameObject.SetActive(true);
                }
                break;
            case UnitInventory.Tower:
                if (isLock)
                {
                    if (SettingManager.Instance._gameSettings._currentTower != ID)
                    {
                        SettingManager.Instance._gameSettings._currentTower = ID;
                        _load.loadTower();
                        SettingManager.Instance.Save();
                    }
                }
                else
                {
                    // mua
                    _buyPanel.set(UnitInventory.Tower, price, ID);
                    _buyPanel.gameObject.SetActive(true);
                }
                break;
            case UnitInventory.Storage:
                if (isLock)
                {
                    if (SettingManager.Instance._gameSettings._currentStorage != ID)
                    {
                        SettingManager.Instance._gameSettings._currentStorage = ID;
                        _load.loadStorage();
                        SettingManager.Instance.Save();
                    }
                }
                else
                {
                    // mua
                    _buyPanel.set(UnitInventory.Storage, price, ID);
                    _buyPanel.gameObject.SetActive(true);
                }
                break;
            case UnitInventory.Warrior:
                if (isLock)
                {
                    if (SettingManager.Instance._gameSettings._currentWarrior != ID)
                    {
                        SettingManager.Instance._gameSettings._currentWarrior = ID;
                        _load.loadWarrior();
                        SettingManager.Instance.Save();
                    }
                }
                else
                {
                    // mua
                    _buyPanel.set(UnitInventory.Warrior, price, ID);
                    _buyPanel.gameObject.SetActive(true);
                }
                break;
            case UnitInventory.Archer:
                if (isLock)
                {
                    if (SettingManager.Instance._gameSettings._currentArcher != ID)
                    {
                        SettingManager.Instance._gameSettings._currentArcher = ID;
                        _load.loadArcher();
                        SettingManager.Instance.Save();
                    }
                }
                else
                {
                    // mua
                    _buyPanel.set(UnitInventory.Archer, price, ID);
                    _buyPanel.gameObject.SetActive(true);
                }
                break;
            case UnitInventory.Lancer:
                if (isLock)
                {
                    if (SettingManager.Instance._gameSettings._currentLancer != ID)
                    {
                        SettingManager.Instance._gameSettings._currentLancer = ID;
                        _load.loadLancer();
                        SettingManager.Instance.Save();
                    }
                }
                else
                {
                    // mua
                    _buyPanel.set(UnitInventory.Lancer, price, ID);
                    _buyPanel.gameObject.SetActive(true);
                }
                break;
            case UnitInventory.Healer:
                if (isLock)
                {
                    if (SettingManager.Instance._gameSettings._currentHealer != ID)
                    {
                        SettingManager.Instance._gameSettings._currentHealer = ID;
                        _load.loadHealer();
                        SettingManager.Instance.Save();
                    }
                }
                else
                {
                    // mua
                    _buyPanel.set(UnitInventory.Healer, price, ID);
                    _buyPanel.gameObject.SetActive(true);
                }
                break;
            case UnitInventory.TNT:
                if (isLock)
                {
                    if (SettingManager.Instance._gameSettings._currentTNT != ID)
                    {
                        SettingManager.Instance._gameSettings._currentTNT = ID;
                        _load.loadTNT();
                        SettingManager.Instance.Save();
                    }
                }
                else
                {
                    // mua
                    _buyPanel.set(UnitInventory.TNT, price, ID);
                    _buyPanel.gameObject.SetActive(true);
                }
                break;
        }
    }
}
