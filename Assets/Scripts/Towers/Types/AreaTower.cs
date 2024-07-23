using UnityEngine;

public class AreaTower: Tower
{
    private float _lastAttackTime;
    
    private void Start()
    {
        _lastAttackTime = -attackRate;
    }
    
    private void Update()
    {
        if (!WaveManager.Instance.WaveActive) return;

        var targets = FindTargets();
        if (targets is not null && targets.Count > 0 && Time.time - _lastAttackTime >= attackRate)
        {
            SpawnParticles();
            foreach (var target in targets)
            {
                Attack(target);
            }
            _lastAttackTime = Time.time;
        }
    }

    public override void Attack(Enemy target)
    {
        target.TakeDamage(damage, runeSlot);
    }

    public override void Delete()
    {
        
    }

    public override void ApplyUpgrade()
    {
        
    }

    private void SpawnParticles()
    {
        
    }
}