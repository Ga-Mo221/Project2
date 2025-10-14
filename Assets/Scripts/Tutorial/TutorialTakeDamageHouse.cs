using System.Collections;
using UnityEngine;

public class TutorialTakeDamageHouse : MonoBehaviour
{
    private HouseHealth _house;
    [SerializeField] private float _time = 1f;

    void Awake()
    {
        _house = GetComponent<HouseHealth>();
    }

    void Start()
    {
        StartCoroutine(damgete());
    }
    
    private IEnumerator damgete()
    {
        yield return new WaitForSeconds(_time);
        _house.takeDamage(1000);
    }
}
