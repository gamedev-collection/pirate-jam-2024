using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static Unity.Collections.Unicode;

public class UIManager : Singleton<UIManager>
{
    public Button nextWaveButton;

    public TMP_Text waveText;
    public TMP_Text enemyText;
    public TMP_Text moneyText;
    public Slider healthSlider;
    public HoverTooltipTrigger healthTooltip;

    public Transform towerShopContainer;
    public GameObject towerShopPrefab;
    public List<GameObject> towerPrefabs = new List<GameObject>();
    public UnityEvent onTowerBought;
    public UnityEvent onRuneBought;
    private List<KeyValuePair<int, Button>> _towerShopButtons = new List<KeyValuePair<int, Button>>();

    public Transform runeShopContainer;
    public GameObject runeShopPrefab;
    public List<GameObject> runePrefabs = new List<GameObject>();
    private List<KeyValuePair<int, Button>> _runeShopButtons = new List<KeyValuePair<int, Button>>();

    public int maxHealth;
    public int CurrentHealth { get; private set; }

    public int money = 100;

    public AnimatedEnableDisable VictoryUI;
    public AnimatedEnableDisable LoseUI;

    private bool _hasActiveItem = false;
    public bool IsGameOver { get; private set; } = false;

    private void Start()
    {
        CurrentHealth = maxHealth;
        UpdateHealthBar();
        CreateTowerShop();
        CreateRuneShop();
    }

    private void Update()
    {
        if (nextWaveButton is not null) nextWaveButton.interactable = !WaveManager.Instance.WaveActive;

        if (waveText is not null) waveText.text = WaveManager.Instance.GetWaveText();

        if (enemyText is not null) enemyText.text = WaveManager.Instance.GetEnemyText();

        if (moneyText is not null) moneyText.text = money.ToString();

        if (_towerShopButtons.Count > 0)
        {
            foreach (var pair in _towerShopButtons)
            {
                pair.Value.interactable = money >= pair.Key;
            }
        }

        if (_runeShopButtons.Count > 0)
        {
            foreach (var pair in _runeShopButtons)
            {
                pair.Value.interactable = money >= pair.Key;
            }
        }
    }

    public void SpawnNextWave()
    {
        WaveManager.Instance.QueueNextWave();
    }

    public void SetActiveTower(GameObject towerPrefab)
    {
        onTowerBought?.Invoke();
        CancelActiveObjects();
        TowerManager.Instance.SetActiveTower(towerPrefab);
    }

    public void SetActiveRune(Rune runePrefab)
    {
        onRuneBought?.Invoke();
        CancelActiveObjects();
        ObeliskManager.Instance.SetActiveRune(runePrefab);
    }

    public void CancelActiveObjects()
    {
        TowerManager.Instance.CancelActiveTower(true);
        ObeliskManager.Instance.CancelActiveRune(true);
    }

    public void TakeDamage(int amount)
    {
        CurrentHealth -= amount;
        UpdateHealthBar();
        if (CurrentHealth <= 0 && !IsGameOver)
        {
            GameOver();
        }
    }

    public void EnableTowerShop()
    {
        towerShopContainer.gameObject.SetActive(true);
    }
    
    public void DisableTowerShop()
    {
        towerShopContainer.gameObject.SetActive(false);
    }
    
    public void EnableRuneShop()
    {
        runeShopContainer.gameObject.SetActive(true);
    }
    
    public void DisableRuneShop()
    {
        runeShopContainer.gameObject.SetActive(false);
    }

    private void UpdateHealthBar()
    {
        healthSlider.value = (float)CurrentHealth / (float)maxHealth;
        healthTooltip.Header = "Health";
        healthTooltip.Body = CurrentHealth + "/" + maxHealth;
    }

    public void AddMoney(int amount)
    {
        money += amount;
    }

    private void GameOver()
    {
        IsGameOver = true;
        WaveManager.Instance.pathVisualiser?.DisablePathVisualiser();
        nextWaveButton.gameObject.SetActive(false);
        LoseUI.AnimatedEnable();
    }

    public void Win()
    {
        IsGameOver = true;
        WaveManager.Instance.pathVisualiser?.DisablePathVisualiser();
        nextWaveButton.gameObject.SetActive(false);
        VictoryUI.AnimatedEnable();
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
            var towerSprite = tower.ShopSprite;
            var towerTooltip = obj.GetComponent<HoverTooltipTrigger>();

            obj.GetComponentInChildren<Image>().sprite = towerSprite;
            towerTooltip.DataObject = towerObj;
            towerTooltip.Type = TooltipType.ShopTower;

            txt.text = tower.cost.ToString();

            btn.interactable = money >= tower.cost;

            btn.onClick.AddListener(delegate { SetActiveTower(towerObj); });

            _towerShopButtons.Add(new KeyValuePair<int, Button>(tower.cost, btn));
        }
    }

    private void CreateRuneShop()
    {
        if (runeShopContainer is null || runeShopPrefab is null || runePrefabs.Count <= 0) return;

        foreach (var runeObj in runePrefabs)
        {
            // Instantiate shop item
            var obj = Instantiate(runeShopPrefab, runeShopContainer);
            var btn = obj.GetComponentInChildren<Button>();
            var txt = obj.GetComponentInChildren<TMP_Text>();
            Sprite runeSprite = null;
            // Get rune component
            Rune rune = runeObj.GetComponent<Rune>();

            var runeTooltip = obj.GetComponent<HoverTooltipTrigger>();
            runeTooltip.DataObject = runeObj;
            if (rune.GetType() == typeof(BuffRune)) runeTooltip.Type = TooltipType.ShopBuffRune;
            if (rune.GetType() == typeof(FireRune)) runeTooltip.Type = TooltipType.ShopFireRune;
            if (rune.GetType() == typeof(FreezeRune)) runeTooltip.Type = TooltipType.ShopFreezeRune;

            runeSprite = rune.runeSprite;
            obj.GetComponentInChildren<Image>().sprite = runeSprite;

            txt.text = rune.cost.ToString();

            btn.interactable = money >= rune.cost;

            btn.onClick.AddListener(delegate { SetActiveRune(rune); });

            _towerShopButtons.Add(new KeyValuePair<int, Button>(rune.cost, btn));
        }
    }
}