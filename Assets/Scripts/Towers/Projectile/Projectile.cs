
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public float lifetime = 2f;
    
    private int _damage;
    private Rune _rune;
    private Vector3 _direction;
    private bool _isInitialised = false;

    public AudioClip detonateAudio;
    private AudioSource _audioSource;

    public void Init(int damage, Rune rune, Vector3 direction)
    {
        _damage = damage;
        _rune = rune;
        _direction = direction;
        _isInitialised = true;

        _audioSource ??= GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!_isInitialised) return;

        transform.position += _direction * speed * Time.deltaTime;

        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        var enemy = col.gameObject.GetComponent<Enemy>();
        
        if (detonateAudio) _audioSource?.PlayOneShot(detonateAudio);
        
        enemy?.TakeDamage(_damage, _rune);
        Destroy(gameObject);
    }
}