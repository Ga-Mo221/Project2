using UnityEngine;

public class Guard : MonoBehaviour
{
    [SerializeField] private WarriorGFX _script;

    public void offCanAttack()
        => _script.offCanAttack();
    public void onCanAttack()
        => _script.onCanAttack();
}
