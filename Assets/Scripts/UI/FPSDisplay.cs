using UnityEngine;
using TMPro;

public class FPSDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fpsText;
    [SerializeField] private float updateInterval = 0.5f; // cập nhật mỗi 0.5 giây

    private float timer;
    private int frames;
    private float fps;

    private void Awake()
    {
        // Giới hạn FPS mong muốn
        Application.targetFrameRate = 300; // hoặc -1 để bỏ giới hạn
        QualitySettings.vSyncCount = 0; // tắt VSync để targetFrameRate có hiệu lực
    }

    private void Update()
    {
        frames++;
        timer += Time.unscaledDeltaTime; // dùng unscaled để không bị ảnh hưởng bởi Time.timeScale

        if (timer >= updateInterval)
        {
            fps = frames / timer;
            fpsText.text = $"FPS : {fps:0.}";

            // Đổi màu theo tốc độ khung hình
            if (fps >= 50)
                fpsText.color = Color.green;
            else if (fps >= 30)
                fpsText.color = Color.yellow;
            else
                fpsText.color = Color.red;

            frames = 0;
            timer = 0f;
        }
    }
}
