using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameSettings
{
    [Header("General")]
    public string _language = "vi";

    [Header("Display")]
    public bool _isFullscreen = true;

    [Header("Audio")]
    public float _overall_Volume = 1f;
    public float _music_Volume = 1f;
    public float _SFX_volume = 1f;

    [Header("Gameplay")]
    public float _mouseSensitivity = 40f; // tốc độ lia camera từ 20 -> 100
    public bool _panCameraRightMouse = false;
    public float _speedPanCamera = 1.5f;

    [Header("GameplayGraphis")]
    public bool _isBloom = true;
    public bool _isWeather = true;

    public bool _Tutorial = true;

    [Header("coin")]
    public int _coin = 0;

    [Header("Skin")]
    public List<int> _listCastle = new List<int>{1};
    public int _currentCastle = 1;
    public List<int> _listTower = new List<int>{1};
    public int _currentTower = 1;
    public List<int> _listStorage = new List<int>{1};
    public int _currentStorage = 1;
    public List<int> _listWarrior = new List<int>{1};
    public int _currentWarrior = 1;
    public List<int> _listArcher = new List<int>{1};
    public int _currentArcher = 1;
    public List<int> _listLancer = new List<int>{1};
    public int _currentLancer = 1;
    public List<int> _listHealer = new List<int>{1};
    public int _currentHealer = 1;
    public List<int> _listTNT = new List<int>{1};
    public int _currentTNT = 1;

    [Header("Autotrain LV1")]
    public int _lv1_WarriorCount;
    [Header("AutoTrain LV2")]
    public int _lv2_WarriorCount;
    public int _lv2_ArcherCount;
    [Header("AutoTrain LV3")]
    public int _lv3_WarriorCount;
    public int _lv3_ArcherCount;
    public int _lv3_LancerCount;
    [Header("AutoTrain LV4")]
    public int _lv4_WarriorCount;
    public int _lv4_ArcherCount;
    public int _lv4_LancerCount;
    public int _lv4_TNTCount;
    [Header("AutoTrain LV5")]
    public int _lv5_WarriorCount;
    public int _lv5_ArcherCount;
    public int _lv5_LancerCount;
    public int _lv5_TNTCount;
    public int _lv5_HealerCount;

    public static GameSettings Default()
    {
        return new GameSettings();
    }
}
