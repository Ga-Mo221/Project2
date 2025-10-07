using UnityEngine;

public class Defense : MonoBehaviour
{
    [SerializeField] private LancerGFX _lancer;
    private GameObject target;

    void Start()
    {
        if (!_lancer)
            Debug.LogError($"[{transform.parent.name}] [Defense] Chưa gán 'LancerGFX'");
    }


    public void farm()
    {
        target = _lancer.target;
        if (target == null) return;
        if (target.CompareTag("Item"))
            target.GetComponent<Item>().farm(_lancer);
    }

    public void setActive() => _lancer.setActive();

    public void off() => _lancer.offDetec();

    public void on() => _lancer.onDetec();
    public void onCanMove() => _lancer.setCanMove(true);
    public void offCanMove() => _lancer.setCanMove(false);
}
