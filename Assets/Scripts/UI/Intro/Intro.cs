using System.Collections;
using NaughtyAttributes;
using UnityEngine;

public class Intro : MonoBehaviour
{

    [SerializeField] private bool _start;
    [ShowIf(nameof(_start))]
    [SerializeField] private GameObject _intro;

    void Start()
    {
        if (_start)
        {
            Debug.Log(1);
            StartCoroutine(startIntro());
        }
    }

    private IEnumerator startIntro()
    {
        Debug.Log(2);
        yield return new WaitForSeconds(2f);
        Debug.Log(3);
        _intro.SetActive(true);
    }


    public void GoHome()
    {
        StartCoroutine(starGoHome());
    }

    private IEnumerator starGoHome()
    {
        yield return new WaitForSeconds(1f);
        GameScene.Instance.OpenSceneMainMenu();
    }
}
