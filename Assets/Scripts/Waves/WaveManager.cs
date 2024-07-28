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
    public bool autoStart = true;

    public PathVisualiser pathVisualiser;

    public bool WaveActive => _activeEnemies.Count > 0 || _waveActive;

    private int _totalEnemiesForCurrentWave = 0;
    private int _enemiesLeftForCurrentWave = 0;

    private readonly List<GameObject> _activeEnemies = new List<GameObject>();

    private Queue<Wave> _waveQueue = new Queue<Wave>();
    private bool _waveActive = false;

    private List<PathNode> _path = new List<PathNode>();

    private int _displayWaves;

    private void Start()
    {
        PathManager.Instance.GenerateAllPaths();
        if (autoStart)
        {
            StartUp();
        }
    }

    public string GetWaveText()
    {
        return $"{_displayWaves} / {waves.Count}";
    }

    public string GetEnemyText()
    {
        return $"{_enemiesLeftForCurrentWave}";
    }

    public void QueueNextWave()
    {
        if (_waveQueue.Count <= 0 || UIManager.Instance.IsGameOver) { pathVisualiser?.DisablePathVisualiser(); return; }

        var wave = _waveQueue.Dequeue();
        StartCoroutine(StartNextWave(wave));
    }
    
    private IEnumerator StartNextWave(Wave wave)
    {
        _waveActive = true;
        pathVisualiser?.DisablePathVisualiser();

        wave.onWaveBegin?.Invoke();

        yield return new WaitForSeconds(waveStartupTime);
        
        yield return StartCoroutine(SpawnWave(wave));
        
        yield return new WaitUntil(() => _activeEnemies.Count == 0);
        
        _waveActive = false;
        wave.onWaveFinish?.Invoke();
        _displayWaves--;

        if(!UIManager.Instance.IsGameOver) pathVisualiser?.EnablePathVisualiser();
        GetNewPath();
    }
    
    private IEnumerator SpawnWave(Wave wave)
    {
        SetCurrentWaveEnemyTotal(wave);
        
        foreach (var enemyCount in wave.enemies)
        {
            for (var i = 0; i < enemyCount.count; i++)
            {
                SpawnEnemy(enemyCount.enemyPrefab, i);
                yield return new WaitForSeconds(1f / enemyCount.rate);
            }
            
            yield return new WaitForSeconds(wave.timeBetweenEnemySpawning);
        }
    }

    private void SpawnEnemy(GameObject prefab, int orderInWave)
    {
        var spawnedObject = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        Enemy spawnedEnemy = spawnedObject.GetComponent<Enemy>();
        spawnedEnemy.OnEnemyDestroyed += HandleEnemyDestroyed;
        spawnedEnemy.SetPath(_path);
        spawnedEnemy.OrderInWave = orderInWave;


        _activeEnemies.Add(spawnedObject);
    }
    
    private void SetCurrentWaveEnemyTotal(Wave wave)
    {
        _enemiesLeftForCurrentWave = _totalEnemiesForCurrentWave = wave.enemies.Sum(waveEnemy => waveEnemy.count);
    }

    private void HandleEnemyDestroyed(GameObject enemy)
    {
        _enemiesLeftForCurrentWave--;
        _activeEnemies.Remove(enemy);
    }

    private void GetNewPath()
    {
        var randomIndex = Random.Range(0, PathManager.Instance.AllPaths.Count);
        _path = PathManager.Instance.AllPaths[randomIndex];
        pathVisualiser?.VisualisePath(_path);
    }

    public void StartUp()
    {
        GetNewPath();
        if (waves is null || waves.Count <= 0) return;
        _waveQueue = new Queue<Wave>(waves);
        _displayWaves = _waveQueue.Count;
    }

    public void GetDummyPath()
    {
        GetNewPath();
    }
}
