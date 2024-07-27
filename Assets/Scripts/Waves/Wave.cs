using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public struct Wave
{
    public float timeBetweenEnemySpawning;
    public UnityEvent onWaveFinish;
    public UnityEvent onWaveBegin;
    public List<EnemyCount> enemies;
}

[Serializable]
public struct EnemyCount
{
    public GameObject enemyPrefab;
    public int count;
    public float rate;
}
