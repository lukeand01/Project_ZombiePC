using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorRunAway : Sequence2
{

    EnemyBase _enemy;
    Transform _player;

    //instead of running to the player it will always try to run away from teh player
    public BehaviorRunAway(EnemyBase _enemy)
    {
        this._enemy = _enemy;

        _player = PlayerHandler.instance.transform;
    }


    public override NodeState Evaluate()
    {
        Vector3 oppositeDir = _enemy.transform.position - _player.position;
        _enemy.SetDestinationForPathfind(oppositeDir);
        return NodeState.Running;
    }
}
