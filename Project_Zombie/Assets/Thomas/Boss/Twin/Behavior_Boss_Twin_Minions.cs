using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_Boss_Twin_Minions : Sequence2
{
    EnemyBoss_Twin _boss;

    float _cooldown_Total;
    float _cooldown_Current;

    EnemyBoss_Twin_Small[] _minionList;

    public Behavior_Boss_Twin_Minions(EnemyBoss_Twin boss, float cooldown_Total, EnemyBoss_Twin_Small[] minionList)
    {
        _boss = boss;
        _cooldown_Total = cooldown_Total;
        _minionList = minionList;
    }

    public override NodeState Evaluate()
    {


        for (int i = 0; i < _boss._phaseLevel; i++)
        {
            if (_minionList[i] != null) continue;
            if (!_minionList[i].gameObject.activeInHierarchy)
            {
                _minionList[i].SetTwinSmall(_boss);
            }
        }


        
        return NodeState.Success;
    }

    void SetOnCooldown()
    {
        _cooldown_Current = Random.Range(_cooldown_Total * 0.7f, _cooldown_Total * 1.2f);
    }


}
