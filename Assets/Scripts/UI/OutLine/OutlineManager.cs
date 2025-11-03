using System.Collections.Generic;
using UnityEngine;

public class OutlineManager : MonoBehaviour
{
    public static OutlineManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public List<PlayerAI> players = new List<PlayerAI>();

    void Start()
    {
        InvokeRepeating(nameof(updateList), 0f, 0.3f);
    }

    void updateList()
    {
        if (Castle.Instance == null) return;
        updatePlayers();
    }

    private void updatePlayers()
    {
        if (Castle.Instance == null) return;

        players.Clear();
        players.AddRange(Castle.Instance._ListWarrior);
        players.AddRange(Castle.Instance._ListArcher);
        players.AddRange(Castle.Instance._ListLancer);
        players.AddRange(Castle.Instance._ListHealer);
        players.AddRange(Castle.Instance._ListTNT);
    }
}
