using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;

public class Behavior_Boss_Devil_Slash : Sequence2
{
    EnemyBoss_Devil _boss;
    Transform _playerTransform;
    int actionIndex;

    float cooldown_Total;
    float cooldown_Current;

    public Behavior_Boss_Devil_Slash(EnemyBoss_Devil boss)
    {
        _boss = boss;
        actionIndex = 0;

        _playerTransform = PlayerHandler.instance.transform;

        cooldown_Total = 1.2f;
        cooldown_Current = 0;
    }

    //but we should only do that once we are no longer acting.
    //animation must always be linked to the attack.
    //nad the player also must move forward.

    public override NodeState Evaluate()
    {

       // if (_boss.actionIndex_Current != actionIndex) return NodeState.Success;
        if (_boss.IsActing)
        {
            Debug.Log("is already acting");
            return NodeState.Failure;
        }


        if(cooldown_Current > 0)
        {
            cooldown_Current -= Time.deltaTime;
            return NodeState.Success;
        }
        //its here we make the player walk.

        float range = AttackRange;

        float distace = Vector3.Distance(_boss.transform.position, _playerTransform.position);

        if(distace <= range * 0.65f)
        {
            //then we call the attack.
            _boss.CallSlash();
            _boss.StopAgent();
            cooldown_Current = Random.Range(cooldown_Total * 0.6f, cooldown_Total * 1.2f);
            Debug.Log("on range");
        }
        else
        {

            _boss.SetDestinationForPathfind(_playerTransform.position);
        }



        return NodeState.Success;
    }

    



    float AttackRange
    {
        get
        {
            float range = 0;

            if (_boss.currentPhase <= 1)
            {
                range = 10;
            }
            if(_boss.currentPhase > 1)
            {
                range = 15;
            }

            return range;
        }
    }


}

//check for range