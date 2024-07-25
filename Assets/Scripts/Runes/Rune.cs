using UnityEngine;

public enum ERuneType
{
    Tower,
    Enemy
}

public abstract class Rune : MonoBehaviour
{
    public string runeName = "Rune";
    public int cost;
    public ERuneType runeType;

    protected Enemy Enemy;
    protected Tower Tower;

    protected bool EffectApplied = false;

    public void Init(Enemy enemy)
    {
        Enemy = enemy;
    }

    public void Init(Tower tower)
    {
        Tower = tower;
    }
    
    public void OnEffectEnd()
    {
        Destroy(this);
    }

    public abstract void ApplyEffect();

    protected abstract void OnEffectRenew();

    protected void TakeDamage(int amount)
    {
        Enemy.TakeDamage(amount);
    }
}
