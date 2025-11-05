using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTrain : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    [SerializeField] private GameObject _text;
    [SerializeField] private ChoseUI _choseUI;

    [SerializeField] private AutoTrainSetting _lv1;
    [SerializeField] private AutoTrainSetting _lv2;
    [SerializeField] private AutoTrainSetting _lv3;
    [SerializeField] private AutoTrainSetting _lv4;
    [SerializeField] private AutoTrainSetting _lv5;

    [SerializeField] private bool _hover = false;
    [SerializeField] private bool ON = false;

    Castle castle;
    GameManager gameManager;

    void Start()
    {
        StartCoroutine(startGame());

        if (Castle.Instance != null)
            castle = Castle.Instance;
        if (GameManager.Instance != null)
            gameManager = GameManager.Instance;

        InvokeRepeating(nameof(autoTrain), 0f, 3f);
    }

    private IEnumerator startGame()
    {
        yield return new WaitForSeconds(3f);
        updateSetting();
    }

    public void updateSetting()
    {
        _lv1 = new AutoTrainSetting(1);
        _lv2 = new AutoTrainSetting(2);
        _lv3 = new AutoTrainSetting(3);
        _lv4 = new AutoTrainSetting(4);
        _lv5 = new AutoTrainSetting(5);
    }


    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.Tutorial) return;
        if (CursorManager.Instance != null && CursorManager.Instance.ID == _choseUI.getID() && CursorManager.Instance.ChoseUI && !_hover)
        {
            _text.SetActive(true);
            _hover = true;
        }
        else if (CursorManager.Instance != null && CursorManager.Instance.ID != _choseUI.getID() && !CursorManager.Instance.ChoseUI && _hover)
        {
            _text.SetActive(false);
            _hover = false;
        }

        if (Input.GetMouseButtonUp(0) && _text.activeSelf)
        {
            bool on = !ON;
            changemode(on);
        }
    }
    public void changemode(bool on)
    {
        ON = on;
        _anim.SetBool("On", ON);
    }

    #region Auto Train
    private void autoTrain()
    {
        if (!ON) return;
        if (castle == null && Castle.Instance != null) castle = Castle.Instance;
        if (castle == null && gameManager == null) return;

        switch (castle._level)
        {
            case 1:
                train(_lv1.WarriorCount, UnitType.Warrior);
                break;
            case 2:
                train(_lv2.WarriorCount, UnitType.Warrior);
                train(_lv2.ArcherCount, UnitType.Archer);
                break;
            case 3:
                train(_lv3.WarriorCount, UnitType.Warrior);
                train(_lv3.ArcherCount, UnitType.Archer);
                train(_lv3.LancerCount, UnitType.Lancer);
                break;
            case 4:
                train(_lv4.WarriorCount, UnitType.Warrior);
                train(_lv4.ArcherCount, UnitType.Archer);
                train(_lv4.LancerCount, UnitType.Lancer);
                train(_lv4.TNTCount, UnitType.TNT);
                break;
            case 5:
                train(_lv5.WarriorCount, UnitType.Warrior);
                train(_lv5.ArcherCount, UnitType.Archer);
                train(_lv5.LancerCount, UnitType.Lancer);
                train(_lv5.TNTCount, UnitType.TNT);
                train(_lv5.HealerCount, UnitType.Healer);
                break;
        }
    }
    #endregion

    private void train(int count, UnitType unit)
    {
        int currentCount = CurrentCountUnit(unit);
        int countTrain = count - currentCount;
        if (countTrain <= 0) return;
        unittrainInfo unittrain = new unittrainInfo();
        for (int i = 0; i < countTrain; i++)
        {
            unittrain = create(unit, unittrain);
        }
        Debug.Log($"[<color=orange>AutoTrain</color>] Train <color=green>{unittrain.count}</color> <color=orange>{unittrain.name}</color>");
    }

    private unittrainInfo create(UnitType unit, unittrainInfo unittrain)
    {
        bool checkW;
        bool checkR;
        bool checkM;
        bool checkG;
        int slot = 0;

        int wood = 0;
        int rock = 0;
        int meat = 0;
        int gold = 0;
        switch (unit)
        {
            case UnitType.Warrior:
                slot = gameManager._warriorPrefab.GetComponent<PlayerAI>()._slot;
                if (castle._currentSlot + slot > castle._maxSlot) return unittrain;
                wood = createReference(unit, gameManager.Info._wood_Warrior);
                checkW = castle._wood >= wood;
                unittrain.name = "Warrior";
                Debug.Log($"<color=red>[Check Autotrain]</color> [{unittrain.name}] \n[Wood] Castle({castle._wood}) unit ({wood})");
                if (checkW)
                {
                    unittrain.count++;
                    gameManager.createWarrior();
                }
                break;
            case UnitType.Archer:
                slot = gameManager._ArcherPrefab.GetComponent<PlayerAI>()._slot;
                if (castle._currentSlot + slot > castle._maxSlot) return unittrain;
                wood = createReference(unit, gameManager.Info._wood_Archer);
                rock = createReference(unit, gameManager.Info._rock_Archer);
                checkW = castle._wood >= wood;
                checkR = castle._rock >= rock;
                unittrain.name = "Archer";
                Debug.Log($"<color=red>[Check Autotrain]</color> [{unittrain.name}] \n[Wood] Castle({castle._wood}) unit ({wood})\n"
                            + $"[Rock] Castle({castle._rock}) unit ({rock})");
                if (checkW && checkR)
                {
                    unittrain.count++;
                    gameManager.createArcher();
                }
                break;
            case UnitType.Lancer:
                slot = gameManager._LancerPrefab.GetComponent<PlayerAI>()._slot;
                if (castle._currentSlot + slot > castle._maxSlot) return unittrain;
                wood = createReference(unit, gameManager.Info._wood_Lancer);
                rock = createReference(unit, gameManager.Info._rock_Lancer);
                meat = createReference(unit, gameManager.Info._meat_Lancer);
                checkW = castle._wood >= wood;
                checkR = castle._rock >= rock;
                checkM = castle._meat >= meat;
                unittrain.name = "Lancer";
                Debug.Log($"<color=red>[Check Autotrain]</color> [{unittrain.name}] \n[Wood] Castle({castle._wood}) unit ({wood})\n"
                            + $"[Rock] Castle({castle._rock}) unit ({rock})\n"
                            + $"[Meat] Castle({castle._meat}) unit ({meat})");
                if (checkW && checkR && checkM)
                {
                    unittrain.count++;
                    gameManager.createLancer();
                }
                break;
            case UnitType.TNT:
                slot = gameManager._TNTPrefab.GetComponent<PlayerAI>()._slot;
                if (castle._currentSlot + slot > castle._maxSlot) return unittrain;
                rock = createReference(unit, gameManager.Info._rock_TNT);
                meat = createReference(unit, gameManager.Info._meat_TNT);
                gold = createReference(unit, gameManager.Info._gold_TNT);
                checkR = castle._rock >= rock;
                checkM = castle._meat >= meat;
                checkG = castle._gold >= gold;
                unittrain.name = "TNT";
                Debug.Log($"<color=red>[Check Autotrain]</color> [{unittrain.name}] \n"
                            +$"[Rock] Castle({castle._rock}) unit ({rock})\n"
                            +$"[Meat] Castle({castle._meat}) unit ({meat})\n"
                            +$"[Gold] Castle({castle._gold}) unit ({gold})");
                if (checkG && checkR && checkM)
                {
                    unittrain.count++;
                    gameManager.createTNT();
                }
                break;
            case UnitType.Healer:
                slot = gameManager._HealerPrefab.GetComponent<PlayerAI>()._slot;
                if (castle._currentSlot + slot > castle._maxSlot) return unittrain;
                checkW = castle._wood >= gameManager.Info._wood_Healer;
                checkR = castle._rock >= gameManager.Info._rock_Healer;
                checkM = castle._meat >= gameManager.Info._meat_Healer;
                checkG = castle._gold >= gameManager.Info._gold_Healer;
                unittrain.name = "Healer";
                Debug.Log($"<color=red>[Check Autotrain]</color> [{unittrain.name}] \n[Wood] Castle({castle._wood}) unit ({gameManager.Info._wood_Healer})\n"
                            +$"[Rock] Castle({castle._rock}) unit ({gameManager.Info._rock_Healer})\n"
                            +$"[Meat] Castle({castle._meat}) unit ({gameManager.Info._meat_Healer})\n"
                            +$"[Gold] Castle({castle._gold}) unit ({gameManager.Info._gold_Healer})");
                if (checkW && checkR && checkM && checkG)
                {
                    unittrain.count++;
                    gameManager.createHealer();
                }
                break;
        }
        return unittrain;
    }
    
    private int createReference(UnitType unit, int value)
    {
        // switch (unit)
        // {
        //     case UnitType.Warrior:
        //         for (int i = 1; i < castle._level; i++)
        //             value += gameManager.Info._playerReferenceBounus;
        //         break;
        //     case UnitType.Archer:
        //         for (int i = 2; i < castle._level; i++)
        //             value += gameManager.Info._playerReferenceBounus;
        //         break;
        //     case UnitType.Lancer:
        //         for (int i = 3; i < castle._level; i++)
        //             value += gameManager.Info._playerReferenceBounus;
        //         break;
        //     case UnitType.TNT:
        //         for (int i = 4; i < castle._level; i++)
        //             value += gameManager.Info._playerReferenceBounus;
        //         break;
        //     default: break;
        // }
        return value;
    }

    private int CurrentCountUnit(UnitType unit)
    {
        int currentCount = 0;
        switch (unit)
        {
            case UnitType.Warrior: currentCount = CountUnitInList(castle._ListWarrior); break;
            case UnitType.Archer: currentCount = CountUnitInList(castle._ListArcher); break;
            case UnitType.Lancer: currentCount = CountUnitInList(castle._ListLancer); break;
            case UnitType.TNT: currentCount = CountUnitInList(castle._ListTNT); break;
            case UnitType.Healer: currentCount = CountUnitInList(castle._ListHealer); break;
        }
        return currentCount;
    }

    private int CountUnitInList(List<PlayerAI> list)
    {
        int currentCount = 0;
        foreach (var player in list)
            if (player.getCreating() || player.getUpTower() || !player.getDie() || player.gameObject.activeSelf)
                currentCount++;
        return currentCount;
    }
}

