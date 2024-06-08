using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade / ReloadProtector")]
public class GunUpgradeData_ReloadProtector : GunUpgradeData
{
    //create a shield and a trigger on reload action.
    //only whne its done reloading. otherwise they can spanm that to abuse the mechanic.
    //but it doesnt make sense in relation to the idea of the ability.

    [Separator("RELOAD PROTECTOR")]
    [SerializeField] float shieldValue;
    [SerializeField] float shieldRegen;

    public override void AddUpgrade(GunClass _gunClass)
    {

        PlayerHandler.instance._playerCombat.ShieldSet(shieldValue);

        PlayerHandler.instance._entityEvents.eventReloadedGun += (data) => _gunClass.RechargeShieldAbilty(data, shieldRegen);
           
    }
    public override void RemoveUpgrade(GunClass _gunClass)
    {

        PlayerHandler.instance._playerCombat.ShieldRemove(shieldValue);
        PlayerHandler.instance._entityEvents.eventReloadedGun -= (data) => _gunClass.RechargeShieldAbilty(data, shieldRegen);
    }



    //it needs to be the same gun.
}
