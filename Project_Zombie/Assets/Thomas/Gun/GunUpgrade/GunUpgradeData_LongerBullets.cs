using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade / LongerBullets")]
public class GunUpgradeData_LongerBullets : GunUpgradeData
{
    [Separator("LONGER BULLETS")]
    [SerializeField] int goThroughPower;
    public override void AddUpgrade(GunClass _gunClass)
    {
        _gunClass.GoThroughPower_Individual_Set(goThroughPower);
    }

    public override void RemoveUpgrade(GunClass _gunClass)
    {
        _gunClass.GoThroughPower_Individual_Set(-goThroughPower);
    }
}
