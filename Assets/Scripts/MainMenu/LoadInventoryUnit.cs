using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadInventoryUnit : MonoBehaviour
{
    [SerializeField] private InventoryManager _manager;
    [SerializeField] private InventoryUnitSprite _sprite;
    [SerializeField] private Image _show;

    [System.Serializable]
    private class UnitSlot
    {
        public Image unitImage;
        public GameObject equipMark;
        public GameObject lockMark;
        public EquiUnit equipUnit;
        public TextMeshProUGUI price;
    }

    [SerializeField] private List<UnitSlot> _slots = new List<UnitSlot>(); // Gồm Unit1–4


    public void setUnit(UnitInventory unit)
    {
        switch (unit)
        {
            case UnitInventory.Castle:
                loadCastle();
                break;
            case UnitInventory.Tower:
                loadTower();
                break;
            case UnitInventory.Storage:
                loadStorage();
                break;
            case UnitInventory.Warrior:
                loadWarrior();
                break;
            case UnitInventory.Archer:
                loadArcher();
                break;
            case UnitInventory.Lancer:
                loadLancer();
                break;
            case UnitInventory.Healer:
                loadHealer();
                break;
            case UnitInventory.TNT:
                loadTNT();
                break;
        }
    }


    #region Castle
    public void loadCastle()
    {
        int current = SettingManager.Instance._gameSettings._currentCastle;
        List<int> unlocked = SettingManager.Instance._gameSettings._listCastle;

        // Load sprite hiển thị
        _show.sprite = _sprite.GetCastleShowSprite(current);

        // Gán trạng thái cho từng slot
        for (int i = 0; i < _slots.Count; i++)
        {
            int id = i + 1;
            bool isUnlocked = unlocked.Contains(id);
            bool isEquipped = (id == current);
            //Debug.Log($"id[{id}] unlock[{isUnlocked}] equi[{isEquipped}]");

            var slot = _slots[i];
            slot.price.gameObject.SetActive(!isUnlocked);
            slot.price.text = _manager.getPrice_castle(id).ToString();
            slot.equipUnit.setButton(UnitInventory.Castle, id, isUnlocked, this, _manager.getPrice_castle(id));
            slot.unitImage.sprite = _sprite.GetCastleUnitSprite(id);
            if (slot.lockMark != null)
                slot.lockMark.SetActive(!isUnlocked);
            slot.equipMark.SetActive(isUnlocked && isEquipped);
        }
    }
    #endregion


    #region Tower
    public void loadTower()
    {
        int current = SettingManager.Instance._gameSettings._currentTower;
        List<int> unlocked = SettingManager.Instance._gameSettings._listTower;

        // Load sprite hiển thị
        _show.sprite = _sprite.GetTowerShowSprite(current);

        // Gán trạng thái cho từng slot
        for (int i = 0; i < _slots.Count; i++)
        {
            int id = i + 1;
            bool isUnlocked = unlocked.Contains(id);
            bool isEquipped = (id == current);

            var slot = _slots[i];
            slot.equipUnit.setButton(UnitInventory.Castle, id, isUnlocked, this, _manager.getPrice_castle(id));
            slot.price.gameObject.SetActive(!isUnlocked);
            slot.price.text = _manager.getPrice_tower(id).ToString();
            slot.unitImage.sprite = _sprite.GetTowerUnitSprite(id);
            if (slot.lockMark != null)
                slot.lockMark.SetActive(!isUnlocked);
            slot.equipMark.SetActive(isUnlocked && isEquipped);
        }
    }
    #endregion


    #region Storage
    public void loadStorage()
    {
        int current = SettingManager.Instance._gameSettings._currentStorage;
        List<int> unlocked = SettingManager.Instance._gameSettings._listStorage;

        // Load sprite hiển thị
        _show.sprite = _sprite.GetStorageShowSprite(current);

        // Gán trạng thái cho từng slot
        for (int i = 0; i < _slots.Count; i++)
        {
            int id = i + 1;
            bool isUnlocked = unlocked.Contains(id);
            bool isEquipped = (id == current);

            var slot = _slots[i];
            slot.equipUnit.setButton(UnitInventory.Castle, id, isUnlocked, this, _manager.getPrice_castle(id));
            slot.price.gameObject.SetActive(!isUnlocked);
            slot.price.text = _manager.getPrice_storage(id).ToString();
            slot.unitImage.sprite = _sprite.GetStorageUnitSprite(id);
            if (slot.lockMark != null)
                slot.lockMark.SetActive(!isUnlocked);
            slot.equipMark.SetActive(isUnlocked && isEquipped);
        }
    }
    #endregion


    #region Warrior
    public void loadWarrior()
    {
        int current = SettingManager.Instance._gameSettings._currentWarrior;
        List<int> unlocked = SettingManager.Instance._gameSettings._listWarrior;

        // Load sprite hiển thị
        _show.sprite = _sprite.GetWarriorShowSprite(current);

        // Gán trạng thái cho từng slot
        for (int i = 0; i < _slots.Count; i++)
        {
            int id = i + 1;
            bool isUnlocked = unlocked.Contains(id);
            bool isEquipped = (id == current);

            var slot = _slots[i];
            slot.equipUnit.setButton(UnitInventory.Castle, id, isUnlocked, this, _manager.getPrice_castle(id));
            slot.price.gameObject.SetActive(!isUnlocked);
            slot.price.text = _manager.getPrice_warrior(id).ToString();
            slot.unitImage.sprite = _sprite.GetWarriorUnitSprite(id);
            if (slot.lockMark != null)
                slot.lockMark.SetActive(!isUnlocked);
            slot.equipMark.SetActive(isUnlocked && isEquipped);
        }
    }
    #endregion


    #region Archer
    public void loadArcher()
    {
        int current = SettingManager.Instance._gameSettings._currentArcher;
        List<int> unlocked = SettingManager.Instance._gameSettings._listArcher;

        // Load sprite hiển thị
        _show.sprite = _sprite.GetArcherShowSprite(current);

        // Gán trạng thái cho từng slot
        for (int i = 0; i < _slots.Count; i++)
        {
            int id = i + 1;
            bool isUnlocked = unlocked.Contains(id);
            bool isEquipped = (id == current);

            var slot = _slots[i];
            slot.equipUnit.setButton(UnitInventory.Castle, id, isUnlocked, this, _manager.getPrice_castle(id));
            slot.price.gameObject.SetActive(!isUnlocked);
            slot.price.text = _manager.getPrice_archer(id).ToString();
            slot.unitImage.sprite = _sprite.GetArcherUnitSprite(id);
            if (slot.lockMark != null)
                slot.lockMark.SetActive(!isUnlocked);
            slot.equipMark.SetActive(isUnlocked && isEquipped);
        }
    }
    #endregion


    #region Lancer
    public void loadLancer()
    {
        int current = SettingManager.Instance._gameSettings._currentLancer;
        List<int> unlocked = SettingManager.Instance._gameSettings._listLancer;

        // Load sprite hiển thị
        _show.sprite = _sprite.GetLancerShowSprite(current);

        // Gán trạng thái cho từng slot
        for (int i = 0; i < _slots.Count; i++)
        {
            int id = i + 1;
            bool isUnlocked = unlocked.Contains(id);
            bool isEquipped = (id == current);

            var slot = _slots[i];
            slot.equipUnit.setButton(UnitInventory.Castle, id, isUnlocked, this, _manager.getPrice_castle(id));
            slot.price.gameObject.SetActive(!isUnlocked);
            slot.price.text = _manager.getPrice_lancer(id).ToString();
            slot.unitImage.sprite = _sprite.GetLancerUnitSprite(id);
            if (slot.lockMark != null)
                slot.lockMark.SetActive(!isUnlocked);
            slot.equipMark.SetActive(isUnlocked && isEquipped);
        }
    }
    #endregion


    #region Healer
    public void loadHealer()
    {
        int current = SettingManager.Instance._gameSettings._currentHealer;
        List<int> unlocked = SettingManager.Instance._gameSettings._listHealer;

        // Load sprite hiển thị
        _show.sprite = _sprite.GetHealerShowSprite(current);

        // Gán trạng thái cho từng slot
        for (int i = 0; i < _slots.Count; i++)
        {
            int id = i + 1;
            bool isUnlocked = unlocked.Contains(id);
            bool isEquipped = (id == current);

            var slot = _slots[i];
            slot.equipUnit.setButton(UnitInventory.Castle, id, isUnlocked, this, _manager.getPrice_castle(id));
            slot.price.gameObject.SetActive(!isUnlocked);
            slot.price.text = _manager.getPrice_healer(id).ToString();
            slot.unitImage.sprite = _sprite.GetHealerUnitSprite(id);
            if (slot.lockMark != null)
                slot.lockMark.SetActive(!isUnlocked);
            slot.equipMark.SetActive(isUnlocked && isEquipped);
        }
    }
    #endregion


    #region TNT
    public void loadTNT()
    {
        int current = SettingManager.Instance._gameSettings._currentTNT;
        List<int> unlocked = SettingManager.Instance._gameSettings._listTNT;

        // Load sprite hiển thị
        _show.sprite = _sprite.GetTNTShowSprite(current);

        // Gán trạng thái cho từng slot
        for (int i = 0; i < _slots.Count; i++)
        {
            int id = i + 1;
            bool isUnlocked = unlocked.Contains(id);
            bool isEquipped = (id == current);

            var slot = _slots[i];
            slot.equipUnit.setButton(UnitInventory.Castle, id, isUnlocked, this, _manager.getPrice_castle(id));
            slot.price.gameObject.SetActive(!isUnlocked);
            slot.price.text = _manager.getPrice_TNT(id).ToString();
            slot.unitImage.sprite = _sprite.GetTNTUnitSprite(id);
            if (slot.lockMark != null)
                slot.lockMark.SetActive(!isUnlocked);
            slot.equipMark.SetActive(isUnlocked && isEquipped);
        }
    }
    #endregion
}
