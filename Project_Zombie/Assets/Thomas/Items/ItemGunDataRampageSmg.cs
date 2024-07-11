using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item / Gun / PermaGun / RampageSmg")]
public class ItemGunDataRampageSmg : ItemGunData
{

    public override void AddGunPassives()
    {
        //we are going to add to the secret passive. every gun shoot one additional bullet.

        PlayerHandler.instance._playerCombat.MakeSecretStatMultipleBulletFlat(1); //we are going to increase by one this value_Level.

    }
    public override void RemoveGunPassives()
    {
        PlayerHandler.instance._playerCombat.MakeSecretStatMultipleBulletFlat(0);
    }


}
