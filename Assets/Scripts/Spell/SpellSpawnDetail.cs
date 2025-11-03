using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SpellSpawnDetail : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject _effect_Detail;
    [SerializeField] private int _count = 100;
    [SerializeField] private float _minDistan = 2f;
    [SerializeField] private float _maxDistan = 4f;
    [SerializeField] private float _time_display = 4f;

    [Header("Spawn Control")]
    [SerializeField] private float _spawnInterval = 0.05f; // Thời gian giữa mỗi lần spawn
    [SerializeField] private int _particlesPerSpawn = 3; // Số particle mỗi lần spawn

    [SerializeField] private List<EffectDetail> _effects = new List<EffectDetail>();

    [Header("Ellipse Settings")]
    public float _radiusX = 3f;
    public float _radiusY = 1.5f;
    public int _segments = 64;
    public Color color = Color.cyan;

    [Header("Fade Time")]
    [Range(0f,0.5f)] [SerializeField] private float _fadeIn = 0.25f;
    [Range(0f,0.8f)] [SerializeField] private float _fadeIdle = 0.35f;

    private SpriteRenderer _spriterender;
    [SerializeField] private Transform _poin_Oder_Sprite;
    [SerializeField] private Light2D _light;
    private Transform _transform;
    
    // Cache để tối ưu
    private int _spawnedCount;
    //private float _goldenAngle = 137.508f; // Golden angle cho phân bố đều
    private WaitForSeconds _spawnWait;

    public bool canBuff = false;

    private void Awake()
    {
        _spriterender = GetComponent<SpriteRenderer>();
        _transform = transform;
        _spawnWait = new WaitForSeconds(_spawnInterval);
    }

    private void OnEnable()
    {
        int Oder = -(int)(_poin_Oder_Sprite.position.y * 100) + 10000;
        _spriterender.sortingOrder = Oder;
        _spawnedCount = 0;
        foreach (var i in _effects)
            i.gameObject.SetActive(false);
        StartCoroutine(FadeAndSpawn(_spriterender));
    }

    private IEnumerator FadeAndSpawn(SpriteRenderer renderer)
    {
        float tIn = _time_display * _fadeIn;
        float tIdle = _time_display * _fadeIdle;
        float tOut = _time_display - tIn - tIdle;

        float t = 0;
        Color c = renderer.color;

        // ---------------- Fade In ----------------
        while (t < tIn)
        {
            t += Time.deltaTime;
            float progress = Mathf.SmoothStep(0, 1, t / tIn);
            c.a = progress;
            _light.intensity = progress;
            renderer.color = c;
            yield return null;
        }
        _light.intensity = 1f;
        // ---------------- Spawn Phase (Idle + 10% FadeOut) ----------------
        float spawnDuration = tIdle + (tOut * 0.1f);
        canBuff = true;
        Coroutine spawnRoutine = StartCoroutine(ContinuousSpawn(spawnDuration));

        // ---------------- Idle ----------------
        yield return new WaitForSeconds(tIdle);

        // ---------------- Fade Out ----------------
        t = 0;
        while (t < tOut)
        {
            t += Time.deltaTime;
            float progress = Mathf.SmoothStep(0, 1, t / tOut);
            c.a = Mathf.Lerp(1, 0, progress);
            _light.intensity = Mathf.Lerp(1, 0, progress); // Giống alpha
            renderer.color = c;
            yield return null;
        }
        _light.intensity = 0f;

        // Đảm bảo spawn routine dừng
        if (spawnRoutine != null)
            StopCoroutine(spawnRoutine);

        gameObject.SetActive(false);
    }

    private IEnumerator ContinuousSpawn(float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration && _spawnedCount < _count)
        {
            SpawnWaveOptimized();
            elapsed += _spawnInterval;
            yield return _spawnWait;
        }
        canBuff = false;
    }

    private void SpawnWaveOptimized()
    {
        int toSpawn = Mathf.Min(_particlesPerSpawn, _count - _spawnedCount);

        for (int i = 0; i < toSpawn; i++)
        {
            EffectDetail detail = GetInactiveDetail();

            // Phương pháp sampling đều trong ellipse
            // Bước 1: Random góc
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;

            // Bước 2: Random radius với căn bậc 2 để phân bố đều diện tích
            float sqrtRandom = Mathf.Sqrt(Random.Range(0f, 1f));

            // Bước 3: Nếu muốn spawn từ tâm -> set _minDistan = 0
            // Nếu muốn vòng rỗng -> giữ _minDistan > 0
            float normalizedRadius = Mathf.Lerp(_minDistan / _maxDistan, 1f, sqrtRandom);

            // Bước 4: Tính vị trí trong ellipse
            float x = Mathf.Cos(angle) * normalizedRadius * _radiusX;
            float y = Mathf.Sin(angle) * normalizedRadius * _radiusY;

            Vector3 pos = _transform.position + new Vector3(x, y, 0f);

            detail.Set(pos);
            detail.gameObject.SetActive(true);

            _spawnedCount++;
        }
    }

    private EffectDetail GetInactiveDetail()
    {
        // Tối ưu: dùng for loop thay vì foreach
        for (int i = 0; i < _effects.Count; i++)
        {
            if (!_effects[i].gameObject.activeSelf)
                return _effects[i];
        }

        // Nếu hết – tạo mới
        GameObject obj = Instantiate(_effect_Detail, _transform);
        EffectDetail newDetail = obj.GetComponent<EffectDetail>();
        _effects.Add(newDetail);
        return newDetail;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Vector3 prevPoint = Vector3.zero;
        Vector3 firstPoint = Vector3.zero;

        for (int i = 0; i <= _segments; i++)
        {
            float angle = i * Mathf.PI * 2f / _segments;
            float x = Mathf.Cos(angle) * _radiusX;
            float y = Mathf.Sin(angle) * _radiusY;
            Vector3 point = transform.position + new Vector3(x, y, 0);

            if (i == 0)
                firstPoint = point;
            else
                Gizmos.DrawLine(prevPoint, point);

            prevPoint = point;
        }
        Gizmos.DrawLine(prevPoint, firstPoint);
    }

    public float getTimeBuff() => _time_display * _fadeIdle;
}