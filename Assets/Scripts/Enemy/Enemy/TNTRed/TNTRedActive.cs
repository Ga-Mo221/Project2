using UnityEngine;

public class TNTRedActive : MonoBehaviour
{
    public void setActive()
    {
        transform.parent.gameObject.SetActive(false);
    }
}
