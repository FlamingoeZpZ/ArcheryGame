using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticUtilities
{
    public static readonly int ChargePercentID = Animator.StringToHash("ChargePercent");
    public static readonly int IsFiringID = Animator.StringToHash("IsFiring");
    public static readonly int ReloadID = Animator.StringToHash("Reload");
    
    public static readonly int PoundID = Animator.StringToHash("Pound");
    public static readonly int PunchID = Animator.StringToHash("Punch");
    public static readonly int HitSmallID = Animator.StringToHash("HitSmall");
    public static readonly int HitBigID = Animator.StringToHash("HitBig");
    public static readonly int IsIdleID = Animator.StringToHash("IsIdle");

    public static readonly int PlayerLayer = 1 << LayerMask.NameToLayer("Player");

}
