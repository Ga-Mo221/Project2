using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class SpellBuff : MonoBehaviour
{
    [SerializeField] private SpellMode type;
    private bool IsHealing => type == SpellMode.Healing;
    private bool IsFury => type == SpellMode.Fury;
    private bool IsEfficiency => type == SpellMode.Efficiency;

    [ShowIf(nameof(IsHealing))]
    [SerializeField] private int healAmount = 2;

    [ShowIf(nameof(IsFury))]
    [Range(1f, 3f)] [SerializeField] private float buffDamage = 1.5f;

    [ShowIf(nameof(IsHealing))]
    [SerializeField] private float healInterval = 1f; // thời gian mỗi lần heal

    [HideIf(nameof(IsHealing))]
    [Range(1f, 2f)] [SerializeField] private float buffSpeedAttack = 1.5f;

    [SerializeField] private SpellSpawnDetail _elipp;

    // Kiểm soát tần suất scan
    [SerializeField] private float _scanInterval = 0.2f;
    private float _scanTimer = 0f;
    private float _healTimer = 0f;

    // HashSets để track players
    private HashSet<PlayerAI> _playersInArea = new HashSet<PlayerAI>();
    private HashSet<PlayerAI> _buffedPlayers = new HashSet<PlayerAI>();

    // Cache
    private Transform _transform;
    private bool _wasBuffActive = false;

    private void Awake()
    {
        _transform = transform;
    }

    private void OnEnable()
    {
        _playersInArea.Clear();
        _buffedPlayers.Clear();
        _scanTimer = 0f;
        _healTimer = 0f;
        _wasBuffActive = false;
    }

    private void OnDisable()
    {
        // CRITICAL: Remove all buffs when spell ends
        RemoveAllBuffs();
    }

    private void Update()
    {
        // Early exit nếu không có OutlineManager
        if (OutlineManager.Instance == null || OutlineManager.Instance.players == null)
            return;

        bool isBuffActive = _elipp != null && _elipp.canBuff;

        // Nếu buff vừa tắt -> remove tất cả buffs
        if (!isBuffActive && _wasBuffActive)
        {
            RemoveAllBuffs();
            _wasBuffActive = false;
            return;
        }

        // Nếu buff chưa active -> skip
        if (!isBuffActive)
            return;

        _wasBuffActive = true;

        // ========== SCAN PLAYERS ==========
        _scanTimer -= Time.deltaTime;
        if (_scanTimer <= 0f)
        {
            _scanTimer = _scanInterval;
            ScanPlayersInArea();
        }

        // ========== APPLY BUFFS ==========
        if (IsHealing)
        {
            UpdateHealing();
        }
        else // Fury or Efficiency
        {
            UpdateBuffs();
        }
    }

    // =================== HEALING MODE ===================
    private void UpdateHealing()
    {
        _healTimer += Time.deltaTime;
        if (_healTimer >= healInterval)
        {
            _healTimer = 0f;
            ApplyHealing();
        }
    }

    private void ApplyHealing()
    {
        foreach (var player in _playersInArea)
        {
            if (!IsPlayerValid(player)) continue;

            // Chỉ heal nếu chưa full HP
            if (player._health < player._maxHealth)
            {
                player.Health(healAmount);
            }
        }
    }

    // =================== BUFF MODE (Fury/Efficiency) ===================
    private void UpdateBuffs()
    {
        // 1. Apply buff cho players MỚI vào vùng
        foreach (var player in _playersInArea)
        {
            if (!_buffedPlayers.Contains(player))
            {
                if (IsPlayerValid(player))
                {
                    ApplyBuffToPlayer(player);
                    _buffedPlayers.Add(player);
                }
            }
        }

        // 2. Remove buff cho players RA KHỎI vùng hoặc chết
        var toRemove = new List<PlayerAI>();
        foreach (var player in _buffedPlayers)
        {
            // Player rời vùng hoặc invalid
            if (!_playersInArea.Contains(player) || !IsPlayerValid(player))
            {
                RemoveBuffFromPlayer(player);
                toRemove.Add(player);
            }
        }

        // 3. Cleanup
        foreach (var player in toRemove)
        {
            _buffedPlayers.Remove(player);
        }
    }

    // =================== BUFF OPERATIONS ===================
    private void ApplyBuffToPlayer(PlayerAI player)
    {
        if (player == null) return;

        if (IsFury)
        {
            player.setBounusDamage(buffDamage);
        }
        
        // Attack speed buff áp dụng cho cả Fury và Efficiency
        player.setBounusAttackSpeed(buffSpeedAttack);
    }

    private void RemoveBuffFromPlayer(PlayerAI player)
    {
        if (player == null) return;

        if (IsFury)
        {
            player.setBounusDamage(1f); // Reset về neutral
        }
        
        player.setBounusAttackSpeed(1f); // Reset về neutral
    }

    private void RemoveAllBuffs()
    {
        foreach (var player in _buffedPlayers)
        {
            RemoveBuffFromPlayer(player);
        }
        _buffedPlayers.Clear();
        _playersInArea.Clear();
    }

    // =================== AREA SCANNING ===================
    private void ScanPlayersInArea()
    {
        _playersInArea.Clear();

        if (_elipp == null || OutlineManager.Instance == null)
            return;

        float radiusX = _elipp._radiusX;
        float radiusY = _elipp._radiusY;
        Vector3 center = _transform.position;

        // Tối ưu: cache radiusX^2 và radiusY^2 để tránh tính lại
        float radiusXSq = radiusX * radiusX;
        float radiusYSq = radiusY * radiusY;

        foreach (var player in OutlineManager.Instance.players)
        {
            if (player == null) continue;

            Vector3 pos = player.transform.position;
            float dx = pos.x - center.x;
            float dy = pos.y - center.y;

            // Ellipse equation: (dx²/a²) + (dy²/b²) <= 1
            if ((dx * dx) / radiusXSq + (dy * dy) / radiusYSq <= 1f)
            {
                _playersInArea.Add(player);
            }
        }
    }

    // =================== VALIDATION ===================
    private bool IsPlayerValid(PlayerAI player)
    {
        if (player == null) return false;
        if (!player.gameObject.activeInHierarchy) return false;
        if (player.getDie()) return false;
        if (player.getCreating()) return false;
        return true;
    }

    // =================== GIZMOS FOR DEBUG ===================
    private void OnDrawGizmosSelected()
    {
        if (_elipp == null) return;

        // Vẽ ellipse buff area
        Gizmos.color = type switch
        {
            SpellMode.Healing => Color.green,
            SpellMode.Fury => Color.red,
            SpellMode.Efficiency => Color.yellow,
            _ => Color.white
        };

        Vector3 prevPoint = Vector3.zero;
        Vector3 firstPoint = Vector3.zero;
        int segments = 64;

        for (int i = 0; i <= segments; i++)
        {
            float angle = i * Mathf.PI * 2f / segments;
            float x = Mathf.Cos(angle) * _elipp._radiusX;
            float y = Mathf.Sin(angle) * _elipp._radiusY;
            Vector3 point = transform.position + new Vector3(x, y, 0);

            if (i == 0)
                firstPoint = point;
            else
                Gizmos.DrawLine(prevPoint, point);

            prevPoint = point;
        }
        Gizmos.DrawLine(prevPoint, firstPoint);
    }

    // healing
    public int getHealValue() => healAmount;
    public float getHealInterval() => healInterval;

    // fury
    public float getBuffDamage() => buffDamage;

    // farm
    public float getBuffSpeedAttack() => buffSpeedAttack;

    public float getBuffTime() => _elipp.getTimeBuff();
}

public enum SpellMode
{
    Healing,
    Fury,
    Efficiency
}