using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_Tree_Seed : Sequence2
{
    EnemyBoss_Tree _boss;
    float _cooldown_Total;
    float _cooldown_Current;

    public Behavior_Tree_Seed(EnemyBoss_Tree boss, float cooldown_Total)
    {
        _boss = boss;
        _cooldown_Total = cooldown_Total;
        _cooldown_Current = 0;
        //StartCooldown();
    }

    public override NodeState Evaluate()
    {


        if (_boss.IsActing) return NodeState.Success;

        if (_cooldown_Current > 0)
        {
            _cooldown_Current -= Time.deltaTime;
            return NodeState.Success;
        }

        _boss.ShootSeed();
        StartCooldown();

        return NodeState.Success;
    }

    void StartCooldown()
    {
        _cooldown_Current = Random.Range(_cooldown_Total * 0.7f, _cooldown_Total * 1.3f);
    }
}
