using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability / Passive / Dodge")]
public class AbilityPassiveDataDodge : AbilityPassiveData
{

    public override void Add(AbilityClass ability)
    {
        base.Add(ability);



        float firstValue = GetFirstValue(ability.stackList);

        BDClass bd = new BDClass("DodgePassive", StatType.Dodge, firstValue, 0, 0);
        PlayerHandler.instance._entityStat.AddBD(bd);


        if(ability.level >= 5)
        {
            PlayerHandler.instance._entityEvents.eventHasDodged += OnSuccessfulDodge;
        }

    }

    public override void Remove(AbilityClass ability)
    {
        base.Remove(ability);
        PlayerHandler.instance._entityStat.RemoveBdWithID("DodgePassive");
        PlayerHandler.instance._entityEvents.eventHasDodged -= OnSuccessfulDodge;
    }

    
    void OnSuccessfulDodge()
    {
        PlayerHandler.instance._playerResources.RestoreHealth(_secondValue);
    }


    public override string GetDamageDescription(AbilityClass ability)
    {
        float firstValue = GetFirstValue(ability.stackList);
        

        return $"Gain {firstValue}% Dodge chance. after 5 stacks every dodge recovers the player´s health by {_secondValue}";
    }

}
