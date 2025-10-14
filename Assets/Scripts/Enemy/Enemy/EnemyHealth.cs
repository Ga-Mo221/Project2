using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public EnemyAI _enemyAI;
    [SerializeField] private GameObject _HP_obj;

    void Start()
    {
        if (_HP_obj == null)
            Debug.LogError($"[{transform.name}] [EnemyHealth] Chưa gán 'GameObject HPbar'!");
        else
            _HP_obj.SetActive(false);
    }

    public void takeDamage(float damage)
    {
        _enemyAI._currentHealth -= damage;
        if (_enemyAI._currentHealth <= 0)
        {
            _enemyAI.dead();
        }

        _HP_obj.SetActive(true);
        if (_hideHP != null)
            StopCoroutine(_hideHP);
        if (gameObject.activeInHierarchy)
            _hideHP = StartCoroutine(hideHP());
    }

    private Coroutine _hideHP;
    private IEnumerator hideHP()
    {
        if (_enemyAI.getDie())
            _HP_obj.SetActive(false);
        yield return new WaitForSeconds(5.5f);
        _HP_obj.SetActive(false);
    }
}
