using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_Boss_Ghost_Teleport : Sequence2
{

    EnemyBoss _boss;
    int _actionIndex;

    float _cooldown_Current;
    float _cooldown_Total;

    public Behavior_Boss_Ghost_Teleport(EnemyBoss boss, float cooldown_Total,  int actionIndex)
    {
        _boss = boss;
        _actionIndex = actionIndex;

        _cooldown_Total = cooldown_Total;
        _cooldown_Current = Random.Range(_cooldown_Total * 0.4f, _cooldown_Total * 1.5f);
    }

    public override NodeState Evaluate()
    {
        //we stop.
        //we start triggeringh
        //then we disappear.
        //the ghost appears somewhere around and clsoe to the player.


        if(_cooldown_Current > 0)
        {
            _cooldown_Current -= Time.deltaTime;
            _boss.SelectRandomAction(); //we reeselect an action.
            return NodeState.Success;
        }

        if (_boss.IsActing) return NodeState.Success;


        _boss.SetActionIndexCurrent(_actionIndex);
        _boss.StartAction("Teleport", 2);
        _boss.StopAgent();
        

        _cooldown_Current = Random.Range(_cooldown_Total * 0.8f, _cooldown_Total * 1.5f);

        return NodeState.Running;
    }


}
