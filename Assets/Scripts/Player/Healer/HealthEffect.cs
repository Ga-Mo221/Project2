using UnityEngine;

public class HealthEffect : MonoBehaviour
{
    [SerializeField] private Collider2D _AOE;
    [SerializeField] private PlayerAI _scipt;
    

    void Start()
    {
        _scipt = transform.parent.GetComponent<PlayerAI>();
        if (!_scipt)
            Debug.LogError($"[{transform.parent.name}] [HealthEffect] Chua lay duoc 'PlayerAI _scipt'");
        if (!_AOE)
                Debug.LogError($"[{transform.parent.name}] [HealthEffect] Chua gan 'Collider2D _AOE'");
    }

    public void heal()
    {
        if (_scipt._health >= _scipt._maxHealth) return;
        _scipt._health += _scipt._healPlus;
        if (_scipt._health > _scipt._maxHealth)
            _scipt._health = _scipt._maxHealth;

        if (_scipt._AOEHeal)
            _AOE.enabled = true;
    }

    public void off()
    {
        gameObject.SetActive(false);
        _scipt._AOEHeal = false;
        _AOE.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Warrior")
        || collision.CompareTag("Archer")
        || collision.CompareTag("Lancer"))
        {
            var scripAI = collision.GetComponent<PlayerAI>();
            scripAI._health += scripAI._healPlus;
            scripAI._healEffect.SetActive(true);
        }
    }
}
