using UnityEngine;

public interface ITargetable
{
    public int Health { get; set; }
    public Transform CurrentTransform { get; set; }

    public virtual void Damage(int newDamage)
    {
        Health -= newDamage;

        if(Health <= 0)
        {
            Die();
        }
    }

    public abstract void Die();
}
