using UnityEngine;

public class shoot : MonoBehaviour
{
    [SerializeField] private ArcherGFX _archer;

    void Start()
    {
        if (!_archer)
            Debug.LogError($"[{transform.parent.name}] [shoot] Chua gan 'ArcherGFX'");
    }

    public void shoots()
    {
        _archer.spawnArrow();
    }
}
