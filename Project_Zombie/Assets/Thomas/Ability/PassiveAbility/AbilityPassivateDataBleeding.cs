using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability / Passive / Bleeding")]
public class AbilityPassivateDataBleeding : AbilityPassiveData
{

    //we need an event when you deal damage to people.
    //this has only one stack.


    public override void Add(AbilityClass ability)
    {
        base.Add(ability);

        PlayerHandler.instance._entityEvents.eventDamagedEntity += ApplyBleeding;

        float firstValue = GetFirstValue(ability.stackList);
        float secondValue = GetSecondValue(ability.stackList);


        BDClass bleedChanceBD = new BDClass("BleedChance", StatType.ElementalChance, firstValue, 0, 0);
        PlayerHandler.instance._entityStat.AddBD(bleedChanceBD);

        if(secondValue > 0)
        {
            BDClass bleedPowerBD = new BDClass("BleedStrenght", StatType.ElementalPower, secondValue, 0, 0);
            PlayerHandler.instance._entityStat.AddBD(bleedPowerBD);
        }

        
    }

    public override void Remove(AbilityClass ability)
    {
        base.Remove(ability);
        PlayerHandler.instance._entityEvents.eventDamagedEntity -= ApplyBleeding;

        PlayerHandler.instance._entityStat.RemoveBdWithID("BleedChance");
        PlayerHandler.instance._entityStat.RemoveBdWithID("BleedStrenght");
    }

    void ApplyBleeding(IDamageable damageable, DamageClass damageUsed)
    {
        //we apply a bleeding. and this bleeding we calculate the intensite based in the player bleedingskill.
        float bleedChance = 15 + PlayerHandler.instance._entityStat.GetTotalValue(StatType.ElementalChance);

        int roll = Random.Range(0, 101);

        Debug.Log("called this");

        if(bleedChance > roll)
        {
            float damageModifier = PlayerHandler.instance._entityStat.GetTotalValue(StatType.ElementalPower);

            BDClass bleedBD = new BDClass("BleedOnHit", BDDamageType.Bleed, damageable, damageModifier, 5, 2.5f);
            damageable.ApplyBD(bleedBD);
        }
        //i dont need to do this actually. i need to do it through the bullet.
    }

    

    public override string GetDamageDescription(AbilityClass ability)
    {
        float firstValue = GetFirstValue(ability.stackList);
        float secondValue = GetSecondValue(ability.stackList);

        return $"every hit has chance to cause bleeding with a base of 15% chance. Elemental chance increased by {firstValue} and elemental damage by {secondValue}";
    }

}
