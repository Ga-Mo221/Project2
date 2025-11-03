using UnityEngine;

public class WeatherAudioManager : MonoBehaviour
{
    [SerializeField] private Weather _weather;
    [SerializeField] private UnitAudio _audio;

    [SerializeField] private bool isSnow;
    [SerializeField] private bool isNight;

    void Start()
    {
        isSnow = _weather.isSnow;
        isNight = GameManager.Instance._timeRTS >= 18 || GameManager.Instance._timeRTS < 6;
    }

    void Update()
    {
        bool newIsSnow = _weather.isSnow;
        bool newIsNight = false; 
        if (GameManager.Instance != null)
            newIsNight = GameManager.Instance._timeRTS >= 18 || GameManager.Instance._timeRTS < 6;

        if (newIsSnow != isSnow || newIsNight != isNight)
        {
            isSnow = newIsSnow;
            isNight = newIsNight;
            changeAudio();
        }
    }
    
    private void changeAudio()
    {
        _audio.StopWeatherSound();

        if (isNight)
        {
            if (_weather.gameObject.activeSelf)
            {
                if (isSnow)
                    _audio.PlayNightSound();
                else
                    _audio.PlayRainSound();
            }
            else
            {
                _audio.PlayNightSound();
            }
        }
        else
        {
            if (_weather.gameObject.activeSelf)
            {
                if (isSnow)
                    _audio.PlaySunSound();
                else
                    _audio.PlayRainSound();
            }
            else
            {
                _audio.PlaySunSound();
            }
        }
    }
}
