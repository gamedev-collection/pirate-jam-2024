using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public struct Wave
{
    public List<EnemyCount> enemies;
    public UnityEvent onWaveFinish;
    public float timeBetweenEnemySpawning;
}

[Serializable]
public struct EnemyCount
{
    public GameObject enemyPrefab;
    public int count;
    public float rate;
}
