using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Transform pointer;
    [SerializeField] private Transform bowHolder;
    [SerializeField] private Vector2 aimBounds;
    [SerializeField, Range(0.01f,2)] private float speedScalar;

    //[Header("Weapons")]
    //[SerializeField]
    private Weapon weapon; // Not serialized when doing upgrades.


    public void Aim(Vector2 direction)
    {
        //This approach to looking natively locks the screen dimension. Alternatively, you can do regular rotation around axes.
        Vector3 loc = pointer.localPosition;
        Vector3 attempt = loc + (Vector3)(direction * speedScalar);
        attempt.x = Mathf.Clamp( attempt.x,-aimBounds.x, aimBounds.x);
        attempt.y = Mathf.Clamp( attempt.y,-aimBounds.y, aimBounds.y);
        pointer.localPosition = attempt;
    }

    public void BeginFire()
    {
        weapon.BeginFire();
    }

    public void EndFire()
    {
        weapon.EndFire();
    }

    public void SetWeaponStats(WeaponSO statsReplace)
    {
        weapon.SetStats(statsReplace);
    }

    public void SetProjectileStats(ProjectileSO statsReplace)
    {
        weapon.ProjectileType.SetStats(statsReplace);
    }

    public void ReplaceWeapon(Weapon objectReplace)
    {
        Weapon newWeapon = Instantiate(objectReplace, bowHolder);
        newWeapon.Init(gameObject);
        if (weapon)
        {
            Projectile projectile = weapon.ProjectileType;
            Destroy(weapon.gameObject);
            newWeapon.SetProjectile(projectile);
        }

        weapon = newWeapon;
    }

    public void ReplaceProjectile(Projectile objectReplace)
    {
        weapon.SetProjectile(objectReplace);
    }
}
