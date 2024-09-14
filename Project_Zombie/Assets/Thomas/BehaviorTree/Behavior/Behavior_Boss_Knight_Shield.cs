using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_Boss_Knight_Shield : Sequence2
{
    EnemyBoss_Knight _boss;
    Transform _playerTransform;
    float _minDistanceForShield;
    public Behavior_Boss_Knight_Shield(EnemyBoss_Knight _boss, float _minDistanceForShield)
    {
        this._boss = _boss;
        this._minDistanceForShield = _minDistanceForShield;
        _playerTransform = PlayerHandler.instance.transform;
    }

    public override NodeState Evaluate()
    {
        //we check if the player is too far, so we raise the shield and 

        if (_boss.IsActing) return NodeState.Success;

        bool tooFarForShield = Vector3.Distance(_boss.transform.position, _playerTransform.position) > _minDistanceForShield ;

        if(tooFarForShield)
        {
            //raise shield
            _boss.CallAnimation("ShieldIdle_New", 2);
        }
        else
        {
            //lower shield
            _boss.CallAnimation("_Idle", 2);
        }


        return NodeState.Success;
    }


}


//animations that we want
//running 1
//death 1
//slash
//thrust
//raise sword in the air, for summoning blade.
//
