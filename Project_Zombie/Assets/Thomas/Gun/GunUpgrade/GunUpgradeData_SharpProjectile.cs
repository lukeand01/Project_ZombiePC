using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade / SharpProjectile")]
public class GunUpgradeData_SharpProjectile : GunUpgradeData
{
    [Separator("SHARP PROJECTILE")]
    [SerializeField] float penValue;

    public override void AddUpgrade(GunClass _gunClass)
    {
        BDClass bd = new BDClass("SharpProjectile", StatType.Pen, 0.25f, 0, 0);
        _gunClass.Gun_AddBD(bd);
    }

    public override void RemoveUpgrade(GunClass _gunClass)
    {
        _gunClass.Gun_RemoveBD("SharpProjectile");
    }
}
