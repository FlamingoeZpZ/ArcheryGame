using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MyStats/Castle", menuName = "MyStats/Castle", order = 10)]
public class CastleSO : ScriptableObject
{
    [field:SerializeField] public float MaxHealth { get; private set; }
    [field:SerializeField] public float CoinMultiplier { get; private set; }
}
