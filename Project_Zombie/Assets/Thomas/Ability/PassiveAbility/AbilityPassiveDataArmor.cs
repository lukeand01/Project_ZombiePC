using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability / Passive / Armor")]
public class AbilityPassiveDataArmor : AbilityPassiveData
{
    //we create a list of all fellas that we add in the abilityclass.
    //then we get the value by multiplying the value 

    //


    public override void Add(AbilityClass ability)
    {
        base.Add(ability);

        float firstValue = GetFirstValue(ability.stackList);

        float secondValue = GetSecondValue(ability.stackList);


       
        BDClass armorBD = new BDClass("Armor", StatType.DamageReduction, firstValue, 0, 0);
        PlayerHandler.instance._entityStat.AddBD(armorBD);

        if(secondValue > 0)
        {
            BDClass spikeBD = new BDClass("Spike", StatType.DamageBack, secondValue, 0, 0);
            PlayerHandler.instance._entityStat.AddBD(spikeBD);
        }

        

    }
    public override void Remove(AbilityClass ability)
    {
        base.Remove(ability);

        
        EntityStat stat = PlayerHandler.instance._entityStat;

        stat.RemoveBdWithID("Armor");
        stat.RemoveBdWithID("Spike");

    }


    public override string GetDamageDescription(AbilityClass ability)
    {

        float firstValue = GetFirstValue(ability.stackList);
        float secondValue = GetSecondValue(ability.stackList);


        return $"Protection increased by {firstValue} and damageback increased by {secondValue}";

    }


}
