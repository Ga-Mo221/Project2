using System.Collections;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialSetUp : MonoBehaviour
{
    public static TutorialSetUp Instance { get; private set; }
    public int ID = 0;

    #region Tutorial List
    public bool _StarSelectUnit = false;
    public bool _openNhatKy = false;
    public bool _TutorialNhatKy = false;
    public bool _DropTower = false;
    public bool _OpenShop = false;
    public bool _CreateArcher = false;
    public bool _CloseShop = false;
    public bool _OpenUpgrade = false;
    public bool _CloseUpgrade = false;
    public bool _War = false;
    public bool _Win = true;
    #endregion

    [SerializeField] private Image _BG1;
    [SerializeField] private float _fadeDuration = 1f;
    [SerializeField] private int _currentDay = 1;

    [SerializeField] private Animator _Baw;
    [SerializeField] private Animator _Mess;

    public GameObject _selectBox;
    public bool _isSelect = false;

    [SerializeField] private Click_Icon _blackBox;
    [SerializeField] private Click_Icon _click;

    [SerializeField] private GameObject _enemyList;

    [Foldout("Buttons")]
    [SerializeField] private GameObject _Tab_Button;
    [Foldout("Buttons")]
    [SerializeField] private GameObject _C_Button;
    [Foldout("Buttons")]
    [SerializeField] private GameObject _U_Button;

    [SerializeField] private CameraTutorial _camera;
    [SerializeField] private GameObject _MoveCamera;

    private TypewriterEffect _typewriter;
    private bool _isTyping = false;
    private bool _skipRequested = false;

    [SerializeField] private bool _controller;
    [SerializeField] private bool _select;


    #region Point
    [Foldout("Point")]
    [SerializeField] private Transform _1;
    [Foldout("Point")]
    [SerializeField] private Transform _2;
    [Foldout("Point")]
    [SerializeField] private Transform _3; // button "Nhat ky cua Baw"
    [Foldout("Point")]
    [SerializeField] private Transform _4; // một tháp cung bị phá hủy
    [Foldout("Point")]
    [SerializeField] private Transform _5; // Button open Building Panel
    [Foldout("Point")]
    [SerializeField] private Transform _6; // Button Create Tower
    [Foldout("Point")]
    [SerializeField] private Transform _7; // vị trí đặt tower mới
    [Foldout("Point")]
    [SerializeField] private Transform _8; // vị trí Button Shop
    [Foldout("Point")]
    [SerializeField] private Transform _9; // vị trí Button Archer
    [Foldout("Point")]
    [SerializeField] private Transform _10; // vị trí slot
    [Foldout("Point")]
    [SerializeField] private Transform _11; // vị trí nut thoát
    [Foldout("Point")]
    [SerializeField] private Transform _12; // vị trí nut Upgrade
    [Foldout("Point")]
    [SerializeField] private Transform _13; // vị trí nut upgrade trong panel upgrade
    [Foldout("Point")]
    [SerializeField] private Transform _14; // vị trí nut thoat panel upgrade
    [Foldout("Point")]
    [SerializeField] private Transform _enemy; // target vào enemy khi xuất hiện
    [Foldout("Point")]
    [SerializeField] private Transform _archer; // hướng dẫn archer vào thành
    [Foldout("Point")]
    [SerializeField] private Transform _buttonArcherInCastle1; // vị trí button cho archer
    [Foldout("Point")]
    [SerializeField] private Transform _buttonArcherInCastle2; // vị trí button cho archer

    private string key = "";
    private string txt = "";

    #endregion

    #region Awke
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        if (GameManager.Instance != null)
            GameManager.Instance.Tutorial = true;
    }
    #endregion


    #region Start
    void Start()
    {
        _currentDay = GameManager.Instance._currentDay;
        GameManager.Instance.setActiveGameUI(false);
        _BG1.gameObject.SetActive(true);
        _Baw.gameObject.SetActive(false);
        _Mess.gameObject.SetActive(false);
        _selectBox.SetActive(false);
        OffBlackBox();
        OffClickIcon();
        _typewriter = _Mess.GetComponent<TypewriterEffect>();

        StartCoroutine(start());
        //StartCoroutine(displayText1());
        //TutorialBuilding();
        //TutorialOpenButtonUpgrade();
        //TutorialArcherInCastle();
    }
    #endregion


    #region  Update
    void Update()
    {
        if (_StarSelectUnit && _isSelect)
        {
            if (!_controller)
            {
                _controller = true;
                StartCoroutine(displayText2());
            }
        }else if (_StarSelectUnit && !_isSelect)
        {
            if (!_select && _controller)
            {
                StopAllCoroutines();
                _select = true;
                StartCoroutine(SelectBox());
            }
        }

        if (GameManager.Instance._timeRTS == 0 && _currentDay != GameManager.Instance._currentDay)
        {
            _currentDay = GameManager.Instance._currentDay;
            StartCoroutine(displayText16());
        }

        if (_War)
        {
            GameManager.Instance.TutorialWar = _War;
            _Win = true;
            foreach (var enemy in EnemyHouse.Instance._listEnemyCreate)
            {
                if (!enemy.getDie())
                {
                    Debug.Log(enemy.name);
                    _Win = false;
                    break;
                }
            }

            if (_Win)
            {
                _War = false;
                StopAllCoroutines();
                StartCoroutine(displayText19());
            }
        }

        // Khi nhấn phím Space
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_isTyping)
                _skipRequested = true; // báo hiệu muốn skip
        }
    }
    #endregion


    #region Start Tutorial
    private IEnumerator start()
    {
        yield return new WaitForSeconds(2f);
        _camera.Move(Castle.Instance.transform.position, _fadeDuration + 4);
        Color col = _BG1.color;
        float time = 0f;
        while (time < _fadeDuration)
        {
            time += Time.deltaTime;
            col.a = Mathf.Lerp(1f, 0f, time / _fadeDuration);
            _BG1.color = col;
            yield return null;
        }
        col.a = 0f;
        _BG1.color = col;
        yield return new WaitForSeconds(2f);
        _Baw.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.5f);
        _Mess.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.3f);
        StopAllCoroutines();
        StartCoroutine(displayText1());
    }
    #endregion


    // giới thiệu và hướng dẫn chọn lính
    #region text 1
    private IEnumerator displayText1()
    {
        _Baw.SetBool("noi", true);

        key = "Tutorial.displaytext1.text1";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        string content = txt;
        yield return StartCoroutine(ShowLine(content));

        key = "Tutorial.displaytext1.text2";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        content = txt;
        yield return StartCoroutine(ShowLine(content));

        key = "Tutorial.displaytext1.text3";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        content = txt;
        yield return StartCoroutine(ShowLine(content));

        key = "Tutorial.displaytext1.text4";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        content = txt;
        yield return StartCoroutine(ShowLine(content));
        _Baw.SetBool("chi", true);

        key = "Tutorial.displaytext1.text5";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        content = txt;
        yield return StartCoroutine(ShowLine(content));
        _StarSelectUnit = true;
        yield return StartCoroutine(ShowLine("",0.1f));
        StartCoroutine(SelectBox());
        _Baw.SetBool("noi", false);
        _Mess.SetTrigger("exit");
    }
    #endregion


    #region SelectBox
    private IEnumerator SelectBox()
    {
        Time.timeScale = 0f;
        OffBlackBox();
        OffClickIcon();
        _select = false;
        _controller = false;
        yield return new WaitForSecondsRealtime(1.5f);
        _camera.Zoom(15, 1.5f);
        _camera.Move(_1.position, 1.5f);
        yield return new WaitForSecondsRealtime(1.6f);
        _selectBox.SetActive(true);
        _Baw.SetBool("chi", false);
    }
    #endregion


    #region text 2
    // hướng dẫn điều khiển lính đến mục tiêu
    private IEnumerator displayText2()
    {
        Time.timeScale = 1f;
        _selectBox.SetActive(false);
        _Baw.SetBool("khen", true);
        yield return new WaitForSecondsRealtime(1.5f);
        _Baw.SetBool("khen", false);

        _Mess.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.3f);
        _Baw.SetBool("noi", true);

        key = "Tutorial.displaytext2.text1";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        string content = txt;
        yield return StartCoroutine(ShowLine(content));
        _camera.Move(_2.position, 1.5f);

        key = "Tutorial.displaytext2.text2";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        content = txt;
        yield return StartCoroutine(ShowLine(content));
        yield return StartCoroutine(ShowLine("",0.1f));
        _Baw.SetBool("noi", false);
        _Baw.gameObject.SetActive(false);
        _Mess.SetTrigger("exit");
        OnBlackBox(_2);
        ID = 1;
        yield return new WaitForSecondsRealtime(0.5f);
        OnClickIcon(_2);
    }
    #endregion


    #region text 3
    // mở ui lên
    private IEnumerator displayText3()
    {
        _selectBox.SetActive(false);
        _StarSelectUnit = false;
        _Baw.gameObject.SetActive(true);
        _Baw.SetBool("khen", true);
        yield return new WaitForSecondsRealtime(1.5f);
        _Baw.SetBool("khen", false);


        _Mess.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.3f);
        _Baw.SetBool("noi", true);

        key = "Tutorial.displaytext3.text1";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        string content = txt;
        yield return StartCoroutine(ShowLine(content));
        _camera.Zoom(20, 2f);

        key = "Tutorial.displaytext3.text2";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        content = txt;
        yield return StartCoroutine(ShowLine(content));
        _Baw.SetBool("noi", false);
        _Baw.gameObject.SetActive(false);
        yield return StartCoroutine(ShowLine("",0.1f));
        _Mess.SetTrigger("exit");
        ID = 2;
        GameManager.Instance.setActiveGameUI(true);
        yield return new WaitForSecondsRealtime(3f);
        StopAllCoroutines();
        StartCoroutine(displayText4());
    }
    #endregion


    #region text 4
    // hướng dẫn chọn vào "Nhật ký của Baw"
    private IEnumerator displayText4()
    {
        _Baw.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        _Mess.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.3f);
        _Baw.SetBool("noi", true);

        key = "Tutorial.displaytext4.text1";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        string content = txt;
        yield return StartCoroutine(ShowLine(content));
        _Baw.SetBool("noi", false);
        _Baw.gameObject.SetActive(false);
        yield return StartCoroutine(ShowLine("",0.1f));
        _Mess.SetTrigger("exit");
        OnBlackBox(_3);
        yield return new WaitForSecondsRealtime(0.5f);
        OnClickIcon(_3);
    }
    #endregion


    #region text 5
    // giới thiệu và cho lời khuyên khi xem "nhật ký của Baw"
    private IEnumerator displayText5()
    {
        _Baw.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        _Mess.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.3f);
        _Baw.SetBool("noi", true);

        key = "Tutorial.displaytext5.text1";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        string content = txt;
        yield return StartCoroutine(ShowLine(content));

        key = "Tutorial.displaytext5.text2";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        content = txt;
        yield return StartCoroutine(ShowLine(content));
        _Baw.SetBool("noi", false);
        _Baw.gameObject.SetActive(false);
        yield return StartCoroutine(ShowLine("",0.1f));
        _Mess.SetTrigger("exit");
    }
    #endregion


    #region text 6
    // giới thiệu về cách tạo Tower
    private IEnumerator displayText6()
    {
        _Baw.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        _Mess.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.3f);
        _Baw.SetBool("noi", true);

        key = "Tutorial.displaytext6.text1";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        string content = txt;
        yield return StartCoroutine(ShowLine(content));
        _camera.Move(_4.position, getSpeedText(content));
        _camera.Zoom(15, getSpeedText(content));

        key = "Tutorial.displaytext6.text2";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        content = txt;
        yield return StartCoroutine(ShowLine(content));

        key = "Tutorial.displaytext6.text3";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        content = txt;
        yield return StartCoroutine(ShowLine(content));
        _camera.Zoom(20, 1.5f);
        yield return new WaitForSecondsRealtime(1.6f);
        OnBlackBox(_5);
        _Baw.SetBool("noi", false);
        _Baw.gameObject.SetActive(false);
        yield return StartCoroutine(ShowLine("",0.1f));
        _Mess.SetTrigger("exit");
        yield return new WaitForSecondsRealtime(0.5f);
        OnClickIcon(_5);
        _Tab_Button.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        ID = 3;
    }
    #endregion


    #region text 7
    // hướng dẫn nhấn nút create Tower
    private IEnumerator displayText7()
    {
        ID = 4;
        yield return new WaitForSecondsRealtime(1f);
        OnClickIcon(_6);
        _camera.Move(_7.position, 1f);
    }
    #endregion


    #region text 8
    // giới thiệu về Tower và cách đặt tower
    private IEnumerator displayText8()
    {
        _Baw.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        _Mess.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.3f);
        _Baw.SetBool("noi", true);

        key = "Tutorial.displaytext8.text1";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        string content = txt;
        yield return StartCoroutine(ShowLine(content));

        key = "Tutorial.displaytext8.text2";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        content = txt;
        yield return StartCoroutine(ShowLine(content));
        _Baw.SetBool("noi", false);
        _Baw.gameObject.SetActive(false);
        yield return StartCoroutine(ShowLine("",0.1f));
        _Mess.SetTrigger("exit");
    }
    #endregion


    #region text 9
    // gioi thieu ve shop tạo lính
    private IEnumerator displayText9()
    {
        _Baw.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        _Mess.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.3f);
        _Baw.SetBool("noi", true);

        key = "Tutorial.displaytext9.text1";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        string content = txt;
        yield return StartCoroutine(ShowLine(content));

        key = "Tutorial.displaytext9.text2";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        content = txt;
        yield return StartCoroutine(ShowLine(content));

        key = "Tutorial.displaytext9.text3";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        content = txt;
        yield return StartCoroutine(ShowLine(content));
        _Baw.SetBool("noi", false);
        _Baw.gameObject.SetActive(false);
        yield return StartCoroutine(ShowLine("",0.1f));
        _Mess.SetTrigger("exit");
        OnBlackBox(_8);
        _C_Button.SetActive(true);
        yield return new WaitForSecondsRealtime(0.3f);
        OnClickIcon(_8);
        ID = 5;
    }
    #endregion


    #region text 10
    // hướng dẫn tạo archer
    private IEnumerator displayText10()
    {
        _C_Button.SetActive(false);
        _Baw.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        _Mess.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.3f);
        _Baw.SetBool("noi", true);

        key = "Tutorial.displaytext10.text1";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        string content = txt;
        yield return StartCoroutine(ShowLine(content));

        key = "Tutorial.displaytext10.text2";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        content = txt;
        yield return StartCoroutine(ShowLine(content));
        _Baw.SetBool("noi", false);
        _Baw.gameObject.SetActive(false);
        yield return StartCoroutine(ShowLine("",0.1f));
        _Mess.SetTrigger("exit");
        OnClickIcon(_9);
    }
    #endregion


    #region text 11
    // Giải thích Về thời gian spawn và Slot
    private IEnumerator displayText11()
    {
        OffClickIcon();
        _Baw.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        _Mess.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.3f);
        _Baw.SetBool("noi", true);

        key = "Tutorial.displaytext11.text1";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        string content = txt;
        yield return StartCoroutine(ShowLine(content));
        OnBlackBox(_10);

        key = "Tutorial.displaytext11.text2";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        content = txt;
        yield return StartCoroutine(ShowLine(content));
        OffBlackBox();

        key = "Tutorial.displaytext11.text3";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        content = txt;
        yield return StartCoroutine(ShowLine(content));
        ID = 2;
        _Baw.SetBool("noi", false);
        _Baw.gameObject.SetActive(false);
        yield return StartCoroutine(ShowLine("",0.1f));
        _Mess.SetTrigger("exit");
        yield return new WaitForSecondsRealtime(0.3f);
        OnClickIcon(_11);
    }
    #endregion


    #region text 12
    // Hướng Dẫn mở nut Upgrade
    private IEnumerator displayText12()
    {
        OffClickIcon();
        _Baw.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        _Mess.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.3f);
        _Baw.SetBool("noi", true);

        key = "Tutorial.displaytext12.text1";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        string content = txt;
        yield return StartCoroutine(ShowLine(content));
        _Baw.SetBool("noi", false);
        _Baw.gameObject.SetActive(false);
        yield return StartCoroutine(ShowLine("",0.1f));
        _Mess.SetTrigger("exit");
        _camera.Zoom(15, getSpeedText(content));
        _camera.Move(Castle.Instance.transform.position, getSpeedText(content));
        yield return StartCoroutine(ShowLine("", 0.1f));
        yield return new WaitForSecondsRealtime(getSpeedText(content)+0.1f);
        ID = 6;
        OnClickIcon(Castle.Instance.transform);
    }
    #endregion


    #region text 13
    // Hướng dẫn mở bản Upgrade bằng button hoặc phím tắt
    private IEnumerator displayText13()
    {
        OffClickIcon();
        ID = 5;
        yield return new WaitForSecondsRealtime(0.5f);
        OnClickIcon(_12);
        _U_Button.SetActive(true);
        ID = 6;
    }
    #endregion

    #region text 14
    // Giải thích về nguyên lý hoạt động của upgrade
    private IEnumerator displayText14()
    {
        _U_Button.SetActive(false);
        OffClickIcon();
        yield return new WaitForSecondsRealtime(1f);
        _Baw.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        _Mess.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.3f);
        _Baw.SetBool("noi", true);

        key = "Tutorial.displaytext14.text1";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        string content = txt;
        yield return StartCoroutine(ShowLine(content));

        key = "Tutorial.displaytext14.text2";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        content = txt;
        yield return StartCoroutine(ShowLine(content));

        key = "Tutorial.displaytext14.text3";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        content = txt;
        yield return StartCoroutine(ShowLine(content));
        yield return StartCoroutine(ShowLine("",0.1f));
        OnClickIcon(_13);
        ID = 7;
        _Baw.SetBool("noi", false);
        _Baw.gameObject.SetActive(false);
        yield return StartCoroutine(ShowLine("",0.1f));
        _Mess.SetTrigger("exit");
    }
    #endregion

    #region text 15
    // Thả cho người chơi tự kiếm tài nguyên
    private IEnumerator displayText15()
    {
        OffClickIcon();
        yield return new WaitForSecondsRealtime(1f);
        _Baw.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        _Mess.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.3f);
        _Baw.SetBool("noi", true);

        key = "Tutorial.displaytext15.text1";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        string content = txt;
        yield return StartCoroutine(ShowLine(content));

        key = "Tutorial.displaytext15.text2";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        content = txt;
        yield return StartCoroutine(ShowLine(content));
        _camera.Zoom(20, 2f);

        key = "Tutorial.displaytext15.text3";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        content = txt;
        yield return StartCoroutine(ShowLine(content));
        _MoveCamera.SetActive(true);

        key = "Tutorial.displaytext15.text4";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        content = txt;
        yield return StartCoroutine(ShowLine(content));
        yield return StartCoroutine(ShowLine("",0.1f));
        GameManager.Instance.setHourRTS(16);
        ID = 2;
        _Baw.SetBool("noi", false);
        _Baw.gameObject.SetActive(false);
        yield return StartCoroutine(ShowLine("",0.1f));
        _Mess.SetTrigger("exit");
        _MoveCamera.SetActive(false);
        GameManager.Instance.Tutorial = false;
    }
    #endregion


    #region text 16
    // Cảnh báo chiến tranh sắp diễn ra
    private IEnumerator displayText16()
    {
        yield return new WaitForSecondsRealtime(5f);
        GameManager.Instance.Tutorial = true;
        _enemyList.SetActive(true);
        Time.timeScale = 0;
        _Baw.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        _Mess.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.3f);
        _Baw.SetBool("noi", true);

        key = "Tutorial.displaytext16.text1";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        string content = txt;
        yield return StartCoroutine(ShowLine(content));

        key = "Tutorial.displaytext16.text2";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        content = txt;
        yield return StartCoroutine(ShowLine(content));
        _Baw.SetBool("noi", false);
        _Baw.gameObject.SetActive(false);
        yield return StartCoroutine(ShowLine("",0.1f));
        _Mess.SetTrigger("exit");
        GameManager.Instance.Tutorial = false;
        Time.timeScale = 1f;
    }
    #endregion


    #region text 17
    // Cảnh báo chiến tranh sắp diễn ra
    private IEnumerator displayText17()
    {
        Time.timeScale = 0;
        _Baw.gameObject.SetActive(true);
        _camera.Move(_enemy.position, 1f);
        yield return new WaitForSecondsRealtime(1f);
        _Mess.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.3f);
        _Baw.SetBool("noi", true);

        key = "Tutorial.displaytext17.text1";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        string content = txt;
        yield return StartCoroutine(ShowLine(content));

        key = "Tutorial.displaytext17.text2";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        content = txt;
        yield return StartCoroutine(ShowLine(content));

        key = "Tutorial.displaytext17.text3";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        content = txt;
        yield return StartCoroutine(ShowLine(content));

        key = "Tutorial.displaytext17.text4";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        content = txt;
        yield return StartCoroutine(ShowLine(content));
        _camera.Move(_archer.position, 1f);
        _camera.Zoom(15, 1f);

        key = "Tutorial.displaytext17.text5";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        content = txt;
        yield return StartCoroutine(ShowLine(content));
        StartCoroutine(displayText18());
        _enemyList.SetActive(true);
        _Baw.SetBool("noi", false);
        _Baw.gameObject.SetActive(false);
        yield return StartCoroutine(ShowLine("",0.1f));
        _Mess.SetTrigger("exit");
        _War = true;
    }
    #endregion


    #region text 18
    // on off công tắt
    private IEnumerator displayText18()
    {
        _camera.Move(_buttonArcherInCastle1.position, 1f);
        yield return new WaitForSecondsRealtime(1.5f);
        OnClickIcon(_buttonArcherInCastle1);
        yield return new WaitForSecondsRealtime(2f);
        OffClickIcon();
        _camera.Move(_buttonArcherInCastle2.position, 1f);
        yield return new WaitForSecondsRealtime(1.5f);
        OnClickIcon(_buttonArcherInCastle2);
        yield return new WaitForSecondsRealtime(1f);
        OffClickIcon();
        yield return new WaitForSecondsRealtime(1f);
        _Baw.gameObject.SetActive(true);
        _camera.Move(_enemy.position, 1f);
        yield return new WaitForSecondsRealtime(1f);
        _Mess.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.3f);
        _Baw.SetBool("noi", true);
        _camera.Move(_enemy.position, 1f);

        key = "Tutorial.displaytext9.text1";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        string content = txt;
        yield return StartCoroutine(ShowLine(content));
        _Baw.SetBool("noi", false);
        _Baw.gameObject.SetActive(false);
        yield return StartCoroutine(ShowLine("",0.1f));
        _Mess.SetTrigger("exit");
        GameManager.Instance.Tutorial = false;
        Time.timeScale = 1f;
    }
    #endregion


    #region text 19
    // Sống sót qua một đêm
    private IEnumerator displayText19()
    {
        GameManager.Instance.Tutorial = true;
        _camera.Move(Castle.Instance.transform.position, 2f);
        _camera.Zoom(20, 2f);
        CameraShake.Instance.ShakeCamera(2f);
        yield return new WaitForSecondsRealtime(2.5f);
        _Baw.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        _Mess.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.3f);
        _Baw.SetBool("noi", true);

        key = "Tutorial.displaytext19.text1";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        string content = txt;
        yield return StartCoroutine(ShowLine(content));

        key = "Tutorial.displaytext19.text2";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        content = txt;
        yield return StartCoroutine(ShowLine(content));

        key = "Tutorial.displaytext19.text3";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        content = txt;
        yield return StartCoroutine(ShowLine(content));

        key = "Tutorial.displaytext19.text4";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        content = txt;
        yield return StartCoroutine(ShowLine(content));

        key = "Tutorial.displaytext19.text5";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        content = txt;
        yield return StartCoroutine(ShowLine(content, 3f));
        _Baw.SetBool("noi", false);
        _Baw.gameObject.SetActive(false);
        yield return StartCoroutine(ShowLine("",0.1f));
        _Mess.SetTrigger("exit");
        StartCoroutine(FadeOut());
    }
    #endregion

    #region text 20
    // Sống sót qua một đêm
    private IEnumerator displayText20()
    {
        GameManager.Instance.Tutorial = true;
        _camera.Move(Castle.Instance.transform.position, 2f);
        _camera.Zoom(20, 2f);
        CameraShake.Instance.ShakeCamera(2f);
        _Baw.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        _Mess.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.3f);
        _Baw.SetBool("noi", true);

        key = "Tutorial.displaytext20.text1";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        string content = txt;
        yield return StartCoroutine(ShowLine(content));

        key = "Tutorial.displaytext20.text2";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        content = txt;
        yield return StartCoroutine(ShowLine(content));

        key = "Tutorial.displaytext20.text3";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        content = txt;
        yield return StartCoroutine(ShowLine(content));

        key = "Tutorial.displaytext20.text4";
        txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        content = txt;
        yield return StartCoroutine(ShowLine(content, 3f));
        _Baw.SetBool("noi", false);
        _Baw.gameObject.SetActive(false);
                yield return StartCoroutine(ShowLine("",0.1f));
        _Mess.SetTrigger("exit");
        StartCoroutine(FadeOut());
    }
    #endregion


    #region Fade Out
    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(2f);
        Color col = _BG1.color;
        float time = 0f;
        while (time < _fadeDuration)
        {
            time += Time.deltaTime;
            col.a = Mathf.Lerp(0f, 1f, time / _fadeDuration);
            _BG1.color = col;
            yield return null;
        }
        col.a = 1f;
        _BG1.color = col;
        SettingManager.Instance._playing = false;
        SettingManager.Instance._gameSettings._Tutorial = false;
        SettingManager.Instance.Save();
        SceneManager.LoadScene("MainMenu");
    }
    #endregion


    #region Public Modifier
    public void GameLose()
    {
        StopAllCoroutines();
        StartCoroutine(displayText20());
    }

    public void PlayerMoveController()
    {
        OffBlackBox();
        OffClickIcon();
        Debug.Log("Xong Điều khiển lính");
        StopAllCoroutines();
        StartCoroutine(displayText3());
    }

    public void Tutorial_Select_Unit()
    {
        _openNhatKy = true;
        OffBlackBox();
        OffClickIcon();
        Debug.Log("Xong hướng dẫn mở Nhật Ký");
        StopAllCoroutines();
        StartCoroutine(displayText5());
    }

    public void TutorialBuilding()
    {
        _TutorialNhatKy = true;
        Debug.Log("Bắt Đầu Hướng dẫn tạo Tower");
        StopAllCoroutines();
        StartCoroutine(displayText6());
    }

    public void TutorialBuyTower()
    {
        
        OffBlackBox();
        OffClickIcon();
        _Tab_Button.SetActive(false);
        Debug.Log("Đã click vào building");
        StopAllCoroutines();
        StartCoroutine(displayText7());
    }

    public void TutorialCreatePlayer()
    {
        Debug.Log("Hướng dẫn mở shop");
        StopAllCoroutines();
        StartCoroutine(displayText9());
        _DropTower = true;
    }

    public void TutorialDropTower()
    {
        OffClickIcon();
        ID = 3;
        GameManager.Instance.UIOpenBuidingPanel();
        ID = 2;
        Debug.Log("Hướng dẫn đặt Tower");
        StopAllCoroutines();
        StartCoroutine(displayText8());
    }

    public void TutorialCreateArcher()
    {
        if (_OpenShop) return;
        OffBlackBox();
        OffClickIcon();
        Debug.Log("Hướng dẫn tạo archer");
        StopAllCoroutines();
        StartCoroutine(displayText10());
        _OpenShop = true;
    }

    public void TutorialCreateTimeAddSlotPlayer()
    {
        Debug.Log("Giải thích về slot");
        _CreateArcher = true;
        StopAllCoroutines();
        StartCoroutine(displayText11());
    }

    public void TutorialOpenButtonUpgrade()
    {
        if (_CloseShop) return;
        Debug.Log("Bắt Đầu hướng dẫn Mở nút upgrade");
        StopAllCoroutines();
        StartCoroutine(displayText12());
        _CloseShop = true;
    }

    public void TutorialOpenPanelUpgrade()
    {
        Debug.Log("Bắt Đầu hướng dẫn Mở Panel upgrade");
        StopAllCoroutines();
        StartCoroutine(displayText13());
    }

    public void TutorialUpgradeAndReferencesAndSlot()
    {
        if (_OpenUpgrade) return;
        Debug.Log("Giải thích về nguyen lý hoạt động của upgrade");
        StopAllCoroutines();
        StartCoroutine(displayText14());
        _OpenUpgrade = true;
    }

    public void TutorialClosePanelUpgrade()
    {
        Debug.Log("Tắt panel upgrade");
        StopAllCoroutines();
        OffClickIcon();
        OnClickIcon(_14);
        ID = 8;
    }

    public void TutorialArcherInCastle()
    {
        if (_CloseUpgrade) return;
        Debug.Log("Thả cho người chơi tự chơi một lúc");
        StopAllCoroutines();
        StartCoroutine(displayText15());
        _CloseUpgrade = true;
    }


    public void TutorialWarningEnemyToHouse()
    {
        Debug.Log("Cảnh Báo Enemy Phát động tấn công");
        StopAllCoroutines();
        StartCoroutine(displayText17());
    }
    #endregion


    private void OnBlackBox(Transform target)
    {
        _blackBox.setTarget(target);
        _blackBox.gameObject.SetActive(true);
    }

    private void OnClickIcon(Transform target)
    {
        _click.setTarget(target);
        _click.gameObject.SetActive(true);
    }

    private void OffBlackBox()
    {
        _blackBox.gameObject.SetActive(false);
    }

    private void OffClickIcon()
        => _click.gameObject.SetActive(false);

    private float getSpeedText(string content)
    {
        int count = 0;
        foreach (var h in content)
            count++;
        return count * _typewriter.getSpeed();
    }


    private IEnumerator ShowLine(string content, float time = 1f)
    {
        _isTyping = true;
        _skipRequested = false;

        // Gõ chữ (có thể bị skip)
        yield return StartCoroutine(_typewriter.TypeText(content, () => _skipRequested));

        _isTyping = false;

        // Luôn chờ đủ thời gian baseWaitTime, kể cả khi skip
        float waitDuration = _skipRequested ? time+0.5f : time;
        float timer = 0f;
        while (timer < waitDuration)
        {
            timer += Time.unscaledDeltaTime;
            yield return null;
        }
    }
}
