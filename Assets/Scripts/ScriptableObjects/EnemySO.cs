using UnityEngine;

[CreateAssetMenu(fileName = "MyStats/Enemy", menuName = "MyStats/Enemy", order = 5)]
public class EnemySo : ScriptableObject
{
    [field: SerializeField] public float MaxHealth { get; private set; }
    [field: SerializeField] public float PhaseThroughFloorTime { get; private set; }
    [field: SerializeField] public float FallSpeed { get; private set; }
    [field: SerializeField] public float Damage { get; private set; }
    [field: SerializeField] public float Value { get; private set; }
    [field: SerializeField] public float AnimationSpeed { get; private set; }

    [field: SerializeField] public AudioClip HitNoise { get; private set; }
    [field: SerializeField] public float StunTolerance { get; private set; }
}

