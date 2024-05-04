using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability / Passive / ReloadSpeed")]
public class AbilityPassiveDataReloadSpeed : AbilityPassiveData
{

    public override void Add(AbilityClass ability)
    {
        base.Add(ability);
    }
    public override void Remove(AbilityClass ability)
    {
        base.Remove(ability);
    }

    void RefundReload()
    {
        //we just tell the player to refund the ammo of the current weapon.
        //it should be based in the level. so we need to find the ability in the player and use that level.


    }

}
