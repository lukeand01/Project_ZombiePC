using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_Boss_Knight_Thrust : Sequence2
{
    EnemyBoss_Knight _boss;
    float _range;
    Transform _playerTransform;
    int _actionIndex;
    bool isAlreadyFacing;
    public Behavior_Boss_Knight_Thrust(EnemyBoss_Knight boss, float range, int actionIndex)
    {
        _boss = boss;
        _range = range;
        _actionIndex = actionIndex;
        _playerTransform = PlayerHandler.instance.transform;
    }

    public override NodeState Evaluate()
    {

        if (!_boss.CanCallThrust())
        {
            Debug.Log("cannot call this");
            _boss.SelectRandomAction();
            return NodeState.Success;
        }

        if (_boss.actionIndex_Current != _actionIndex)
        {
            isAlreadyFacing = false;
            Debug.Log("1");
            return NodeState.Success;
        }


        if (!isAlreadyFacing)
        {

            float angle = Vector3.Angle(_boss.transform.forward, PlayerHandler.instance.transform.position);

            Vector3 dir = PlayerHandler.instance.transform.position - _boss.transform.position;
            dir.y = 0;//This allows the object to only rotate on its y axis
            Quaternion rot = Quaternion.LookRotation(dir);
            _boss.transform.rotation = Quaternion.Lerp(_boss.transform.rotation, rot, 4f * Time.deltaTime);
            _boss.CallAnimation("Idle", 1);

            float dot = Quaternion.Dot(rot, _boss.transform.rotation);

            if(Mathf.Abs(dot) > 0.999f)
            {
                isAlreadyFacing = true;
            }
            else
            {
                return NodeState.Running;
            }



        }

        


        bool isInRange = Vector3.Distance(_boss.transform.position, _playerTransform.position) + 2 <= _range;

        if (isInRange)
        {
            //call animation
            //Attack_01
            _boss.StartAction("Thrust", 2, true);
            _boss.CallAnimation("Idle", 1);
            Debug.Log("2");
            return NodeState.Running;
        }
        else
        {

        }

        Debug.Log("3");
        return NodeState.Success;
    }
}
