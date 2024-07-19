using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : Singleton<WaveManager>
{
    public List<Wave> waves;
    public Transform spawnPoint;
    public float timeBetweenWaves = 5f;
    
    private readonly List<GameObject> _activeEnemies = new List<GameObject>();

    private void Start()
    {
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
            Debug.Log($"Spawning wave {enemyCount.count}, {enemyCount.enemyPrefab.name}");
            for (var i = 0; i < enemyCount.count; i++)
            {
                SpawnEnemy(enemyCount.enemyPrefab);
                yield return new WaitForSeconds(1f / enemyCount.rate);
            }
            
            yield return new WaitForSeconds(wave.timeBetweenEnemySpawning);
        }
    }

    private void SpawnEnemy(GameObject prefab)
    {
        var spawnedObject = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        
        spawnedObject.GetComponent<Enemy>().OnEnemyDestroyed += HandleEnemyDestroyed;
        
        _activeEnemies.Add(spawnedObject);
    }

    private void HandleEnemyDestroyed(GameObject enemy)
    {
        _activeEnemies.Remove(enemy);
    }
}
