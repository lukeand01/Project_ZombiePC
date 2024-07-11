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

    [Separator("ONLY TEMP GUNS")]
    [SerializeField] bool hasBeenFound;
    [SerializeField] int additionalRollRequired;

    public bool HasBeenFound { get { return hasBeenFound; } }

    public int GetAdditionalRollRequired {  get { return additionalRollRequired; } }

    public void SetFound(bool hasBeenFound)
    {
        this.hasBeenFound = hasBeenFound;
    }

    public int storeIndex { get; private set; }

    public void SetIndex(int index)
    {
        storeIndex = index;
    }


    public virtual AbilityActiveData GetActive() => null;
    public virtual AbilityPassiveData GetPassive() => null;

    public virtual string GetDamageDescription(AbilityClass ability)
    {
        return "Base Description. need to change it";
    }

}
