using UnityEngine;

public class TestAddCoin : MonoBehaviour
{
    [SerializeField] private int coin;

    public void addcoin()
    {
        if (SettingManager.Instance == null || MainMenu.Instance == null) return;
        SettingManager.Instance._gameSettings._coin += coin;
        SettingManager.Instance.Save();
        MainMenu.Instance.UpdateCoin();
    }
}
