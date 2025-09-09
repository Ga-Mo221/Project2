using UnityEngine;

public class Moba2DCameraController : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 20f;   // tốc độ di chuyển
    [SerializeField] public float edgeSize = 10f;    // khoảng cách mép màn hình để di chuyển

    void Update()
    {
        Vector3 pos = transform.position;

        // --- Di chuyển bằng phím WASD / mũi tên ---
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        pos += new Vector3(h, v, 0) * moveSpeed * Time.deltaTime;

        // --- Di chuyển bằng mép màn hình ---
        if (Input.mousePosition.x >= Screen.width - edgeSize) // mép phải
            pos.x += moveSpeed * Time.deltaTime;
        if (Input.mousePosition.x <= edgeSize) // mép trái
            pos.x -= moveSpeed * Time.deltaTime;
        if (Input.mousePosition.y >= Screen.height - edgeSize) // mép trên
            pos.y += moveSpeed * Time.deltaTime;
        if (Input.mousePosition.y <= edgeSize) // mép dưới
            pos.y -= moveSpeed * Time.deltaTime;

        transform.position = pos;
    }
}
