using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Ability / Active / Explosive Reload")]
public class AbilityActiveDataFullReload : AbilityActiveData
{



    public override bool Call(AbilityClass ability)
    {
        //full reload and damage around the player.

        PlayerHandler.instance._playerCombat.FullInstantReload();

        return true;
    }

}
