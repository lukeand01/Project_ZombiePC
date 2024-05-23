using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability / Passive / ReloadSpeed")]
public class AbilityPassiveDataReloadSpeed : AbilityPassiveData
{



    public override void Add(AbilityClass ability)
    {

        float firstValue = GetFirstValue(ability.stackList);
        int level = ability.level;

        BDClass reloadBD = new BDClass("ReloadPassive", StatType.ReloadSpeed, firstValue, 0, 0);
        PlayerHandler.instance._entityStat.AddBD(reloadBD);

        if(level >= 5)
        {
            //killing an enemy refunds current ammo.
            PlayerHandler.instance._entityEvents.eventKilledEnemy += RefundReload;
        }

        base.Add(ability);
    }
    public override void Remove(AbilityClass ability)
    {
        base.Remove(ability);

        PlayerHandler.instance._entityStat.RemoveBdWithID("ReloadPassive");
        PlayerHandler.instance._entityEvents.eventKilledEnemy -= RefundReload;
    }

    void RefundReload(EnemyBase enemy)
    {
        //we just tell the player to refund the ammo of the current weapon.
        //it should be based in the level. so we need to find the ability in the player and use that level.
        PlayerHandler.instance._playerCombat.RefundCurrentAmmo();

    }

    public override string GetDamageDescription(AbilityClass ability)
    {
        float firstValue = GetFirstValue(ability.stackList);
        float secondValue = GetSecondValue(ability.stackList);

        return $"Reload Speed increased by {firstValue} and each enemy killed refreshes {secondValue}% of your current weapon´s ammo";
    }

}
