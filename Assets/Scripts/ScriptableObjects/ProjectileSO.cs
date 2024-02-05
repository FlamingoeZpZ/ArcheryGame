using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MyStats/Projectile", menuName = "MyStats/Projectile", order = 20)]
public class ProjectileSO : ScriptableObject
{
    [field: SerializeField] public float MinSpeed { get; private set; }
    [field: SerializeField] public float MaxSpeed { get; private set; }
    [field: SerializeField] public float MinDamage { get; private set; }
    [field: SerializeField] public float MaxDamage { get; private set; }

    [SerializeField] private AudioPairing[] HitSounds;

    public void PlayOnHit(LayerMask layer, Vector3 position)
    {
        int val = 1 << layer;
        foreach (AudioPairing ap in HitSounds)
        {
            Debug.Log(val + ", " +  ap.targetLayer.value);
            if (val == ap.targetLayer)
            {
                AudioSource.PlayClipAtPoint(ap.sound, position);
                return;
            }
        }
    }

    [Serializable]
    public struct AudioPairing
    {
        public AudioClip sound;
        public LayerMask targetLayer;
    }

}
