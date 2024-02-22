using System;
using UnityEngine;

[CreateAssetMenu(fileName = "MyStats/Round", menuName = "MyStats/Round", order = 10)]
public class RoundSO : ScriptableObject
{
    
    [Serializable]
    public struct SpawnAmount
    {
        public Enemy spawnObject;
        public int amount; // The first enemy is guaranteed to be spawned.
    }
    [field:SerializeField] public SpawnAmount [] SpawnedEnemies { get; private set; }
    [field:SerializeField] public float DayDuration { get; private set; }
}
