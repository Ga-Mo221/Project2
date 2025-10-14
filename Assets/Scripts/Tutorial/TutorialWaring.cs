using UnityEngine;

public class TutorialWaring : MonoBehaviour
{
    [SerializeField] private bool war = false;

    void Update()
    {
        if (GameManager.Instance._timeRTS < 4)
            war = true;
        else
            war = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;
        if (!war) return;
        if (collision.CompareTag("Enemy"))
        {
            GameManager.Instance.Tutorial = true;
            TutorialSetUp.Instance.TutorialWarningEnemyToHouse();
            gameObject.SetActive(false);
        }
    }
}
