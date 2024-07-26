using UnityEngine;

public class BuffRune : Rune
{
    public int damageBuff;
    public float rangeBuff;
    public float attackRateBuff;


    public override void ApplyEffect()
    {
        if (EffectApplied) return;

        Tower.damage += damageBuff;
        Tower.attackRate += attackRateBuff;
        Tower.range += rangeBuff;
        EffectApplied = true;
    }

    protected override void OnEffectRenew()
    {
        
    }
}