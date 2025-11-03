using System.Collections.Generic;
using UnityEngine;

public class IsDetecOBJ : MonoBehaviour, IDetectable
{
    [Header("Settings")]
    [SerializeField] private ModelType modelType;
    [SerializeField] private GameObject chillEffect;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private FindPath _findPath;

    [Header("Script References")]
    [SerializeField] private Item item;
    [SerializeField] private AnimalAI animal;
    [SerializeField] private EnemyAI enemy;

    // State
    private HashSet<Rada> seers = new HashSet<Rada>();
    public bool isChillActive = false;
    private Vector2 lastPosition;

    // IDetectable implementation
    public Vector2 Position => transform.position;
    public bool IsValid => gameObject.activeInHierarchy;

    public bool HasSeemer(Rada seemer) => seers.Contains(seemer);

    void Start()
    {
        if (DetectionManager.Instance != null)
        {
            DetectionManager.Instance.RegisterDetectable(this);
        }

        lastPosition = transform.position;
        if (chillEffect != null) chillEffect.SetActive(false);
        if (_findPath != null) _findPath.enabled = false;

        //InvokeRepeating(nameof(UpdateChillEffect), 5f, 5f);
    }

    public void AddSeemer(Rada seemer)
    {
        if (seers.Add(seemer))
        {
            UpdateChillEffect();
        }
    }

    public void RemoveSeemer(Rada seemer)
    {
        if (seers.Remove(seemer))
        {
            UpdateChillEffect();
        }
    }

    void OnEnable()
    {
        if (DetectionManager.Instance != null)
        {
            DetectionManager.Instance.RegisterDetectable(this);
        }

        lastPosition = transform.position;
        if (chillEffect != null) chillEffect.SetActive(false);
        if (_findPath != null) _findPath.enabled = false;
    }


    void OnDisable()
    {
        if (DetectionManager.Instance != null)
        {
            DetectionManager.Instance.UnregisterDetectable(this);
        }

        if (chillEffect != null) chillEffect.SetActive(false);
        if (_findPath != null) _findPath.enabled = false;
    }

    void Update()
    {
        // Update position
        Vector2 currentPos = transform.position;
        if (Vector2.Distance(currentPos, lastPosition) > 0.5f)
        {
            if (DetectionManager.Instance != null)
            {
                DetectionManager.Instance.UpdateDetectablePosition(this, lastPosition, currentPos);
            }
            lastPosition = currentPos;
        }
    }

    public void UpdateChillEffect(bool API = false)
    {
        if (DetectionManager.Instance != null && !DetectionManager.Instance.canActiveOBJ) return;
        if (CanShow())
        {
            if (enemy != null && enemy.getDie())
            {
                chillEffect.SetActive(false);
                if (_findPath != null)
                    _findPath.enabled = false;
            }
            Debug.Log($"[{transform.name}] <color=red>[Not Active]</color>", this);
            return;
        }

        if (API)
            isChillActive = true;

        bool shouldShowChill = seers.Count > 0 && CanShowChill();
        Debug.Log($"[{transform.name}] <color=yellow>Can Show</color> [<color=green>{shouldShowChill} Count {seers.Count}</color> : <color=red>{isChillActive}</color>]", this);
        if (shouldShowChill != isChillActive)
        {
            isChillActive = shouldShowChill;
            if (chillEffect != null)
            {
                chillEffect.SetActive(shouldShowChill);
                if (_findPath != null)
                    _findPath.enabled = shouldShowChill;
            }
        }
    }

    private bool CanShowChill()
    {
        switch (modelType)
        {
            case ModelType.Tree:
            case ModelType.Rock:
                return item == null || !item._respawning;
            case ModelType.Animal:
                return animal == null || !animal._respawning;
            case ModelType.Enemy:
                return enemy == null || !enemy.getDie();
            default:
                return true;
        }
    }

    private bool CanShow()
    {
        bool canshow = false;
        switch (modelType)
        {
            case ModelType.Tree:
            case ModelType.Rock:
                if (item != null)
                    canshow = item._respawning;
                break;
            case ModelType.Animal:
                if (animal != null)
                    canshow = animal._respawning;
                break;
            case ModelType.Enemy:
                if (enemy != null)
                    canshow = enemy.getDie() || !enemy._inPatrol;
                break;
        }
        return canshow;
    }
}