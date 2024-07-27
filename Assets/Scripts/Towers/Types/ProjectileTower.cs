using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class HomingTower : Tower
{
    [SerializeField] bool _volley = false;
    [SerializeField] int _volleyAmount;
    [SerializeField] float _volleyDelay;
    [SerializeField] Transform _projectileSpawn;
    private float _lastAttackTime;

    private int _currentVolleyShot = 0;
    private float _currentVolleyDelay = 0;
    private Enemy _target;

    private void Start()
    {
        _lastAttackTime = -1f / attackRate;
    }

    private void Update()
    {
        if (!WaveManager.Instance.WaveActive) return;

        var targets = FindTargets();
        if (targets is not null && targets.Count > 0 && Time.time - _lastAttackTime >= 1f / attackRate)
        {
            _target = targets.OrderBy(enemy => enemy.CurrentHp).First();
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

        if (_currentVolleyDelay <= 0 || !_volley)
        {
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

            Attack(_target);

            if (_volley)
            {
                _currentVolleyShot++;
                if (_currentVolleyShot >= _volleyAmount) { _currentVolleyDelay = _volleyDelay; _currentVolleyShot = 0; }
            }

        }
    }

    public override void Attack(Enemy target)
    {
        if (projectile == null) return;

        var projectileInstance = Instantiate(projectile, _projectileSpawn.position, Quaternion.identity);
        var projectileScript = projectileInstance.GetComponent<HomingProjectile>();

        if (projectileScript != null)
        {
            projectileScript.Init(damage, runeSlot, target.transform);
        }

    }

    public override void Delete()
    {
    }
}
