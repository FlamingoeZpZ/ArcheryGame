using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamagable
{
    [Header("Movement")]
    [SerializeField] private Transform pointer;
    [SerializeField] private Vector2 aimBounds;
    [SerializeField, Range(0.01f,2)] private float speedScalar;

    [Header("Weapons")]
    [SerializeField] private Weapon weapon;
    [field: SerializeField] public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }
    public GameObject PreviousAttacker { get; set; }

    public static Transform PlayerTransform;

    //public readonly Dictionary<Projectile, int> AmmoCounts = new();
    /*
    private AmmoInfo[] startingAmmos;

  
    [Serializable]
    public struct AmmoInfo
    {
        public Projectile ammoType;
        public int startingAmount;
    }
    */



    // Start is called before the first frame update
    void Awake()
    {
        PlayerControls.Instance.MouseMoveDelta += Aim;
        PlayerControls.Instance.OnMousePressed += BeginFire;
        PlayerControls.Instance.OnMouseReleased += EndFire;
        PlayerControls.Instance.OnSwapWeapon += SwapWeapon;
        PlayerControls.Instance.OnSwapProjectile += SwapProjectile;
/*
        foreach (AmmoInfo ammoInfo in startingAmmos)
        {
            AmmoCounts.Add(ammoInfo.ammoType, ammoInfo.startingAmount);
        }
*/

        //This swaps in the current weapon, but also updates the ammo shenanigans.
        //PlayerControls.Instance.OnSwapWeapon(weapon);
        
        weapon.Init(gameObject);
        
        CurrentHealth = MaxHealth;
        PlayerTransform = transform;
    }


    private void Aim(Vector2 direction)
    {
        Vector3 loc = pointer.localPosition;
        Vector3 attempt = loc + (Vector3)(direction * speedScalar);
        attempt.x = Mathf.Clamp( attempt.x,-aimBounds.x, aimBounds.x);
        attempt.y = Mathf.Clamp( attempt.y,-aimBounds.y, aimBounds.y);
        pointer.localPosition = attempt;
    }

    private void BeginFire()
    {
        //if (AmmoCounts[weapon.ProjectileType] > 0)
        //    AmmoCounts[weapon.ProjectileType] -= 1;
        weapon.BeginFire();
    }

    private void EndFire()
    {
        weapon.EndFire();
    }

    public GameObject GetSelf()
    {
        return gameObject;
    }

    public void OnDie()
    {
        print("Death");
        PlayerControls.Instance.OnDeath();
    }

    public void OnHit(float amount)
    {
        // Update the healthbar??
        PlayerControls.Instance.SetHealth(CurrentHealth/ MaxHealth);
    }

    public void SwapWeapon(Weapon w)
    {
        weapon.EndFire();
        Transform wepParent = weapon.transform.parent;
        Destroy(weapon.gameObject);
        weapon = Instantiate(w, wepParent);
        weapon.Init(gameObject);
    }

    public void SwapProjectile(Projectile p)
    {
        weapon.EndFire();
        weapon.SetProjectile(p);
    }

    //UI affecting
    public void AddScore(float statsValue)
    {
        PlayerControls.Instance.AddScore(statsValue);
    }

    /*
    public void GiveAmmo(Projectile dtDrop, int dtAmount)
    {
        //Add ammo into that dictionary.
        if (!AmmoCounts.TryGetValue(dtDrop, out var count))
        {
            AmmoCounts.Add(dtDrop, dtAmount);
            //Add new button in UI.
            return;
        }
        

        if(count < 0) return; // Do not allow this to work if value is negative.
        AmmoCounts[dtDrop] = count + dtAmount;
        
        //Update the UI.
        
    } */
}
