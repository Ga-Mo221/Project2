using System.Collections;
using UnityEngine;

public class AnimalHealth : MonoBehaviour
{
    public AnimalAI _animalAi;
    [SerializeField] private GameObject _HP_obj;

    void Start()
    {
        if (_HP_obj == null)
            Debug.LogError($"[{transform.name}] [AnimalHealth] Chưa gán 'GameObject HPbar'!");
        else
            _HP_obj.SetActive(false);
    }

    public void takeDamage(float damage, GameObject attackr)
    {
        _animalAi._health -= damage;
        _animalAi.FleeFrom(attackr);

        if (_animalAi._health <= 0)
        {
            _animalAi.Die();
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
        if (_animalAi.getDie())
            _HP_obj.SetActive(false);
        yield return new WaitForSeconds(5.5f);
        _HP_obj.SetActive(false);
    }
}
