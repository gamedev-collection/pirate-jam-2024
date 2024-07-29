using UnityEngine;

public enum ERuneType
{
    Tower,
    Enemy
}

public abstract class Rune : MonoBehaviour
{
    public string runeName = "Rune"; 
    [TextArea] public string runeDescription = "Rune Description";
    public int cost;
    public ERuneType runeType;
    public Sprite runeSprite;
    public Color runeColor;

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
        if (Enemy) Enemy.movementSpeed = Enemy.OriginalMovementSpeed;
        Destroy(this.gameObject);
    }

    public abstract void ApplyEffect();

    protected abstract void OnEffectRenew();

    protected void TakeDamage(int amount)
    {
        Enemy.TakeDamage(amount);
    }
}
