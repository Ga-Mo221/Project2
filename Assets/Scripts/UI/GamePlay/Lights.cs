using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Lights : MonoBehaviour
{
    private Light2D _light;

    [Header("Target Color")]
    [SerializeField] private Color _nightColor;
    [SerializeField] private Color _defaultColor;

    [Header("Status Change")]
    [SerializeField] private float transitionTime;
    [SerializeField] private float timer = 0f;            // đếm thời gian đang chuyển
    [SerializeField] private bool isTransitioning = false;

    [Header("Status Color")]
    [SerializeField] private Color startColor;
    [SerializeField] private Color targetColor;

    private void Awake()
    {
        _light = GetComponent<Light2D>();
    }

    private void Start()
    {
        _light.color = _defaultColor;
        transitionTime = GameManager.Instance._2Hours_Sec * 2;
    }
    
    void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.On_onNight += OnNightStart;
            GameManager.Instance.On_offNight += OnNightEnd;
        }
    }

    void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.On_onNight -= OnNightStart;
            GameManager.Instance.On_offNight -= OnNightEnd;
        }
    }

    // ✅ Khi vào ban đêm
    private void OnNightStart()
    {
        StartTransition(_defaultColor, _nightColor);
    }

    // ✅ Khi trở lại ban ngày
    private void OnNightEnd()
    {
        StartTransition(_nightColor, _defaultColor);
    }

    // ✅ Bắt đầu chuyển màu — không cần Update nữa
    private void StartTransition(Color from, Color to)
    {
        if (isTransitioning) return;
        startColor = from;
        targetColor = to;
        timer = 0f;
        isTransitioning = true;
        StartCoroutine(DoTransition());
    }

    private System.Collections.IEnumerator DoTransition()
    {
        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / transitionTime);
            _light.color = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }

        _light.color = targetColor;
        isTransitioning = false;
    }
}
