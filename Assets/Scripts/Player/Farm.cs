using UnityEngine;

public class Farm : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;
        if (collision.CompareTag("Item"))
        {
            var _script = collision.GetComponent<Item>();
            _script.farm();
        }
    }
}
