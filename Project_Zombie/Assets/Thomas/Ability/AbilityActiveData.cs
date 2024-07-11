using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityActiveData : AbilityBaseData
{
    [Separator("ACTIVE")]
    public float abilityCooldown;
    public int abilityRoundCooldown;
    public float chargeDuration;

    [Separator("STORE")]
    [SerializeField] bool store;
    [field: SerializeField] public RequirementToUnluckClass requirementToUnluck { get; private set; }

    public List<string> GetStringPriceList()
    {
        List<ResourceClass> resourceList = requirementToUnluck.requiredResourceList;
        List<string> stringList = new();


        foreach (var item in resourceList)
        {
            string text = "";

            text += item.data.itemName + ": ";
            text += item.quantity.ToString();

            stringList.Add(text);
        }

        return stringList;
    }

    


    public virtual bool Call(AbilityClass ability)
    {
        return true;
    }

    public virtual void StartCharge(AbilityClass ability)
    {

    }
    public virtual void StopCharge(AbilityClass ability)
    {

    }

    public override AbilityActiveData GetActive() => this;
    
}
