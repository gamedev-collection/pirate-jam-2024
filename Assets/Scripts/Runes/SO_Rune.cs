using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OnHitEffect
{
    None,
    DOT,
    Slow
}

[CreateAssetMenu(fileName = "SO_Rune_", menuName = "Scriptable Objects/SO_Rune")]
public class SO_Rune : ScriptableObject
{
    public string runeName = "Rune";
    public int cost;

    [Header("On-Hit Effect")]
    public OnHitEffect onHitEffect = OnHitEffect.None;

    [Header("Buffs")]
    public int damageBuff = 0;
    public float attackRateBuff = 0;
    public float rangeBuff = 0;
}
