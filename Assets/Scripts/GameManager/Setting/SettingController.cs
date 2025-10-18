using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingController : MonoBehaviour
{
    [SerializeField] private List<string> _listLanguageCode = new List<string>();
    private GetMainCam _getMainCamera;

    [SerializeField] private int _index_language = 0;

    [Foldout("UI")]
    [SerializeField] private TextMeshProUGUI _text_Language;
    [Foldout("UI")]
    [SerializeField] private TextMeshProUGUI _text_DisplayMode;
    [Foldout("UI")]
    [SerializeField] private Slider _slider_Overrall_Volume;
    [Foldout("UI")]
    [SerializeField] private Slider _slider_Music_Volume;
    [Foldout("UI")]
    [SerializeField] private Slider _slider_SFX_Volume;
    [Foldout("UI")]
    [SerializeField] private Toggle _toggle_Bloom;
    [Foldout("UI")]
    [SerializeField] private Toggle _toggle_Weather;
    [Foldout("UI")]
    [SerializeField] private Slider _slider_MouseSensitivity;
    [Foldout("UI")]
    [SerializeField] private Toggle _toggle_RightMousePanCamera;
    [Foldout("UI")]
    [SerializeField] private Slider _slider_RightMouseSpeed;
    [Foldout("UI")]
    [SerializeField] private TextMeshProUGUI _text_RightMouseSpeed;
    [Foldout("UI")]
    [SerializeField] private Animator _exitGamePanel;
    [Foldout("UI")]
    [SerializeField] private TextMeshProUGUI _text_exitGame;
    [Foldout("UI")]
    [SerializeField] private GameObject _OutScene;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        _getMainCamera = GetComponent<GetMainCam>();
    }

    void Start()
    {
        loadData();
    }

    public void loadData()
    {
        _getMainCamera.setupCamera();
        loadSetting();
    }

    #region Close Setting
    public void Close()
    {
        SettingManager.Instance?.CloseSetting();
    }
    #endregion

    #region Load Setting
    private void loadSetting()
    {
        _index_language = getIndexLanguage();
        applyLanguage(false);

        loadDisplayMode(false);

        _slider_Overrall_Volume.value = SettingManager.Instance._gameSettings._overall_Volume;
        _slider_Music_Volume.value = SettingManager.Instance._gameSettings._music_Volume;
        _slider_SFX_Volume.value = SettingManager.Instance._gameSettings._SFX_volume;
        _slider_MouseSensitivity.value = SettingManager.Instance._gameSettings._mouseSensitivity;
        _slider_RightMouseSpeed.value = SettingManager.Instance._gameSettings._speedPanCamera;

        _toggle_Bloom.isOn = SettingManager.Instance._gameSettings._isBloom;
        _toggle_Weather.isOn = SettingManager.Instance._gameSettings._isWeather;
        _toggle_RightMousePanCamera.isOn = SettingManager.Instance._gameSettings._panCameraRightMouse;

        _text_RightMouseSpeed.gameObject.SetActive(SettingManager.Instance._gameSettings._panCameraRightMouse);
        _slider_RightMouseSpeed.gameObject.SetActive(SettingManager.Instance._gameSettings._panCameraRightMouse);
    }
    #endregion

    #region Change Language Right
    public void ChangeLanguage_Right()
    {
        _index_language = getIndexLanguage();

        if (_index_language == _listLanguageCode.Count - 1)
        {
            _index_language = 0;
            Debug.Log($"index[{_index_language}] || count[{_listLanguageCode.Count -1}] || right1");
        }
        else
        {
            _index_language++;
            Debug.Log($"index[{_index_language}] || count[{_listLanguageCode.Count -1}] || right2");
        }
            

        applyLanguage();
    }
    #endregion


    #region Change Language Left
    public void ChangeLanguage_Left()
    {
        _index_language = getIndexLanguage();

        if (_index_language == 0)
            _index_language = _listLanguageCode.Count - 1;
        else
            _index_language--;

        Debug.Log(_listLanguageCode.Count - 1);

        applyLanguage();
    }
    #endregion


    #region Apply Language
    private void applyLanguage(bool apply = true)
    {
        string code = _listLanguageCode[_index_language];
        if (LocalizationManager.Instance != null)
            LocalizationManager.Instance.LoadLanguage(code);
        _text_Language.text = LocalizationManager.Instance.currentLocalization.languageName;
        if (apply)
        {
            SettingManager.Instance._gameSettings._language = code;
            SettingManager.Instance?.applyLanguage();
            SettingManager.Instance?.Save();
        }
    }
    #endregion


    #region Get Index Language
    private int getIndexLanguage()
    {
        int index = 0;
        for (int i = 0; i < _listLanguageCode.Count; i++)
        {
            string code = "";
            if (LocalizationManager.Instance != null)
                code = LocalizationManager.Instance.currentLocalization.languageCode;
            if (_listLanguageCode[i] == code)
            {
                index = i;
                break;
            }
        }
        return index;
    }
    #endregion


    #region Full Windown
    public void ChangeDisplayMode()
    {
        bool isfull = SettingManager.Instance._gameSettings._isFullscreen;
        isfull = !isfull;
        SettingManager.Instance._gameSettings._isFullscreen = isfull;
        applyLanguage();
        loadDisplayMode();
    }

    private void loadDisplayMode(bool apply = true)
    {
        string key = "";
        bool isfull = SettingManager.Instance._gameSettings._isFullscreen;
        if (isfull)
            key = "setting.settingAll.fullScreen";
        else
            key = "setting.settingAll.notFullScreen";

        _text_DisplayMode.text = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        if (apply)
        {
            if (!isfull)
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
    }
    #endregion


    #region Change Overall Volume
    public void changeOverallVolume(float value)
    {
        SettingManager.Instance._gameSettings._overall_Volume = value;
        SettingManager.Instance?.OnVolumeChanged();
        SettingManager.Instance?.Save();
    }
    #endregion

    #region Change music Volume
    public void changeMusicVolume(float value)
    {
        SettingManager.Instance._gameSettings._music_Volume = value;
        SettingManager.Instance?.OnVolumeChanged();
        SettingManager.Instance?.Save();
    }
    #endregion

    #region Change SFX Volume
    public void changeSFXVolume(float value)
    {
        SettingManager.Instance._gameSettings._SFX_volume = value;
        SettingManager.Instance?.Save();
    }
    #endregion

    #region Change Mouse Sensitivity
    public void changeMouseSensitivity(float value)
    {
        SettingManager.Instance._gameSettings._mouseSensitivity = value;
        SettingManager.Instance?.OnMouseChanged();
        SettingManager.Instance?.Save();
    }
    #endregion

    #region Bloom
    public void onOffBloom(bool value)
    {
        SettingManager.Instance._gameSettings._isBloom = value;
        SettingManager.Instance?.OnBloom();
        SettingManager.Instance?.Save();
    }
    #endregion


    #region Weather
    public void onOffWeather(bool value)
    {
        SettingManager.Instance._gameSettings._isWeather = value;
        SettingManager.Instance?.OnWeather();
        SettingManager.Instance?.Save();
    }
    #endregion


    #region Weather
    public void onRightMousePanCamera(bool value)
    {
        SettingManager.Instance._gameSettings._panCameraRightMouse = value;
        _text_RightMouseSpeed.gameObject.SetActive(SettingManager.Instance._gameSettings._panCameraRightMouse);
        _slider_RightMouseSpeed.gameObject.SetActive(SettingManager.Instance._gameSettings._panCameraRightMouse);
        SettingManager.Instance?.OnRightMousePanCamera();
        SettingManager.Instance?.Save();
    }
    #endregion


    #region Change Mouse Sensitivity
    public void ChangeRightMouseSpeed(float value)
    {
        SettingManager.Instance._gameSettings._speedPanCamera = value;
        SettingManager.Instance?.OnRightMouseSpeed();
        SettingManager.Instance?.Save();
    }
    #endregion


    #region Exit Game
    public void ExitGame()
    {
        string key = "";
        string txt = "";
        if (SettingManager.Instance._playing)
        {
            key = "setting.ExitGameTitle";
            txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        }
        else
        {
            key = "setting.ExitAppGameTitle";
            txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        }

        _text_exitGame.text = txt;
        _exitGamePanel.gameObject.SetActive(true);
    }
    #endregion


    #region Button No Exit Game
    public void OnButtonNOExitGame()
    {
        _exitGamePanel.SetTrigger("exit");
    }
    #endregion


    #region Button Yes Exit Game
    public void OnButtonYesExitGame()
    {
        if (SettingManager.Instance._playing)
        {
            SettingManager.Instance.OnExitGame();
            _exitGamePanel.SetTrigger("exit");
            _OutScene.SetActive(true);
        }
        else
        {
            Application.Quit();
        }
    }
    #endregion
}