using UnityEngine;

public class HPBar : MonoBehaviour
{
    [Header("Sprite Config")]
    [SerializeField] private SpriteRenderer _hpFill;
    [SerializeField] private Sprite _xanh;
    [SerializeField] private Sprite _do;
    [SerializeField] private bool _isPlayer = false;

    private Vector3 _originalScale;
    private float _originalWidth;

    void Awake()
    {
        _originalScale = _hpFill.transform.localScale;
        _originalWidth = _hpFill.bounds.size.x;

        _hpFill.sprite = _isPlayer ? _xanh : _do;
    }

    /// <summary>
    /// ratio = currentHP / maxHP (0 → 1)
    /// </summary>
    public void SetHealth(float ratio)
    {
        ratio = Mathf.Clamp01(ratio);

        // Scale X thay đổi theo HP
        float newScaleX = _originalScale.x * ratio;
        _hpFill.transform.localScale = new Vector3(newScaleX, _originalScale.y, _originalScale.z);

        // Offset để giữ mép trái cố định
        float offset = (_originalWidth * (1 - ratio)) * 0.5f;
        _hpFill.transform.localPosition = new Vector3(-offset, _hpFill.transform.localPosition.y, 0f);
    }
}
