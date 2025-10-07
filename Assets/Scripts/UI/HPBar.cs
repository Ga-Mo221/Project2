using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    [SerializeField] private bool _player = false;

    [SerializeField] private Image _Hp_img;
    [SerializeField] private Sprite _xanh;
    [SerializeField] private Sprite _do;

    void Start()
    {
        if (_player) _Hp_img.sprite = _xanh;
        else _Hp_img.sprite = _do;
    }
}
