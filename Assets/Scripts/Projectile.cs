using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))] // Forced to rigidbody
public class Projectile : MonoBehaviour
{
    [field: SerializeField] public ProjectileSO Stats { get; private set; }


    protected GameObject Owner;
    private Rigidbody rb;
    private TrailRenderer tr;
    private Collider c;
    private float charge;    

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        c = GetComponentInChildren<Collider>();
        tr = GetComponentInChildren<TrailRenderer>();
    }

    public void Init(float chargePercent, GameObject owner)
    {
        charge = chargePercent; 
        
        print(Mathf.Lerp(Stats.MinSpeed, Stats.MaxSpeed, charge));
        
        rb.AddForce(transform.forward * Mathf.Lerp(Stats.MinSpeed, Stats.MaxSpeed, charge), ForceMode.Impulse);
        Owner = owner;
    }

    private void LateUpdate()
    {
        if(rb.velocity.magnitude > 0) transform.forward = rb.velocity.normalized;
    }

    public void OnDisable()
    {
        rb.isKinematic = true;
        tr.enabled = false;
        c.enabled = false;
    }

    public void OnEnable()
    {
        rb.isKinematic = false;
        tr.enabled = true;
        c.enabled = true;
    }


    private void OnCollisionEnter(Collision other)
    {
        rb.isKinematic = true;
        Debug.Log("I hit: " + other.gameObject.name);
        OnHit(other,other.contacts[0].point);
        c.enabled = false;
    }

    protected virtual void OnHit(Collision other, Vector3 hitPoint)
    {
        //Take Impact Damage and bind effects
        Stats.PlayOnHit(other.gameObject.layer, hitPoint);
        
        if (other.transform.root.TryGetComponent(out IDamagable damagable))
        {
            damagable.TakeDamage(Owner, Mathf.Lerp(Stats.MinDamage, Stats.MaxDamage, charge));
        }

        if (other.rigidbody)
        {
            other.rigidbody.AddForce(rb.velocity, ForceMode.Impulse);
        }

        transform.position = hitPoint;
        transform.parent = other.transform;
    }

}
