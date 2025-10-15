using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class UnitAudio : MonoBehaviour
{
    [SerializeField] private UnitSound _type;
    private bool IsArtor => _type == UnitSound.Actor;
    private bool IsBuilding => _type == UnitSound.Buiding;
    private bool IsItem => _type == UnitSound.Item;
    private bool IsMoveTo => _type == UnitSound.MoveTo;
    private bool IsMainHome => _type == UnitSound.MainHome;
    private bool IsWeather => _type == UnitSound.Weather;


    [Header("Audio Volume")]
    [Range(0.1f, 1f)][SerializeField] private float _music_Volume = 1f;
    [Range(0.1f, 1f)][SerializeField] private float _sfx_Volume = 1f;


    [Foldout("Audio Settings")]
    [Range(0f, 1f)] public float spatialBlend = 1f; // 3D sound
    [Foldout("Audio Settings")]
    public float minDistance = 10f;
    [Foldout("Audio Settings")]
    public float maxDistance = 50f;
    [Foldout("Audio Settings")]
    public bool randomizePitch = true;
    [Foldout("Audio Settings")]
    [Range(0.8f, 1.2f)] public float pitchVariation = 0.1f;



    [Foldout("AudioClip")]
    [ShowIf(nameof(IsArtor))]
    [SerializeField] private bool TNTOrSheep = false;

    [Foldout("AudioClip")]
    [ShowIf(nameof(IsItem))]
    [SerializeField] private AudioClip _Hit_Damage_Clip;


    [Foldout("AudioClip")]
    [ShowIf(nameof(IsBuilding))]
    [SerializeField] private AudioClip _Fire_Clip;


    private bool showArcherUpbool => IsArtor && !TNTOrSheep;
    [Foldout("AudioClip")]
    [ShowIf(nameof(showArcherUpbool))]
    [SerializeField] private bool ArcherUp = false;
    [Foldout("AudioClip")]
    [ShowIf(nameof(IsItem))]
    [SerializeField] private bool TreeOrRockOrGold = false;
    private bool showDie => (IsArtor && !ArcherUp) || IsBuilding || (IsItem && TreeOrRockOrGold);
    [Foldout("AudioClip")]
    [ShowIf(nameof(showDie))]
    [SerializeField] private AudioClip _Die_Clip;


    [Foldout("AudioClip")]
    [ShowIf(nameof(IsBuilding))]
    [SerializeField] private bool Enemy = false;
    [Foldout("AudioClip")]
    [ShowIf(nameof(IsBuilding))]
    [SerializeField] private bool Storage = false;



    private bool showAttack => IsArtor && !TNTOrSheep;
    [Foldout("AudioClip")]
    [ShowIf(nameof(showAttack))]
    [SerializeField] private AudioClip _Attack_Clip;

    [Foldout("AudioClip")]
    [ShowIf(nameof(IsArtor))]
    [SerializeField] private bool Animal = false;
    private bool showLevelUp => (IsArtor && !TNTOrSheep && !ArcherUp && !Animal) || (IsBuilding && !Enemy);
    [Foldout("AudioClip")]
    [ShowIf(nameof(showLevelUp))]
    [SerializeField] private AudioClip _Level_Up_Clip;

    private bool showArcherUp => IsBuilding && !Enemy && !Storage;
    [Foldout("AudioClip")]
    [ShowIf(nameof(showArcherUp))]
    [SerializeField] private AudioClip _Archer_Up_Clip;

    private bool showCanCreate => IsBuilding && !Enemy;
    [Foldout("AudioClip")]
    [ShowIf(nameof(showCanCreate))]
    [SerializeField] private bool CanCreate = false;
    private bool showCreating => IsBuilding && !Enemy && CanCreate;
    [Foldout("AudioClip")]
    [ShowIf(nameof(showCreating))]
    [SerializeField] private AudioClip _Creating_Clip;

    private bool showWarning => IsBuilding && Enemy && Storage;
    [Foldout("AudioClip")]
    [ShowIf(nameof(showWarning))]
    [SerializeField] private AudioClip _Warning_Clip;

    [Foldout("AudioClip")]
    [ShowIf(nameof(IsMoveTo))]
    [SerializeField] private AudioClip _MoveTo_Clip;

    [Foldout("AudioClip")]
    [ShowIf(nameof(IsMainHome))]
    [SerializeField] private AudioClip _BackgroundMusic;

    [Foldout("AudioClip")]
    [ShowIf(nameof(IsWeather))]
    [SerializeField] private AudioClip _Sun_Clip;
    [Foldout("AudioClip")]
    [ShowIf(nameof(IsWeather))]
    [SerializeField] private AudioClip _Rain_Clip;
    [Foldout("AudioClip")]
    [ShowIf(nameof(IsWeather))]
    [SerializeField] private AudioClip _Night_Clip;


    

    private AudioSource _audio;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
        _audio.playOnAwake = false;
        _audio.spatialBlend = spatialBlend;
        _audio.minDistance = minDistance;
        _audio.maxDistance = maxDistance;
    }

    void OnEnable()
    {
        SettingManager.Instance._onVolumeChanged += ApplyVolumeChange;
    }

    void OnDisable()
    {
        SettingManager.Instance._onVolumeChanged -= ApplyVolumeChange;
    }

    private void ApplyVolumeChange()
    {
        float overall = SettingManager.Instance._gameSettings._overall_Volume;
        float music = SettingManager.Instance._gameSettings._music_Volume;
        float sfx = SettingManager.Instance._gameSettings._SFX_volume;

        _audio.volume = _audio.loop
            ? _music_Volume * music * overall
            : _sfx_Volume * sfx * overall;
    }


    private void PlayClip(AudioClip clip, bool loop)
    {
        if (clip == null) return;

        if (randomizePitch)
            _audio.pitch = Random.Range(1f - pitchVariation, 1f + pitchVariation);
        else
            _audio.pitch = 1f;


        if (loop)
        {
            // Nếu là loop → dùng clip trực tiếp
            if (_audio.isPlaying && _audio.clip == clip) return; // tránh trùng lặp
            _audio.clip = clip;
            _audio.loop = true;
            ApplyVolumeChange();
            _audio.Play();
        }
        else
        {
            // Nếu không loop → phát tạm 1 lần
            _audio.loop = false;
            ApplyVolumeChange();
            _audio.PlayOneShot(clip, _audio.volume);
        }
    }

    // -----------------------------
    // 📢 Public Methods for Actions
    // -----------------------------
    // play
    public void PlayAttackSound() => PlayClip(_Attack_Clip, false);
    public void PlayDieSound() => PlayClip(_Die_Clip, false);
    public void PlayFarmOrHitDamageSound() => PlayClip(_Hit_Damage_Clip, false);
    public void PlayLevelUpSound() => PlayClip(_Level_Up_Clip, false);
    public void PlayArcherUpSound() => PlayClip(_Archer_Up_Clip, false);
    public void PlayWarningSound() => PlayClip(_Warning_Clip, false);
    public void PlayMoveToSound() => PlayClip(_MoveTo_Clip, false);

    // Loop sounds
    public void PlayFireSound() => PlayClip(_Fire_Clip, true);
    public void PlayCreatingSound() => PlayClip(_Creating_Clip, true);
    public void PlayBackgroundSound() => PlayClip(_BackgroundMusic, true);
    public void PlaySunSound() => PlayClip(_Sun_Clip, true);
    public void PlayRainSound() => PlayClip(_Rain_Clip, true);
    public void PlayNightSound() => PlayClip(_Night_Clip, true);

    // Stop loop sounds
    public void StopFireSound()
    {
        if (_audio.isPlaying && _audio.clip != null && _audio.clip == _Fire_Clip)
        {
            Debug.Log(_audio.clip);
            _audio.Stop();
        }
    }

    public void StopCreatingSound()
    {
        if (_audio.isPlaying && _audio.clip != null && _audio.clip == _Creating_Clip)
            _audio.Stop();
    }

    public void StopBackgroundSound()
    {
        if (_audio.isPlaying && _audio.clip != null && _audio.clip == _BackgroundMusic)
            _audio.Stop();
    }

    public void StopWeatherSound()
    {
        if (_audio.isPlaying)
            _audio.Stop();
    }

    public bool checkPlayingWarSound()
    {
        return _audio.clip == _Sun_Clip;
    }

    public bool checkMusicGameplaySound()
    {
        return _audio.clip == _Rain_Clip;
    }
}

public enum UnitSound
{
    Actor,
    Buiding,
    Item,
    MoveTo,
    MainHome,
    Weather
}
