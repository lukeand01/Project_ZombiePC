using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Ability / Passive / HealthAndShield")]
public class AbilityPassiveData_MaxHealth_Shield : AbilityPassiveData
{
    //the 


    public override void Add(AbilityClass ability)
    {
        base.Add(ability);


        float firstValue = GetFirstValue(ability.stackList);



        BDClass bd = new BDClass("Health_Shield", StatType.Health, 0, firstValue, 0);
        PlayerHandler.instance._entityStat.AddBD(bd);

        if(ability.level >= 3)
        {
            float shieldValue = _secondValue; //i plan on changing this later.
            PlayerHandler.instance._playerCombat.ShieldSet(shieldValue);

        }


    }
    public override void Remove(AbilityClass ability)
    {
        base.Remove(ability);
        PlayerHandler.instance._entityStat.RemoveBdWithID("Health_Shield");
        PlayerHandler.instance._playerCombat.ShieldRemove();


    }

    public override string GetDamageDescription(AbilityClass ability)
    {
        float firstValue = GetFirstValue(ability.stackList);
        float secondValue = GetSecondValue(ability.stackList);

        return $"Increase Max health by {firstValue}% and at 3 stacks you gain a shield based ";
    }
}
