using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BasicTower : Tower
{
    private float _lastAttackTime;

    private void Start()
    {
        _lastAttackTime = -actualAttackRate;
    }

    private void Update()
    {
        if (!WaveManager.Instance.WaveActive) return;

        var targets = FindTargets();
        if (targets is not null && targets.Count > 0 && Time.time - _lastAttackTime >= actualAttackRate)
        {
            var target = targets.OrderBy(enemy => enemy.CurrentHp).First();
            Attack(target);
            _lastAttackTime = Time.time;
        }
    }

    /*
    public override Enemy FindTarget()
    {
        var enemies = FindObjectsOfType<Enemy>();
        Enemy nearestEnemy = null;
        var nearestDistance = float.MaxValue;

        foreach (var enemy in enemies)
        {
            var distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (!(distance < nearestDistance) || !(distance <= range)) continue;
            nearestEnemy = enemy;
            nearestDistance = distance;
        }

        return nearestEnemy;
    }
    */
    
    public override void Attack(Enemy target)
    {
        if (projectile == null) return;
        
        var projectileInstance = Instantiate(projectile, transform.position, Quaternion.identity);
        var projectileScript = projectileInstance.GetComponent<Projectile>();
        
        var direction = (target.transform.position - transform.position).normalized;
        
        if (projectileScript != null)
        {
            projectileScript.Init(actualDamage, runeSlot, direction);
        }
    }

    public override void Delete()
    {
    }
}