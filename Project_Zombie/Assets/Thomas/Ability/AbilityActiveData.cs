using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityActiveData : AbilityBaseData
{
    [Separator("ACTIVE")]
    public float abilityCooldown;

    public virtual void Call(AbilityClass ability)
    {

    }

    public override AbilityActiveData GetActive() => this;
    
}
