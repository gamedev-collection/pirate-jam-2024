﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BasicTower : Tower
{
    private float _lastAttackTime;

    private void Start()
    {
        _lastAttackTime = -1f / attackRate;
    }

    private void Update()
    {
        CheckForDeletion();
        CheckForHover();
        
        if (!WaveManager.Instance.WaveActive) return;

        var targets = FindTargets();
        if (targets is not null && targets.Count > 0 && Time.time - _lastAttackTime >= 1f / attackRate)
        {
            var target = targets.OrderBy(enemy => enemy.CurrentHp).First();
            Attack(target);
            _lastAttackTime = Time.time;
        }
    }
    
    public override void Attack(Enemy target)
    {
        if (projectile == null) return;
        
        PlayAttackClip();
        
        var projectileInstance = Instantiate(projectile, transform.position, Quaternion.identity);
        var projectileScript = projectileInstance.GetComponent<Projectile>();
        
        var direction = (target.transform.position - transform.position).normalized;
        
        if (projectileScript != null)
        {
            projectileScript.Init(damage, runeSlot, direction);
        }
    }
}