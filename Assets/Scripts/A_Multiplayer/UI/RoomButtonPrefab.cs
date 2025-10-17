using TMPro;
using UnityEngine;
using Photon.Pun;
using System.Collections;

public class RoomButtonPrefab : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _roomName;
    [SerializeField] private TextMeshProUGUI _playerValue;
    [SerializeField] private GameObject _lockIcon;
    [SerializeField] private string _pass = "";
    [SerializeField] private bool _isLocked = false;
    [SerializeField] private int _currentPlayers;
    [SerializeField] private int _maxPlayers;

    void OnEnable()
    {
        MainMenu.Instance._onEnterPassJoineRoom += JoineRoomEvent;
    }

    void OnDisable()
    {
        MainMenu.Instance._onEnterPassJoineRoom -= JoineRoomEvent;
    }

    private void JoineRoomEvent()
    {
            Debug.Log(1);
        if (MainMenu.Instance.getNameRoom() == _roomName.text)
        {
            MainMenu.Instance.JoinRoom();
            StartCoroutine(join());
        }
    }
    
    private IEnumerator join()
    {
        yield return new WaitForSeconds(0.5f);
        PhotonNetwork.JoinRoom(_roomName.text);
    }

    // Truyền thêm mật khẩu + số người
    public void SetRoom(string name, bool isLocked, string pass = "", int current = 0, int max = 0)
    {
        _roomName.text = name;
        _isLocked = isLocked;
        _pass = pass;
        _lockIcon.SetActive(_isLocked);

        _currentPlayers = current;
        _maxPlayers = max;

        _playerValue.text = $"{_currentPlayers}/{_maxPlayers}";
    }

    public void JoinRoom()
    {
        // Kiểm tra full
        if (_currentPlayers >= _maxPlayers)
        {
            Debug.LogWarning($"⚠️ Phòng {_roomName.text} đã đầy ({_currentPlayers}/{_maxPlayers})");
            return;
        }

        if (_isLocked)
        {
            Debug.Log($"🔒 Phòng '{_roomName.text}' bị khóa. Mật khẩu: {_pass}");
            MainMenu.Instance.cacheRomPropety(_roomName.text, _pass);
            // Mở popup nhập mật khẩu tại đây nếu bạn có UI
            return;
        }

        if (string.IsNullOrEmpty(_roomName.text))
        {
            Debug.LogWarning("⚠️ Không có tên phòng!");
            return;
        }

        PhotonNetwork.JoinRoom(_roomName.text);
    }
}
