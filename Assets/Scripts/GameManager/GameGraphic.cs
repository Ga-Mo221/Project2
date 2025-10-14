using UnityEngine;

public class GameGraphic : MonoBehaviour
{
    [SerializeField] private GameObject _Bloom;
    [SerializeField] private GameObject _Weather;

    void Start()
    {
        _Bloom.SetActive(SettingManager.Instance._gameSettings._isBloom);
        _Weather.SetActive(SettingManager.Instance._gameSettings._isWeather);
    }

    void OnEnable()
    {
        if (SettingManager.Instance != null)
        {
            SettingManager.Instance._onBloom += Bloom;
            SettingManager.Instance._onWeather += Weather;
        }
    }

    void OnDisable()
    {
        if (SettingManager.Instance != null)
        {
            SettingManager.Instance._onBloom -= Bloom;
            SettingManager.Instance._onWeather -= Weather;
        }
    }

    private void Bloom()
    {
        _Bloom.SetActive(SettingManager.Instance._gameSettings._isBloom);
    }
    
    private void Weather()
    {
        _Weather.SetActive(SettingManager.Instance._gameSettings._isWeather);
    }
}
