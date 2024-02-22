using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Transform pointer;
    [SerializeField] private Vector2 aimBounds;
    [SerializeField, Range(0.01f,2)] private float speedScalar;

    [Header("Weapons")]
    [SerializeField] private Weapon weapon;
    
    //Levels
    public int WeaponLevel { get; private set; }
    public int ProjectileLevel { get; private set; } 
    
    
    // Start is called before the first frame update
    void Awake()
    {
        weapon.Init(gameObject);
    }
    
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

}
