using System.Collections;
using NaughtyAttributes;
using UnityEngine;

public class Deco : MonoBehaviour
{
    [SerializeField] private bool _Deco = true;
    private SpriteRenderer _spriteRenderer;
    [ShowIf(nameof(_Deco))]
    [SerializeField] private Transform _point;

    [HideIf(nameof(_Deco))]
    [SerializeField] private Item _item;

    [HideIf(nameof(_Deco))]
    [SerializeField] private bool _canDetec = true;

    private void Awake()
    {
        if (_Deco)
            _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        if (_spriteRenderer != null)
            _spriteRenderer.sortingOrder = -(int)(_point.position.y * 100) + 10000;

        if (!_Deco && !_canDetec)
        {
            StartCoroutine(offCanDetec());
        }
    }

    private IEnumerator offCanDetec()
    {
        yield return new WaitForSeconds(1f);
        _item._stack = 0;
    }
}
