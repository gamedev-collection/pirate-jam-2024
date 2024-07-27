using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.Rendering.DebugUI;

public class HomingProjectile : MonoBehaviour
{
    public float lifetime = 2f;
    public float speed;
    public GameObject visual;

    [Header("Homing")]
    public bool homing = false;

    [Header("Piercing")]
    public bool piercing = false;
    public int piercingAmount = 1;

    [Header("Scaling")]
    public bool scaleOverDistance = false;
    public AnimationCurve scaleCurve = AnimationCurve.Constant(0, 1, 1);

    [Header("Slowdown")]
    public bool slowdownWithScale = false;
    public float slowestSpeed;
    [Range(0, 1)] public float slowestCurvePoint;

    [Header("OnHit Instantiate")]
    public bool onHitInstantiate = false;
    public OnHitSplash onHitObject;

    [Header("Visual")]
    public Color colorNormal;
    public Color colorDOT;
    public Color colorSlow;

    private bool _isInitialised = false;
    private int _damage;
    private int _hits;
    private float _startingDistance;
    private float _actualSpeed;
    private Transform _target;
    private Vector3 _startingPosition;
    private Vector3 _startingScale;
    private Vector3 _lastTargetPos;
    private Vector3 _direction;
    private Rune _rune;

    public void Init(int damage, Rune rune, Transform target)
    {
        _damage = damage;
        _rune = rune;
        _target = target;
        _lastTargetPos = _target.position;
        _direction = target.position - transform.position;
        _startingPosition = transform.position;
        _startingScale = visual.transform.localScale;
        _startingDistance = Vector3.Distance(_lastTargetPos, _startingPosition);
        _actualSpeed = speed;

        Color color = colorNormal;
        if (rune != null)
        {
            if (rune.GetType() == typeof(FireRune)) { color = colorDOT; }
            if (rune.GetType() == typeof(FreezeRune)) { color = colorSlow; }
        }

        if (!piercing) piercingAmount = 1;
        
        if(visual) visual.GetComponent<SpriteRenderer>().material.color = color;

        _isInitialised = true;

    }

    private void Update()
    {
        if (!_isInitialised) return;
        if (_target && homing)
        {
            _lastTargetPos = _target.position;
            _direction = _lastTargetPos - transform.position;
        }

        if (scaleOverDistance) UpdateScale(Vector3.Distance(_lastTargetPos, transform.position));
        if (slowdownWithScale) UpdateSpeed(Vector3.Distance(_lastTargetPos, transform.position));

        _direction.Normalize();

        visual.transform.Rotate(0, 0, _actualSpeed * 100 * Time.deltaTime);
        transform.position += _direction * _actualSpeed * Time.deltaTime;

        if ((scaleOverDistance || onHitInstantiate) && Vector3.Distance(_startingPosition, transform.position) > _startingDistance)
        {
            if(onHitInstantiate) InstantiateSplashObject();
            Destroy(gameObject);
        }
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        OnHit(col);
    }

    private void UpdateScale(float distance)
    {
        float percentage = 1 - (distance / _startingDistance);
        float value = scaleCurve.Evaluate(percentage);
        visual.transform.localScale = new Vector3(_startingScale.x * value, _startingScale.y * value, 1);
    }

    private void UpdateSpeed(float distance)
    {
        float percentage = 1 - (distance / _startingDistance);
        if (percentage < slowestCurvePoint)
        {
            _actualSpeed = Mathf.Lerp(speed, slowestSpeed, Mathf.InverseLerp(0f, slowestCurvePoint, percentage));
        }

        if (percentage > slowestCurvePoint)
        {
            _actualSpeed = Mathf.Lerp(slowestSpeed, speed, Mathf.InverseLerp(slowestCurvePoint, 1f, percentage));
        }
    }

    protected virtual void OnHit(Collider2D col)
    {
        if (_hits < piercingAmount)
        {
            if (onHitInstantiate)
            {
                InstantiateSplashObject();
            }
            else
            {
                var enemy = col.gameObject.GetComponent<Enemy>();
                enemy?.TakeDamage(_damage, _rune);
            }
            _hits++;
            homing = false;
        }
        else { Destroy(gameObject); }
    }

    protected virtual void InstantiateSplashObject()
    {
        OnHitSplash splash = Instantiate(onHitObject, new Vector3(_lastTargetPos.x, _lastTargetPos.y, _lastTargetPos.z), Quaternion.identity);
        splash.Initialize(_damage, _rune);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;

        Gizmos.DrawLine(transform.position, _lastTargetPos);
    }
}

