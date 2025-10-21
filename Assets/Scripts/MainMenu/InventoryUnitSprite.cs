using NaughtyAttributes;
using UnityEngine;

public class InventoryUnitSprite : MonoBehaviour
{
    [Foldout("Sprite")]
    [Header("Castle")]
    public Sprite _castle_button_1;
    [Foldout("Sprite")]
    public Sprite _castle_show_1;
    [Foldout("Sprite")]
    public Sprite _castle_button_2;
    [Foldout("Sprite")]
    public Sprite _castle_show_2;
    [Foldout("Sprite")]
    public Sprite _castle_button_3;
    [Foldout("Sprite")]
    public Sprite _castle_show_3;
    [Foldout("Sprite")]
    public Sprite _castle_button_4;
    [Foldout("Sprite")]
    public Sprite _castle_show_4;
    
    [Foldout("Sprite")]
    [Header("Tower")]
    public Sprite _tower_button_1;
    [Foldout("Sprite")]
    public Sprite _tower_show_1;
    [Foldout("Sprite")]
    public Sprite _tower_button_2;
    [Foldout("Sprite")]
    public Sprite _tower_show_2;
    [Foldout("Sprite")]
    public Sprite _tower_button_3;
    [Foldout("Sprite")]
    public Sprite _tower_show_3;
    [Foldout("Sprite")]
    public Sprite _tower_button_4;
    [Foldout("Sprite")]
    public Sprite _tower_show_4;

    [Foldout("Sprite")]
    [Header("Storage")]
    public Sprite _storage_button_1;
    [Foldout("Sprite")]
    public Sprite _storage_show_1;
    [Foldout("Sprite")]
    public Sprite _storage_button_2;
    [Foldout("Sprite")]
    public Sprite _storage_show_2;
    [Foldout("Sprite")]
    public Sprite _storage_button_3;
    [Foldout("Sprite")]
    public Sprite _storage_show_3;
    [Foldout("Sprite")]
    public Sprite _storage_button_4;
    [Foldout("Sprite")]
    public Sprite _storage_show_4;

    [Foldout("Sprite")]
    [Header("Warrior")]
    public Sprite _warrior_button_1;
    [Foldout("Sprite")]
    public Sprite _warrior_show_1;
    [Foldout("Sprite")]
    public Sprite _warrior_button_2;
    [Foldout("Sprite")]
    public Sprite _warrior_show_2;
    [Foldout("Sprite")]
    public Sprite _warrior_button_3;
    [Foldout("Sprite")]
    public Sprite _warrior_show_3;
    [Foldout("Sprite")]
    public Sprite _warrior_button_4;
    [Foldout("Sprite")]
    public Sprite _warrior_show_4;

    [Foldout("Sprite")]
    [Header("Archer")]
    public Sprite _archer_button_1;
    [Foldout("Sprite")]
    public Sprite _archer_show_1;
    [Foldout("Sprite")]
    public Sprite _archer_button_2;
    [Foldout("Sprite")]
    public Sprite _archer_show_2;
    [Foldout("Sprite")]
    public Sprite _archer_button_3;
    [Foldout("Sprite")]
    public Sprite _archer_show_3;
    [Foldout("Sprite")]
    public Sprite _archer_button_4;
    [Foldout("Sprite")]
    public Sprite _archer_show_4;

    [Foldout("Sprite")]
    [Header("Lancer")]
    public Sprite _lancer_button_1;
    [Foldout("Sprite")]
    public Sprite _lancer_show_1;
    [Foldout("Sprite")]
    public Sprite _lancer_button_2;
    [Foldout("Sprite")]
    public Sprite _lancer_show_2;
    [Foldout("Sprite")]
    public Sprite _lancer_button_3;
    [Foldout("Sprite")]
    public Sprite _lancer_show_3;
    [Foldout("Sprite")]
    public Sprite _lancer_button_4;
    [Foldout("Sprite")]
    public Sprite _lancer_show_4;

    [Foldout("Sprite")]
    [Header("Healer")]
    public Sprite _healer_button_1;
    [Foldout("Sprite")]
    public Sprite _healer_show_1;
    [Foldout("Sprite")]
    public Sprite _healer_button_2;
    [Foldout("Sprite")]
    public Sprite _healer_show_2;
    [Foldout("Sprite")]
    public Sprite _healer_button_3;
    [Foldout("Sprite")]
    public Sprite _healer_show_3;
    [Foldout("Sprite")]
    public Sprite _healer_button_4;
    [Foldout("Sprite")]
    public Sprite _healer_show_4;

