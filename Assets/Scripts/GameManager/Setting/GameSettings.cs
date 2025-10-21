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

    public static GameSettings Default()
    {
        return new GameSettings();
    }
}
