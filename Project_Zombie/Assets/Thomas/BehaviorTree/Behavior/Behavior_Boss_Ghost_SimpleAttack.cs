using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_Boss_Ghost_SimpleAttack : Sequence2
{
    EnemyBoss_Ghost _boss;
    float _range;
    Transform _playerTransform;

    float cooldown_Total;
    float cooldown_Current;

    public Behavior_Boss_Ghost_SimpleAttack(EnemyBoss_Ghost _boss, float range, float cooldown)
    {
        this._boss = _boss;
        _range = range;

        cooldown_Total = cooldown;
        cooldown_Current = 0;

        _playerTransform = PlayerHandler.instance.transform;
    }

    public override NodeState Evaluate()
    {
        //we always call it when we can. as long as the player is close enough

        if(cooldown_Current > 0)
        {
            cooldown_Current -= Time.deltaTime;
            return NodeState.Success;
        }


        bool isCloseEnough = Vector3.Distance(_boss.transform.position, _playerTransform.position) <= _range;

        if (isCloseEnough)
        {
            //then we call an attack and we set the cooldown.

            cooldown_Current = cooldown_Total;

        }


        return NodeState.Success;
    }


}
