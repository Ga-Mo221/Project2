using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class Weather : MonoBehaviour
{
    [Header("Weather Prefabs")]
    [SerializeField] private GameObject _snow;
    [SerializeField] private GameObject _rain;
    [SerializeField] private Light2D _light;
    [SerializeField] private GameObject _cloudShadow;

    [Header("Lightning Settings")]
    [Range(0, 1)][SerializeField] private float _crit_snow = 0.7f;
    [SerializeField] private float _minLightningDelay = 5f;
    [SerializeField] private float _maxLightningDelay = 15f;
    [SerializeField] private float _flashDuration = 0.1f;
    [SerializeField] private float _flashIntensity = 3f;

    [Header("Weather Cycle Settings")]
    [SerializeField] private float _weatherDuration = 60f; // thời gian giữa 2 lần random thời tiết

    private Coroutine lightningCoroutine;
    private Coroutine weatherCycleCoroutine;

    private void Start()
    {
        _cloudShadow.SetActive(true);
        weatherCycleCoroutine = StartCoroutine(WeatherCycleRoutine());
    }

    private IEnumerator WeatherCycleRoutine()
    {
        while (true)
        {
            RandomizeWeather();
            yield return new WaitForSeconds(_weatherDuration);
        }
    }

    private void RandomizeWeather()
    {
        float chance = Random.value;
        bool isSnow = chance <= _crit_snow; // 70% snow, 30% rain

        // Gọi coroutine fade để chuyển mượt giữa hai trạng thái
        StartCoroutine(SmoothTransition(isSnow));
    }

    private IEnumerator SmoothTransition(bool isSnow)
    {
        float duration = 2f; // thời gian fade
        float elapsed = 0f;

        GameObject enableObj = isSnow ? _snow : _rain;
        GameObject disableObj = isSnow ? _rain : _snow;

        if (!enableObj.activeSelf)
            enableObj.SetActive(true);

        var enableParticles = enableObj.GetComponent<ParticleSystem>();
        var disableParticles = disableObj.GetComponent<ParticleSystem>();

        // fade in - fade out particle intensity nếu có
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            if (enableParticles != null)
            {
                var emission = enableParticles.emission;
                emission.rateOverTime = Mathf.Lerp(0, 100, t);
            }
            if (disableParticles != null)
            {
                var emission = disableParticles.emission;
                emission.rateOverTime = Mathf.Lerp(100, 0, t);
            }

            yield return null;
        }

        disableObj.SetActive(false);

        // Quản lý lightning
        if (!isSnow)
        {
            if (lightningCoroutine == null)
                lightningCoroutine = StartCoroutine(LightningRoutine());
        }
        else
        {
            if (lightningCoroutine != null)
            {
                StopCoroutine(lightningCoroutine);
                lightningCoroutine = null;
            }
            _light.intensity = 1f;
        }
    }

    private IEnumerator LightningRoutine()
    {
        while (true)
        {
            float wait = Random.Range(_minLightningDelay, _maxLightningDelay);
            yield return new WaitForSeconds(wait);

            _light.intensity = _flashIntensity;
            yield return new WaitForSeconds(_flashDuration);
            _light.intensity = 1f;
        }
    }
}
