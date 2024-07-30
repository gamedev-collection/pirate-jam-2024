using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ProjectileTower : Tower
{
    [SerializeField] private bool _volley = false;
    [SerializeField] private int _volleyAmount;
    [SerializeField] private float _volleyDelay;
    [SerializeField] private Transform _projectileSpawn;
    private float _lastAttackTime;

    private int _currentVolleyShot = 0;
    private float _currentVolleyDelay = 0;

    private void Start()
    {
        _lastAttackTime = 0;
    }

    private void Update()
    {
        CheckForDeletion();
        CheckForHover();
        
        if (!WaveManager.Instance.WaveActive || InBuildMode) return;
        
        _lastAttackTime -= Time.deltaTime;
        
        if (_lastAttackTime <= 0)
        {
            if (_volley && _currentVolleyDelay > 0)
            {
                _currentVolleyDelay -= Time.deltaTime;
                return;
            }

            var targets = FindTargets();
            if (targets is null || targets.Count <= 0) return;
            
            animator.SetTrigger("Attack");
            maskAnimator.SetTrigger("Attack");
            
            _lastAttackTime = 1f / attackRate;
        }
    }

    public void Throw()
    {
        if (_volley && _currentVolleyDelay > 0) return;
        
        var targets = FindTargets();
        if (targets is not null && targets.Count > 0)
        {
            var target = targetingFocus switch
            {
                TargetingFocus.LowestHealth => targets.OrderBy(enemy => enemy.CurrentHp).First(),
                TargetingFocus.HighestHealth => targets.OrderBy(enemy => enemy.CurrentHp).Last(),
                TargetingFocus.FirstIn => targets.OrderBy(enemy => enemy.OrderInWave).First(),
                _ => targets.First()
            };

            Attack(target);
        }

        if (_volley)
        {
            _currentVolleyShot++;
            if (_currentVolleyShot >= _volleyAmount)
            {
                _currentVolleyDelay = _volleyDelay;
                _currentVolleyShot = 0;
            }
        }
        else
        {
            _lastAttackTime = 1f / attackRate;
        }
    }

    public override void Attack(Enemy target)
    {
        if (projectile == null) return;

        if (target is null || target.CurrentHp <= 0) return;
        
        PlayAttackClip();
        
        var projectileInstance = Instantiate(projectile, _projectileSpawn.position, Quaternion.identity);
        var projectileScript = projectileInstance.GetComponent<HomingProjectile>();

        if (projectileScript != null)
        {
            projectileScript.Init(damage, runeSlot, target.transform);
        }

    }
}
