using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability / Passive / DamageImmune")]
public class AbilityPassiveDataSkillCooldown : AbilityPassiveData
{


    
    
    public override void Add(AbilityClass ability)
    {
        base.Add(ability);

        float firstValue = GetFirstValue(ability.stackList);
        float secondValue = GetSecondValue(ability.stackList);

       
        //this reduces cooldown indefinetly till it is removed.
        
        BDClass bd = new BDClass("AbilitySkillCooldown", StatType.SkillCooldown, firstValue,0 ,0);       
        PlayerHandler.instance._entityStat.AddBD(bd);

        if (secondValue > 0)
        {
            //Debug.Log("supposed to add skill damage");
            BDClass bd2 = new BDClass("AbilitySkillDamage", StatType.SkillDamage, secondValue, 0, 0);
            PlayerHandler.instance._entityStat.AddBD(bd2);
        }

        
        //we need to replace this ability at playerskill

    }
    public override void Remove(AbilityClass ability)
    {

        base.Remove(ability);


        PlayerHandler.instance._entityStat.RemoveBdWithID("AbilitySkillDamage");
        
        PlayerHandler.instance._entityStat.RemoveBdWithID("AbilitySkillCooldown");

    }


    public override string GetDamageDescription(AbilityClass ability)
    {
        float firstValue = GetFirstValue(ability.stackList);
        float secondValue = GetSecondValue(ability.stackList);

        return $"Skill Cooldown Reduction is increased by {firstValue} and skill damage is increased by {secondValue}%";
    }


}
