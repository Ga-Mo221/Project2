using NaughtyAttributes;
using TMPro;
using System;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance { get; private set; }


    [SerializeField] private Animator _anim;
    [SerializeField] private Animator _loadGameAnim;

    [SerializeField] private TextMeshProUGUI _coin;

    [Foldout("Online")]
    [SerializeField] private GameObject _button_StartGame;
    [Foldout("Online")]
    [SerializeField] private GameObject _button_Mutiplayer;
    [Foldout("Online")]
    [SerializeField] private GameObject _panel_Online;
    [Foldout("Online")]
    [SerializeField] private GameObject _panel_Loadding;
    [Foldout("Online")]
    [SerializeField] private GameObject _panel_InRoom;
    [Foldout("Online")]
    [SerializeField] private GameObject _panel_OutRoom;
    [Foldout("Online")]
    [SerializeField] private TextMeshProUGUI _InputPassINRoom;
    [Foldout("Online")]
    [SerializeField] private Animator _panel_InputPass;

    private string room_name;
    //private string pass;
    //public event Action _onEnterPassJoineRoom;

    private UnitAudio _audio;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _audio = GetComponent<UnitAudio>();
    }

    void Start()
    {
        CursorManager.Instance.SetNormalCursor();
        _audio.PlayBackgroundSound();
        UpdateCoin();
    }

    void OnEnable()
    {
        UpdateCoin();
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!CursorManager.Instance.ChoseUI)
            {
                if (_anim.gameObject.activeInHierarchy)
                    _anim.SetTrigger("exit");
                if (_panel_InputPass.gameObject.activeInHierarchy)
                    _panel_InputPass.SetTrigger("exit");
            }
        }

    }

    public void UpdateCoin()
    {
        if (SettingManager.Instance != null)
            _coin.text = SettingManager.Instance._gameSettings._coin.ToString();
    }

    public void OpenSetting()
    {
        if (SettingManager.Instance != null)
            SettingManager.Instance.OpenSetting();
    }

    public void OpenMenu()
    {
        _anim.gameObject.SetActive(true);
    }

    public void OnOnline()
    {
        // PhotonNetwork.ConnectUsingSettings(); // bắt đầu connect
        // Debug.Log("🔄 Đang kết nối Photon...");
        // _panel_Loadding.SetActive(true);
    }

    public void OutOnlineOrRoom()
    {
        // if (SettingManager.Instance.getOnline() && SettingManager.Instance.getInRoom())
        // {
        //     if (PhotonNetwork.InRoom)
        //     {
        //         Debug.Log("Đang rời khỏi phòng...");
        //         _panel_Loadding.SetActive(true);
        //         PhotonNetwork.LeaveRoom();
        //     }
        // }
        // else if (SettingManager.Instance.getOnline() && !SettingManager.Instance.getInRoom())
        // {
        //     _button_Mutiplayer.SetActive(true);
        //     _button_StartGame.SetActive(true);
        //     _panel_Online.SetActive(false);
        //     PhotonNetwork.Disconnect();
        // }
    }

    public void ReturnToLobby()
    {
        SettingManager.Instance.setInRoom(false);
        _panel_InRoom.SetActive(false);
        _panel_OutRoom.SetActive(true);
    }

    public void IsOnline()
    {
        SettingManager.Instance.setOnline(true);
        _panel_Loadding.SetActive(false);
        _panel_Online.SetActive(true);
    }

    public void OnInRoom()
    {
        Debug.Log("Đang Tạo Phòng");
        _panel_Loadding.SetActive(true);
    }

    public void isInRoom()
    {
        _panel_Loadding.SetActive(false);
        SettingManager.Instance.setInRoom(true);

        _panel_InRoom.SetActive(true);
        _panel_OutRoom.SetActive(false);
    }

    public void cacheRomPropety(string name, string paSS)
    {
        // room_name = name;
        // pass = paSS;
        // _panel_InputPass.gameObject.SetActive(true);
    }

    public void EnterPass()
    {
        // string input = _InputPassINRoom.text.Trim();
        // string target = pass.Trim();

        // Debug.Log($"[CHECK] Input='{input}' | Pass='{target}'");

        // if (CheckString.Check(input, pass))
        // {
        //     _onEnterPassJoineRoom?.Invoke();
        //     Debug.Log("ok");
        // }
        // else Debug.Log("sai");
    }
    public string getNameRoom() => room_name;

    public void JoinRoom()
    {
        _panel_InputPass.SetTrigger("exit");
    }

    public void Tutorial()
    {
        if (SettingManager.Instance == null) return;
        SettingManager.Instance._gameSettings._Tutorial = true;
        StartGameWithMapSelection();
    }

    public void StartGameWithMapSelection()
    {
        // Gọi hàm này thay vì NewGame() hiện tại
        _anim.SetTrigger("exit");
        _loadGameAnim.SetTrigger("Load");

        SettingManager.Instance._playing = true;
        
        // Random chọn loại map
        if (MapSelectionManager.Instance != null)
        {
            MapSelectionManager.Instance.SelectRandomMap();
            string loadingScene = MapSelectionManager.Instance.GetLoadingSceneName();

            Debug.Log($"[MainMenu] Loading scene: {loadingScene}");
            var script = _loadGameAnim.GetComponent<SceneController>();
            script.OpenSceneLoadMap(loadingScene);
        }
        else
        {
            Debug.LogError("[MainMenu] MapSelectionManager not found!");
        }
    }
}
