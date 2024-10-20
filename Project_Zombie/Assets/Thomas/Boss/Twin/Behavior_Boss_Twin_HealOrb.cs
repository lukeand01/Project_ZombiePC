using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_Boss_Twin_HealOrb : Sequence2
{
    EnemyBoss_Twin _boss;

    float _cooldown_Total;
    float _cooldown_Current;

    public Behavior_Boss_Twin_HealOrb(EnemyBoss_Twin boss, float cooldown_Total)
    {
        _boss = boss;
        _cooldown_Total = cooldown_Total;
    }

    public override NodeState Evaluate()
    {
        if (_boss.IsActing) return NodeState.Success;
        if (_cooldown_Current > 0)
        {
            _cooldown_Current -= Time.deltaTime;
            return NodeState.Success;
        }

        _boss.CallHealOrb();
        SetOnCooldown();
        return NodeState.Success;
    }

    void SetOnCooldown()
    {
        _cooldown_Current = Random.Range(_cooldown_Total * 0.7f, _cooldown_Total * 1.2f);
    }
}
