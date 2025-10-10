using UnityEngine;

public class NgonDut : MonoBehaviour
{
    [SerializeField] private GameObject Fire;

    void Start()
    {
        Fire.SetActive(false);

        InvokeRepeating(nameof(Onfire), 1f, 1f);
    }


    private void Onfire()
    {
        if (GameManager.Instance._timeRTS >= 18)
        {
            Fire.SetActive(true);
        }
        else if (GameManager.Instance._timeRTS >= 6)
        {
            Fire.SetActive(false);
        }
    }
}
