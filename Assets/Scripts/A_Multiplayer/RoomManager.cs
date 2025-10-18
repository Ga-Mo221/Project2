using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using ExitGames.Client.Photon;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField _textCreateRoomName;
    [SerializeField] private TMP_InputField _passwordInput;
    [SerializeField] private Toggle _lock;
    [SerializeField] private GameObject _roomPrefab;
    [SerializeField] private Transform _gridRoom;
    [SerializeField] private InRoomController _inroomController;

    private readonly Dictionary<string, RoomInfo> cachedRoomList = new Dictionary<string, RoomInfo>();

    // Khi vào lobby, Photon tự gửi danh sách phòng hiện có
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (var info in roomList)
        {
            if (info.RemovedFromList)
            {
                if (cachedRoomList.ContainsKey(info.Name))
                    cachedRoomList.Remove(info.Name);
            }
            else
                cachedRoomList[info.Name] = info;
        }

        UpdateRoomListUI();
    }

    private void UpdateRoomListUI()
    {
        foreach (Transform child in _gridRoom)
            Destroy(child.gameObject);

        foreach (var room in cachedRoomList.Values)
        {
            GameObject obj = Instantiate(_roomPrefab, _gridRoom);
            var script = obj.GetComponent<RoomButtonPrefab>();

            bool isLocked = false;
            string pass = "";
            int currentPlayers = room.PlayerCount;
            int maxPlayers = room.MaxPlayers;

            if (room.CustomProperties.ContainsKey("lock"))
                isLocked = (bool)room.CustomProperties["lock"];
            if (room.CustomProperties.ContainsKey("pass"))
                pass = (string)room.CustomProperties["pass"];

            // 👇 truyền luôn cả PlayerCount và MaxPlayers vào
            script.SetRoom(room.Name, isLocked, pass, currentPlayers, maxPlayers);
        }
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(_textCreateRoomName.text))
        {
            Debug.LogWarning("⚠️ Chưa nhập tên phòng!");
            return;
        }

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = SettingManager.Instance.getPlayerValue();
        roomOptions.IsVisible = true;

        var customProps = new Hashtable
        {
            { "lock", _lock.isOn },
            { "pass", _passwordInput.text }
        };
        roomOptions.CustomRoomProperties = customProps;
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "lock", "pass" };
        
        PhotonNetwork.CreateRoom(_textCreateRoomName.text, roomOptions, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("✅ Phòng đã được tạo!");
        MainMenu.Instance.OnInRoom();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("🔹 Đã vào phòng!");
        PhotonNetwork.AutomaticallySyncScene = true;
        MainMenu.Instance.isInRoom();
        _inroomController.ResetRoom();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"❌ Tạo phòng thất bại: {message}");
    }

    public override void OnLeftRoom()
    {
        Debug.Log("↩️ Rời khỏi phòng!");
        MainMenu.Instance.ReturnToLobby();
    }

    public void Active2Player() => SettingManager.Instance.setPlayerValue(2);
    public void Active3Player() => SettingManager.Instance.setPlayerValue(3);
    public void Active4Player() => SettingManager.Instance.setPlayerValue(4);
}
