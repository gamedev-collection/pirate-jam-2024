using System.Collections;
using UnityEngine;

public class FreezeRune : Rune
{
    public float slowAmount;
    public float duration;

    private float _originalSpeed;
    private Coroutine _freezeCoroutine;
    
    public override void ApplyEffect()
    {
        if (EffectApplied)
        {
            OnEffectRenew();
        }
        else
        {
            EffectApplied = true;
            _originalSpeed = Enemy.movementSpeed;
            StartFreezeCoroutine();
        }
    }

    protected override void OnEffectRenew()
    {
        if (_freezeCoroutine != null)
        {
            StopCoroutine(_freezeCoroutine);
        }
        StartFreezeCoroutine();
    }
    
    private void StartFreezeCoroutine()
    {
        _freezeCoroutine = StartCoroutine(Freeze());
    }

    private IEnumerator Freeze()
    {
        Enemy.movementSpeed = _originalSpeed;
        if (Enemy.movementSpeed < slowAmount) Enemy.movementSpeed = 0.1f;
        else
            Enemy.movementSpeed = _originalSpeed * slowAmount + Enemy.slowReist;
        
        yield return new WaitForSeconds(duration);
        
        EffectApplied = false;
        _freezeCoroutine = null;
        Enemy.movementSpeed = _originalSpeed;
        OnEffectEnd();
    }
}