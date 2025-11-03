using UnityEngine;

public enum ChosenType
{
    War,
    Support,
    Rally
}

public class RadialChose : MonoBehaviour
{
    [SerializeField] private ChosenType _type;
    [SerializeField] private RadialMenu _menu;
    [SerializeField] private ChoseUI _choseUI;

    void Update()
    {
        // ✅ Kiểm tra chuột có nằm trong RectTransform không
        if (CursorManager.Instance == null) return;
        bool isInside = CursorManager.Instance.ChoseUI && CursorManager.Instance.ID == _choseUI.getID();

        // ✅ Kích hoạt animation tương ứng
        string animName = _type.ToString();
        _menu.OnAnimation(animName, isInside);
    }
}