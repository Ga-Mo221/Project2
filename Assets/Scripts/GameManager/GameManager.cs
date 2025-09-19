using NaughtyAttributes;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

    public int _currentDay = 1;
    public bool _night = false;
    public float _4Hours_Sec = 120;
    public int _timeRTS = 0;
    public Vector2 _playTime = Vector2.zero;

    public int _maxSlot = 50;
    public int _currentSlot = 0;

    [SerializeField] private GameUI _ui;

    // Warrior
    [Foldout("Warrior")]
    public int _wood_Warrior = 10;
    // Archer
    [Foldout("Archer")]
    public int _wood_Archer = 10;
    [Foldout("Archer")]
    public int _rock_Archer = 10;
    // Lancer
    [Foldout("Lancer")]
    public int _wood_Lancer = 10;
    [Foldout("Lancer")]
    public int _rock_Lancer = 10;
    [Foldout("Lancer")]
    public int _meat_Lancer = 10;
    // TNT
    [Foldout("TNT")]
    public int _rock_TNT = 10;
    [Foldout("TNT")]
    public int _meat_TNT = 10;
    [Foldout("TNT")]
    public int _gold_TNT = 10;
    // Healer
    [Foldout("Healer")]
    public int _wood_Healer = 10;
    [Foldout("Healer")]
    public int _rock_Healer = 10;
    [Foldout("Healer")]
    public int _meat_Healer = 10;
    [Foldout("Healer")]
    public int _gold_Healer = 10;

    [Foldout("Prefab")]
    public GameObject _warriorPrefab;
    [Foldout("Prefab")]
    public GameObject _ArcherPrefab;
    [Foldout("Prefab")]
    public GameObject _LancerPrefab;
    [Foldout("Prefab")]
    public GameObject _TNTPrefab;
    [Foldout("Prefab")]
    public GameObject _HealerPrefab;
    [Foldout("Prefab")]
    public GameObject _createPrefab;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        UIupdateReferences();
        UIupdatePlayerValue();
    }

    public void UIupdateReferences()
        => _ui.updateReferent();

    public void UIupdatePlayerValue()
        => _ui.updatePlayerValue();

    public void UIloadPlayer()
        => _ui.loadPlayer();
}
