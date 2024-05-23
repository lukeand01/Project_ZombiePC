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

        wallAndPlayerLayer |= (1 << 3);
        wallAndPlayerLayer |= (1 << 7);
        wallAndPlayerLayer |= (1 << 9);
    }



    public override NodeState Evaluate()
    {

        enemy.CallAbilityIndicator(0, 0); //we disable it.

        if(enemy.IsStunned())
        {
            Debug.Log("cannot chase. it is stunned");
            return NodeState.Failure;
        }


        //in here we simply check around. if you find any other ally then we are target that.
        //also if player is invisible 



        Transform currentTarget = playerTransform;
        bool isTargettingAlly = false;
        //we need to check the surrounding to see if we find allies.


        if(Physics.SphereCast(enemy.transform.position, 15, Vector3.forward, out RaycastHit hit, 100, allyLayer))
        {
            Debug.Log("yo");

            if(hit.collider != null)
            {
                float playerDistance = Vector3.Distance(currentTarget.transform.position, enemy.transform.position);
                float targetDistance = Vector3.Distance(hit.collider.transform.position, enemy.transform.position);

                if(playerDistance > targetDistance)
                {
                    Debug.Log("found ally");
                    currentTarget = hit.collider.transform;
                    isTargettingAlly = true;
                }

            }

        }
        else
        {
            //Debug.Log("found nothing");
        }



        bool isPlayerDead = PlayerHandler.instance._playerResources.IsDead();

        if (isPlayerDead && !isTargettingAlly) return NodeState.Failure;

        bool isPlayerInvisible = PlayerHandler.instance._entityStat.IsInvisible;

        if (isPlayerInvisible && !isTargettingAlly) return NodeState.Failure;

        //we check if there are allies in range.



        enemy.SetNewtarget(currentTarget.gameObject);
        enemy.SetDestinationForPathfind(currentTarget.position);

        float distanceFromCurrentTarget = Vector3.Distance(currentTarget.position, enemy.transform.position);


        float distanceReduction = enemyData.attackRange * 0.1f;

        if(enemyData.attackRange - distanceReduction >= distanceFromCurrentTarget )
        {
            //then we call this and apss to the next
            //then we force right at way for the enemy to stop
            enemy.StopAgent();
            return NodeState.Success;
        }
        else
        {
            return NodeState.Running;
        }
        
        
    }
}
