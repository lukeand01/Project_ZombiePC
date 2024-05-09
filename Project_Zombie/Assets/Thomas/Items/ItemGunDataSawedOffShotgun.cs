using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item / Gun / PermaGun / SawedOffShotgun")]
public class ItemGunDataSawedOffShotgun : ItemGunData
{

    public override void AddGunPassives()
    {


        //every additional bullet increases the damage by 3%
        PlayerHandler.instance._playerCombat.MakeSecretStatMultipleBulletDamageModifier(0.03f);
    }
    public override void RemoveGunPassives()
    {


        PlayerHandler.instance._playerCombat.MakeSecretStatMultipleBulletDamageModifier(0);
    }


}
