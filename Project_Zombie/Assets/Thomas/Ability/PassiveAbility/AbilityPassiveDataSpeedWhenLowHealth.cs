using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Ability / Passive / SpeedWhenLowHealth")]
public class AbilityPassiveDataSpeedWhenLowHealth : AbilityPassiveData
{
    public override void Add(AbilityClass ability)
    {
        base.Add(ability);

        //just add an event
        //it will not be event. instead it wil be direct in the player.

        //we check when it goes damage or healed. then we give a perma buff depending on the health
        //also 

        PlayerHandler.instance._entityEvents.eventDamageTaken += CheckIfLowEnough;
        PlayerHandler.instance._entityEvents.eventHealed += CheckIfLowEnough;

        CheckIfLowEnough();
    }
    public override void Remove(AbilityClass ability)
    {
        base.Remove(ability);
        PlayerHandler.instance._entityEvents.eventDamageTaken -= CheckIfLowEnough;
        PlayerHandler.instance._entityEvents.eventHealed -= CheckIfLowEnough;
        RemoveBDFromPlayer("SpeedWhenLowHealth");
    }

    void CheckIfLowEnough()
    {

        float currentHealth = PlayerHandler.instance._playerResources.GetTargetCurrentHealth();
        float totalHealth = PlayerHandler.instance._playerResources.GetTargetMaxHealth();

        //Debug.Log("check passive " + currentHealth.ToString() + " / " + totalHealth.ToString());

        if(currentHealth / totalHealth <= 0.25f)
        {
            //Debug.Log("low enough " + currentHealth / totalHealth);

            //the problem is that i 
            BDClass bd = new BDClass("SpeedWhenLowHealth", StatType.Speed, 0, _firstValue, 0);
            bd.MakeShowInUI();
            bd.MakeStack(1, false);
            AddBDToPlayer(bd);
        }
        else
        {
            //Debug.Log("not low enough");
            RemoveBDFromPlayer("SpeedWhenLowHealth");
        }

    }

    public override string GetDamageDescription(AbilityClass ability)
    {
        return base.GetDamageDescription(ability);
    }
}
