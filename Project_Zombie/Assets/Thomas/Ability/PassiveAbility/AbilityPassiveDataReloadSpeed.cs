using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability / Passive / ReloadSpeed")]
public class AbilityPassiveDataReloadSpeed : AbilityPassiveData
{

    [SerializeField] float reloadSpeedModifier;
    [SerializeField] float reloadRefreshModifier;

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

    public override string GetDamageDescription(int level)
    {
        float value = reloadSpeedModifier * level;
        int secondSkillModifier = MyUtils.GetSecondPassiveModifier(level);
        float secondSkillValue = secondSkillModifier * reloadRefreshModifier;

        return $"Reload Speed increased by {value} and each enemy killed refreshes {secondSkillValue}% of your current weapon´s ammo";
    }

}
