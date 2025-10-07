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


    [Header("Audio Settings")]
    [Range(0f, 1f)] public float volume = 1f;
    [Range(0f, 1f)] public float spatialBlend = 1f; // 3D sound
    public float minDistance = 10f;
    public float maxDistance = 50f;
    public bool randomizePitch = true;
    [Range(0.8f, 1.2f)] public float pitchVariation = 0.1f;

    private AudioSource _audio;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
        _audio.playOnAwake = false;
        _audio.spatialBlend = spatialBlend;
        _audio.minDistance = minDistance;
        _audio.maxDistance = maxDistance;
    }

    private void PlayClip(AudioClip clip)
    {
        if (clip == null) return;

        if (randomizePitch)
            _audio.pitch = Random.Range(1f - pitchVariation, 1f + pitchVariation);
        else
            _audio.pitch = 1f;

        _audio.volume = volume;
        _audio.PlayOneShot(clip);
    }

    // -----------------------------
    // 📢 Public Methods for Actions
    // -----------------------------
    // play
    public void PlayAttackSound() => PlayClip(_Attack_Clip);
    public void PlayDieSound() => PlayClip(_Die_Clip);
    public void PlayFarmOrHitDamageSound() => PlayClip(_Hit_Damage_Clip);
    public void PlayLevelUpSound() => PlayClip(_Level_Up_Clip);
    public void PlayFireSound() => PlayClip(_Fire_Clip);
    public void PlayArcherUpSound() => PlayClip(_Archer_Up_Clip);
    public void PlayWarningSound() => PlayClip(_Warning_Clip);
    public void PlayMoveToSound() => PlayClip(_MoveTo_Clip);
    public void PlayerCreatingSound() => PlayClip(_Creating_Clip);

    // stop
    public void StopFireSound() { }
    public void StopCreatingSound(){}
}

public enum UnitSound
{
    Actor,
    Buiding,
    Item,
    MoveTo
}
