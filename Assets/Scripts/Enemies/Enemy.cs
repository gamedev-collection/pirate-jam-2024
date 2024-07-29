using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Enemy: MonoBehaviour
{
    public int CurrentHp { get; private set; }
    public int maxHp = 10;
    public float movementSpeed = 5;
    public float size = 1;
    public int damage = 1;
    public int price = 1;
    public float pathingOffset = 0.2f;
    public float animationSpeedOffset = -0.5f;
    public float slowReist = 0f;

    public ParticleSystem damageParticle;
    public Animator animator;
    public int OrderInWave {  get; set; }

    private List<PathNode> _path;
    private List<Vector3> _offsetPath;
    private int _pathIndex = 0;
    private bool _isDead = false;

    public event Action<GameObject> OnEnemyDestroyed;

    private GameObject _runeInstance;
    private Rune _rune;

    public float OriginalMovementSpeed { get; private set; }

    public AudioClip deathAudio;
    private AudioSource _audioSource;

    private void Start()
    {
        CurrentHp = maxHp;
        animator.SetFloat("SpeedMultiplier", animationSpeedOffset);
        _audioSource ??= GetComponent<AudioSource>();

        OriginalMovementSpeed = movementSpeed;
    }

    private void Update()
    {
        if(!_isDead) Move();
    }

    private void Move()
    {
        if (_pathIndex < _offsetPath.Count)
        {
            var target = _offsetPath[_pathIndex];
            var direction = target - transform.position;
            transform.Translate(direction.normalized * (movementSpeed * Time.deltaTime), Space.World);

            if (Vector3.Distance(transform.position, target) < 0.2f)
            {
                _pathIndex++;
            }
        }
        else
        {
            UIManager.Instance.TakeDamage(damage);
            Die();
        }
    }
    
    public void TakeDamage(int amount, Rune rune)
    {
        CurrentHp -= amount;
        if (rune is not null && rune.runeType == ERuneType.Enemy)
        {
            var runeType = rune.GetType();
            if (runeType == typeof(FreezeRune))
            {
                var fr = gameObject.GetComponentInChildren<FreezeRune>();
                fr?.OnEffectEnd();
            } else if (runeType == typeof(FireRune))
            {
                var fr = gameObject.GetComponentInChildren<FireRune>();
                fr?.OnEffectEnd();
            }
            
            var runeInstance = Instantiate(rune.gameObject, this.transform);
            var runeComp = runeInstance.GetComponent<Rune>();

            runeComp?.Init(this);
            runeComp?.ApplyEffect();
        }
        damageParticle?.Play();
        if (CurrentHp > 0) return;
        DieWithMoney();
    }
    
    public void TakeDamage(int amount)
    {
        CurrentHp -= amount;
        if (CurrentHp > 0) return;
        DieWithMoney();
    }
    
    private void Die()
    {
        _isDead = true;
        if (deathAudio) _audioSource?.PlayOneShot(deathAudio);
        animator.SetTrigger("Die");
        OnEnemyDestroyed?.Invoke(gameObject);
    }

    public void DestroyEnemy()
    {
        Destroy(this.gameObject);
    }

    private void DieWithMoney()
    {
        UIManager.Instance.money += price;
        Die();
    }

    public void SetPath(List<PathNode> path)
    {
        _path = path;
        _offsetPath = new List<Vector3>();

        foreach (var node in _path)
        {
            var offset = new Vector3(
                UnityEngine.Random.Range(-pathingOffset, pathingOffset),
                UnityEngine.Random.Range(-pathingOffset, pathingOffset),
                0
            );

            _offsetPath.Add(node.transform.position + offset);
        }
    }
}
