using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDie : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Image _icon;
    [SerializeField] private Image _iconDie;

    [SerializeField] private Sprite _WarriorIcon;
    [SerializeField] private Sprite _ArcherIcon;
    [SerializeField] private Sprite _LancerIcon;
    [SerializeField] private Sprite _HealerIcon;
    [SerializeField] private Sprite _TNTIcon;

    [SerializeField] private float fadeDuration = 2f;
    [SerializeField] private float timeUp = 1f;
    [SerializeField] float height;
    private RectTransform rect;

    private Color _textC;
    private Color _icon1C;
    private Color _icon2C;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        height = rect.rect.height;

        StartCoroutine(FadeOut());
    }

    private Coroutine die;
    public void add(UnitType type)
    {
        switch (type)
        {
            case UnitType.Warrior:
                _icon.sprite = _WarriorIcon;
                break;
            case UnitType.Archer:
                _icon.sprite = _ArcherIcon;
                break;
            case UnitType.Lancer:
                _icon.sprite = _LancerIcon;
                break;
            case UnitType.Healer:
                _icon.sprite = _HealerIcon;
                break;
            case UnitType.TNT:
                _icon.sprite = _TNTIcon;
                break;
        }

        if (die == null)
            if (gameObject.activeInHierarchy)
                die = StartCoroutine(FadeOut());
    }


    public void getColor()
    {
        _textC = _text.color;
        _icon1C = _icon.color;
        _icon2C = _iconDie.color;
    }


    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(1f);

        float time = 0f;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            Color t = _text.color;
            Color ic1 = _icon.color;
            Color ic2 = _iconDie.color;
            t.a = Mathf.Lerp(1f, 0f, time / fadeDuration);
            ic1.a = Mathf.Lerp(1f, 0f, time / fadeDuration);
            ic2.a = Mathf.Lerp(1f, 0f, time / fadeDuration);
            _text.color = t;
            _icon.color = ic1;
            _iconDie.color = ic2;
            yield return null;
        }
        gameObject.SetActive(false);
    }


    public void respawn(Vector3 pos, UnitType type)
    {
        _text.color = _textC;
        _icon.color = _icon1C;
        _iconDie.color = _icon2C;
        transform.position = pos;
        die = null;
        add(type);
    }


    public void UP()
    {
        if (!gameObject.activeInHierarchy) return;

        if (rect == null)
            rect = GetComponent<RectTransform>();

        StartCoroutine(Uppos());
    }
    
    private IEnumerator Uppos()
    {
        Vector3 startPos = rect.anchoredPosition;
        Vector3 targetPos = startPos + new Vector3(0, height, 0);

        float time = 0f;
        while (time < timeUp)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / timeUp);
            rect.anchoredPosition = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        rect.anchoredPosition = targetPos; // Đảm bảo về đúng vị trí cuối
    }
}
