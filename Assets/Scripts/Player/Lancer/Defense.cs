using UnityEngine;

public class Defense : MonoBehaviour
{
    [SerializeField] private LancerGFX _lancer;

    void Start()
    {
        if (!_lancer)
            Debug.LogError($"[{transform.parent.name}] [Defense] Chưa gán 'LancerGFX'");
    }


    public void off()
    {
        _lancer.offCanAttack();
    }

    public void on()
    {
        _lancer.onCanAttack();
    }
}
