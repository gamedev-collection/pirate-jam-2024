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
        if (Enemy.movementSpeed <= slowAmount) Enemy.movementSpeed = 1;
        else 
            Enemy.movementSpeed = _originalSpeed - slowAmount;
        
        yield return new WaitForSeconds(duration);
        
        EffectApplied = false;
        _freezeCoroutine = null;
        Enemy.movementSpeed = _originalSpeed;
        OnEffectEnd();
    }
}