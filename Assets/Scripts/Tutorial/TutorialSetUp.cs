using System.Collections;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
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
    [SerializeField] private TextMeshProUGUI _textMess;

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

    private TypewriterEffect _typewriter;
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
    }
    #endregion


    #region Start
    void Start()
    {
        _currentDay = GameManager.Instance._currentDay;
        GameManager.Instance.Tutorial = true;
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
            GameManager.Instance.Tutorial = true;
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
                StartCoroutine(displayText20());
            }
        }
    }
    #endregion


    #region Start Tutorial
    private IEnumerator start()
    {
        yield return new WaitForSeconds(2f);
        _camera.Move(Castle.Instance.transform.position, _fadeDuration + 2);
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
        string content = "…Tỉnh rồi à, chỉ huy?";
        _Baw.SetBool("noi", true);
        _typewriter.StartTyping(content);
        yield return new WaitForSecondsRealtime(getSpeedText(content) + 1f);
        content = "Cả vùng đất này... đã thành tro tàn. Căn cứ của ta, đồng đội của ta – chỉ còn vài linh hồn chưa gục ngã.";
        _typewriter.StartTyping(content);
        yield return new WaitForSecondsRealtime(getSpeedText(content) + 1f);
        content = "Ta là Baw. Cựu chỉ huy tiền tuyến của đội trinh sát phương Bắc.";
        _typewriter.StartTyping(content);
        yield return new WaitForSecondsRealtime(getSpeedText(content) + 1f);
        content = "Giờ thì... xem ra ta lại thành hướng dẫn viên bất đắc dĩ rồi.";
        _typewriter.StartTyping(content);
        yield return new WaitForSecondsRealtime(getSpeedText(content) + 1f);
        _Baw.SetBool("chi", true);
        content = "Không sao đâu, ta sẽ chỉ ngươi từng bước. Trước hết... thử kéo chuột trái đi – chọn mấy người sống sót quanh nhà chính xem nào.";
        _typewriter.StartTyping(content);
        _StarSelectUnit = true;
        yield return new WaitForSecondsRealtime(getSpeedText(content) + 1f);
        _typewriter.StartTyping("");
        StartCoroutine(SelectBox());
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
        _Baw.SetBool("chi", false);
        _Baw.SetBool("noi", false);
        _typewriter.StartTyping("");
        _Mess.SetTrigger("exit");
        _selectBox.SetActive(true);
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
        string content = "Tốt! Ngươi vẫn còn nhớ cách nắm lấy vận mệnh của mình.";
        _typewriter.StartTyping(content);
        yield return new WaitForSecondsRealtime(getSpeedText(content) + 1f);
        _camera.Move(_2.position, 1.5f);
        content = "Giờ thì thử bảo họ di chuyển. Nhấn chuột phải vào vùng sáng phía trước đi.";
        _typewriter.StartTyping(content);
        yield return new WaitForSecondsRealtime(getSpeedText(content) + 1f);
        _typewriter.StartTyping("");
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
        _StarSelectUnit = false;
        _Baw.gameObject.SetActive(true);
        _Baw.SetBool("khen", true);
        yield return new WaitForSecondsRealtime(1.5f);
        _Baw.SetBool("khen", false);


        _Mess.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.3f);
        _Baw.SetBool("noi", true);
        string content = "Tốt lắm, họ nghe lệnh của ngươi rồi.";
        _typewriter.StartTyping(content);
        yield return new WaitForSecondsRealtime(getSpeedText(content) + 1f);
        _camera.Zoom(20, 2f);
        content = "Ngươi là niềm hy vọng cuối cùng của vùng đất này, chỉ huy.";
        _typewriter.StartTyping(content);
        yield return new WaitForSecondsRealtime(getSpeedText(content) + 1f);
        _Baw.SetBool("noi", false);
        _Baw.gameObject.SetActive(false);
        _typewriter.StartTyping("");
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
        string content = "Ta không còn nhiều thời gian. Nên giờ thì hãy tự mình đọc Nhật ký của ta đi.";
        _typewriter.StartTyping(content);
        yield return new WaitForSecondsRealtime(getSpeedText(content) + 1f);
        _Baw.SetBool("noi", false);
        _Baw.gameObject.SetActive(false);
        _typewriter.StartTyping("");
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
        string content = "Đây là Nhật ký của ta khi còn ở thời huy hoàng... mà thôi, giờ không phải lúc để kể chuyện cũ.";
        _typewriter.StartTyping(content);
        yield return new WaitForSecondsRealtime(getSpeedText(content) + 1f);
        content = "Ngươi hãy xem thật kỹ, vì thời gian của chúng ta không còn nhiều đâu.";
        _typewriter.StartTyping(content);
        yield return new WaitForSecondsRealtime(getSpeedText(content) + 1f);
        _Baw.SetBool("noi", false);
        _Baw.gameObject.SetActive(false);
        _typewriter.StartTyping("");
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
        string content = "Nhìn quanh đi, chỉ huy. Ban đêm là lúc bọn man rợ sẽ mò tới.";
        _typewriter.StartTyping(content);
        yield return new WaitForSecondsRealtime(getSpeedText(content) + 1f);
        _camera.Move(_4.position, getSpeedText(content));
        _camera.Zoom(15, getSpeedText(content));
        content = "Các công trình này… từng là niềm kiêu hãnh của ta, giờ chỉ còn là đống sắt vụn.";
        _typewriter.StartTyping(content);
        yield return new WaitForSecondsRealtime(getSpeedText(content) + 1f);
        content = "Ta cần ngươi giúp ta dựng lại một tháp cung – thứ duy nhất còn khiến bọn quái phải dè chừng.";
        _typewriter.StartTyping(content);
        yield return new WaitForSecondsRealtime(getSpeedText(content) + 1f);
        _camera.Zoom(20, 1.5f);
        yield return new WaitForSecondsRealtime(1.6f);
        OnBlackBox(_5);
        _Baw.SetBool("noi", false);
        _Baw.gameObject.SetActive(false);
        _typewriter.StartTyping("");
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
        string content = "Tháp cung sẽ giúp ngươi chống chịu được các đợt tấn công ban đêm.";
        _typewriter.StartTyping(content);
        yield return new WaitForSecondsRealtime(getSpeedText(content) + 1f);
        content = "Hãy đặt nó ở đây đi – click vào dấu tích màu xanh. Chọn đúng, và nó sẽ trụ vững hơn cả niềm tin của ta.";
        _typewriter.StartTyping(content);
        yield return new WaitForSecondsRealtime(getSpeedText(content) + 1f);
        _Baw.SetBool("noi", false);
        _Baw.gameObject.SetActive(false);
        _typewriter.StartTyping("");
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
        string content = "Công trình này cần thời gian để hoàn thành. Trong lúc chờ, hãy chiêu mộ thêm binh sĩ đi.";
        _typewriter.StartTyping(content);
        yield return new WaitForSecondsRealtime(getSpeedText(content) + 1f);
        content = "Chúng ta đang cần một cung thủ cho tuyến phòng thủ đêm nay.";
        _typewriter.StartTyping(content);
        yield return new WaitForSecondsRealtime(getSpeedText(content) + 1f);
        content = "Họ cần thời gian để đến nơi, nên chiêu mộ ngay từ bây giờ là khôn ngoan đấy.";
        _typewriter.StartTyping(content);
        yield return new WaitForSecondsRealtime(getSpeedText(content) + 1f);
        _Baw.SetBool("noi", false);
        _Baw.gameObject.SetActive(false);
        _typewriter.StartTyping("");
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
        string content = "Nhớ nhé, chiêu mộ lính sẽ tiêu hao tài nguyên. Càng mạnh, càng tốn.";
        _typewriter.StartTyping(content);
        yield return new WaitForSecondsRealtime(getSpeedText(content) + 1f);
        content = "Thế nên hãy học cách khai thác tài nguyên, kẻo chưa thấy địch đã đói lả.";
        _typewriter.StartTyping(content);
        yield return new WaitForSecondsRealtime(getSpeedText(content) + 1f);
        _Baw.SetBool("noi", false);
        _Baw.gameObject.SetActive(false);
        _typewriter.StartTyping("");
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
        string content = "Tốt! Họ sẽ đến nhanh thôi.";
        _typewriter.StartTyping(content);
        yield return new WaitForSecondsRealtime(getSpeedText(content) + 1f);
        OnBlackBox(_10);
        content = "Nhưng... nhìn kìa, chỗ ở cho lính mới sắp đầy rồi.";
        _typewriter.StartTyping(content);
        yield return new WaitForSecondsRealtime(getSpeedText(content) + 1f);
        OffBlackBox();
        content = "Đi nào, ta sẽ chỉ ngươi cách nâng cấp thành chính để mở rộng căn cứ.";
        _typewriter.StartTyping(content);
        yield return new WaitForSecondsRealtime(getSpeedText(content) + 1f);
        ID = 2;
        _Baw.SetBool("noi", false);
        _Baw.gameObject.SetActive(false);
        _typewriter.StartTyping("");
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
        string content = "Click vào Nhà chính đi, chỉ huy.";
        _typewriter.StartTyping(content);
        _camera.Zoom(15, getSpeedText(content));
        _camera.Move(Castle.Instance.transform.position, getSpeedText(content));
        yield return new WaitForSecondsRealtime(getSpeedText(content) + 1f);
        _typewriter.StartTyping("");
        ID = 6;
        OnClickIcon(Castle.Instance.transform);
        _Baw.SetBool("noi", false);
        _Baw.gameObject.SetActive(false);
        _typewriter.StartTyping("");
        _Mess.SetTrigger("exit");
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
        string content = "Nâng cấp thành chính tốn tài nguyên lớn, nhưng đổi lại toàn bộ công trình và binh sĩ sẽ mạnh hơn.";
        _typewriter.StartTyping(content);
        yield return new WaitForSecondsRealtime(getSpeedText(content) + 1f);
        content = "Tài nguyên tiêu hao sau này cũng nhiều hơn, nhưng ta tin ngươi sẽ xoay xở được.";
        _typewriter.StartTyping(content);
        yield return new WaitForSecondsRealtime(getSpeedText(content) + 1f);
        content = "Và quan trọng nhất – không gian cho quân lính cũng tăng lên. Một món hời, phải không?";
        _typewriter.StartTyping(content);
        yield return new WaitForSecondsRealtime(getSpeedText(content) + 1f);
        _typewriter.StartTyping("");
        OnClickIcon(_13);
        ID = 7;
        _Baw.SetBool("noi", false);
        _Baw.gameObject.SetActive(false);
        _typewriter.StartTyping("");
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
        string content = "Tốt lắm, thành chính nâng cấp rồi. Ánh sáng đó... là dấu hiệu của hy vọng.";
        _typewriter.StartTyping(content);
        yield return new WaitForSecondsRealtime(getSpeedText(content) + 1f);
        content = "Ban ngày, bọn quái sẽ không dám bén mảng lại gần đâu.";
        _typewriter.StartTyping(content);
        yield return new WaitForSecondsRealtime(getSpeedText(content) + 1f);
        _camera.Zoom(20, 2f);
        content = "Mau tranh thủ thu thập tài nguyên và chiêu mộ thêm binh sĩ. Đêm nay... sẽ dài đấy.";
        _typewriter.StartTyping(content);
        yield return new WaitForSecondsRealtime(getSpeedText(content) + 1f);
        _typewriter.StartTyping("");
        GameManager.Instance.setHourRTS(16);
        ID = 2;
        _Baw.SetBool("noi", false);
        _Baw.gameObject.SetActive(false);
        _typewriter.StartTyping("");
        _Mess.SetTrigger("exit");
        GameManager.Instance.Tutorial = false;
    }
    #endregion


    #region text 16
    // Cảnh báo chiến tranh sắp diễn ra
    private IEnumerator displayText16()
    {
        yield return new WaitForSecondsRealtime(5f);
        _enemyList.SetActive(true);
        Time.timeScale = 0;
        _Baw.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        _Mess.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.3f);
        _Baw.SetBool("noi", true);
        string content = "Chỉ huy, nghe cho kỹ này. Mỗi đêm, vào giờ này, bọn quái vật sẽ kéo tới.";
        _typewriter.StartTyping(content);
        yield return new WaitForSecondsRealtime(getSpeedText(content) + 1f);
        content = "Nhưng đừng lo. Ta đã sống sót hàng chục lần, và giờ ta sẽ chỉ ngươi cách chỉ huy một đội quân thực thụ.";
        _typewriter.StartTyping(content);
        yield return new WaitForSecondsRealtime(getSpeedText(content) + 1f);
        _Baw.SetBool("noi", false);
        _Baw.gameObject.SetActive(false);
        _typewriter.StartTyping("");
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
        string content = "Bọn Chúng đã đến rồi.";
        _typewriter.StartTyping(content);
        yield return new WaitForSecondsRealtime(getSpeedText(content) + 1f);
        content = "Bọn chúng đến rồi! Thấy chưa – lần này còn dắt theo cả con to đầu đấy!";
        _typewriter.StartTyping(content);
        yield return new WaitForSecondsRealtime(getSpeedText(content) + 1f);
        content = "Nhanh lên, tập hợp binh lính! Giữ V và nhấn chuột trái để mở Radial Menu – chọn mệnh lệnh ngay đi!";
        _typewriter.StartTyping(content);
        yield return new WaitForSecondsRealtime(getSpeedText(content) + 1f);
        _enemyList.SetActive(true);
        _Baw.SetBool("noi", false);
        _Baw.gameObject.SetActive(false);
        _typewriter.StartTyping("");
        _Mess.SetTrigger("exit");
        GameManager.Instance.Tutorial = false;
        _War = true;
        Time.timeScale = 1f;
    }
    #endregion


    #region text 18
    // Hướng dẫn đưa archer vào thành
    private IEnumerator displayText18()
    {
        yield return new WaitForSecondsRealtime(2f);
        Time.timeScale = 0;
        GameManager.Instance.Tutorial = true;
        _Baw.gameObject.SetActive(true);
        _camera.Move(_enemy.position, 1f);
        yield return new WaitForSecondsRealtime(1f);
        _Mess.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.3f);
        _Baw.SetBool("noi", true);
        string content = "Khốn kiếp, một công trình đã bị phá hủy!";
        _typewriter.StartTyping(content);
        yield return new WaitForSecondsRealtime(getSpeedText(content) + 1f);
        content = "Lũ này có vẻ chuẩn bị kỹ hơn ta tưởng đấy.";
        _typewriter.StartTyping(content);
        yield return new WaitForSecondsRealtime(getSpeedText(content) + 1f);
        content = "Điều động cung thủ vào trong thành ngay! Chúng là tuyến thủ cuối cùng của ta.";
        _typewriter.StartTyping(content);
        yield return new WaitForSecondsRealtime(getSpeedText(content) + 1f);
        _camera.Move(_archer.position, 1f);
        _camera.Zoom(15, 1f);
        content = "Chọn một cung thủ, rồi click vào công tắc trên tháp để ra lệnh cho hắn vào vị trí.";
        _typewriter.StartTyping(content);
        yield return new WaitForSecondsRealtime(getSpeedText(content) + 1f);
        _enemyList.SetActive(true);
        _Baw.SetBool("noi", false);
        _Baw.gameObject.SetActive(false);
        _typewriter.StartTyping("");
        _Mess.SetTrigger("exit");
        _camera.Zoom(20, 1f);
        Time.timeScale = 1f;
        GameManager.Instance.Tutorial = false;
    }
    #endregion

    #region text 19
    // on off công tắt
    private IEnumerator displayText19()
    {
        _camera.Move(_buttonArcherInCastle1.position,1f);
        yield return new WaitForSecondsRealtime(1.5f);
        OnClickIcon(_buttonArcherInCastle1);
        yield return new WaitForSecondsRealtime(2f);
        OffClickIcon();
        _camera.Move(_buttonArcherInCastle2.position,1f);
        yield return new WaitForSecondsRealtime(1.5f);
        OnClickIcon(_buttonArcherInCastle2);
        yield return new WaitForSecondsRealtime(1f);
        OffClickIcon();
        GameManager.Instance.Tutorial = false;
    }
    #endregion


    #region text 20
    // Sống sót qua một đêm
    private IEnumerator displayText20()
    {
        CameraShake.Instance.ShakeCamera(2f);
        yield return new WaitForSecondsRealtime(2.5f);
        _Baw.gameObject.SetActive(true);
        _camera.Move(_enemy.position, 1f);
        yield return new WaitForSecondsRealtime(1f);
        _Mess.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.3f);
        _Baw.SetBool("noi", true);
        string content = "Tốt lắm, chỉ huy. Chúng ta vẫn trụ được... lần này.";
        _typewriter.StartTyping(content);
        yield return new WaitForSecondsRealtime(getSpeedText(content) + 1f);
        content = "Ngươi đã bảo vệ được căn cứ đầu tiên. Không tệ cho tân chỉ huy vừa hồi sinh giữa tro tàn.";
        _typewriter.StartTyping(content);
        yield return new WaitForSecondsRealtime(getSpeedText(content) + 1f);
        content = "Trận chiến vừa rồi... có thể ngươi còn vụng về, nhưng lửa chiến đã bùng lên trong mắt ngươi.";
        _typewriter.StartTyping(content);
        yield return new WaitForSecondsRealtime(getSpeedText(content) + 1f);
        content = "Còn nhiều thứ phía trước – nhưng ít nhất, giờ ta không còn phải chiến đấu một mình nữa.";
        _typewriter.StartTyping(content);
        yield return new WaitForSecondsRealtime(getSpeedText(content) + 1f);
        content = "Chào mừng ngươi đến với chiến trường thực thụ, chỉ huy.";
        _typewriter.StartTyping(content);
        yield return new WaitForSecondsRealtime(getSpeedText(content) + 4f);
        _Baw.SetBool("noi", false);
        _Baw.gameObject.SetActive(false);
        _typewriter.StartTyping("");
        _Mess.SetTrigger("exit");
        StartCoroutine(FadeOut());
    }
    #endregion

    #region text 20
    // Sống sót qua một đêm
    private IEnumerator displayText21()
    {
        CameraShake.Instance.ShakeCamera(2f);
        _Baw.gameObject.SetActive(true);
        _camera.Move(_enemy.position, 1f);
        yield return new WaitForSecondsRealtime(1f);
        _Mess.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.3f);
        _Baw.SetBool("noi", true);
        string content = "Ừ... đừng cúi đầu, chỉ huy. Thua trận không có nghĩa là hết đường.";
        _typewriter.StartTyping(content);
        yield return new WaitForSecondsRealtime(getSpeedText(content) + 1f);
        content = "Ta từng mất cả đơn vị, cả căn cứ... nhưng ta vẫn đứng dậy, vì không ai khác sẽ làm điều đó thay ta.";
        _typewriter.StartTyping(content);
        yield return new WaitForSecondsRealtime(getSpeedText(content) + 1f);
        content = "Rút kinh nghiệm, tính toán lại, rồi thử lại lần nữa. Kẻ sống sót không phải kẻ mạnh nhất – mà là kẻ không bỏ cuộc.";
        _typewriter.StartTyping(content);
        yield return new WaitForSecondsRealtime(getSpeedText(content) + 1f);
        content = "Giờ thì... lau bụi trên giáp, thẳng lưng lên, và cho ta thấy lửa trong mắt ngươi vẫn chưa tắt.";
        _typewriter.StartTyping(content);
        yield return new WaitForSecondsRealtime(getSpeedText(content) + 4f);
        _Baw.SetBool("noi", false);
        _Baw.gameObject.SetActive(false);
        _typewriter.StartTyping("");
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
        Debug.Log("Thoat");
    }
    #endregion


    #region Public Modifier
    public void GameLose()
    {
        StopAllCoroutines();
        StartCoroutine(displayText21());
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


    public void TutorialArcherInTowerAndCatlse()
    {
        Debug.Log("Hướng dẫn đưa Archer Vào Thành");
        StopAllCoroutines();
        StartCoroutine(displayText18());
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
}
