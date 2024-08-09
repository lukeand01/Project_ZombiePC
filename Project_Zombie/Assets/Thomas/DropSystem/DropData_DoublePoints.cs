using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Drop / DoublePoints")]
public class DropData_DoublePoints : DropData
{
    [Separator("Points")]
    [SerializeField] float pointModifier;
    [SerializeField] float timer ;



    public override void CallDrop()
    {
        base.CallDrop();


        BDClass bd = new BDClass("Drop_DoublePoints", EspecialConditionType.PointsModifier, pointModifier);
        bd.MakeTemp(timer);
        bd.MakeShowInUI();
        PlayerHandler.instance._entityStat.AddBD(bd);




    }
    //assign an event here would be easier.

    void IncreasePoints()
    {

    }

    public override void RemoveDrop()
    {
        base.RemoveDrop();
    }
}
