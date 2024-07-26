using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OnHitSplash : MonoBehaviour
{
    [SerializeField] protected int _damage = 1;
    [SerializeField] protected float _range = 1;
    [SerializeField] protected ContactFilter2D filter;

    [Header("Visuals")]
    [SerializeField] private GameObject _visualNormal;
    [SerializeField] private GameObject _visualDOT;
    [SerializeField] private GameObject _visualSlow;

    protected Rune _rune;

    protected virtual void Initialize(Rune rune)
    {
        
        if(rune.GetType() != typeof(FireRune)) { _visualDOT.SetActive(false);}

        this._rune = rune;
        DoHit(FindTargets());
    }

    protected List<Enemy> FindTargets()
    {
        var results = new List<Collider2D>();
        var enemies = new List<Enemy>();
        var hits = Physics2D.OverlapCircle(transform.position, _range, filter, results);

        if (hits > 0 && results.Count > 0)
        {
            enemies.AddRange(results.Select(result => result.GetComponent<Enemy>()).Where(enemy => enemy is not null));
        }

        return enemies;
    }

    protected virtual void DoHit(List<Enemy> enemies)
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.TakeDamage(_damage, _rune);
        }

        Destroy(gameObject);
    }
}
