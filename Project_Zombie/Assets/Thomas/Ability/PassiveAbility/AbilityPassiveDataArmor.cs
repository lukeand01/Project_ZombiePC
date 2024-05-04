using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability / Passive / Armor")]
public class AbilityPassiveDataArmor : AbilityPassiveData
{
    //

    [Separator("ARMOR")]
    [Range(0, 1)][SerializeField] float firstValue = 1;
    [Range(0, 1)][SerializeField] float secondValue = 1;

    public override void Add(AbilityClass ability)
    {
        base.Add(ability);


        int level = ability.level;
        float value = firstValue * level;
        int secondSkillModifier = MyUtils.GetSecondPassiveModifier(level);
        float secondSkillValue = secondSkillModifier * secondValue;


        

        BDClass armorBD = new BDClass("Armor", StatType.DamageReduction, value, 0, 0);

        if(secondSkillValue > 0)
        {
            BDClass spikeBD = new BDClass("Spike", StatType.DamageBack, secondSkillValue, 0, 0);
        }

    }
    public override void Remove(AbilityClass ability)
    {
        base.Remove(ability);

        

        EntityStat stat = PlayerHandler.instance._entityStat;

        stat.RemoveBdWithID("Armor");
        stat.RemoveBdWithID("Spike");


    }



}
