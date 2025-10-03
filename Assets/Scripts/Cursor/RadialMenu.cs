using System.Collections.Generic;
using UnityEngine;

public class RadialMenu : MonoBehaviour
{
    private Animator _anim;
    private Vector3 _pos;
    bool _war = false;
    bool _support = false;
    bool _rally = false;
    GameObject _co;

    void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    public void onAnimation(string name, bool active)
    {
        _anim.SetBool(name, active);
    }

    public void setPos(Vector3 pos) => _pos = pos;

    void Update()
    {
        if (!Castle.Instance._V) gameObject.SetActive(false);
        _war = _anim.GetBool("war");
        _support = _anim.GetBool("support");
        _rally = _anim.GetBool("rally");

        if (Input.GetMouseButtonUp(0))
        {
            controller();
            gameObject.SetActive(false);
        }
    }

    private void controller()
    {
        if (Castle.Instance._Q && Castle.Instance._V)
        {
            playerController(Castle.Instance._ListWarrior, false);
        }
        else if (Castle.Instance._W && Castle.Instance._V)
        {
            playerController(Castle.Instance._ListArcher, false);
        }
        else if (Castle.Instance._E && Castle.Instance._V)
        {
            playerController(Castle.Instance._ListLancer, false);
        }
        else if (Castle.Instance._A && Castle.Instance._V)
        {
            playerController(Castle.Instance._ListHealer, false);
        }
        else if (Castle.Instance._S && Castle.Instance._V)
        {
            playerController(Castle.Instance._ListTNT, false);
        }
        else
        {
            playerController(Castle.Instance._ListWarrior, false);
            playerController(Castle.Instance._ListArcher, true);
            playerController(Castle.Instance._ListLancer, true);
            playerController(Castle.Instance._ListHealer, true);
            playerController(Castle.Instance._ListTNT, true);
        }
    }

    private void playerController(List<PlayerAI> _list, bool add)
    {
        if (_list == null) return;
        List<GameObject> _listPlayer = new List<GameObject>();
        foreach (var player in _list)
        {
            if (!player.gameObject.activeSelf) continue;
            if (_war)
            {
                player.setTarget(_pos, false);
            }
            if (_support)
            {
                player.setTarget(GetRandomPositionAround(Castle.Instance.transform, 8), false);
            }
            if (_rally)
            {
                player.setTarget(_pos, true);
            }
            _listPlayer.Add(player.gameObject);
        }
        if (_listPlayer.Count == 0) return;
        if (_war || _support)
        {
            if (!add)
            {
                if (_support)
                    _pos = Castle.Instance.transform.position;
                _co = Instantiate(SelectionBox.Instance._moveTo, _pos, Quaternion.identity);
            }

            if (_support)
            {
                _co.GetComponent<MoveTo>().SetChosen(_listPlayer, add, 8.5f);
            }
            else
            {
                _co.GetComponent<MoveTo>().SetChosen(_listPlayer, add);
            }
        }
    }

    Vector3 GetRandomPositionAround(Transform center, float radius)
    {
        // Tạo 1 điểm ngẫu nhiên trong hình tròn (X, Z)
        Vector2 randomCircle = Random.insideUnitCircle * radius;

        // Gán vào vị trí 3D (X, Y, Z)
        Vector3 randomPos = center.position + new Vector3(randomCircle.x, 0f, randomCircle.y);

        return randomPos;
    }
}
