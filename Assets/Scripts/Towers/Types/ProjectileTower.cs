using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class HomingTower : Tower
{
    [SerializeField] bool _volley = false;
    [SerializeField] int _volleyAmount;
    [SerializeField] float _volleyDelay;
    private float _lastAttackTime;

    private int _currentVolleyShot = 0;
    private float _currentVolleyDelay = 0;

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

            if (_currentVolleyDelay <= 0 || !_volley)
            {
                var target = targets.OrderBy(enemy => enemy.CurrentHp).First();
                Attack(target);
                _lastAttackTime = Time.time;

                if (_volley)
                {
                    _currentVolleyShot++;
                    if (_currentVolleyShot >= _volleyAmount) { _currentVolleyDelay = _volleyDelay; _currentVolleyShot = 0; }
                }

            }
        }

        if (_volley && _currentVolleyDelay > 0)
        {
            _currentVolleyDelay -= Time.deltaTime;
        }
    }

    public override void Attack(Enemy target)
    {
        if (projectile == null) return;

        var projectileInstance = Instantiate(projectile, transform.position, Quaternion.identity);
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
