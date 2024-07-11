
using MyBox;
using System.Diagnostics;
using UnityEngine;

[CreateAssetMenu(menuName = "Drop / GunBoxPrice")]
public class DropData_GunBoxPrice : DropData
{
    [Separator("GunBox")]
    [SerializeField] float priceModifier;
    [SerializeField] float timer;
    public override void CallDrop()
    {
        BDClass bd = new BDClass("Drop_GunBox", EspecialConditionType.GunBoxPriceModifier, priceModifier);
        bd.MakeTemp(timer);
        PlayerHandler.instance._entityStat.AddBD(bd);
    }
}
