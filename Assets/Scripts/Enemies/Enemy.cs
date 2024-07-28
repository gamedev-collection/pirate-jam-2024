using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Enemy: MonoBehaviour
{
    public int CurrentHp { get; private set; }
    public int maxHp = 10;
    public float movementSpeed = 5;
    public float size = 1;
    public int damage = 1;
    public int price = 1;
    public float offsetWidth = 0.5f;

    public ParticleSystem damageParticle;
    public int OrderInWave {  get; set; }

    private List<PathNode> _path;
    private int _pathIndex = 0;

    public event Action<GameObject> OnEnemyDestroyed;

    private GameObject _runeInstance;
    private Rune _rune;

    private Dictionary<Rune, GameObject> _runes = new Dictionary<Rune, GameObject>();

    public AudioClip deathAudio;
    private AudioSource _audioSource;

    private void Start()
    {
        CurrentHp = maxHp;
        
        _audioSource ??= GetComponent<AudioSource>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
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
            UIManager.Instance.TakeDamage(damage);
            Die();
        }
    }
    
    public void TakeDamage(int amount, Rune rune)
    {
        CurrentHp -= amount;
        if (rune is not null && rune.runeType == ERuneType.Enemy)
        {
            Rune runeComp;
            if (_runes.ContainsKey(rune))
            { 
                runeComp = _runes[rune].GetComponent<Rune>();
            }
            else
            {
                var runeInstance = Instantiate(rune.gameObject, this.transform);
                runeComp = runeInstance.GetComponent<Rune>();
                _runes[rune] = runeInstance;
            }
            
            runeComp.Init(this);
            runeComp.ApplyEffect();
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
        if (deathAudio) _audioSource?.PlayOneShot(deathAudio);
        
        OnEnemyDestroyed?.Invoke(gameObject);
        Destroy(gameObject);
    }

    private void DieWithMoney()
    {
        UIManager.Instance.money += price;
        Die();
    }

    public void SetPath(List<PathNode> path)
    {
        _path = path;
    }
}
