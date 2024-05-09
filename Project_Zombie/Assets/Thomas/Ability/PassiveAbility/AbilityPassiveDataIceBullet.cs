using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability / Passive / SnowBullet")]
public class AbilityPassiveDataIceBullet : AbilityPassiveData
{

    //add a bullet behavior. this bullet behavior will slow based in the player abilityclass.

    [SerializeField] float slowModifier;
    [SerializeField] float stunModifier;

    public override void Add(AbilityClass ability)
    {
        base.Add(ability);
    }
    public override void Remove(AbilityClass ability)
    {
        base.Remove(ability);
    }


    public override string GetDamageDescription(int level)
    {
        float value = slowModifier * level;
        int secondSkillModifier = MyUtils.GetSecondPassiveModifier(level);
        float secondSkillValue = secondSkillModifier * stunModifier;

        return $"Slow in every shot increased by {value} and stun chance in every chance increased by {secondSkillValue}";


    }

}
