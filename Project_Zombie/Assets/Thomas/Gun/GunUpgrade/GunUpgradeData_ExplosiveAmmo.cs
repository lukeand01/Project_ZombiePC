using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade / ExplosiveAmmo")]
public class GunUpgradeData_ExplosiveAmmo : GunUpgradeData
{
    [Separator("EXPLSIVE")]
    [SerializeField] BulletBehavior explosiveBehavior;
    public override void AddUpgrade(GunClass _gunClass)
    {

        //add a bullet behavior to the target.
        _gunClass.AddBulletBehavior(explosiveBehavior);
    }

    public override void RemoveUpgrade(GunClass _gunClass)
    {

        _gunClass.RemoveBulletBehavior(explosiveBehavior);
    }

}
