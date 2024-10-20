using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_Boss_Devil_HellSummon : Sequence2
{
    

    EnemyBoss_Devil _boss;

    float _cooldown_Total;
    float _cooldown_Current;

    public Behavior_Boss_Devil_HellSummon(EnemyBoss_Devil boss)
    {
        _boss = boss;

        _cooldown_Current = 0;
        _cooldown_Total = 10;

    }

    public override NodeState Evaluate()
    {
        if (_boss.currentPhase <= 2) return NodeState.Success;
        if(_boss.IsActing) return NodeState.Success;

        if(_cooldown_Current > 0)
        {
            _cooldown_Current -= Time.deltaTime;
            return NodeState.Success;
        }

        _boss.CallHellSummon();
        _cooldown_Current = _cooldown_Total;

        return NodeState.Success;
    }

}
