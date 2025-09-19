using System.Collections;
using UnityEngine;

public class HideUI : MonoBehaviour
{
    [SerializeField] private Transform _startPos;
    [SerializeField] private Transform _endPos;
    [SerializeField] private float _timeMove = 0.3f;

    private bool isOut = false;
    private Coroutine _moveCoroutine;

    void Start()
    {
        transform.position = _startPos.position;
    }

    public void move()
    {
        if (_moveCoroutine != null) StopCoroutine(_moveCoroutine);

        if (!isOut)
            _moveCoroutine = StartCoroutine(MoveTo(_endPos.position));
        else
            _moveCoroutine = StartCoroutine(MoveTo(_startPos.position));

        isOut = !isOut; // đảo trạng thái
    }

    private IEnumerator MoveTo(Vector3 target)
    {
        Vector3 start = transform.position;
        float elapsed = 0f;

        while (elapsed < _timeMove)
        {
            transform.position = Vector3.Lerp(start, target, elapsed / _timeMove);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = target; // đảm bảo tới chính xác vị trí
    }
}
