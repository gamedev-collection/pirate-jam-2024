﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    public int damage;
    public float attackRate;
    public float range;
    public int cost;
    public Rune runeSlot;
    public GameObject projectile;
    public Animator animator;
    public Animator maskAnimator;
    public string speedMultiplierParam = "SpeedMultiplier";
    public float speedMultiplier = 1f;
    public TargetingFocus targetingFocus = TargetingFocus.LowestHealth;
    public ContactFilter2D filter;
    public Sprite ShopSprite;

    public string towerName = "Tower";
    [TextArea] public string towerDescription = "Tower Description";
    
    public SpriteRenderer rangeIndicator;

    private int _originalDamage;
    private float _originalRate;
    private float _originalRange;
    private Dictionary<Rune, GameObject> _runes = new Dictionary<Rune, GameObject>();
    public bool InBuildMode { get; set; } = true;

    private bool _isMouseOver = false;

    public SpriteRenderer towerMask;
    private Color _maskColor;

    public AudioClip attackAudio;
    private AudioSource _audioSource;
   
    public abstract void Attack(Enemy target);
    
    private void Awake()
    {
        if (rangeIndicator is null) return;

        _originalDamage = damage;
        _originalRate = attackRate;
        _originalRange = range;
        
        if (runeSlot) ApplyRune(runeSlot);
        
        rangeIndicator.enabled = false;
        rangeIndicator.transform.localScale = new Vector2(range * 2, range * 2);
        animator.SetFloat(speedMultiplierParam, speedMultiplier);
        maskAnimator.SetFloat(speedMultiplierParam, speedMultiplier);

        if (towerMask) _maskColor = towerMask.color;

        _audioSource ??= GetComponent<AudioSource>();
    }
    
    protected void CheckForHover()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition = new Vector3(mousePosition.x, mousePosition.y, 0);
        if (Vector3.Distance(new Vector3(transform.position.x, transform.position.y, 0), mousePosition) < 0.4)
        {
            EnableRangeIndicator();
            _isMouseOver = true;
        }
        else
        {
            DisableRangeIndicator();
            _isMouseOver = false;
        }
    }

    public List<Enemy> FindTargets()
    {
        var results = new List<Collider2D>();
        var enemies = new List<Enemy>();
        
        var hits = Physics2D.OverlapCircle(transform.position, range, filter, results);

        if (hits > 0 && results.Count > 0)
        {
            enemies.AddRange(results.Select(result => result.GetComponent<Enemy>()).Where(enemy => enemy is not null));
        }
        
        return enemies;
    }
    
    public void ApplyRune(Rune rune)
    {
        runeSlot = rune;

        if (towerMask) towerMask.color = rune.runeColor;

        if (rune.runeType != ERuneType.Tower) return;
        
        Rune runeComp;
        if (_runes.ContainsKey(rune))
        {
            runeComp = _runes[rune].GetComponent<Rune>();
        }
        else
        {
            var runeInstance = Instantiate(rune.gameObject, this.transform);
            runeInstance.GetComponent<SpriteRenderer>().enabled = false;
            runeComp = runeInstance.GetComponent<Rune>();
            _runes[rune] = runeInstance;
        }
            
        runeComp.Init(this);
        runeComp.ApplyEffect();
    }

    public void RemoveRune(Rune rune)
    {
        if (rune is not null && rune.runeType == ERuneType.Tower)
        {
            var instance = _runes[rune];
            instance?.GetComponent<Rune>().OnEffectEnd();

            ResetStats();
        }

        if (towerMask) towerMask.color = _maskColor;
        
        runeSlot = null;
    }

    public void EnableRangeIndicator()
    {
        rangeIndicator.enabled = true;
        rangeIndicator.transform.localScale = new Vector2(range * 2, range * 2);
    }
    
    public void DisableRangeIndicator()
    {
        rangeIndicator.enabled = false;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, range);
    }
    
    private void ResetStats()
    {
        damage = _originalDamage;
        attackRate = _originalRate;
        range = _originalRange;
    }

    protected void CheckForDeletion()
    {
        if (!_isMouseOver) return;

        if (Input.GetMouseButtonDown(1) && !TowerManager.Instance.InBuildmode)
        {
            Delete();
        }
    }
    
    protected void PlayAttackClip()
    {
        if (attackAudio) _audioSource.PlayOneShot(attackAudio);
    }

    private void Delete()
    {
        UIManager.Instance.money += Mathf.RoundToInt(cost / 2f);
        TowerManager.Instance.RemoveActiveTower(transform.position);
        ObeliskManager.Instance.OnTowerSold(this);
        Destroy(gameObject);
    }
}

[Serializable]
public enum TargetingFocus
{
    HighestHealth,
    LowestHealth,
    FirstIn
}