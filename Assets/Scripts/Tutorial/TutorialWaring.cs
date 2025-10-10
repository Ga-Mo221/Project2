using UnityEngine;

public class TutorialWaring : MonoBehaviour
{

    [SerializeField] private bool _detec = false;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (_detec) return;
        _detec = true;
        GameManager.Instance.Tutorial = true;
        TutorialSetUp.Instance.TutorialWarningEnemyToHouse();
    }
}