    [Foldout("Sprite")]
    [Header("TNT")]
    public Sprite _TNT_button_1;
    [Foldout("Sprite")]
    public Sprite _TNT_show_1;
    [Foldout("Sprite")]
    public Sprite _TNT_button_2;
    [Foldout("Sprite")]
    public Sprite _TNT_show_2;
    [Foldout("Sprite")]
    public Sprite _TNT_button_3;
    [Foldout("Sprite")]
    public Sprite _TNT_show_3;
    [Foldout("Sprite")]
    public Sprite _TNT_button_4;
    [Foldout("Sprite")]
    public Sprite _TNT_show_4;


    public Sprite GetCastleShowSprite(int id)
    {
        switch (id)
        {
            case 1: return _castle_show_1;
            case 2: return _castle_show_2;
            case 3: return _castle_show_3;
            case 4: return _castle_show_4;
            default: return null;
        }
    }
    public Sprite GetCastleUnitSprite(int id)
    {
        switch (id)
        {
            case 1: return _castle_button_1;
            case 2: return _castle_button_2;
            case 3: return _castle_button_3;
            case 4: return _castle_button_4;
            default: return null;
        }
    }

    public Sprite GetTowerShowSprite(int id)
    {
        switch (id)
        {
            case 1: return _tower_show_1;
            case 2: return _tower_show_2;
            case 3: return _tower_show_3;
            case 4: return _tower_show_4;
            default: return null;
        }
    }
    public Sprite GetTowerUnitSprite(int id)
    {
        switch (id)
        {
            case 1: return _tower_button_1;
            case 2: return _tower_button_2;
            case 3: return _tower_button_3;
            case 4: return _tower_button_4;
            default: return null;
        }
    }

    public Sprite GetStorageShowSprite(int id)
    {
        switch (id)
        {
            case 1: return _storage_show_1;
            case 2: return _storage_show_2;
            case 3: return _storage_show_3;
            case 4: return _storage_show_4;
            default: return null;
        }
    }
    public Sprite GetStorageUnitSprite(int id)
    {
        switch (id)
        {
            case 1: return _storage_button_1;
            case 2: return _storage_button_2;
            case 3: return _storage_button_3;
            case 4: return _storage_button_4;
            default: return null;
        }
    }

    public Sprite GetWarriorShowSprite(int id)
    {
        switch (id)
        {
            case 1: return _warrior_show_1;
            case 2: return _warrior_show_2;
            case 3: return _warrior_show_3;
            case 4: return _warrior_show_4;
            default: return null;
        }
    }
    public Sprite GetWarriorUnitSprite(int id)
    {
        switch (id)
        {
            case 1: return _warrior_button_1;
            case 2: return _warrior_button_2;
            case 3: return _warrior_button_3;
            case 4: return _warrior_button_4;
            default: return null;
        }
    }

    public Sprite GetArcherShowSprite(int id)
    {
        switch (id)
        {
            case 1: return _archer_show_1;
            case 2: return _archer_show_2;
            case 3: return _archer_show_3;
            case 4: return _archer_show_4;
            default: return null;
        }
    }
    public Sprite GetArcherUnitSprite(int id)
    {
        switch (id)
        {
            case 1: return _archer_button_1;
            case 2: return _archer_button_2;
            case 3: return _archer_button_3;
            case 4: return _archer_button_4;
            default: return null;
        }
    }

    public Sprite GetLancerShowSprite(int id)
    {
        switch (id)
        {
            case 1: return _lancer_show_1;
            case 2: return _lancer_show_2;
            case 3: return _lancer_show_3;
            case 4: return _lancer_show_4;
            default: return null;
        }
    }
    public Sprite GetLancerUnitSprite(int id)
    {
        switch (id)
        {
            case 1: return _lancer_button_1;
            case 2: return _lancer_button_2;
            case 3: return _lancer_button_3;
            case 4: return _lancer_button_4;
            default: return null;
        }
    }

    public Sprite GetHealerShowSprite(int id)
    {
        switch (id)
        {
            case 1: return _healer_show_1;
            case 2: return _healer_show_2;
            case 3: return _healer_show_3;
            case 4: return _healer_show_4;
            default: return null;
        }
    }
    public Sprite GetHealerUnitSprite(int id)
    {
        switch (id)
        {
            case 1: return _healer_button_1;
            case 2: return _healer_button_2;
            case 3: return _healer_button_3;
            case 4: return _healer_button_4;
            default: return null;
        }
    }

    public Sprite GetTNTShowSprite(int id)
    {
        switch (id)
        {
            case 1: return _TNT_show_1;
            case 2: return _TNT_show_2;
            case 3: return _TNT_show_3;
            case 4: return _TNT_show_4;
            default: return null;
        }
    }
    public Sprite GetTNTUnitSprite(int id)
    {
        switch (id)
        {
            case 1: return _TNT_button_1;
            case 2: return _TNT_button_2;
            case 3: return _TNT_button_3;
            case 4: return _TNT_button_4;
            default: return null;
        }
    }
}
