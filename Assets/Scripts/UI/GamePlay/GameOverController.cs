using UnityEngine;

public class GameOverController : MonoBehaviour
{
    private UnitAudio _audio;
    void Awake()
    {
        _audio = GetComponent<UnitAudio>();
    }

    void Start()
    {
        GameManager.Instance.StopSoundMusic();
        if (GameManager.Instance.getGameOver() && GameManager.Instance.getWin())
            _audio.PlaySunSound();
        else if (GameManager.Instance.getGameOver() && !GameManager.Instance.getWin())
            _audio.PlayRainSound();
    }
}
