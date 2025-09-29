using UnityEngine;

public class shoot : MonoBehaviour
{
    [SerializeField] private ArcherGFX _archer;
    private GameObject target;

    void Start()
    {
        if (!_archer)
            Debug.LogError($"[{transform.parent.name}] [shoot] Chua gan 'ArcherGFX'");
    }

    public void farm()
    {
        target = _archer.target;
        if (target == null) return;
        if (target.CompareTag("Item"))
            target.GetComponent<Item>().farm(_archer);
    }
    
    public void setActive() => _archer.setActive();

    public void shoots()
    {
        _archer.spawnArrow();
    }
}
