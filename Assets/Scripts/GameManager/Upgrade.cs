using NaughtyAttributes;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    // Warrior
    [Foldout("Warrior")]
    public int _wood_Warrior = 10;
    // Archer
    [Foldout("Archer")]
    public int _wood_Archer = 10;
    [Foldout("Archer")]
    public int _rock_Archer = 10;
    // Lancer
    [Foldout("Lancer")]
    public int _wood_Lancer = 10;
    [Foldout("Lancer")]
    public int _rock_Lancer = 10;
    [Foldout("Lancer")]
    public int _meat_Lancer = 10;
    // TNT
    [Foldout("TNT")]
    public int _rock_TNT = 10;
    [Foldout("TNT")]
    public int _meat_TNT = 10;
    [Foldout("TNT")]
    public int _gold_TNT = 10;
    // Healer
    [Foldout("Healer")]
    public int _wood_Healer = 10;
    [Foldout("Healer")]
    public int _rock_Healer = 10;
    [Foldout("Healer")]
    public int _meat_Healer = 10;
    [Foldout("Healer")]
    public int _gold_Healer = 10;

    // Tower
    [Foldout("Tower")]
    public int _wood_Tower = 100;
    [Foldout("Tower")]
    public int _rock_Tower = 100;
    [Foldout("Tower")]
    public int _gold_Tower = 100;

    //Storage
    [Foldout("Storage")]
    public int _wood_Storage = 100;
    [Foldout("Storage")]
    public int _rock_Storage = 100;
    [Foldout("Storage")]
    public int _gold_Storage = 100;

    [Header("Bounus Upgrade")]
    public int _buidingReferenceBounus = 50;
    public int _buidingHealthBounus = 50;
    public int _storageBounus = 20;
    public int _damageBounus = 3;
    public int _playerHealthBounus = 10;
    public int _playerReferenceBounus = 10;


    public void upgradeWarrior()
    {
        _wood_Warrior += _playerReferenceBounus;
    }

    public void upgradeArcher()
    {
        _wood_Archer += _playerReferenceBounus;
        _rock_Archer += _playerReferenceBounus;
    }

    public void upgradeLancer()
    {
        _wood_Lancer += _playerReferenceBounus;
        _rock_Lancer += _playerReferenceBounus;
        _meat_Lancer += _playerReferenceBounus;
    }

    public void upgradeTNT()
    {
        _rock_TNT += _playerReferenceBounus;
        _meat_TNT += _playerReferenceBounus;
        _gold_TNT += _playerReferenceBounus;
    }
}
