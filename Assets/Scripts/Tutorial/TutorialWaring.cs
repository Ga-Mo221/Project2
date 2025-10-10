using UnityEngine;

public class TutorialWaring : MonoBehaviour
{
    private bool war = false;
    void Update()
    {
        if (GameManager.Instance._timeRTS == 0)
            war = true;
    }

    [SerializeField] private bool _detec = false;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (_detec) return;
        if (!war) return;
        GameManager.Instance.Tutorial = true;
        TutorialSetUp.Instance.TutorialWarningEnemyToHouse();
        gameObject.SetActive(false);
    }
}
