using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Lights : MonoBehaviour
{
    private Light2D _light;

    [SerializeField] private Color _nightColor;
    [SerializeField] private Color _defaultColor;

    private float transitionTime;
    private float timer = 0f;            // đếm thời gian đang chuyển
    private bool isTransitioning = false;
    private Color startColor;
    private Color targetColor;

    private void Start()
    {
        _light = GetComponent<Light2D>();
        _light.color = _defaultColor;
        transitionTime = GameManager.Instance._2Hours_Sec;
    }

    private void Update()
    {
        int timeRTS = GameManager.Instance._timeRTS; // giá trị: 4, 8, 12, 16, 20, 24

        // Khi tới 16h → bắt đầu chuyển sang night
        if (timeRTS == 16 && !isTransitioning)
        {
            StartTransition(_defaultColor, _nightColor);
        }

        // Khi tới 4h → bắt đầu chuyển sang default
        if (timeRTS == 4 && !isTransitioning)
        {
            StartTransition(_nightColor, _defaultColor);
        }

        // Nếu đang chuyển màu
        if (isTransitioning)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / transitionTime);
            _light.color = Color.Lerp(startColor, targetColor, t);

            if (t >= 1f)
                isTransitioning = false;
        }
    }

    private void StartTransition(Color from, Color to)
    {
        startColor = from;
        targetColor = to;
        timer = 0f;
        isTransitioning = true;
    }
}
