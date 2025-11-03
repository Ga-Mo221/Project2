using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILoadingGenerateMap : MonoBehaviour
{
    [SerializeField] private Image _img_load;
    [SerializeField] private TextMeshProUGUI _txt_status;
    [SerializeField] private TextMeshProUGUI _txt_detail;

    [Header("Sprite Frames (0% → 100%)")]
    [SerializeField] private Sprite[] _sprites_loading; // 11 sprites từ 0% → 100%

    [Header("Smooth Settings")]
    [SerializeField] private float smoothSpeed = 2f; // tốc độ nội suy (càng cao càng nhanh)

    private float smoothProgress = 0f;

    void Update()
    {
        if (LoadingSceneController.Instance == null)
            return;

        // Cập nhật text
        _txt_status.text = LoadingSceneController.Instance.statusText;
        _txt_detail.text = LoadingSceneController.Instance.detailText;

        // Lấy progress thực từ LoadingSceneController
        float targetProgress = Mathf.Clamp01(LoadingSceneController.Instance.currentProgress);

        // Làm mượt chuyển tiếp
        smoothProgress = Mathf.MoveTowards(smoothProgress, targetProgress, Time.deltaTime * smoothSpeed);

        // Cập nhật sprite theo progress
        UpdateLoadingSprite(smoothProgress);
    }

    private void UpdateLoadingSprite(float progress)
    {
        if (_sprites_loading == null || _sprites_loading.Length == 0)
            return;

        // Tính chỉ số sprite tương ứng (0 → 10)
        int spriteIndex = Mathf.Clamp(Mathf.FloorToInt(progress * (_sprites_loading.Length - 1)), 0, _sprites_loading.Length - 1);

        // Đổi hình
        _img_load.sprite = _sprites_loading[spriteIndex];
    }
}
