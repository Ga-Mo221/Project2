using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private TNTGFX _TNT;

    public void setActive() => _TNT.setActive();
}