using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability / Passive / SnowBullet")]
public class AbilityPassiveDataIceBullet : AbilityPassiveData
{

    //add a bullet behavior. this bullet behavior will slow based in the player abilityclass.

    

    public override void Add(AbilityClass ability)
    {
        base.Add(ability);
    }
    public override void Remove(AbilityClass ability)
    {
        base.Remove(ability);
    }



}
