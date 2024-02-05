using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    public float MaxHealth { get; set; }
    public float CurrentHealth { get; set;  }
    public GameObject PreviousAttacker { get; set; }

    public void TakeDamage(GameObject causer, float amount)
    {
        if (causer == GetSelf()) return;
        PreviousAttacker = causer;

        CurrentHealth -= amount;
        Debug.Log($"Damage taken, {amount} --> {CurrentHealth}");
        
        if (CurrentHealth <= 0)
        {
            OnDie();
        }
        else
        {
            OnHit(amount);
        }
    }

    GameObject GetSelf();


    public void OnDie();
    public void OnHit(float amount);
}
