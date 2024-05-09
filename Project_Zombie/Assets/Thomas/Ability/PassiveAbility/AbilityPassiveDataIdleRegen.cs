using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability / Passive / IdleRegen")]
public class AbilityPassiveDataIdleRegen : AbilityPassiveData
{
    //when outside of combat. not taking damage for long enough.
    //regen when idle for long enough.

    [SerializeField] float healthRegenValue; //just increase the value.
    [SerializeField] float idleRegenValue;


    public override void Add(AbilityClass ability)
    {
        base.Add(ability);
    }

    public override void Remove(AbilityClass ability)
    {
        base.Remove(ability);
    }



    public override string GetDamageDescription(int level)
    {
        float value = healthRegenValue * level;
        int secondSkillModifier = MyUtils.GetSecondPassiveModifier(level);
        float secondSkillValue = secondSkillModifier * idleRegenValue;

        return $"Health Regen increased by {value} and Idle regen is increased by {secondSkillValue}";
    }
}
