using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability / Active / Invisibility")]
public class AbilityActiveDataInvisibility : AbilityActiveData
{

    public override bool Call(AbilityClass ability)
    {

        //we are simply going to add a bd.

        EntityStat stat = PlayerHandler.instance._entityStat;

        BDClass bd = new BDClass("ActiveInvisibility", BDType.Invisible, 5);
        bd.MakeShowInUI();


        stat.AddBD(bd);


        return base.Call(ability);
    }

}
