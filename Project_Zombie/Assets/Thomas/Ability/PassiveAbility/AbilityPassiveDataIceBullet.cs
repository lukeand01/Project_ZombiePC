using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability / Passive / SnowBullet")]
public class AbilityPassiveDataIceBullet : AbilityPassiveData
{

    //add a bullet behavior. this bullet behavior will slow based in the player abilityclass.
    [Separator("ICE BULLET")]
    [SerializeField] BulletBehavior bulletBehavior_Slow_1;
    [SerializeField] BulletBehavior bulletBehavior_Slow_2;
    [SerializeField] BulletBehavior bulletBehavior_Slow_3;
    [SerializeField] BulletBehavior bulletBehavior_Stun;
    
    //each thing increase the slow.

    public override void Add(AbilityClass ability)
    {
        base.Add(ability);
        PlayerCombat _combat = PlayerHandler.instance._playerCombat;


        if(ability.level == 1)
        {
            _combat.AddForcedBulletBehavior(bulletBehavior_Slow_1);
        }
        if(ability.level == 2)
        {
            _combat.AddForcedBulletBehavior(bulletBehavior_Slow_2);
        }
        if(ability.level == 3)
        {
            _combat.AddForcedBulletBehavior(bulletBehavior_Slow_3);
            _combat.AddForcedBulletBehavior(bulletBehavior_Stun);
        }


    }
    public override void Remove(AbilityClass ability)
    {
        base.Remove(ability);

        PlayerCombat _combat = PlayerHandler.instance._playerCombat;

        _combat.RemoveForcedBulletBehavior(bulletBehavior_Slow_1);
        _combat.RemoveForcedBulletBehavior(bulletBehavior_Stun);
    }


    public override string GetDamageDescription(AbilityClass ability)
    {
        float firstValue = GetFirstValue(ability.stackList);
        float secondValue = GetSecondValue(ability.stackList);

        return $"Slow in every shot increased by {firstValue} and stun chance in every chance increased by {secondValue}";


    }

}
