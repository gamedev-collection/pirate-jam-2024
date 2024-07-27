using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AreaTower : Tower
{
    [SerializeField] private ParticleSystem _aoeParticles;
    private float _lastAttackTime;
    private List<Enemy> targets;

    private void Start()
    {
        _lastAttackTime = -attackRate;
        var main = _aoeParticles.main;
        main.startSize = new ParticleSystem.MinMaxCurve(range + 1);
    }
    
    private void Update()
    {
        if (!WaveManager.Instance.WaveActive) return;

        targets = FindTargets();
        if (targets is not null && targets.Count > 0 && Time.time - _lastAttackTime >= attackRate)
        {
            _lastAttackTime = Time.time;
            animator.SetTrigger("Attack");
        }
    }

    public void Pulse()
    {
        foreach (var target in targets)
        {
            Attack(target);
        }
    }

    public override void Attack(Enemy target)
    {
        target.TakeDamage(damage, runeSlot);
    }

    public override void Delete()
    {
        
    }

    public override void ApplyRune(Rune rune)
    {
        
    }

    private void SpawnParticles()
    {
        if(_aoeParticles) _aoeParticles.Play();
    }
}