using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_Boss_Ghost_BlindArea : Sequence2
{

    EnemyBoss _boss;
    int _actionIndex;

    float _cooldown_Total;
    float _cooldown_Current;


    public Behavior_Boss_Ghost_BlindArea(EnemyBoss boss, float cooldown_Total, int actionIndex)
    {
        _boss = boss;
        _actionIndex = actionIndex;

        _cooldown_Total = cooldown_Total;
        _cooldown_Current = cooldown_Total;
    }


    public override NodeState Evaluate()
    {
        //this doesnt make the ghost stop. it simply throws a thing off
        //it has a cooldown assigned to it.
        if (_cooldown_Current > 0)
        {
            _cooldown_Current -= Time.deltaTime;
            return NodeState.Success;
        }

        if (_boss.IsActing) return NodeState.Success;


        //then here we choose a random target in an area and call a damage area for this place. 
        //but this damage area gives slow, blind and damage.

       AreaDamage _areaDamage =  GameHandler.instance._pool.GetAreaDamage(_boss.transform);


        

        //the target will be a random area in the radius around the player
        //also the target area must be ground.


        return NodeState.Success;
    }

}
