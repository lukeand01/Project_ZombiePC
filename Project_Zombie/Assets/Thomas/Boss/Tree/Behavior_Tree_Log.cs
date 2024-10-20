using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_Tree_Log : Sequence2
{

    EnemyBoss_Tree _boss;

    float _cooldown_Total;
    float _cooldown_Current;

    public Behavior_Tree_Log(EnemyBoss_Tree boss, float cooldown_Total) 
    { 
        _boss = boss;
        _cooldown_Total = cooldown_Total;
        
    }

    public override NodeState Evaluate()
    {
        if (_boss.IsActing) return NodeState.Success;
        if(_cooldown_Current > 0)
        {
            _cooldown_Current -= Time.deltaTime;
            return NodeState.Success;
        }

        _boss.CallTreeLog();
        _cooldown_Current = Random.Range(_cooldown_Current * 0.7f, _cooldown_Current * 1.2f);

        return NodeState.Success;
    }

}
