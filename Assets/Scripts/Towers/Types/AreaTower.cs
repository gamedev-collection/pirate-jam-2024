using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AreaTower : Tower
{
    [SerializeField] private ParticleSystem _aoeParticles;
    private float _lastAttackTime;

    private void Start()
    {
        _lastAttackTime = -attackRate;
        var main = _aoeParticles.main;
        main.startSize = new ParticleSystem.MinMaxCurve(range + 1);
    }
    
    private void Update()
    {
        CheckForDeletion();
        
        if (!WaveManager.Instance.WaveActive) return;
        
        if (Time.time - _lastAttackTime >= attackRate)
        {
            _lastAttackTime = Time.time;
            animator.SetTrigger("Attack");
        }
    }

    public void Pulse()
    {
        var targets = FindTargets();
        if (targets is null || targets.Count <= 0) return;
        
        foreach (var target in targets)
        {
            Attack(target);
        }
    }

    public override void Attack(Enemy target)
    {
        target.TakeDamage(damage, runeSlot);
    }

    private void SpawnParticles()
    {
        if(_aoeParticles) _aoeParticles.Play();
    }
}