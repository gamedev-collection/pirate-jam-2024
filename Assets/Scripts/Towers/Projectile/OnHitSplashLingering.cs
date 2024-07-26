using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.Unicode;

public class OnHitSplashLingering : OnHitSplash
{
    [Header("Lingering")]
    [SerializeField] private float _duration = 3;
    [SerializeField] private CircleCollider2D _col;

    private float _lastAttackTime;

    public override void Initialize(int damage, Rune rune = null)
    {
        _col.radius = _range;

        base.Initialize(damage, rune);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.TakeDamage(_damage, _rune);
        }
    }

    private void Update()
    {
        Destroy(gameObject, _duration);
    }
}
