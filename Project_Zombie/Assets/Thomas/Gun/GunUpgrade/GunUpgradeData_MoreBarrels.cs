using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade / More Barrels")]
public class GunUpgradeData_MoreBarrels : GunUpgradeData
{
    [Separator("MORE BARRELS")]
    [SerializeField] int additionalBullets;
    public override void AddUpgrade(GunClass _gunClass)
    {
        _gunClass.IncreaseBulletPerShot(additionalBullets);
    }

    public override void RemoveUpgrade(GunClass _gunClass)
    {
        _gunClass.DecreaseBulletPerShot(additionalBullets);
    }
}
