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
            if (!c.attachedRigidbody) return;
            if (c.attachedRigidbody.TryGetComponent(out IDamagable ctx))
            {
                Vector3 direction = c.ClosestPoint(hitPoint) - hitPoint;
                float dist = Mathf.Sqrt(direction.x * direction.x + direction.y * direction.y +
                                        direction.z * direction.z);
                Vector3 normalized = direction / dist;
                float perc = explosionStats.DamageFallOff.Evaluate(1-Mathf.Min(1,dist/explosionStats.ExplosionRadius));
                
                ctx.TakeDamage(Owner, hitPoint , normalized * explosionStats.Force * perc, explosionStats.Damage );
            }
        }
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, explosionStats.ExplosionRadius);
    }
}
