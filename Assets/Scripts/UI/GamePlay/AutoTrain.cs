using UnityEngine;

public class AutoTrain : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    [SerializeField] private GameObject _text;

    [SerializeField] private bool _hover = false;
    [SerializeField] private bool ON = false;

    void Update()
    {
        if (GameManager.Instance.Tutorial) return;
        if (CursorManager.Instance.ID == 2 && CursorManager.Instance.ChoseUI && !_hover)
        {
            _text.SetActive(true);
            _hover = true;
        }
        else if (CursorManager.Instance.ID != 2 && !CursorManager.Instance.ChoseUI && _hover)
        {
            _text.SetActive(false);
            _hover = false;
        }

        if (Input.GetMouseButtonUp(0) && _text.activeSelf)
        {
            ON = !ON;
            GameManager.Instance.setAutoTrain(ON);
            _anim.SetBool("On", ON);
        }

        if (GameManager.Instance.getAutoTrain() != ON)
        {
            ON = GameManager.Instance.getAutoTrain();
            _anim.SetBool("On", ON);
        }
    }
}
