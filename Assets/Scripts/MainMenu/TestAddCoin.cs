using UnityEngine;

public class TestAddCoin : MonoBehaviour
{
    [SerializeField] private int coin;

    public void addcoin()
    {
        SettingManager.Instance._gameSettings._coin += coin;
        MainMenu.Instance.UpdateCoin();
    }
}
