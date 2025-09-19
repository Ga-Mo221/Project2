using UnityEngine;

public class HouseHealth : MonoBehaviour
{
    [SerializeField] private bool _canDetec = false;

    public void setCanDetec(bool amount) => _canDetec = amount;
    public bool getCanDetec() => _canDetec;
}
