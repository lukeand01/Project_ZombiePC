using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Upgrade / BloodConsumer")]
public class GunUpgradeData_BloodConsumer : GunUpgradeData
{
    [Separator("BLOOD CONSUMER")]
    [SerializeField] float vampirismValue;

    public override void AddUpgrade(GunClass _gunClass)
    {
        BDClass bd = new BDClass("BloodConsumer", StatType.Vampirism, vampirismValue, 0, 0);
        _gunClass.Gun_AddBD(bd);
    }

    public override void RemoveUpgrade(GunClass _gunClass)
    {
        _gunClass.Gun_RemoveBD("BloodConsumer");
    }
}
