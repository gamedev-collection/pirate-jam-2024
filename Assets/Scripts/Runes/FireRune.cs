using System.Collections;
using UnityEngine;

public class FireRune : Rune
{
    public int damagePerTick;
    public float tickRate;
    public int tickTimes;

    private bool _hasStarted = false;
    private Coroutine _damageCoroutine;
    

    public override void ApplyEffect()
    {
        if (_hasStarted)
        {
            OnEffectRenew();
        }
        else
        {
            _hasStarted = true;
            StartDamageCoroutine();
        }
    }

    protected override void OnEffectRenew()
    {
        if (_damageCoroutine != null)
        {
            StopCoroutine(_damageCoroutine);
        }
        StartDamageCoroutine();
    }

    private void StartDamageCoroutine()
    {
        _damageCoroutine = StartCoroutine(DealDamageOverTime());
    }
    
    private IEnumerator DealDamageOverTime()
    {
        var tickCount = 0;

        while (tickCount < tickTimes)
        {
            TakeDamage(damagePerTick);
            tickCount++;
            yield return new WaitForSeconds(tickRate);
        }
        _hasStarted = false;
        _damageCoroutine = null;
        OnEffectEnd();
    }
}