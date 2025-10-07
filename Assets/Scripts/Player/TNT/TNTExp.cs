using UnityEngine;

public class TNTExp : MonoBehaviour
{
    [SerializeField] private PlayerAI _player;

    private CircleCollider2D _col;

    void Awake()
    {
        _col = GetComponent<CircleCollider2D>();
        if (_col != null && _player != null)
        {
            _col.radius = _player._range;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;
        if (collision.CompareTag("Animal"))
        {
            CameraShake.Instance.ShakeCamera();
            var animal = collision.GetComponent<AnimalHealth>();
            if (animal != null && _player != null)
                animal.takeDamage(_player._damage, gameObject);
        }
        else if (collision.CompareTag("Enemy"))
        {
            CameraShake.Instance.ShakeCamera();
            var enemy = collision.GetComponent<EnemyHealth>();
            if (enemy != null && _player != null)
                enemy.takeDamage(_player._damage);
        }
    }
}
