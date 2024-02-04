using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))] // Forced to rigidbody
public class Projectile : MonoBehaviour
{
    [SerializeField] private ProjectileSO stats;
    
    
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
        
        print(Mathf.Lerp(stats.MinSpeed, stats.MaxSpeed, charge));
        
        rb.AddForce(transform.forward * Mathf.Lerp(stats.MinSpeed, stats.MaxSpeed, charge), ForceMode.Impulse);
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
        OnHit(other,other.contacts[0].point);
    }

    protected virtual void OnHit(Collision other, Vector3 hitPoint)
    {
        //Take Impact Damage and bind effects
        stats.PlayOnHit(other.gameObject.layer, hitPoint);
        
        if (!other.rigidbody) return; // Can only damage thing w/ rb

        if (other.rigidbody.TryGetComponent(out IDamagable damagable))
        {
            damagable.TakeDamage(Owner, hitPoint, rb.velocity, Mathf.Lerp(stats.MinDamage, stats.MaxDamage, charge));
        }
    }

}
