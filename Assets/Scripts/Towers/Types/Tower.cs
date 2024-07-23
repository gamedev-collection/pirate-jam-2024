using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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

    protected int actualDamage;
    protected float actualAttackRate;
    protected float actualRange;

    private void Awake()
    {
        if(runeSlot) ApplyRune(runeSlot);
        else ResetStats();
    }

    //public abstract Enemy FindTarget();
    public virtual List<Enemy> FindTargets()
    {
        var results = new List<Collider2D>();
        var enemies = new List<Enemy>();
        
        var hits = Physics2D.OverlapCircle(transform.position, actualRange, filter, results);

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
        if (rune == runeSlot) return;

        ResetStats();
        runeSlot = rune;
        SO_Rune runeData = rune.RuneData;

        actualDamage += runeData.damageBuff;
        actualAttackRate += runeData.attackRateBuff;
        actualRange += runeData.rangeBuff;
        Debug.Log("Applied Rune: " + rune.RuneData.runeName);
    }

    public virtual void RemoveRune()
    {
        ResetStats();
        Debug.Log("Removed Rune: " + runeSlot.RuneData.runeName);
        runeSlot = null;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, actualRange);
    }

    private void ResetStats()
    {
        actualDamage = damage;
        actualAttackRate = attackRate;
        actualRange = range;
    }
}