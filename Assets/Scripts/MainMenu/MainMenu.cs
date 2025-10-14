using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    [SerializeField] private Animator _loadGameAnim;
    private UnitAudio _audio;

    void Awake()
    {
        _audio = GetComponent<UnitAudio>();
    }

    void Start()
    {
        CursorManager.Instance.SetNormalCursor();
        _audio.PlayBackgroundSound();
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            if (!CursorManager.Instance.ChoseUI)
                _anim.SetTrigger("exit");
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


    public void NewGame()
    {
        _anim.SetTrigger("exit");
        _loadGameAnim.SetTrigger("Load");
        SettingManager.Instance._playing = true;
    }
}
