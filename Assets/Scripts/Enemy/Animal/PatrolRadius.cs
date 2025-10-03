using UnityEngine;

public class PatrolRadius : MonoBehaviour
{
    [SerializeField] private AnimalAI _PatrolRadius;

    #region Draw
    protected virtual void OnDrawGizmosSelected()
    {
        // radius patrol
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _PatrolRadius._PatrolRadius);
    }
    #endregion

}
