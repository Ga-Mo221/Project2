using TMPro;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;

public class InRoomController : MonoBehaviourPunCallbacks
{
    [Header("UI Player Slots")]
    [SerializeField] private GameObject _player1;
    [SerializeField] private GameObject _player2;
    [SerializeField] private GameObject _player2_Block;
    [SerializeField] private GameObject _player2_Ready;
    [SerializeField] private GameObject _player3;
    [SerializeField] private GameObject _player3_Block;
    [SerializeField] private GameObject _player3_Ready;
    [SerializeField] private GameObject _player4;
    [SerializeField] private GameObject _player4_Block;
    [SerializeField] private GameObject _player4_Ready;

    [Header("UI Texts")]
    [SerializeField] private TextMeshProUGUI _roomName1;
    [SerializeField] private TextMeshProUGUI _roomName2;
    [SerializeField] private TextMeshProUGUI _button_Start_or_Ready;

    private int _currentPlayers;

    void Start()
    {
        ResetRoom();
    }

    public void ResetRoom()
    {
        if (PhotonNetwork.CurrentRoom == null) return;

        _currentPlayers = PhotonNetwork.CurrentRoom.PlayerCount;
        _roomName1.text = PhotonNetwork.CurrentRoom.Name;
        _roomName2.text = _roomName1.text;

        UpdatePlayerSlots();
        UpdateButtonState();
        UpdateAllReadyIcons();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"🟢 {newPlayer.NickName} vào phòng");
        UpdatePlayerSlots();
        UpdateAllReadyIcons();
        CheckAllPlayersReady();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"🔴 {otherPlayer.NickName} rời phòng");
        UpdatePlayerSlots();
        UpdateAllReadyIcons();
        CheckAllPlayersReady();
    }

    private void UpdatePlayerSlots()
    {
        if (PhotonNetwork.CurrentRoom == null) return;

        _currentPlayers = PhotonNetwork.CurrentRoom.PlayerCount;
        int maxPlayers = PhotonNetwork.CurrentRoom.MaxPlayers;

        _player1.SetActive(_currentPlayers >= 1);
        _player2.SetActive(_currentPlayers >= 2);
        _player3.SetActive(_currentPlayers >= 3);
        _player4.SetActive(_currentPlayers >= 4);

        Debug.Log($"👥 {_currentPlayers}/{maxPlayers}");
    }

    private void UpdateButtonState()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _button_Start_or_Ready.text = "Start";
            _player2_Block.SetActive(true);
            _player3_Block.SetActive(true);
            _player4_Block.SetActive(true);
        }
        else
        {
            _button_Start_or_Ready.text = "Ready";
            _player2_Block.SetActive(false);
            _player3_Block.SetActive(false);
            _player4_Block.SetActive(false);
        }
    }

    // Gọi khi nhấn nút Ready
    public void OnClickReady()
    {
        bool ready = PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("isReady")
            ? !(bool)PhotonNetwork.LocalPlayer.CustomProperties["isReady"]
            : true;

        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable
        {
            { "isReady", ready }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        Debug.Log($"🔹 Bạn {(ready ? "đã sẵn sàng" : "huỷ sẵn sàng")}");
    }

    // Gọi khi Master nhấn Start
    public void OnClickStart()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        bool allReady = true;
        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (player.IsMasterClient) continue;
            if (!player.CustomProperties.ContainsKey("isReady") || !(bool)player.CustomProperties["isReady"])
            {
                allReady = false;
                break;
            }
        }

        if (allReady)
        {
            Debug.Log("🚀 Tất cả đã sẵn sàng — bắt đầu trận đấu!");
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            SettingManager.Instance._playing = true;
            PhotonNetwork.LoadLevel("Multiplayer");
        }
        else
        {
            Debug.LogWarning("❌ Không thể Start — có người chưa Ready!");
        }
    }

    // Kiểm tra tất cả player đã ready chưa
    // Kiểm tra tất cả player (ngoại trừ master) đã ready chưa
    private void CheckAllPlayersReady()
    {
        bool allReady = true;

        foreach (var p in PhotonNetwork.PlayerList)
        {
            if (p.IsMasterClient)
                continue; // 👈 Bỏ qua master client

            if (!p.CustomProperties.ContainsKey("isReady") || !(bool)p.CustomProperties["isReady"])
            {
                allReady = false;
                break;
            }
        }

        if (PhotonNetwork.IsMasterClient)
            _button_Start_or_Ready.text = allReady ? "Start" : "Waiting...";
    }

    // Cập nhật hình ảnh ready cho từng player theo thứ tự
    private void UpdateAllReadyIcons()
    {
        // reset toàn bộ icon
        _player2_Ready.SetActive(false);
        _player3_Ready.SetActive(false);
        _player4_Ready.SetActive(false);

        // lấy danh sách người chơi theo thứ tự vào
        var sortedPlayers = PhotonNetwork.PlayerList.OrderBy(p => p.ActorNumber).ToList();

        for (int i = 0; i < sortedPlayers.Count; i++)
        {
            var p = sortedPlayers[i];
            bool isReady = p.CustomProperties.ContainsKey("isReady") && (bool)p.CustomProperties["isReady"];

            switch (i)
            {
                case 0:
                    // player 1 (master) không cần icon Ready
                    break;
                case 1:
                    _player2_Ready.SetActive(isReady);
                    break;
                case 2:
                    _player3_Ready.SetActive(isReady);
                    break;
                case 3:
                    _player4_Ready.SetActive(isReady);
                    break;
            }
        }
    }

    // Mỗi khi có ai đó đổi ready, update lại icon cho tất cả
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        UpdateAllReadyIcons();
        CheckAllPlayersReady();
    }

    public void ButtonClick()
    {
        if (PhotonNetwork.IsMasterClient)
            OnClickStart();
        else
            OnClickReady();
    }
}
