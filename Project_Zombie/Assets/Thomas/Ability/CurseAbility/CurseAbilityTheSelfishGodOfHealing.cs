
using UnityEngine;

[CreateAssetMenu(menuName = "Ability / Curse / TheSelfishGodOfHealing")]
public class CurseAbilityTheSelfishGodOfHealing : AbilityPassiveData
{

    public override void Add(AbilityClass ability)
    {
        base.Add(ability);
        PlayerHandler.instance._entityEvents.eventPassedRound += HealEachRound;
        PlayerHandler.instance._entityEvents.eventDelegate_Healed += ModifyHealing;

    }

    public override void Remove(AbilityClass ability)
    {
        base.Remove(ability);



        PlayerHandler.instance._entityEvents.eventPassedRound -= HealEachRound;
        PlayerHandler.instance._entityEvents.eventDelegate_Healed -= ModifyHealing;

    }


    void HealEachRound()
    {
        //we heal a percent player
        PlayerHandler.instance._playerResources.RestoreHealthBasedInPercent(0.2f);
    }

    void ModifyHealing(ref float healing)
    {
        healing *= 0.5f;
    }

    public override bool IsCursed()
    {
        return true;
    }


}
