using UnityEngine;

public abstract class Rune : MonoBehaviour
{
    public string runeName = "Rune";
    public int cost;

    protected Enemy Enemy;

    public void Init(Enemy enemy)
    {
        Enemy = enemy;
    }

    public abstract void ApplyEffect();

    protected abstract void OnEffectRenew();

    protected void TakeDamage(int amount)
    {
        Enemy.TakeDamage(amount);
    }

    protected void OnEffectEnd()
    {
        Destroy(this);
    }
}
