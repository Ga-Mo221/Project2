using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ServerManager : MonoBehaviourPunCallbacks
{
    public static ServerManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("✅ Kết nối Photon thành công!");
        PhotonNetwork.JoinLobby(); // Photon tự đồng bộ danh sách phòng
        MainMenu.Instance.IsOnline();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("🟢 Đã vào lobby, chờ danh sách phòng...");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarning("❌ Mất kết nối: " + cause);
        SettingManager.Instance.setOnline(false);
    }
}
