using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyPrefab
{
    public Vector2 _count;
    public float _delay;
    public EnemyType _enemy;
}

[System.Serializable]
public class Wave
{
    public float _delay;
    public List<EnemyPrefab> _listEnemy;
}

[System.Serializable]
public class War
{
    public int _startDay;
    public int _endDay;
    public List<Wave> _listWave;
}