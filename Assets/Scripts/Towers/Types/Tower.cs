using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using static Unity.Collections.Unicode;

public abstract class Tower : MonoBehaviour
{
    public int damage;
    public float attackRate;
    public float range;
    public int cost;
    public Rune runeSlot;
    public GameObject projectile;

    public ContactFilter2D filter;

    //protected int actualDamage;
    //protected float actualAttackRate;
    //protected float actualRange;

    public SpriteRenderer rangeIndicator;
    
    private void Awake()
    {
        //if(runeSlot) ApplyRune(runeSlot);
        //else RemoveRune();
        
        if (rangeIndicator is null) return; 
        
        rangeIndicator.enabled = false;
        rangeIndicator.transform.localScale = new Vector2(range * 2, range * 2);
    }
    
    private void OnMouseEnter()
    {
        EnableRangeIndicator();
    }
    
    private void OnMouseExit()
    {
        DisableRangeIndicator();
    }

    public virtual List<Enemy> FindTargets()
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
    
    public abstract void Attack(Enemy target);
    public abstract void Delete();
    
    public virtual void ApplyRune(Rune rune)
    {
        runeSlot = rune;
    }

    public virtual void RemoveRune()
    {
        runeSlot = null;
    }

    public void EnableRangeIndicator()
    {
        rangeIndicator.enabled = true;
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

    /*
    private void ResetStats()
    {
        actualDamage = damage;
        actualAttackRate = attackRate;
        actualRange = range;
    }
    */
}