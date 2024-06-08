using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Power / ReplenishAmmo")]
public class PowerData_ReplenishAmmo : PowerData
{
    public override void ActivatePower()
    {
        PlayerHandler.instance._playerCombat.RefreshAllReserveAmmo();
    }
}
