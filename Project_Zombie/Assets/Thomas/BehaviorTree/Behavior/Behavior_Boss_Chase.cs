using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_Boss_Chase : Sequence2
{

    EnemyBoss _boss;
    EnemyData _bossData;

    float updateAgent_Total;
    float updateAgent_Current;

    float updateCheckForAlly_Total;
    float updateCheckForAlly_Current;

    Transform playerTransform;

    LayerMask allyLayer;
    LayerMask wallAndPlayerLayer;
    LayerMask enemyLayer;

    public Behavior_Boss_Chase(EnemyBoss _boss, EnemyData _bossData)
    {
        this._boss = _boss;
        this._bossData = _bossData;


        updateAgent_Total = 0.03f;
        updateAgent_Current = updateAgent_Total;

        updateCheckForAlly_Total = 0.08f;
        updateCheckForAlly_Current = updateCheckForAlly_Total;

        if(PlayerHandler.instance == null)
        {
            Debug.Log("here");
        }

        playerTransform = PlayerHandler.instance.transform;

        allyLayer |= (1 << 8);

        enemyLayer |= (1 << 6);

        wallAndPlayerLayer |= (1 << 3);
        wallAndPlayerLayer |= (1 << 7);
        wallAndPlayerLayer |= (1 << 9);


    }

    public override NodeState Evaluate()
    {
        //_enemy.CallAbilityIndicator(0, 0); //we disable it.

        if (_boss.IsStunned)
        {
            Debug.Log("cannot chase. it is stunned");
            _boss.CallAnimation("Idle", 1);
            return NodeState.Failure;
        }

        if (_boss.IsActing) return NodeState.Success;


        Transform currentTarget = GetTargetAsEnemy();


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
            //_boss.SetNewtarget(currentTarget.gameObject);
            _boss.SetDestinationForPathfind(currentTarget.position);
            updateAgent_Current = 0;
        }
        else
        {
            updateAgent_Current += Time.deltaTime;
        }

        _boss.CallAnimation("Run", 1);
        return NodeState.Success;
       
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


        RaycastHit[] allies = Physics.SphereCastAll(_boss.transform.position, 20, Vector2.up, 50, allyLayer);


        if (allies.Length > 0)
        {

            float playerDistance = Vector3.Distance(PlayerHandler.instance.transform.position, _boss.transform.position);
            float targetDistance = Vector3.Distance(allies[0].collider.transform.position, _boss.transform.position);

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

}
