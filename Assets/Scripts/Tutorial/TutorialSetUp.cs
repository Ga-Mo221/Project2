using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSetUp : MonoBehaviour
{
    public static TutorialSetUp Instance { get; private set; }
    [SerializeField] private Image _BG1;
    [SerializeField] private float _fadeDuration = 1f;

    [SerializeField] private Animator _Baw;
    [SerializeField] private Animator _Mess;
    [SerializeField] private TextMeshProUGUI _textMess;

    public GameObject _selectBox;
    public bool _isSelect = false;

    [SerializeField] private Animator _blackBox;
    [SerializeField] private GameObject _click;

    [SerializeField] private CameraTutorial _camera;

    private TypewriterEffect _typewriter;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        GameManager.Instance.setActiveGameUI(false);
        _BG1.gameObject.SetActive(true);
        _Baw.gameObject.SetActive(false);
        _Mess.gameObject.SetActive(false);
        _selectBox.SetActive(false);
        _blackBox.gameObject.SetActive(false);
        _click.SetActive(false);
        _typewriter = _Mess.GetComponent<TypewriterEffect>();

        StartCoroutine(start());
    }


    void Update()
    {
        if (_isSelect)
        {
            _isSelect = false;
            StartCoroutine(displayText2());
        }
    }

    private IEnumerator start()
    {
        yield return new WaitForSeconds(2f);
        _camera.Move(Castle.Instance.transform.position, _fadeDuration + 1);
        Color col = _BG1.color;
        float time = 0f;
        while (time < _fadeDuration)
        {
            time += Time.deltaTime;
            col.a = Mathf.Lerp(1f, 0f, time / _fadeDuration);
            _BG1.color = col;
            yield return null;
        }
        col.a = 1f;
        yield return new WaitForSeconds(2f);
        Time.timeScale = 0f;
        _Baw.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.5f);
        _Mess.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.3f);
        StartCoroutine(displayText1());
    }

    private IEnumerator displayText1()
    {
        string content = "...Tỉnh rồi à, chỉ huy?";
        _Baw.SetBool("noi", true);
        // _typewriter.StartTyping(content);
        // yield return new WaitForSecondsRealtime(getSpeedText(content) + 1f);
        // content = "Cả vùng đất này... đã thành tro tàn. Căn cứ của ta, đồng đội của ta – chỉ còn lại vài linh hồn chưa gục ngã.";
        // _typewriter.StartTyping(content);
        // yield return new WaitForSecondsRealtime(getSpeedText(content) + 1f);
        // content = "Ta là Baw. Cựu chỉ huy tiền tuyến của đội trinh sát phương Bắc.";
        // _typewriter.StartTyping(content);
        // yield return new WaitForSecondsRealtime(getSpeedText(content) + 1f);
        // content = "Giờ thì... xem ra ta lại thành hướng dẫn viên bất đắc dĩ rồi.";
        // _typewriter.StartTyping(content);
        // yield return new WaitForSecondsRealtime(getSpeedText(content) + 1f);
        content = "Không sao đâu, ta sẽ chỉ ngươi từng bước. Trước hết... thử kéo chuột trái đi – chọn mấy người sống sót quanh nhà chính xem nào.";
        _typewriter.StartTyping(content);
        _Baw.SetBool("chi", true);
        yield return new WaitForSecondsRealtime(getSpeedText(content) / 3);
        _camera.Zoom(15, 1.5f);
        Vector2 pos = (Vector2)Castle.Instance.transform.position + new Vector2(-10, -10);
        _camera.Move(pos, 1.5f);
        SelectionBox.Instance.setTutorial(true);
        yield return new WaitForSecondsRealtime(1.5f);
        _Baw.SetBool("chi", false);
        _Baw.SetBool("noi", false);
        _Mess.SetTrigger("exit");
        _selectBox.SetActive(true);
    }

    private IEnumerator displayText2()
    {
        _Baw.SetBool("khen", true);
        yield return new WaitForSecondsRealtime(1.5f);
        _Baw.SetBool("khen", false);

        _typewriter.StartTyping("");
        _Mess.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.3f);
        _Baw.SetBool("noi", true);
        string content = "Tốt! Ngươi vẫn còn nhớ cách nắm lấy vận mệnh của mình.";
        // _typewriter.StartTyping(content);
        // yield return new WaitForSecondsRealtime(getSpeedText(content) + 1f);
        Vector2 pos = (Vector2)Castle.Instance.transform.position + new Vector2(-15, -15);
        _camera.Move(pos, 1.5f);
        content = "Giờ thì thử bảo họ di chuyển. Nhấn chuột phải vào vùng sáng phía trước.";
        _typewriter.StartTyping(content);
        yield return new WaitForSecondsRealtime(getSpeedText(content) + 1f);
        _Baw.SetBool("noi", false);
        _Baw.gameObject.SetActive(false);
        _Mess.SetTrigger("exit");
        _blackBox.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        _click.SetActive(true);
    }


    private IEnumerator displayText3()
    {
        _Baw.gameObject.SetActive(true);
        _Baw.SetBool("khen", true);
        yield return new WaitForSecondsRealtime(1.5f);
        _Baw.SetBool("khen", false);


        _Mess.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.3f);
        _Baw.SetBool("noi", true);
        string content = "Tốt lắm, họ nghe lệnh của ngươi rồi.";
        // _typewriter.StartTyping(content);
        // yield return new WaitForSecondsRealtime(getSpeedText(content) + 1f);
        _camera.Zoom(20, 2f);
        content = "Ngươi là niềm hy vọng cuối cùng của vùng đất này, chỉ huy. Giờ thì... hãy học cách điều khiển từng binh chủng. Chúng ta còn nhiều việc phải làm.";
        _typewriter.StartTyping(content);
        yield return new WaitForSecondsRealtime(getSpeedText(content) + 1f);
        _Baw.SetBool("noi", false);
        SelectionBox.Instance.setTutorial(false);
        GameManager.Instance.setActiveGameUI(true);
    }

    public void PlayerMoveController()
    {
        _blackBox.gameObject.SetActive(false);
        _click.SetActive(false);
        StartCoroutine(displayText3());
    }

    private float getSpeedText(string content)
    {
        int count = 0;
        foreach (var h in content)
            count++;
        return count * _typewriter.getSpeed();
    }
}
