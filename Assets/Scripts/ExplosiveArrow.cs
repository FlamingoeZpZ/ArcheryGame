using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveArrow : Projectile
{
    [SerializeField] private ExplosiveSO explosionStats;
    protected override void OnHit(Collision other, Vector3 hitPoint)
    {
        explosionStats.PlayExplosive(hitPoint);
        Collider [] cols = Physics.OverlapSphere(hitPoint, explosionStats.ExplosionRadius);
            
        foreach (Collider c in cols)
        {
            Vector3 direction = c.transform.position - hitPoint;
            float dist = Mathf.Sqrt(direction.x * direction.x + direction.y * direction.y +
                                    direction.z * direction.z);
            Vector3 normalized = direction / dist;
            float perc = explosionStats.DamageFallOff.Evaluate(1-Mathf.Min(1,dist/explosionStats.ExplosionRadius));

            if (c.transform.root.TryGetComponent(out IDamagable ctx))
            {
                //ctx.TakeDamage(Owner , explosionStats.Damage );
                ctx.TakeDamage(explosionStats.Damage );
            }
            
            if (other.rigidbody)
            {
                other.rigidbody.AddForce(normalized * explosionStats.Force * perc, ForceMode.Impulse);
            }
        }
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, explosionStats.ExplosionRadius);
    }
}
