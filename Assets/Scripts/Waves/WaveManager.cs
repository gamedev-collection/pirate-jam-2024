using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaveManager : Singleton<WaveManager>
{
    public List<Wave> waves;
    public Transform spawnPoint;
    public float waveStartupTime = 2f;

    public bool WaveActive => _activeEnemies.Count > 0 || _waveActive;

    public int WaveCount => waves.Count;
    public int EnemiesLeft => _activeEnemies.Count;
    public int totalEnemiesForCurrentWave = 0;
    public int enemiesLeftForCurrentWave = 0;
    
    private readonly List<GameObject> _activeEnemies = new List<GameObject>();

    private Queue<Wave> _waveQueue = new Queue<Wave>();
    private bool _waveActive = false;

    private void Start()
    {
        PathManager.Instance.GenerateAllPaths();

        if (waves is null || waves.Count <= 0) return;

        _waveQueue = new Queue<Wave>(waves);
    }

    public void QueueNextWave()
    {
        if (_waveQueue.Count <= 0) return;

        var wave = _waveQueue.Dequeue();
        StartCoroutine(StartNextWave(wave));
    }
    
    private IEnumerator StartNextWave(Wave wave)
    {
        _waveActive = true;
        yield return new WaitForSeconds(waveStartupTime);
        
        yield return StartCoroutine(SpawnWave(wave));
        
        yield return new WaitUntil(() => _activeEnemies.Count == 0);
        _waveActive = false;
        wave.onWaveFinish?.Invoke();
    }
    
    private IEnumerator SpawnWave(Wave wave)
    {
        SetCurrentWaveEnemyTotal(wave);
        
        foreach (var enemyCount in wave.enemies)
        {
            var randomIndex = Random.Range(0, PathManager.Instance.AllPaths.Count);
            var path = PathManager.Instance.AllPaths[randomIndex];
            
            for (var i = 0; i < enemyCount.count; i++)
            {
                SpawnEnemy(enemyCount.enemyPrefab, path);
                enemiesLeftForCurrentWave--;
                yield return new WaitForSeconds(1f / enemyCount.rate);
            }
            
            yield return new WaitForSeconds(wave.timeBetweenEnemySpawning);
        }
    }

    private void SpawnEnemy(GameObject prefab, List<PathNode> path)
    {
        var spawnedObject = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        
        spawnedObject.GetComponent<Enemy>().OnEnemyDestroyed += HandleEnemyDestroyed;
        spawnedObject.GetComponent<Enemy>().SetPath(path);
        
        _activeEnemies.Add(spawnedObject);
    }
    
    private void SetCurrentWaveEnemyTotal(Wave wave)
    {
        enemiesLeftForCurrentWave = totalEnemiesForCurrentWave = wave.enemies.Sum(waveEnemy => waveEnemy.count);
    }

    private void HandleEnemyDestroyed(GameObject enemy)
    {
        _activeEnemies.Remove(enemy);
    }
}
