using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPassiveDataCritChance : AbilityPassiveData
{
    public override void Add(AbilityClass ability)
    {

        float firstValue = GetFirstValue(ability.stackList);
        int level = ability.level;

        BDClass critChanceBD = new BDClass("ReloadPassive", StatType.CritChance, firstValue, 0, 0);


        if(level >= 5)
        {
            PlayerHandler.instance._entityEvents.eventCrit += OnCrit;
        }

        PlayerHandler.instance._entityStat.AddBD(critChanceBD);

        base.Add(ability);
    }
    public override void Remove(AbilityClass ability)
    {
        base.Remove(ability);

        PlayerHandler.instance._entityStat.RemoveBdWithID("ReloadPassive");
        PlayerHandler.instance._entityEvents.eventCrit -= OnCrit;
    }


    void OnCrit()
    {
        BDClass smallDamageBuff = new BDClass("SmallDamageBuff", StatType.Damage, 2.5f, 0, 0);
        smallDamageBuff.MakeStack(5, false);
        PlayerHandler.instance._entityStat.AddBD(smallDamageBuff);
    }

    public override string GetDamageDescription(AbilityClass ability)
    {
        float firstValue = GetFirstValue(ability.stackList);

        return $"Crit Chance is increased by {firstValue} and after 5 stacks each crit will grant you a small damage buff that can stack up to 5";
    }
}
