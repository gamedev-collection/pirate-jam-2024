using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager: Singleton<UIManager>
{
    public Button nextWaveButton;

    public TMP_Text waveText;
    public TMP_Text enemyText;
    public TMP_Text moneyText;
    public Slider healthSlider;

    public Transform towerShopContainer;
    public GameObject towerShopPrefab;
    public List<GameObject> towerPrefabs = new List<GameObject>();
    private List<KeyValuePair<int, Button>> _towerShopButtons = new List<KeyValuePair<int, Button>>();
    
    public int maxHealth;
    public int CurrentHealth { get; private set; }

    public int money = 100;

    private void Start()
    {
        CurrentHealth = maxHealth;
        UpdateHealthBar();
        CreateTowerShop();
    }

    private void Update()
    {
        if (nextWaveButton is not null) nextWaveButton.interactable = !WaveManager.Instance.WaveActive;

        if (waveText is not null) waveText.text = WaveManager.Instance.GetWaveText();

        if (enemyText is not null) enemyText.text = WaveManager.Instance.GetEnemyText();

        if (moneyText is not null) moneyText.text = money.ToString();

        if (_towerShopButtons.Count <= 0) return;
        foreach (var pair in _towerShopButtons)
        {
            pair.Value.interactable = money >= pair.Key;
        }
    }

    public void SpawnNextWave()
    {
        WaveManager.Instance.QueueNextWave();
    }

    public void SetActiveTower(GameObject towerPrefab)
    {
        TowerManager.Instance.SetActiveTower(towerPrefab);
    }

    public void TakeDamage(int amount)
    {
        CurrentHealth -= amount;
        UpdateHealthBar();
        if (CurrentHealth <= 0)
        {
            GameOver();
        }
    }

    private void UpdateHealthBar()
    {
        healthSlider.value = (float)CurrentHealth / (float)maxHealth;
    }

    private void GameOver()
    {
        // TODO: Gameover
        Debug.Log("Game Over");
    }

    private void CreateTowerShop()
    {
        if (towerShopContainer is null || towerShopPrefab is null || towerPrefabs.Count <= 0) return;

        foreach (var towerObj in towerPrefabs)
        {
            // Instantiate shop item
            var obj = Instantiate(towerShopPrefab, towerShopContainer);
            var btn = obj.GetComponentInChildren<Button>();
            var txt = obj.GetComponentInChildren<TMP_Text>();
            
            // Get tower component
            var tower = towerObj.GetComponent<Tower>();
            var towerSprite = towerObj.GetComponent<SpriteRenderer>().sprite;

            obj.GetComponentInChildren<Image>().sprite = towerSprite;
            
            txt.text = tower.cost.ToString();
            
            btn.interactable = money >= tower.cost;

            btn.onClick.AddListener(delegate { SetActiveTower(towerObj); });
            
            _towerShopButtons.Add(new KeyValuePair<int, Button>(tower.cost, btn));
        }
    }
}