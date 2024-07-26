using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OnHitSplash : MonoBehaviour
{
    [SerializeField] protected float _range = 1;
    [SerializeField] protected ContactFilter2D filter;

    [Header("Visuals")]
    [SerializeField] private GameObject _visualContainer;
    [SerializeField] private GameObject _visualNormal;
    [SerializeField] private GameObject _visualDOT;
    [SerializeField] private GameObject _visualSlow;

    protected int _damage;
    protected Rune _rune;

    public virtual void Initialize(int damage, Rune rune = null)
    {
        _visualContainer.transform.localScale *= _range;
        _visualDOT.SetActive(false);
        _visualSlow.SetActive(false);
        _visualNormal.SetActive(false);

        if (rune != null)
        {
            if (rune.GetType() == typeof(BuffRune)) { _visualNormal.SetActive(true); }
            if (rune.GetType() == typeof(FireRune)) { _visualDOT.SetActive(true); }
            if (rune.GetType() == typeof(FreezeRune)) { _visualSlow.SetActive(true); }
        }
        else _visualNormal.SetActive(true);

        _damage = damage;
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

        Destroy(gameObject, 0.2f);
    }
}
