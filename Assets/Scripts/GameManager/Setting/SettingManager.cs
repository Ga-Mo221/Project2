using System.Collections.Generic;
using UnityEngine;
using System;
using NaughtyAttributes;

public class SettingManager : MonoBehaviour
{
    public static SettingManager Instance { get; private set; }

    [SerializeField] private GameObject _settingPrefab;
    private SettingController _settingController;
    public GameSettings _gameSettings;
    public event Action _onCloseSetting;
    public event Action _onBloom;
    public event Action _onWeather;
    public event Action _onVolumeChanged;
    public event Action _onMouseChanged;
    public event Action _onRightMousePanCamera;
    public event Action _onRightMouseSpeed;
    public event Action _onExitGame;
    

    public bool _playing = false;

    [Header("Online")]
    [SerializeField] private bool _Online = false;
    [SerializeField] private bool _InRoom = false;
    [SerializeField] private int _maxPlayer = 4;

    [Foldout("Button Clip")]
    public AudioClip _button_hover;
    [Foldout("Button Clip")]
    public AudioClip _button_click;
    [Foldout("Button Clip")]
    public AudioClip _valuchangeClip;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _gameSettings = SettingsIO.Load();

        if (!_gameSettings._isFullscreen)
        {
            // Set kích thước cửa sổ khi không fullscreen
            int width = 1280;
            int height = 720;
            Screen.SetResolution(width, height, false);
        }
        else
        {
            // Chế độ toàn màn hình: lấy kích thước thật của màn hình
            Resolution resolution = Screen.currentResolution;
            Screen.SetResolution(resolution.width, resolution.height, true);
        }
    }


    private List<LocalizedText> _Texts = new List<LocalizedText>();

    void Start()
    {
        GameObject _setting = Instantiate(_settingPrefab);
        _settingController = _setting.GetComponent<SettingController>();
        _setting.SetActive(false);

        if (_settingController == null)
            Debug.LogError("Không tìm thấy SettingController");
    }

    public void OpenSetting()
    {
        _settingController.loadData();
        _settingController.gameObject.SetActive(true);
    }

    public void CloseSetting()
    {
        _onCloseSetting?.Invoke();
    }

    public void Save()
    {
        SettingsIO.Save(_gameSettings);
    }

    public void addText(LocalizedText text)
    {
        _Texts.Add(text);
    }

    public void applyLanguage()
    {
        foreach (var text in _Texts)
            text.ApplyText();
    }

    public void OnBloom()
    {
        _onBloom?.Invoke();
    }

    public void OnWeather()
    {
        _onWeather?.Invoke();
    }

    public void OnVolumeChanged()
    {
        _onVolumeChanged?.Invoke();
    }

    public void OnMouseChanged()
    {
        _onMouseChanged?.Invoke();
    }

    public void OnRightMousePanCamera()
    {
        _onRightMousePanCamera?.Invoke();
    }

    public void OnRightMouseSpeed()
    {
        _onRightMouseSpeed?.Invoke();
    }

    public void OnExitGame()
    {
        _onExitGame?.Invoke();
    }

    public void setOnline(bool amount)
    {
        _Online = amount;
    }

    public bool getOnline()
    {
        return _Online;
    }

    public void setInRoom(bool amount)
    {
        _InRoom = amount;
    }

    public bool getInRoom()
    {
        return _InRoom;
    }

    public int getPlayerValue() => _maxPlayer;
    public void setPlayerValue(int value) => _maxPlayer = value;
}
