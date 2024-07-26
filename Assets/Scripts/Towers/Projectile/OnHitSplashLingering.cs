using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.Unicode;

public class OnHitSplashLingering : OnHitSplash
{
    [SerializeField] private float _duration = 3;
    [SerializeField] private float _tickRate = 1;
    [SerializeField] private CircleCollider2D _col;


    private float _lastAttackTime;
    private List<Enemy> enemies;

    protected override void Initialize(Rune rune)
    {
        _col.radius = _range;
        this._rune = rune;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemies.Add(enemy);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<Enemy>(out Enemy enemy))
        {
            if (enemies.Contains(enemy)) enemies.Remove(enemy);
        }
    }

    private void Update()
    {
        if (!WaveManager.Instance.WaveActive) return;

        if (enemies is not null && enemies.Count > 0 && Time.time - _lastAttackTime >= _tickRate)
        {
            DoHit(enemies);
            _lastAttackTime = Time.time;
        }

        Destroy(gameObject, _duration);
    }

    protected override void DoHit(List<Enemy> enemies)
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.TakeDamage(_damage, _rune);
        }


    }
}
