using System.Collections;
using UnityEngine;

public class MusicGameplay : MonoBehaviour
{
    [SerializeField] private UnitAudio _audio;

    void Start()
    {
        _audio.PlaySunSound();
    }

    public void StopAudio()
        => _audio.StopWeatherSound();

    public void PlayWarSound()
    {
        if (_audio.checkPlayingWarSound()) return;
        _audio.StopWeatherSound();
        _audio.PlayRainSound();

        StartCoroutine(ChangeSound());
    }

    private IEnumerator ChangeSound()
    {
        yield return new WaitForSeconds(5.5f);
        _audio.StopWeatherSound();
        _audio.PlaySunSound();
    }
}
