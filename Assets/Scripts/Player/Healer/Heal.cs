using UnityEngine;

public class Heal : MonoBehaviour
{
    [SerializeField] private HealerGFX _heal;

    void Start()
    {
        if (!_heal)
            Debug.LogError($"[{transform.parent.name}] [Heal] Chua gan 'HealGFX'");
    }

    public void setActive() => _heal.setActive();

    public void heal()
    {
        _heal.heals();
    }
    
    public void onCanMove() => _heal.setCanMove(true);
    public void offCanMove() => _heal.setCanMove(false);
}
