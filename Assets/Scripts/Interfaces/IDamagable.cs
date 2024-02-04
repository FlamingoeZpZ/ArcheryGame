using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    public float MaxHealth { get; set; }
    public float CurrentHealth { get; set;  }
    public GameObject PreviousAttacker { get; set; }

    public void TakeDamage(GameObject causer, Vector3 hitLocation, Vector3 hitDirection, float amount)
    {
        PreviousAttacker = causer;
        ApplyKnockBack(hitLocation, hitDirection);

        CurrentHealth -= amount;
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
        }
    }

    public void ApplyKnockBack(Vector3 hitLocation, Vector3 hitDirection);


    public void OnDie();
    
}
