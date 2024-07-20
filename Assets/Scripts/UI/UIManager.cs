using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager: Singleton<UIManager>
{
    public Button nextWaveButton;

    private void Update()
    {
        nextWaveButton.interactable = !WaveManager.Instance.WaveActive;
    }

    public void SpawnNextWave()
    {
        WaveManager.Instance.QueueNextWave();
    }

    public void SetActiveTower(GameObject towerPrefab)
    {
        TowerManager.Instance.SetActiveTower(towerPrefab);
    }
}