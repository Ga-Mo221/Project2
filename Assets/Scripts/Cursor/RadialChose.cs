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
    private Collider2D col;

    void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    void Update()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (col != null && col.OverlapPoint(mouseWorld))
        {
            if (_type == ChosenType.War)
            {
                _menu.onAnimation("war", true);
            }
            else if (_type == ChosenType.Support)
            {
                _menu.onAnimation("support", true);
            }
            else if (_type == ChosenType.Rally)
            {
                _menu.onAnimation("rally", true);
            }
        }
        else
        {
            if (_type == ChosenType.War)
            {
                _menu.onAnimation("war", false);
            }
            else if (_type == ChosenType.Support)
            {
                _menu.onAnimation("support", false);
            }
            else if (_type == ChosenType.Rally)
            {
                _menu.onAnimation("rally", false);
            }
        }
    }
}
