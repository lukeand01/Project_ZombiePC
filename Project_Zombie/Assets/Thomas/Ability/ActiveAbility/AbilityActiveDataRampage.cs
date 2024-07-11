using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability / Active / Rampage")]
public class AbilityActiveDataRampage : AbilityActiveData
{

    public override bool Call(AbilityClass ability)
    {
        //put a value_Level here for a short duration.

        BDClass bd = new BDClass("Rampage", BDType.SecretBulletMultipler, 6);
        PlayerHandler.instance._entityStat.AddBD(bd);
        return true;
    }

}
