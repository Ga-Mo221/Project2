using UnityEngine;
using System.Collections;

public class HideUI : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private Vector2 _startPos;
    [SerializeField] private Vector2 _endPos;
    [SerializeField] private float _timeMove = 0.3f;
    private GameManager gameManager;

    private bool isOut = false;
    private Coroutine _moveCoroutine;

    void Start()
    {
        _rectTransform.anchoredPosition = _startPos;
        if (GameManager.Instance != null)
            gameManager = GameManager.Instance;
    }

    void Update()
    {
        if (!isOut) return;
        gameManager.UIcheckButtonBuyBuiding();
    }

    public void move()
    {
        if (GameManager.Instance.Tutorial && (TutorialSetUp.Instance.ID == 2|| TutorialSetUp.Instance.ID == 4)) return;
        if (!GameManager.Instance.getCanBuy()) return;
        GameManager.Instance.UIcheckButtonBuyBuiding();

        if (_moveCoroutine != null) StopCoroutine(_moveCoroutine);

        Vector2 target = isOut ? _startPos : _endPos;
        _moveCoroutine = StartCoroutine(MoveTo(target));

        isOut = !isOut;
        if (isOut && GameManager.Instance.Tutorial && TutorialSetUp.Instance.ID == 3)
            TutorialSetUp.Instance.TutorialBuyTower();
    }

    private IEnumerator MoveTo(Vector2 target)
    {
        Vector2 start = _rectTransform.anchoredPosition;
        float elapsed = 0f;

        while (elapsed < _timeMove)
        {
            float t = Mathf.Clamp01(elapsed / _timeMove);
            _rectTransform.anchoredPosition = Vector2.Lerp(start, target, t);
            elapsed += Time.unscaledDeltaTime; // UI nên chạy theo real-time, không bị ảnh hưởng Time.timeScale
            yield return null;
        }

        _rectTransform.anchoredPosition = target;
    }
}
