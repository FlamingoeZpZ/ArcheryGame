using System;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "MyStats/Enemy", menuName = "MyStats/Enemy", order = 5)]
public class EnemySO : ScriptableObject
{
    [field: SerializeField] public float MaxHealth { get; private set; }
    [field: SerializeField] public float PhaseThroughFloorTime { get; private set; }
    [field: SerializeField] public float FallSpeed { get; private set; }
    [field: SerializeField] public float MinAttackTime { get; private set; }
    [field: SerializeField] public float MaxAttackTime { get; private set; }
    [field: SerializeField] public float Value { get; private set; }

    [SerializeField] private DeathTable[] deathTable;

    public void OnDeath(Player p)
    {
        foreach (DeathTable dt in deathTable)
        {
            if (dt.spawnChance <= Random.Range(0, 1))
            {
                Debug.Log("On Death Rewarded");
                //p.GiveAmmo(dt.drop, dt.amount);
            }
        }
    }

}

[Serializable]
public struct DeathTable
{
    [Range(0, 1)] public float spawnChance;
    public Projectile drop;
    [Min(1)] public int amount;
}
