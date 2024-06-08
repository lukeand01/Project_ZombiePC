using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade / ThickBullets")]
public class GunUpgradeData_ThickBullets : GunUpgradeData
{
    [Separator("Bullet Pen")]
    [SerializeField] int additionalPen;

    public override void AddUpgrade(GunClass _gunClass)
    {
        //add pen to the player.
        //but wont it become too broken with the other stuff?
    }

    public override void RemoveUpgrade(GunClass _gunClass)
    {
        
    }
}
