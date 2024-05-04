using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability / Passive / IdleRegen")]
public class AbilityPassiveDataIdleRegen : AbilityPassiveData
{
    //when outside of combat. not taking damage for long enough.
    //regen when idle for long enough.



    public override void Add(AbilityClass ability)
    {
        base.Add(ability);
    }

    public override void Remove(AbilityClass ability)
    {
        base.Remove(ability);
    }



}
