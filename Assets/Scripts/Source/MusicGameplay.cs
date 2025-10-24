using System.Collections;
using UnityEngine;

public class MusicGameplay : MonoBehaviour
{
    [SerializeField] private UnitAudio _audio;
    private Coroutine _onWarSound;

    void Start()
    {
        _audio.PlaySunSound();
    }

    public void StopAudio()
        => _audio.StopWeatherSound();

    public void PlayWarSound()
    {
        Debug.Log("Play War sound");
        if (_audio.checkPlayingWarSound())
        {
            StopCoroutine(_onWarSound);
            _onWarSound = StartCoroutine(ChangeSound());
            return;
        }
        _audio.StopWeatherSound();
        _audio.PlayRainSound();

        _onWarSound = StartCoroutine(ChangeSound());
    }

    private IEnumerator ChangeSound()
    {
        yield return new WaitForSeconds(5.5f);
        _audio.StopWeatherSound();
        _audio.PlaySunSound();
    }
}
