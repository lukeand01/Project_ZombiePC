using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability / Passive / Luck")]
public class AbilityPassiveDataLuck : AbilityPassiveData
{
    public override void Add(AbilityClass ability)
    {
        base.Add(ability);
        BDClass bd = new BDClass("Luck", StatType.Luck, ability.level * _firstValue, 0, 0);
        AddBDToPlayer(bd);

        if (ability.level >= 3)
        {
            //then we assign the event of opening a chest.
            PlayerHandler.instance._entityEvents.eventOpenChest += OpenChest;
        }

        
    }
    public override void Remove(AbilityClass ability)
    {
        base.Remove(ability);


        RemoveBDFromPlayer("Luck");
        PlayerHandler.instance._entityEvents.eventOpenChest -= OpenChest;
    }

    void OpenChest()
    {
        //we just give the player something additiioanl.
        Debug.Log("gained the additional resource");
    }

    public override string GetDamageDescription(AbilityClass ability)
    {
        return base.GetDamageDescription(ability);
    }
}
