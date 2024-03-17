using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "MyStats/Upgrade", menuName = "MyStats/Upgrade", order = 10)]
public class UpgradeSO : ScriptableObject
{
    [field: SerializeField] public float Cost { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public UnityEvent Action { get; private set; }

    [field: SerializeField] public bool MustApply { get; private set; }
    
    //name is built in.

    public void ReplaceCastleStats(CastleSO replace)
    {
        Castle.Instance.SetStats(replace);   
    }

    public void ReplaceWeaponStats(WeaponSO replace)
    {
        PlayerControls.LocalPlayer.SetWeaponStats(replace);
    }
    
    public void ReplaceProjectileStats(ProjectileSO replace)
    {
        PlayerControls.LocalPlayer.SetProjectileStats(replace);
    }

    public void ReplaceWeaponObject(Weapon replace)
    {
        PlayerControls.LocalPlayer.ReplaceWeapon(replace);
    }

    public void ReplaceProjectileObject(Projectile replace)
    {
        PlayerControls.LocalPlayer.ReplaceProjectile(replace);
    }



}
