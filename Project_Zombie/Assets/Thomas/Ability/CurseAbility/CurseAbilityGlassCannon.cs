using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;


[CreateAssetMenu(menuName = "Ability / Curse / GlassCannon")]
public class CurseAbilityGlassCannon : AbilityPassiveData
{

    //take more damage, but lose 20% current health.
    //deal 30% more damage, and take 30% more damage.
 

    public override void Add(AbilityClass ability)
    {
        base.Add(ability);
      
        PlayerHandler.instance._entityEvents.eventDelegate_DealDamageToEntity += ModifierDamageDealt;
        PlayerHandler.instance._entityEvents.eventDelegate_DamageTaken += ModifierDamageTaken;

        Debug.Log("added");

    }
    public override void Remove(AbilityClass ability)
    {
        base.Remove(ability);
        PlayerHandler.instance._entityEvents.eventDelegate_DealDamageToEntity -= ModifierDamageDealt;
        PlayerHandler.instance._entityEvents.eventDelegate_DamageTaken -= ModifierDamageTaken;
    }

    void ModifierDamageTaken(ref DamageClass damage)
    {
        damage.Make_TotalDamageModifier(damage.totalDamageModifier * 1.3f);
    }
    void ModifierDamageDealt(ref DamageClass damage)
    {
        damage.Make_TotalDamageModifier(damage.totalDamageModifier * 1.3f);
    }


    public override bool IsCursed()
    {
        return true;
    }
}


//this increases damage by 20% and add a debuff to the player.

//but how will damage work in this game?
//just as a _value. percent increase.

//lets create at least 5 curse abilities:
//glasscannon, more damage but reduce max health
//cannot have shield but 
//always crit but the enemies also always crit.
//each turn spend a portion 
//increase the duration of allies by 50% 
//doubles the chance of dodging, but takes more damage.
//heals after every turn, but all other forms of healing are reduced by 50%


//The Selfish God of healing

//heals after every turn, but all others forms of healing by 50%
//Feeble but agile, AddIngredient chance of dodging by 50% but every _enemy that actually hits teh player has a small chance of stunnng the player
//increase the duration of allies by 50% but 
//Critical Situation, always crit but the enemies also always crit. critchance is transformed in critdamage.
//


//Always crits, but cannot dash anymore
//Amon´s Burden, 
//Enemies always crit.
//All pointprices are increased but gain armory
//all pointprices are reduced but 
//
//Gain a lot of money, but every point you make go towards paying it with interest.
//