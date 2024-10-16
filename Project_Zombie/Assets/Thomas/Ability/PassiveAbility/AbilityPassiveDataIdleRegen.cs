using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability / Passive / IdleRegen")]
public class AbilityPassiveDataIdleRegen : AbilityPassiveData
{
    //when outside of combat. not taking damage for long enough.
    //regen when idle for long enough.




    public override void Add(AbilityClass ability)
    {
        base.Add(ability);
    }

    public override void Remove(AbilityClass ability)
    {
        base.Remove(ability);
    }



    public override string GetDamageDescription(AbilityClass ability)
    {
        float firstValue = GetFirstValue(ability.stackList);
        float secondValue = GetSecondValue(ability.stackList);

        return $"Health Regen increased by {firstValue} and Idle regen is increased by {secondValue}";
    }
}
