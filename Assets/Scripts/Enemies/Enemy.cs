using System;
using System.Collections.Generic;
using UnityEngine;

public class Enemy: MonoBehaviour
{
    private int _currentHp;
    public int maxHp;
    public float movementSpeed;
    public float size;
    public float damage;

    private List<PathNode> _path;
    private int _pathIndex = 0;

    public event Action<GameObject> OnEnemyDestroyed;
    
    private void Update()
    {
        Move();
    }

    public virtual void Move()
    {
        if (_pathIndex < _path.Count)
        {
            var target = _path[_pathIndex];
            var direction = target.transform.position - transform.position;
            transform.Translate(direction.normalized * (movementSpeed * Time.deltaTime), Space.World);

            if (Vector3.Distance(transform.position, target.transform.position) < 0.2f)
            {
                _pathIndex++;
            }
        }
        else
        {
            // TODO: Enemy reached end.
            Die();
        }
    }
    
    public virtual void TakeDamage(int amount)
    {
        _currentHp -= amount;
        if (_currentHp <= 0)
        {
            Die();
        }
    }
    
    public virtual void Die()
    {
        OnEnemyDestroyed?.Invoke(gameObject);
        Destroy(gameObject);
    }

    public void SetPath(List<PathNode> path)
    {
        _path = path;
    }
}
