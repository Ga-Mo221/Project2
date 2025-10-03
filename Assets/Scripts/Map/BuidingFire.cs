using System.Collections.Generic;
using UnityEngine;

public class BuidingFire : MonoBehaviour
{
    [SerializeField] private List<SpriteRenderer> _listFire = new List<SpriteRenderer>();
    [SerializeField] private int _yOder = 0;
    [SerializeField] private bool _IsSet = false;

    void Start()
    {
        // lấy tất cả SpriteRenderer trong object con
        SpriteRenderer[] renders = GetComponentsInChildren<SpriteRenderer>();

        _listFire.AddRange(renders);
    }

    void Update()
    {
        if (!_IsSet && _listFire != null)
        {
            _IsSet = true;
            foreach (var sprite in _listFire)
            {
                sprite.sortingOrder = _yOder + 2;
            }
        }
    }

    public void oder(int oder)
    {
        _yOder = oder;
    }

    public void displayFireLight(bool amount)
    {
        if (_listFire == null) return;
        foreach (var fire in _listFire)
        {
            GameObject Light = fire.transform.Find("Light").gameObject;
            if (Light == null) continue;
            Light.SetActive(amount);
        }
    }
}
