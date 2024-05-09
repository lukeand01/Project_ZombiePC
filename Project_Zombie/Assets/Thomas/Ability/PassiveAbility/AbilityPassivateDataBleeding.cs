using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability / Passive / Bleeding")]
public class AbilityPassivateDataBleeding : AbilityPassiveData
{

    //we need an event when you deal damage to people.
    //this has only one stack.
    [SerializeField] float bleedChanceIncrement;

    public override void Add(AbilityClass ability)
    {
        base.Add(ability);
        PlayerHandler.instance._entityEvents.eventDamagedEntity += ApplyBleeding;
    }

    public override void Remove(AbilityClass ability)
    {
        base.Remove(ability);
        PlayerHandler.instance._entityEvents.eventDamagedEntity -= ApplyBleeding;
    }

    void ApplyBleeding(IDamageable damageable)
    {
        //we apply a bleeding. and this bleeding we calculate the intensite based in the player bleedingskill.


    }

    public override string GetDamageDescription(int level)
    {
        return $"Bleed chance increased by {bleedChanceIncrement * level}";

    }

}
