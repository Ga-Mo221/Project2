using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadCreate : MonoBehaviour
{
    public UnitType _unitClass;

    [Foldout("References")]
    [SerializeField] private GameObject _w_Avata;
    [Foldout("References")]
    [SerializeField] private GameObject _w_icon;

    [Foldout("References")]
    [SerializeField] private GameObject _A_Avata;
    [Foldout("References")]
    [SerializeField] private GameObject _A_icon;

    [Foldout("References")]
    [SerializeField] private GameObject _L_Avata;
    [Foldout("References")]
    [SerializeField] private GameObject _L_icon;

    [Foldout("References")]
    [SerializeField] private GameObject _T_Avata;
    [Foldout("References")]
    [SerializeField] private GameObject _T_icon;

    [Foldout("References")]
    [SerializeField] private GameObject _H_Avata;
    [Foldout("References")]
    [SerializeField] private GameObject _H_icon;

    [Foldout("References")]
    [SerializeField] private Image _loadImg;
    [Foldout("References")]
    [SerializeField] private TextMeshProUGUI _valueText;


    [SerializeField] private int _value = 0;
    [SerializeField] private float _load = 1;
    [SerializeField] private int _playerCreateTime = 0;
    [SerializeField] private float elapsed = 0f;
    public bool _create = true;

    [SerializeField] private List<GameObject> _listCreate = new List<GameObject>();

    void Update()
    {
        if (_listCreate.Count > 0 && _create)
        {
            if (elapsed < _playerCreateTime)
            {
                elapsed += Time.deltaTime;
                // Giảm dần từ 1 -> 0 theo tỉ lệ
                _load = Mathf.Lerp(1f, 0f, elapsed / _playerCreateTime);
            }
            _loadImg.fillAmount = _load;
            if (_load <= 0)
            {
                _listCreate[0].SetActive(true);
                _listCreate[0].GetComponent<PlayerAI>().setCreating(false);
                GameManager.Instance.UIloadPlayer();
                _load = 1;
                _listCreate.RemoveAt(0);
                _value--;
                _valueText.text = _value.ToString();
                elapsed = 0;
            }
        }
        if (_listCreate.Count == 0 && _create)
            gameObject.SetActive(false);
    }

    public void add(GameObject obj)
    {
        var scrip = obj.GetComponent<PlayerAI>();
        _unitClass = scrip._unitClass;
        _value++;
        _valueText.text = _value.ToString();
        _listCreate.Add(obj);
        _playerCreateTime = scrip._createTime_sec;

        switch (_unitClass)
        {
            case UnitType.Warrior:
                _w_Avata.SetActive(true);
                _w_icon.SetActive(true);
                _A_Avata.SetActive(false);
                _A_icon.SetActive(false);
                _L_Avata.SetActive(false);
                _L_icon.SetActive(false);
                _T_Avata.SetActive(false);
                _T_icon.SetActive(false);
                _H_Avata.SetActive(false);
                _H_icon.SetActive(false);
                break;
            case UnitType.Archer:
                _w_Avata.SetActive(false);
                _w_icon.SetActive(false);
                _A_Avata.SetActive(true);
                _A_icon.SetActive(true);
                _L_Avata.SetActive(false);
                _L_icon.SetActive(false);
                _T_Avata.SetActive(false);
                _T_icon.SetActive(false);
                _H_Avata.SetActive(false);
                _H_icon.SetActive(false);
                break;
            case UnitType.Lancer:
                _w_Avata.SetActive(false);
                _w_icon.SetActive(false);
                _A_Avata.SetActive(false);
                _A_icon.SetActive(false);
                _L_Avata.SetActive(true);
                _L_icon.SetActive(true);
                _T_Avata.SetActive(false);
                _T_icon.SetActive(false);
                _H_Avata.SetActive(false);
                _H_icon.SetActive(false);
                break;
            case UnitType.TNT:
                _w_Avata.SetActive(false);
                _w_icon.SetActive(false);
                _A_Avata.SetActive(false);
                _A_icon.SetActive(false);
                _L_Avata.SetActive(false);
                _L_icon.SetActive(false);
                _T_Avata.SetActive(true);
                _T_icon.SetActive(true);
                _H_Avata.SetActive(false);
                _H_icon.SetActive(false);
                break;
            case UnitType.Healer:
                _w_Avata.SetActive(false);
                _w_icon.SetActive(false);
                _A_Avata.SetActive(false);
                _A_icon.SetActive(false);
                _L_Avata.SetActive(false);
                _L_icon.SetActive(false);
                _T_Avata.SetActive(false);
                _T_icon.SetActive(false);
                _H_Avata.SetActive(true);
                _H_icon.SetActive(true);
                break;
        }
    }

    public void resetValue() => _value = 0;

    public void addValue(GameObject obj)
    {
        _create = false;
        var scrip = obj.GetComponent<PlayerAI>();
        _unitClass = scrip._unitClass;
        _value++;
        _valueText.text = _value.ToString();

        switch (_unitClass)
        {
            case UnitType.Warrior:
                _w_Avata.SetActive(true);
                _w_icon.SetActive(true);
                _A_Avata.SetActive(false);
                _A_icon.SetActive(false);
                _L_Avata.SetActive(false);
                _L_icon.SetActive(false);
                _T_Avata.SetActive(false);
                _T_icon.SetActive(false);
                _H_Avata.SetActive(false);
                _H_icon.SetActive(false);
                break;
            case UnitType.Archer:
                _w_Avata.SetActive(false);
                _w_icon.SetActive(false);
                _A_Avata.SetActive(true);
                _A_icon.SetActive(true);
                _L_Avata.SetActive(false);
                _L_icon.SetActive(false);
                _T_Avata.SetActive(false);
                _T_icon.SetActive(false);
                _H_Avata.SetActive(false);
                _H_icon.SetActive(false);
                break;
            case UnitType.Lancer:
                _w_Avata.SetActive(false);
                _w_icon.SetActive(false);
                _A_Avata.SetActive(false);
                _A_icon.SetActive(false);
                _L_Avata.SetActive(true);
                _L_icon.SetActive(true);
                _T_Avata.SetActive(false);
                _T_icon.SetActive(false);
                _H_Avata.SetActive(false);
                _H_icon.SetActive(false);
                break;
            case UnitType.TNT:
                _w_Avata.SetActive(false);
                _w_icon.SetActive(false);
                _A_Avata.SetActive(false);
                _A_icon.SetActive(false);
                _L_Avata.SetActive(false);
                _L_icon.SetActive(false);
                _T_Avata.SetActive(true);
                _T_icon.SetActive(true);
                _H_Avata.SetActive(false);
                _H_icon.SetActive(false);
                break;
            case UnitType.Healer:
                _w_Avata.SetActive(false);
                _w_icon.SetActive(false);
                _A_Avata.SetActive(false);
                _A_icon.SetActive(false);
                _L_Avata.SetActive(false);
                _L_icon.SetActive(false);
                _T_Avata.SetActive(false);
                _T_icon.SetActive(false);
                _H_Avata.SetActive(true);
                _H_icon.SetActive(true);
                break;
        }
    }
}
