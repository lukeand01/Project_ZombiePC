using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_Bomber : Sequence2
{

    EnemyBomber _enemy;
    public Behavior_Bomber(EnemyBomber enemy)
    {
        _enemy  = enemy;

    }

    public override NodeState Evaluate()
    {

        _enemy._entityAnimation.CallAnimation_Run(0);
        _enemy._entityAnimation.CallAnimation_Run(2);

        if (_enemy.isExploding) return NodeState.Success;

        _enemy.CallAttack();
        _enemy.SetIsAttacking_Animation(true);
        

        return NodeState.Success;
    }



}
