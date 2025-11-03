using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EffectDetail : MonoBehaviour
{
    [Header("firefly")]
    [SerializeField] private bool _firefly = false;

    [ShowIf(nameof(_firefly))]
    [Header("Firefly Movement (Sin Wave)")]
    [SerializeField] private float _horizontalAmplitude = 0.5f; // Biên độ dao động ngang
    [ShowIf(nameof(_firefly))]
    [SerializeField] private float _verticalAmplitude = 0.2f;   // Biên độ dao động dọc (tuỳ chọn)
    [ShowIf(nameof(_firefly))]
    [SerializeField] private float _waveFrequency = 2f;          // Tần số sóng
    private float _timeOffset; // để mỗi con bay lệch pha nhau
    private Vector3 _startPos;

    [Header("Scale")]
    [SerializeField] private float _minScale = 1f;
    [SerializeField] private float _maxScale = 3f;

    [Header("Time Display")]
    [SerializeField] private float _minTimeDisplay = 2f;
    [SerializeField] private float _maxTimeDisplay = 4f;

    [Header("Speed")]
    [SerializeField] private float _minMoveSpeed = 1f;
    [SerializeField] private float _maxMoveSpeed = 2f;

    [Header("Sprite")]
    [SerializeField] private Sprite[] _sprite;

    [Header("Light")]
    [SerializeField] private Light2D _light;

    private float _moveSpeed;
    private float _timeDisplay;
    private float _scale;
    [SerializeField] private SpriteRenderer _spriterender;
    private Coroutine _displayRoutine;

    private void Awake()
    {
        if (_spriterender == null)
            _spriterender = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!_firefly)
            transform.position += Vector3.up * _moveSpeed * Time.deltaTime;
        else
        {
            // Đom đóm bay hình sin
            float time = Time.time + _timeOffset;

            // Sóng ngang: dao động theo trục X
            float offsetX = Mathf.Sin(time * _waveFrequency) * _horizontalAmplitude;

            // Sóng dọc nhẹ: dao động thêm trên Y
            float offsetY = Mathf.Sin(time * _waveFrequency * 0.5f) * _verticalAmplitude;

            Vector3 pos = transform.position;
            pos += new Vector3(offsetX, _moveSpeed * Time.deltaTime + offsetY * Time.deltaTime, 0f);

            transform.position = pos;
        }
    }

    void OnEnable()
    {
        if (_displayRoutine != null)
            StopCoroutine(_displayRoutine);
        _displayRoutine = StartCoroutine(Display(_scale));
    }

    public void Set(Vector2 startPosition)
    {
        transform.position = startPosition;

        _light.intensity = Random.Range(1.5f, 2f);

        _timeDisplay = Random.Range(_minTimeDisplay, _maxTimeDisplay);
        _moveSpeed = Random.Range(_minMoveSpeed, _maxMoveSpeed);

        // Scale tỉ lệ thuận với thời gian tồn tại
        float normalizedLife = Mathf.InverseLerp(_minTimeDisplay, _maxTimeDisplay, _timeDisplay);
        _scale = Mathf.Lerp(_minScale, _maxScale, normalizedLife);

        int spriteIndex = Random.Range(0, _sprite.Length);
        _spriterender.sprite = _sprite[spriteIndex];

        _spriterender.sortingOrder = -(int)(startPosition.y * 100) + 10000;

        // Reset trạng thái
        Color c = _spriterender.color;
        c.a = 0f;
        _spriterender.color = c;
        transform.localScale = Vector3.zero;

        _startPos = startPosition;
        _timeOffset = Random.Range(0f, Mathf.PI * 2f); // lệch pha giúp bay không đồng bộ
    }

    private IEnumerator Display(float scale)
    {
        float tIn = _timeDisplay * 0.25f;
        float tIdle = _timeDisplay * 0.35f;
        float tOut = _timeDisplay - tIn - tIdle;

        float t = 0f;
        Color color = _spriterender.color;
        Vector3 baseScale = Vector3.one * scale;

        // Fade In + Phóng to (Ease Out)
        while (t < tIn)
        {
            t += Time.deltaTime;
            float progress = Mathf.Clamp01(t / tIn);
            float eased = EaseOutCubic(progress);

            color.a = Mathf.Lerp(0f, 1f, eased);
            transform.localScale = Vector3.Lerp(Vector3.zero, baseScale, eased);

            _spriterender.color = color;
            yield return null;
        }

        color.a = 1f;
        _spriterender.color = color;
        transform.localScale = baseScale;

        // Idle với dao động nhẹ (pulse)
        float idleTimer = 0f;
        while (idleTimer < tIdle)
        {
            idleTimer += Time.deltaTime;
            float pulse = 1f + Mathf.Sin(Time.time * 6f) * 0.05f; // 5% dao động
            transform.localScale = baseScale * pulse;
            yield return null;
        }

        // Fade Out + Thu nhỏ (Ease In)
        t = 0f;
        while (t < tOut)
        {
            t += Time.deltaTime;
            float progress = Mathf.Clamp01(t / tOut);
            float eased = EaseInCubic(progress);

            color.a = Mathf.Lerp(1f, 0f, eased);
            transform.localScale = Vector3.Lerp(baseScale, Vector3.zero, eased);

            _spriterender.color = color;
            yield return null;
        }

        gameObject.SetActive(false);
    }

    private float EaseOutCubic(float x) => 1 - Mathf.Pow(1 - x, 3);
    private float EaseInCubic(float x) => x * x * x;

}
