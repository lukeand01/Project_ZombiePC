using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability / Passive / CritDamage_HealthDamage")]
public class AbilityPassiveData_CritDamage_HealthDamage : AbilityPassiveData
{
    public override void Add(AbilityClass ability)
    {
        base.Add(ability);


        float firstValue = GetFirstValue(ability.stackList);

        BDClass bd = new BDClass("CritDamage_HealthDamage", StatType.CritDamage, firstValue, 0, 0);
        AddBDToPlayer(bd);

        if (ability.level >= 5)
        {
            PlayerHandler.instance._entityEvents.eventDamagedEntity += DealAdditionalDamage;
        }


    }
    public override void Remove(AbilityClass ability)
    {
        base.Remove(ability);


        RemoveBDFromPlayer("CritDamage_HealthDamage");
        PlayerHandler.instance._entityEvents.eventDamagedEntity -= DealAdditionalDamage;

    }


    void DealAdditionalDamage(IDamageable damageable, DamageClass damageClassBeingUsed)
    {
        //need to check how much health it has in percent.
        //then we need to up the damage 

        float currentHealth = damageable.GetTargetCurrentHealth();
        float totalHealth = damageable.GetTargetMaxHealth();
        bool hasEnoughHealth = currentHealth / totalHealth > 0.8f;

        if (hasEnoughHealth)
        {
            //WONT BE USING THIS PASSIVE ANYMORE
            //damageClassBeingUsed.MakePureDamageModifier(_secondValue);
        }
        

    }

    public override string GetDamageDescription(AbilityClass ability)
    {
        return base.GetDamageDescription(ability);
    }
}
