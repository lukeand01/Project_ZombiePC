using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Item / Gun / PermaGun / DuelistPistol")]
public class ItemGunDataDuelistPistol : ItemGunData
{
    [Separator("DUELIST PISTOl")]
    [SerializeField] float dodgeValue;
    [SerializeField] float damageBackValue;

    public override void AddGunPassives()
    {
        //we add a bd for speed and also tell the player to count dash cooldown buff.
        BDClass bdDodge = new BDClass("PermaDuelistPistol_Dodge", StatType.Dodge, dodgeValue, 0, 0);
        PlayerHandler.instance._entityStat.AddBD(bdDodge);

        BDClass bdDamageBack = new BDClass("PermaDuelistPistol_DamageBack", StatType.DamageBack, damageBackValue, 0, 0);
        PlayerHandler.instance._entityStat.AddBD(bdDamageBack);

       

    }
    public override void RemoveGunPassives()
    {
        //we just remove those things.

        PlayerHandler.instance._entityStat.RemoveBdWithID("PermaDuelistPistol_Dodge");
        PlayerHandler.instance._entityStat.RemoveBdWithID("PermaDuelistPistol_DamageBack");
    }
}
