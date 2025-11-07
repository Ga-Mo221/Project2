using System.Collections;
using NaughtyAttributes;
using UnityEngine;

public class Intro : MonoBehaviour
{

    [SerializeField] private bool _start;
    [ShowIf(nameof(_start))]
    [SerializeField] private GameObject _intro;
    [SerializeField] private UnitAudio _audio;

    void Start()
    {
        if (_start)
        {
            StartCoroutine(startIntro());
        }
    }

    private IEnumerator startIntro()
    {
        yield return new WaitForSeconds(2f);
        _audio.PlaySunSound();
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
