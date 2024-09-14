using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_Boss_CheckAction : Sequence2
{
    EnemyBoss _boss;


    public Behavior_Boss_CheckAction(EnemyBoss _boss)
    {
        this._boss = _boss;  
    }

    public override NodeState Evaluate()
    {

        if (_boss.actionIndex_Current != -1 && !_boss.ShouldChangeAction()) return NodeState.Success;

        _boss.SelectRandomAction();

        return NodeState.Success;

    }

    //
    //


    //we will check each till we found someone that we should be checking actually.
    //


    //check shield => player is far and is shooting 
    //check slash => player is close. we play x animations?
    //check lance => player is less far and we roll for it. 
    //check sword => we must roll for it. and also it enters in cooldown.

}
