using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    public int damage;
    public float attackRate;
    public float range;
    public int cost;
    public Rune runeSlot;
    public ERangeType rangeType;

    public abstract void FindTarget();
    public abstract void Attack();
    public abstract void Delete();
    public abstract void ApplyUpgrade();
}