[System.Serializable]
public class AutoTrainSetting
{
    public int Level;
    public int slot;
    public int WarriorCount;
    public int ArcherCount;
    public int LancerCount;
    public int TNTCount;
    public int HealerCount;

    public AutoTrainSetting(int level)
    {
        if (Castle.Instance == null && SettingManager.Instance == null) return;
        var castle = Castle.Instance;
        var setting = SettingManager.Instance._gameSettings;
        switch (level)
        {
            case 1:
                Level = 1;
                slot = castle._maxSlot;
                WarriorCount = setting._lv1_WarriorCount;
                break;
            case 2:
                Level = 2;
                slot = castle._lv2_MaxSlot;
                WarriorCount = setting._lv2_WarriorCount;
                ArcherCount = setting._lv2_ArcherCount;
                break;
            case 3:
                Level = 3;
                slot = castle._lv3_MaxSlot;
                WarriorCount = setting._lv3_WarriorCount;
                ArcherCount = setting._lv3_ArcherCount;
                LancerCount = setting._lv3_LancerCount;
                break;
            case 4:
                Level = 4;
                slot = castle._lv4_MaxSlot;
                WarriorCount = setting._lv4_WarriorCount;
                ArcherCount = setting._lv4_ArcherCount;
                LancerCount = setting._lv4_LancerCount;
                TNTCount = setting._lv4_TNTCount;
                break;
            case 5:
                Level = 5;
                slot = castle._lv5_MaxSlot;
                WarriorCount = setting._lv5_WarriorCount;
                ArcherCount = setting._lv5_ArcherCount;
                LancerCount = setting._lv5_LancerCount;
                TNTCount = setting._lv5_TNTCount;
                HealerCount = setting._lv5_HealerCount;
                break;
        }
    }
}

public class unittrainInfo
{
    public int count;
    public string name;
    public unittrainInfo()
    {
        count = 0;
        name = "can't Train";
    }
}
