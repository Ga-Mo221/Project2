using UnityEngine;

public class TowerDie : MonoBehaviour
{
    [SerializeField] private House _tower;
    [SerializeField] private bool die = false;


    void Update()
    {
        if (_tower._currentHealth <= 0 && !die)
        {
            die = true;
            TutorialSetUp.Instance.TutorialArcherInTowerAndCatlse();
        }
    }
}
