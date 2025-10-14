using System.Collections.Generic;
using UnityEngine;

public class RadialMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator _anim;
    private Vector3 _pos;
    private MoveTo _moveto;

    private bool _war;
    private bool _support;
    private bool _rally;

    private void Awake()
    {
        if (_anim == null)
            _anim = GetComponent<Animator>();
    }

    /// <summary>
    /// Kích hoạt hoặc tắt animation state
    /// </summary>
    public void OnAnimation(string name, bool active)
    {
        if (_anim == null) return;
        _anim.SetBool(name, active);

        // Cập nhật cờ logic
        switch (name)
        {
            case "war": _war = active; break;
            case "support": _support = active; break;
            case "rally": _rally = active; break;
        }
    }

    public void SetPos(Vector3 pos) => _pos = pos;

    private void Update()
    {
        // Nếu Castle không khả dụng hoặc V == false thì tắt menu
        if (Castle.Instance == null || !Castle.Instance._V)
        {
            gameObject.SetActive(false);
            return;
        }

        // Khi thả chuột trái
        if (Input.GetMouseButtonUp(0))
        {
            HandleCommand();
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Xử lý lệnh điều khiển quân theo phím và trạng thái lính
    /// </summary>
    private void HandleCommand()
    {
        _moveto = null;
        var castle = Castle.Instance;
        if (castle == null) return;

        // Gom tất cả danh sách lính
        var unitGroups = new List<(bool key, List<PlayerAI> list)>
        {
            (castle._Q, castle._ListWarrior),
            (castle._W, castle._ListArcher),
            (castle._E, castle._ListLancer),
            (castle._A, castle._ListHealer),
            (castle._S, castle._ListTNT)
        };

        // Nếu đang giữ phím cụ thể thì chỉ điều khiển nhóm đó
        foreach (var (key, list) in unitGroups)
        {
            if (key && castle._V)
            {
                PlayerController(list, false);
                return;
            }
        }

        // Nếu không giữ phím nào, chọn nhóm đầu tiên còn lính chết
        foreach (var (_, list) in unitGroups)
        {
            if (HasInactive(list))
            {
                // Nhóm chính (revive)
                PlayerController(list, false);

                // Các nhóm khác đặt chế độ add = true
                foreach (var (_, otherList) in unitGroups)
                    if (otherList != list)
                    {
                        PlayerController(otherList, true);
                    }

                return;
            }
        }
    }

    /// <summary>
    /// Kiểm tra xem có unit nào trong danh sách đang bị tắt
    /// </summary>
    private bool HasInactive(List<PlayerAI> list)
    {
        if (list == null) return false;
        foreach (var p in list)
        {
            if (p != null && p.gameObject.activeSelf &&  !p.getUpTower())
                return true;
        }
        return false;
    }

    /// <summary>
    /// Ra lệnh cho nhóm đơn vị di chuyển / hỗ trợ / rally
    /// </summary>
    private void PlayerController(List<PlayerAI> list, bool add)
    {
        if (list == null || list.Count == 0) return;

        List<GameObject> activePlayers = new List<GameObject>();
        foreach (var player in list)
        {
            if (player == null || !player.gameObject.activeSelf || player.getUpTower() || player._movingToTower) continue;
            Vector3 targetPos = _support
                ? RandomPoint(Castle.Instance._In_Castle_Pos, 8)
                : _pos;

            player.setTarget(targetPos, true);
            activePlayers.Add(player.gameObject);
        }

        if (activePlayers.Count == 0) return;

        //Spawn marker
        if (!add)
        {
            if (_support)
                _pos = Castle.Instance._In_Castle_Pos.position;
            _moveto = MoveToManager.Instance.CreateMovePoint(activePlayers, _pos);
        }

        if (_moveto == null) return;


        float radius = _support ? 8.5f : -1f;
        bool active = _rally ? false : true;
        _moveto.SetChosen(activePlayers, true, add, active, radius, _support);
    }

    /// <summary>
    /// Trả về một vị trí ngẫu nhiên xung quanh tâm trong bán kính radius
    /// </summary>
    private Vector3 RandomPoint(Transform center, float radius)
    {
        if (center == null) return Vector3.zero;

        Vector2 circle = Random.insideUnitCircle * radius;
        return center.position + new Vector3(circle.x, 0f, circle.y);
    }
}
