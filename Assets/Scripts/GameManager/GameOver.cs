using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] private GameObject _gameOver;

    [Header("BackGround")]
    [SerializeField] private GameObject _Win;
    [SerializeField] private GameObject _Lose;

    [Header("Tile")]
    [SerializeField] private Image _Tile;
    [SerializeField] private Sprite _E_Victory;
    [SerializeField] private Sprite _VN_Victory;
    [SerializeField] private Sprite _E_Lose;
    [SerializeField] private Sprite _VN_Lose;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI _content;
    [SerializeField] private TextMeshProUGUI _Lv_Castle;
    [SerializeField] private TextMeshProUGUI _playTime;
    [SerializeField] private TextMeshProUGUI _totalDay;
    [SerializeField] private TextMeshProUGUI _archer;
    [SerializeField] private TextMeshProUGUI _warrior;
    [SerializeField] private TextMeshProUGUI _lancer;
    [SerializeField] private TextMeshProUGUI _healer;
    [SerializeField] private TextMeshProUGUI _TNT;
    [SerializeField] private TextMeshProUGUI _wood;
    [SerializeField] private TextMeshProUGUI _rock;
    [SerializeField] private TextMeshProUGUI _meat;
    [SerializeField] private TextMeshProUGUI _gold;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Image _blackImg;


    private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 2f;

    void Start()
    {
        canvasGroup = _gameOver.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
    }


    #region Display
    public void display()
    {
        // hiện panel win lose
        bool win = GameManager.Instance.getWin();
        _Win.SetActive(win);
        _Lose.SetActive(!win);


        // tile
        if (win)
        {
            // sử lý ngôn ngữ ở đây
            _Tile.sprite = _VN_Victory;
        }
        else
        {
            // sử lý ngôn ngữ ở đây
            _Tile.sprite = _VN_Lose;
        }


        _Lv_Castle.text = $"LV.{Castle.Instance._level}";
        _playTime.text = $"Thời gian chơi: {GameManager.Instance._playTime.x}:{GameManager.Instance._playTime.y}";
        _totalDay.text = $"Số Ngày: {GameManager.Instance._currentDay}";


        // player
        _warrior.text = GameManager.Instance._warriorValue.ToString();
        _archer.text = GameManager.Instance._archerValue.ToString();
        _lancer.text = GameManager.Instance._lancerValue.ToString();
        _healer.text = GameManager.Instance._healerValue.ToString();
        _TNT.text = GameManager.Instance._tntValue.ToString();


        // references
        _wood.text = GameManager.Instance._wood.ToString();
        _rock.text = GameManager.Instance._rock.ToString();
        _meat.text = GameManager.Instance._meat.ToString();
        _gold.text = GameManager.Instance._gold.ToString();


        _content.text = GameManager.Instance._contentGameOver;


        // display game over panel
        _gameOver.SetActive(true);
        if (_fadeIn == null)
            _fadeIn = StartCoroutine(FadeIn());
    }
    #endregion


    #region Fade In
    private Coroutine _fadeIn;
    private IEnumerator FadeIn()
    {
        float time = 0f;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, time / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1f;
        // pause game
        //Time.timeScale = 0f; // Dừng mọi hoạt động dựa theo Time.deltaTime
    }
    #endregion


    #region Fade Out
    private IEnumerator FadeOut()
    {
        float time = 0f;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, time / fadeDuration);
            Color c = _blackImg.color;
            c.a = Mathf.Lerp(0f, 1f, time / fadeDuration);
            _blackImg.color = c;
            yield return null;
        }
        canvasGroup.alpha = 0f;
        StartCoroutine(BackToHome());
    }
    #endregion


    #region Back Home
    private IEnumerator BackToHome()
    {
        yield return new WaitForSeconds(1f);
        GameScene.Instance.OpenSceneMainMenu();
    }
    #endregion


    #region Button Exit Click
    public void exit()
    {
        // pause game
        //Time.timeScale = 1f; // Dừng mọi hoạt động dựa theo Time.deltaTime

        StartCoroutine(FadeOut());
        _exitButton.interactable = false;
    }
    #endregion
}
