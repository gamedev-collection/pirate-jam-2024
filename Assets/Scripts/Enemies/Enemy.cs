using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Enemy: MonoBehaviour
{
    private int _currentHp;
    public int maxHp;
    public float movementSpeed;
    public float size;

    private List<PathNode> _path;
    private int _pathIndex = 0;

    public event Action<GameObject> OnEnemyDestroyed;

    private void Start()
    {
        FindPath();
    }

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
            transform.Translate(direction.normalized * movementSpeed * Time.deltaTime, Space.World);

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
    
    private void FindPath()
    {
        //var randomIndex = Random.Range(0, PathManager.Instance.AllPaths.Count);
        //_path = PathManager.Instance.AllPaths[randomIndex];
        _path = PathManager.Instance.GetPathWithLeastNodes();
    }
}
