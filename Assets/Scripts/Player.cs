using System.Collections;
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
    
    
    // Start is called before the first frame update
    void Awake()
    {
        PlayerControls.Instance.MouseMoveDelta += Aim;
        PlayerControls.Instance.OnMousePressed += BeginFire;
        PlayerControls.Instance.OnMouseReleased += EndFire;
        PlayerControls.Instance.OnSwapWeapon += SwapWeapon;
        PlayerControls.Instance.OnSwapProjectile += SwapProjectile;
        
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
    }

    public void OnHit(float amount)
    {
        // Update the healthbar??
        
    }

    public void SwapWeapon(Weapon w)
    {
        weapon.EndFire();
        Transform wepParent = weapon.transform.parent;
        Destroy(weapon.gameObject);
        weapon = Instantiate(w, wepParent);
    }

    public void SwapProjectile(Projectile p)
    {
        weapon.EndFire();
        weapon.SetProjectile(p);
    }

}
