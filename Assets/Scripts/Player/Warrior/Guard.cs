using UnityEngine;

public class Guard : MonoBehaviour
{
    [SerializeField] private WarriorGFX _script;
    private GameObject target;

    public void farm()
    {
        target = _script.target;
        if (target == null) return;
        if (target.CompareTag("Item"))
            target.GetComponent<Item>().farm(_script);
    }

    public void setActive() => _script.setActive();

    public void offDetec()
        => _script.offDetec();
    public void onDetec()
        => _script.onDetec();
    public void onCanMove() => _script.setCanMove(true);
    public void offCanMove() => _script.setCanMove(false);
}
