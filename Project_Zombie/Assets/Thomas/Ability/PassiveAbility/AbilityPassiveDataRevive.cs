using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability / Passive / Revive")]
public class AbilityPassiveDataRevive : AbilityPassiveData
{
    public override void Add(AbilityClass ability)
    {
        base.Add(ability);

        //just add an event
        //it will not be event. instead it wil be direct in the player.
        PlayerHandler.instance._playerResources.AddRevive();

    }
    public override void Remove(AbilityClass ability)
    {
        base.Remove(ability);
        PlayerHandler.instance._playerResources.RemoveRevive();

    }



    public override string GetDamageDescription(AbilityClass ability)
    {
        return base.GetDamageDescription(ability);
    }
}
