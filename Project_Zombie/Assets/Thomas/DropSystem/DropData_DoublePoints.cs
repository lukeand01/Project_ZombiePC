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
        //create a bd that does that.

        //i want a bd that can call an event.

        //we just add a bd

        BDClass bd = new BDClass("Drop_DoublePoints", EspecialConditionType.PointsModifier, pointModifier);
        bd.MakeTemp(timer);
        PlayerHandler.instance._entityStat.AddBD(bd);

    }
}
