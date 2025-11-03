using System.Collections.Generic;
using UnityEngine;

public class Outline_Rada_Tree : MonoBehaviour
{
    [SerializeField] private OutLine _outline;
    [SerializeField] private float _range = 5f;
    void Update()
    {
        var players = OutlineManager.Instance.players;
        if (players == null || players.Count == 0) return;

        Transform target = MinDistPlayer(players);
        if (target != null && target != _outline.player)
            _outline.setPlayer(target);
    }

    private Transform MinDistPlayer(List<PlayerAI> players)
    {
        float minDist = 1000000f;
        Transform minPlayer = null;
        foreach (var player in players)
        {
            if (player.getDie()) continue;
            float dist = Vector2.Distance(player.transform.position, transform.position);
            if (dist < _range && dist < minDist)
            {
                minDist = dist;
                minPlayer = player.transform;
            }
        }
        return minPlayer;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _range);
    }
}
