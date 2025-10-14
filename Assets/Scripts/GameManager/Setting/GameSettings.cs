using System;
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

    public static GameSettings Default()
    {
        return new GameSettings();
    }
}
