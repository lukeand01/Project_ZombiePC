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
    LayerMask enemyLayer;
    EnemyBase enemy;
    EnemyData enemyData;

    float updateAgent_Total;
    float updateAgent_Current;

    float updateCheckForAlly_Total;
    float updateCheckForAlly_Current;

    public BehaviorChase(EnemyBase enemy)
    {
        this.enemy = enemy;
        enemyData = enemy.GetData();

        updateAgent_Total = 0.03f;
        updateAgent_Current = updateAgent_Total;

        updateCheckForAlly_Total = 0.08f;
        updateCheckForAlly_Current = updateCheckForAlly_Total;

        playerTransform = PlayerHandler.instance.transform;

        allyLayer |= (1 << 8);

        enemyLayer |= (1 << 6);

        wallAndPlayerLayer |= (1 << 3);
        wallAndPlayerLayer |= (1 << 7);
        wallAndPlayerLayer |= (1 << 9);


    }



    public override NodeState Evaluate()
    {

        Debug.Log("chase");

        enemy.CallAbilityIndicator(0, 0); //we disable it.

        if(enemy.IsStunned())
        {
            Debug.Log("cannot chase. it is stunned");
            return NodeState.Failure;
        }


        Transform currentTarget = null;



        if (enemy.IsAlly)
        {
            currentTarget = GetTargetAsAlly();
        }
        else
        {
            currentTarget = GetTargetAsEnemy();
        }

        if (currentTarget == null)
        {
            //Debug.Log("no target");
            return NodeState.Failure;
        }
        else
        {
            //Debug.Log("other side");
        }

        //we will cut here



        //we need to set this less often.
        if (updateAgent_Current > updateAgent_Total)
        {
            enemy.SetNewtarget(currentTarget.gameObject);
            enemy.SetDestinationForPathfind(currentTarget.position);
            updateAgent_Current = 0;
        }
        else
        {
            updateAgent_Current += Time.deltaTime;
        }


        float distanceFromCurrentTarget = Vector3.Distance(currentTarget.position, enemy.transform.position);


        float distanceReduction = enemyData.attackRange * 0.1f;

        if(enemyData.attackRange - distanceReduction >= distanceFromCurrentTarget )
        {
            //then we call this and apss to the next
            //then we force right at way for the _enemy to stop
            //we dont play
           
            return NodeState.Success;
        }
        else
        {
            //here we play animation for running
            enemy._entityAnimation.CallAnimation_Run(0);
            enemy._entityAnimation.CallAnimation_Run(2);
            return NodeState.Running;
        }
        
        
    }



    Transform GetTargetAsEnemy()
    {
        Transform currentTarget = null;


        if (updateCheckForAlly_Current > 0)
        {
            updateCheckForAlly_Current -= Time.fixedDeltaTime;

            return null;

        }

        updateCheckForAlly_Current = updateCheckForAlly_Total;


            RaycastHit[] allies = Physics.SphereCastAll(enemy.transform.position, 20, Vector2.up, 50, allyLayer);


            if (allies.Length > 0)
            {

                float playerDistance = Vector3.Distance(PlayerHandler.instance.transform.position, enemy.transform.position);
                float targetDistance = Vector3.Distance(allies[0].collider.transform.position, enemy.transform.position);

                if (playerDistance > targetDistance)
                {
                    updateCheckForAlly_Current = updateCheckForAlly_Total;
                    return allies[0].collider.transform;

                }
                else
                {
                    //Debug.Log("not distance");
                }


            }
            else
            {
                //Debug.Log("no allies");
            }

            updateCheckForAlly_Current = updateCheckForAlly_Total;

        



        if (currentTarget != null)
        {


        }
        else
        {
            bool isPlayerDead = PlayerHandler.instance._playerResources.IsDead();

            if (isPlayerDead) return null;

            bool isPlayerInvisible = PlayerHandler.instance._entityStat.IsInvisible;

            if (isPlayerInvisible) return null;

            return PlayerHandler.instance.transform;
            //we check if there are allies in range.

        }


        return currentTarget;
    }
    Transform GetTargetAsAlly()
    {
        Transform currentTarget = null;

        if (enemy.HasEnemyCurrentTarget()) return enemy.targetObject.transform;
       
        if (updateCheckForAlly_Current > 0)
        {
            updateCheckForAlly_Current -= Time.fixedDeltaTime;
        }
        else
        {
            RaycastHit[] allies = Physics.SphereCastAll(enemy.transform.position, 20, Vector2.up, 50, enemyLayer);

            if (allies.Length > 0)
            {
                return allies[0].transform;


            }
            

            updateCheckForAlly_Current = updateCheckForAlly_Total;

        }

        return null;
    }

    //i will put here 

}
