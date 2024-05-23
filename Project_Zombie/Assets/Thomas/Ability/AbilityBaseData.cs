using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityBaseData : ScriptableObject
{
    [Separator("ABILITY BASE")]
    public string abilityName;
    [TextArea]public string abilityDescription;
    public Sprite abilityIcon;
    public int abilityStackMax;
    public TierType abilityTier;

    public virtual AbilityActiveData GetActive() => null;
    public virtual AbilityPassiveData GetPassive() => null;

    public virtual string GetDamageDescription(AbilityClass ability)
    {
        return "Base Description. need to change it";
    }

}
