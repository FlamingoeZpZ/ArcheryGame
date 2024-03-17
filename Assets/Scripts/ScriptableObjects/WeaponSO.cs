using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MyStats/Weapon", menuName = "MyStats/Weapon", order = 10)]
public class WeaponSO : ScriptableObject
{
    [field:SerializeField, Range(0,1)] public float MinChargePercent { get; private set; }
    [field:SerializeField] public float FullChargeTime { get; private set; }
    [field: SerializeField] public bool IsFullAuto { get; private set; }
    
    [SerializeField, Tooltip("How long you can hold an arrow before it fires anyways")] private float holdTime;
    

    public WaitForSeconds HoldTime { get; private set; }

    private void OnEnable()
    {
        HoldTime = new WaitForSeconds(holdTime);
    }
}
