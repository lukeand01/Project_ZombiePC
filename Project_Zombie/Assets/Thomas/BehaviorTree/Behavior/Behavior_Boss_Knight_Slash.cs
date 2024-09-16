using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_Boss_Knight_Slash : Sequence2
{

    EnemyBoss _boss;
    float _range;
    Transform _playerTransform;
    int _actionIndex;
    public Behavior_Boss_Knight_Slash(EnemyBoss boss, float range, int actionIndex)
    {
        _boss = boss;
        _range = range;
        _actionIndex = actionIndex;
        _playerTransform = PlayerHandler.instance.transform;
    }
    public override NodeState Evaluate()
    {
        if (_boss.actionIndex_Current != _actionIndex) return NodeState.Success;

        bool isInRange = Vector3.Distance(_boss.transform.position, _playerTransform.position) <= _range + 4;

        if (isInRange)
        {
            //call animation
            //Attack_01
            _boss.StartAction("Attack_01", 2);
            _boss.CallAnimation("Idle", 1);

            return NodeState.Running;
        }
        else
        {

        }


        return NodeState.Success;
    }
}
