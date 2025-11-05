using System.Collections;
using UnityEngine;

public class NgonDut : MonoBehaviour
{
    [SerializeField] private GameObject Fire;
    private bool _on = false;

    void Start()
    {
        Fire.SetActive(false);

        InvokeRepeating(nameof(Onfire), 1f, 1f);
    }

    void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance._timeRTS> 18 || GameManager.Instance._timeRTS < 6)
            {
                _on = !_on;
                float delay = Random.Range(0, 1.5f);
                StartCoroutine(setActive(delay, true));
            }
        }
    }


    private void Onfire()
    {
        if (GameManager.Instance != null && GameManager.Instance._timeRTS == 18 && !_on)
        {
            _on = !_on;
            float delay = Random.Range(0, 1.5f);
            StartCoroutine(setActive(delay, true));
        }
        else if (GameManager.Instance != null && GameManager.Instance._timeRTS == 6 && _on)
        {
            _on = !_on;
            float delay = Random.Range(0, 1.5f);
            StartCoroutine(setActive(delay, false));
        }
    }

    private IEnumerator setActive(float delay, bool amount)
    {
        yield return new WaitForSeconds(delay);
        Fire.SetActive(amount);
    }
}
