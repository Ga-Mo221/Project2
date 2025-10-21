using NaughtyAttributes;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private bool _isOpenUnitInventory = false;
    [SerializeField] private UnitInventory _currenUnitInventory;

    [Header("Gameobj")]
    [SerializeField] private GameObject _buttons;
    [SerializeField] private GameObject _allInventoryPanel;
    [SerializeField] private GameObject _buttonSetting;
    [SerializeField] private GameObject _buttonInventory;
    [SerializeField] private GameObject _buttonStart;
    [SerializeField] private GameObject _buttonMultiplayer;

    [SerializeField] private Animator _anim;

    [Foldout("Price Unit")]
    [Header("Castle")]
    public int _castle_Unit2_price = 1000;
    [Foldout("Price Unit")]
    public int _castle_Unit3_price = 2000;
    [Foldout("Price Unit")]
    public int _castle_Unit4_price = 3000;

    [Foldout("Price Unit")]
    [Header("Tower")]
    public int _tower_Unit2_price = 1000;
    [Foldout("Price Unit")]
    public int _tower_Unit3_price = 2000;
    [Foldout("Price Unit")]
    public int _tower_Unit4_price = 3000;

    [Foldout("Price Unit")]
    [Header("Storage")]
    public int _storage_Unit2_price = 1000;
    [Foldout("Price Unit")]
    public int _storage_Unit3_price = 2000;
    [Foldout("Price Unit")]
    public int _storage_Unit4_price = 3000;

    [Foldout("Price Unit")]
    [Header("Warrior")]
    public int _warrior_Unit2_price = 1000;
    [Foldout("Price Unit")]
    public int _warrior_Unit3_price = 2000;
    [Foldout("Price Unit")]
    public int _warrior_Unit4_price = 3000;

    [Foldout("Price Unit")]
    [Header("Archer")]
    public int _archer_Unit2_price = 1000;
    [Foldout("Price Unit")]
    public int _archer_Unit3_price = 2000;
    [Foldout("Price Unit")]
    public int _archer_Unit4_price = 3000;

    [Foldout("Price Unit")]
    [Header("Lancer")]
    public int _lancer_Unit2_price = 1000;
    [Foldout("Price Unit")]
    public int _lancer_Unit3_price = 2000;
    [Foldout("Price Unit")]
    public int _lancer_Unit4_price = 3000;

    [Foldout("Price Unit")]
    [Header("Healer")]
    public int _healer_Unit2_price = 1000;
    [Foldout("Price Unit")]
    public int _healer_Unit3_price = 2000;
    [Foldout("Price Unit")]
    public int _healer_Unit4_price = 3000;

    [Foldout("Price Unit")]
    [Header("TNT")]
    public int _TNT_Unit2_price = 1000;
    [Foldout("Price Unit")]
    public int _TNT_Unit3_price = 2000;
    [Foldout("Price Unit")]
    public int _TNT_Unit4_price = 3000;

    public void OpenInventory() => _isOpenUnitInventory = true;
    public void setUnitInventory(UnitInventory _unit) => _currenUnitInventory = _unit;
    public UnitInventory getUnitInventory() => _currenUnitInventory;

    void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    public void Exit()
    {
        if (_isOpenUnitInventory)
        {
            _buttons.SetActive(true);
            _allInventoryPanel.SetActive(false);
            _isOpenUnitInventory = false;
        }
        else
        {
            _buttonInventory.SetActive(true);
            _buttonMultiplayer.SetActive(true);
            _buttonSetting.SetActive(true);
            _buttonStart.SetActive(true);
            _anim.SetTrigger("exit");
        }
    }

    public void setActive() => gameObject.SetActive(false);

    public int getPrice_castle(int id)
    {
        switch (id)
        {
            case 2: return _castle_Unit2_price;
            case 3: return _castle_Unit3_price;
            case 4: return _castle_Unit4_price;
            default: return 0;
        }
    }

    public int getPrice_tower(int id)
    {
        switch (id)
        {
            case 2: return _tower_Unit2_price;
            case 3: return _tower_Unit3_price;
            case 4: return _tower_Unit4_price;
            default: return 0;
        }
    }

    public int getPrice_storage(int id)
    {
        switch (id)
        {
            case 2: return _storage_Unit2_price;
            case 3: return _storage_Unit3_price;
            case 4: return _storage_Unit4_price;
            default: return 0;
        }
    }

    public int getPrice_warrior(int id)
    {
        switch (id)
        {
            case 2: return _warrior_Unit2_price;
            case 3: return _warrior_Unit3_price;
            case 4: return _warrior_Unit4_price;
            default: return 0;
        }
    }

    public int getPrice_archer(int id)
    {
        switch (id)
        {
            case 2: return _archer_Unit2_price;
            case 3: return _archer_Unit3_price;
            case 4: return _archer_Unit4_price;
            default: return 0;
        }
    }

    public int getPrice_lancer(int id)
    {
        switch (id)
        {
            case 2: return _lancer_Unit2_price;
            case 3: return _lancer_Unit3_price;
            case 4: return _lancer_Unit4_price;
            default: return 0;
        }
    }
    
    public int getPrice_healer(int id)
    {
        switch (id)
        {
            case 2: return _healer_Unit2_price;
            case 3: return _healer_Unit3_price;
            case 4: return _healer_Unit4_price;
            default: return 0;
        }
    }

    public int getPrice_TNT(int id)
    {
        switch (id)
        {
            case 2: return _TNT_Unit2_price;
            case 3: return _TNT_Unit3_price;
            case 4: return _TNT_Unit4_price;
            default: return 0;
        }
    }
}

public enum UnitInventory
{
    Castle,
    Tower,
    Storage,
    Warrior,
    Archer,
    Lancer,
    Healer,
    TNT
}
