using UnityEngine;

public interface IDamagable
{
    public float MaxHealth { get; set; }
    public float CurrentHealth { get; set;  }
    //public GameObject PreviousAttacker { get; set; } Can be used to tell who attacked us last

    public void TakeDamage(/*GameObject causer,*/ float amount)
    {
        //PreviousAttacker = causer;

        CurrentHealth -= amount;
        
        if (CurrentHealth <= 0)
        {
            OnDie();
        }
        else
        {
            OnHit(amount);
        }
    }

    public void OnDie();
    public void OnHit(float amount);
}
