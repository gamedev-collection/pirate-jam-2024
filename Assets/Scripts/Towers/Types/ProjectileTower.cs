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
        _lastAttackTime = -1f / attackRate;
    }

    private void Update()
    {
        CheckForDeletion();
        
        if (!WaveManager.Instance.WaveActive || InBuildMode) return;
        
        if (Time.time - _lastAttackTime >= 1f / attackRate && _currentVolleyDelay <= 0)
        {
            _lastAttackTime = Time.time;
            animator.SetTrigger("Attack");
        }

        if (_volley && _currentVolleyDelay > 0)
        {
            _currentVolleyDelay -= Time.deltaTime;
        }
    }

    public void Throw()
    {
        if (!(_currentVolleyDelay <= 0) && _volley) return;
        //if (_savedTarget.transform.position.x > transform.position.x)
        //{
        //    visual.GetComponent<SpriteRenderer>().flipX = true;
        //    _projectileSpawn.transform.position = new Vector3(_projectileSpawn.transform.position.x * - 1, _projectileSpawn.transform.position.y, _projectileSpawn.transform.position.z);
        //}
        //else 
        //{
        //    visual.GetComponent<SpriteRenderer>().flipX = false;
        //    _projectileSpawn.transform.position = new Vector3(_projectileSpawn.transform.position.x * - 1, _projectileSpawn.transform.position.y, _projectileSpawn.transform.position.z);
        //}
            
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

        if (!_volley) return;
        _currentVolleyShot++;
        if (_currentVolleyShot < _volleyAmount) return;
        _currentVolleyDelay = _volleyDelay; _currentVolleyShot = 0;
    }

    public override void Attack(Enemy target)
    {
        if (projectile == null) return;

        if (target is null || target.CurrentHp <= 0) return;
        
        var projectileInstance = Instantiate(projectile, _projectileSpawn.position, Quaternion.identity);
        var projectileScript = projectileInstance.GetComponent<HomingProjectile>();

        if (projectileScript != null)
        {
            projectileScript.Init(damage, runeSlot, target.transform);
        }

    }
}
