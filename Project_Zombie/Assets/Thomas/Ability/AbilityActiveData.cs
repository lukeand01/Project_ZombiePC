using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityActiveData : AbilityBaseData
{
    [Separator("ACTIVE")]
    public float abilityCooldown;

    public virtual bool Call(AbilityClass ability)
    {
        return true;
    }

    public override AbilityActiveData GetActive() => this;
    
}
