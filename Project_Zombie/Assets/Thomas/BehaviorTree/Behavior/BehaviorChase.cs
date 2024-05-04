using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorChase : Sequence2
{

    //if we are here we must go after the player.
    //but we check for other things we can target around. otherwise we go to the player.

    
    Transform playerTransform;
    LayerMask allyLayer;
    LayerMask wallAndPlayerLayer;
    EnemyBase enemy;
    EnemyData enemyData;

    public BehaviorChase(EnemyBase enemy)
    {
        this.enemy = enemy;
        enemyData = enemy.GetData();


        playerTransform = PlayerHandler.instance.transform;

        allyLayer |= (1 << 8);

        wallAndPlayerLayer = (1 << 3);
        wallAndPlayerLayer = (1 << 7);
        wallAndPlayerLayer = (1 << 9);
    }



    public override NodeState Evaluate()
    {

        enemy.CallAbilityIndicator(0, 0); //we disable it.

        if(enemy.IsStunned())
        {
            Debug.Log("cannot chase. it is stunned");
            return NodeState.Failure;
        }




        Transform currentTarget = playerTransform;
        bool isPlayerDead = PlayerHandler.instance._playerResources.IsDead();

        if (isPlayerDead) return NodeState.Failure;

        //we check if there are allies in range.



        enemy.SetNewtarget(currentTarget.gameObject);
        enemy.SetDestinationForPathfind(currentTarget.position);

        float distanceFromCurrentTarget = Vector3.Distance(currentTarget.position, enemy.transform.position);


        float distanceReduction = enemyData.attackRange * 0.1f;

        if(enemyData.attackRange - distanceReduction >= distanceFromCurrentTarget )
        {
            //then we call this and apss to the next
            return NodeState.Success;
        }
        else
        {
            return NodeState.Running;
        }
        
        
    }
}
