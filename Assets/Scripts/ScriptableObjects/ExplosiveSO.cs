using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MyStats/Explosive", menuName = "MyStats/Explosive", order = 21)]
public class ExplosiveSO : ScriptableObject
{
    
    [field:SerializeField] public float Damage { get; private set; }
    [field:SerializeField] public float Force { get; private set; }
    [field:SerializeField] public float ExplosionRadius { get; private set; }
    [field: SerializeField] public AnimationCurve DamageFallOff  { get; private set; }
    
    [Header("Particles")]
    [SerializeField] private ParticleSystem effect;
    [SerializeField] private AudioClip sfx;
    [SerializeField] private float lifeTime;
    

    public void PlayExplosive(Vector3 location)
    {
        ParticleSystem st = Instantiate(effect, location, Quaternion.identity);
        AudioSource.PlayClipAtPoint(sfx, location);
        Destroy(st.gameObject, lifeTime);
    }


}
