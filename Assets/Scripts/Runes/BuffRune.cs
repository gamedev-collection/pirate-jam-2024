using UnityEngine;

public class BuffRune : Rune
{
    public int damageBuff;
    public float rangeBuff;
    public float attackRateBuff;

    private bool _effectApplied;
    
    public override void ApplyEffect()
    {
        if (_effectApplied) return;

        Tower.damage += damageBuff;
        Tower.attackRate += attackRateBuff;
        Tower.range += rangeBuff;
        _effectApplied = true;
    }

    protected override void OnEffectRenew()
    {
        
    }
}