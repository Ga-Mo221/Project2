using UnityEngine;

public class Rada : MonoBehaviour
{
    [Header("Detection Radii")]
    [Tooltip("Bán kính hiển thị outline/minimap")]
    public float displayRadius = 20f;
    
    [Tooltip("Bán kính phát hiện chill effect")]
    public float detectionRadius = 30f;

    [Header("State")]
    [SerializeField] private bool isActive = true;
    [SerializeField] private bool isDead = false;

    // Properties for manager
    public Vector2 Position => transform.position;
    public float DisplayRadius => displayRadius;
    public float DetectionRadius => detectionRadius;
    public bool IsActive => isActive && !isDead;
    public bool IsDead => isDead;

    // Cached position for optimization
    private Vector2 lastPosition;
    private float positionCheckInterval = 0.1f;
    private float positionCheckTimer = 0f;
    
    // Registration flag
    private bool isRegistered = false;

    void Start()
    {
        RegisterWithManager();
        lastPosition = transform.position;
    }

    void OnEnable()
    {
        // Đợi 1 frame để đảm bảo DetectionManager đã sẵn sàng
        if (Application.isPlaying && !isRegistered)
        {
            Invoke(nameof(RegisterWithManager), 0.1f);
        }
        
        lastPosition = transform.position;
    }

    void OnDisable()
    {
        UnregisterFromManager();
    }

    void OnDestroy()
    {
        UnregisterFromManager();
    }

    private void RegisterWithManager()
    {
        if (isRegistered) return;
        
        if (DetectionManager.Instance != null)
        {
            DetectionManager.Instance.RegisterRadar(this);
            isRegistered = true;
            //Debug.Log($"[Rada] {gameObject.name} registered successfully");
        }
        else
        {
            Debug.LogWarning($"[Rada] {gameObject.name} - DetectionManager not found! Retrying...");
            // Thử lại sau 0.5s
            Invoke(nameof(RegisterWithManager), 0.5f);
        }
    }

    private void UnregisterFromManager()
    {
        if (!isRegistered) return;
        
        if (DetectionManager.Instance != null)
        {
            DetectionManager.Instance.UnregisterRadar(this);
            isRegistered = false;
            Debug.Log($"[Rada] {gameObject.name} unregistered");
        }
    }

    void Update()
    {
        // Đảm bảo đã đăng ký
        if (!isRegistered && DetectionManager.Instance != null)
        {
            RegisterWithManager();
        }

        // Chỉ check position thay đổi mỗi 0.1s
        positionCheckTimer += Time.deltaTime;
        if (positionCheckTimer >= positionCheckInterval)
        {
            positionCheckTimer = 0f;
            
            Vector2 currentPos = transform.position;
            if (Vector2.Distance(currentPos, lastPosition) > 0.1f)
            {
                lastPosition = currentPos;
            }
        }
    }

    public void SetActive(bool active)
    {
        isActive = active;
        Debug.Log($"[Rada] {gameObject.name} active state: {isActive}");
    }
    
    public void SetDead(bool dead)
    {
        isDead = dead;
        if (isDead)
        {
            Debug.Log($"[Rada] {gameObject.name} is now dead");
        }
    }

    void OnDrawGizmosSelected()
    {
        // Display radius (đỏ)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, displayRadius);

        // Detection radius (xanh)
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}

// using UnityEngine;

// public class Rada : MonoBehaviour
// {
//     [Header("Detection Radii")]
//     [Tooltip("Bán kính hiển thị outline/minimap")]
//     public float displayRadius = 20f;
    
//     [Tooltip("Bán kính phát hiện chill effect")]
//     public float detectionRadius = 30f;

//     [Header("State")]
//     [SerializeField] private bool isActive = true;
//     [SerializeField] private bool isDead = false;

//     // Properties for manager
//     public Vector2 Position => transform.position;
//     public float DisplayRadius => displayRadius;
//     public float DetectionRadius => detectionRadius;
//     public bool IsActive => isActive && !isDead;
//     public bool IsDead => isDead;

//     // Cached position for optimization
//     private Vector2 lastPosition;
//     private float positionCheckInterval = 0.1f;
//     private float positionCheckTimer = 0f;

//     void Start()
//     {
//         // Đăng ký với manager
//         if (DetectionManager.Instance != null)
//         {
//             DetectionManager.Instance.RegisterRadar(this);
//         }
        
//         lastPosition = transform.position;
//     }

//     void OnEnable()
//     {
//         // Đăng ký với manager
//         if (DetectionManager.Instance != null)
//         {
//             DetectionManager.Instance.RegisterRadar(this);
//         }
        
//         lastPosition = transform.position;
//     }

//     void OnDisable()
//     {
//         // Hủy đăng ký
//         if (DetectionManager.Instance != null)
//         {
//             DetectionManager.Instance.UnregisterRadar(this);
//         }
//     }

//     void Update()
//     {
//         // Chỉ check position thay đổi mỗi 0.1s
//         positionCheckTimer += Time.deltaTime;
//         if (positionCheckTimer >= positionCheckInterval)
//         {
//             positionCheckTimer = 0f;
            
//             Vector2 currentPos = transform.position;
//             if (Vector2.Distance(currentPos, lastPosition) > 0.1f)
//             {
//                 lastPosition = currentPos;
//             }
//         }
//     }

//     public void SetActive(bool active) => isActive = active;
//     public void SetDead(bool dead) => isDead = dead;

//     void OnDrawGizmosSelected()
//     {
//         // Display radius (đỏ)
//         Gizmos.color = Color.red;
//         Gizmos.DrawWireSphere(transform.position, displayRadius);

//         // Detection radius (xanh)
//         Gizmos.color = Color.green;
//         Gizmos.DrawWireSphere(transform.position, detectionRadius);
//     }
// }