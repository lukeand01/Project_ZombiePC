using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability / Passive / CritChance_CritVampirism")]
public class AbilityPassiveData_CritChance_CritVampirism : AbilityPassiveData
{
    public override void Add(AbilityClass ability)
    {
        base.Add(ability);


        float firstValue = GetFirstValue(ability.stackList);

        BDClass bd = new BDClass("CritChance_CritVampirism", StatType.CritChance, firstValue, 0, 0);
        AddBDToPlayer(bd);

        if(ability.level >= 5)
        {
            PlayerHandler.instance._entityEvents.eventCrit += CritVampirism;
        }


    }
    public override void Remove(AbilityClass ability)
    {
        base.Remove(ability);

        
        RemoveBDFromPlayer("CritChance_CritVampirism");
        PlayerHandler.instance._entityEvents.eventCrit -= CritVampirism;
        
    }


    void CritVampirism()
    {
        float critChance = PlayerHandler.instance._entityStat.GetTotalValue(StatType.CritChance);

        PlayerHandler.instance._playerResources.RecoverHealth(_secondValue * critChance);

    }

    public override string GetDamageDescription(AbilityClass ability)
    {
        return base.GetDamageDescription(ability);
    }

}
