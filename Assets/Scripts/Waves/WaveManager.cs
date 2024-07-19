using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaveManager : Singleton<WaveManager>
{
    public List<Wave> waves;
    public Transform spawnPoint;
    public float timeBetweenWaves = 5f;
    
    private readonly List<GameObject> _activeEnemies = new List<GameObject>();

    private void Start()
    {
        PathManager.Instance.GenerateAllPaths();
        StartCoroutine(StartNextWave());
    }

    private IEnumerator StartNextWave()
    {
        foreach (var wave in waves)
        {
            yield return StartCoroutine(SpawnWave(wave));
            
            yield return new WaitUntil(() => _activeEnemies.Count == 0);
            
            yield return new WaitForSeconds(timeBetweenWaves);
            
            wave.onWaveFinish?.Invoke();
        }
    }

    private IEnumerator SpawnWave(Wave wave)
    {
        foreach (var enemyCount in wave.enemies)
        {
            var randomIndex = Random.Range(0, PathManager.Instance.AllPaths.Count);
            var path = PathManager.Instance.AllPaths[randomIndex];
            
            for (var i = 0; i < enemyCount.count; i++)
            {
                SpawnEnemy(enemyCount.enemyPrefab, path);
                
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

    private void HandleEnemyDestroyed(GameObject enemy)
    {
        _activeEnemies.Remove(enemy);
    }
}
