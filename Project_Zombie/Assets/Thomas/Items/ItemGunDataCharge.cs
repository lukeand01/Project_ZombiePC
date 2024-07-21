using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item / Gun / ChargeGun")]
public class ItemGunDataCharge : ItemGunData
{
    //
    [Separator("CHARGE")]
    public float chargeDuration;

    public override void Shoot(GunClass gun, string ownerId, BulletScript bulletTemplate, Vector3 gunDir, List<BulletBehavior> newBulletBehaviorList)
    {
        base.Shoot(gun, ownerId, bulletTemplate, gunDir, newBulletBehaviorList);


        //instead of shooting we call it to shoot after charging is done.

    }

    public override void CallCharge(ItemClass item)
    {
        base.CallCharge(item);

        //

    }


    public override ItemGunDataCharge GetGunCharge() { return this; }

}
