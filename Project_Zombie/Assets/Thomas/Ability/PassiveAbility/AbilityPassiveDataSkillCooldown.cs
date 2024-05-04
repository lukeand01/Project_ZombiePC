using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability / Passive / DamageImmune")]
public class AbilityPassiveDataSkillCooldown : AbilityPassiveData
{
    [Separator("SKILL COOLDOWN")]
    [Range(0,1)][SerializeField] float firstValue = 1;
    [Range(0,1)][SerializeField] float secondValue = 1;

    
    
    public override void Add(AbilityClass ability)
    {
        base.Add(ability);

        int level = ability.level;
        float value = firstValue * level;
        int secondSkillModifier = MyUtils.GetSecondPassiveModifier(level);
        float secondSkillValue = secondSkillModifier * secondValue;

       
        //this reduces cooldown indefinetly till it is removed.
        
        BDClass bd = new BDClass("AbilitySkillCooldown", StatType.SkillCooldown, value,0 ,0);       
        PlayerHandler.instance._entityStat.AddBD(bd);

        if (secondSkillModifier > 0)
        {
            //Debug.Log("supposed to add skill damage");
            BDClass bd2 = new BDClass("AbilitySkillDamage", StatType.SkillDamage, secondSkillValue, 0, 0);
            PlayerHandler.instance._entityStat.AddBD(bd2);
        }

        //we need to replace this ability at playerskill

    }
    public override void Remove(AbilityClass ability)
    {

        base.Remove(ability);
        int secondSkillModifier = MyUtils.GetSecondPassiveModifier(ability.level);
        if(secondSkillModifier > 0)
        {
            PlayerHandler.instance._entityStat.RemoveBdWithID("AbilitySkillDamage");
        }
        PlayerHandler.instance._entityStat.RemoveBdWithID("AbilitySkillCooldown");

    }



}
