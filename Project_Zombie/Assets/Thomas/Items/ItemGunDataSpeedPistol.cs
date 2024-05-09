using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item / Gun / PermaGun / SpeedPistol")]
public class ItemGunDataSpeedPistol : ItemGunData
{

    public override void AddGunPassives()
    {
        //we add a bd for speed and also tell the player to count dash cooldown buff.

        Debug.Log("add speed");

        BDClass bd = new BDClass("PermaSpeedPistol", StatType.Speed, 0, 0.3f, 0);
        PlayerHandler.instance._entityStat.AddBD(bd);

        PlayerHandler.instance._playerMovement.AddDashCooldownReduction();

    }
    public override void RemoveGunPassives()
    {
        //we just remove those things.
        Debug.Log("remove speed");
        PlayerHandler.instance._entityStat.RemoveBdWithID("PermaSpeedPistol");

        PlayerHandler.instance._playerMovement.RemoveDashCooldownReduction();
    }

}
