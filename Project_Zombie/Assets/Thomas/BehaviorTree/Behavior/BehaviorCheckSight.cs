using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorCheckSight : Sequence2
{
    Transform playerTransform;
    LayerMask allyLayer;
    LayerMask wallAndPlayerLayer;
    EnemyBase enemy;
    EnemyData enemyData;
    Transform[] eyeArray;

    public BehaviorCheckSight(EnemyBase enemy, Transform[] eyeArray)
    {
        this.enemy = enemy;
        enemyData = enemy.GetData();
        this.eyeArray = eyeArray;

        playerTransform = PlayerHandler.instance.transform;

        allyLayer |= (1 << 8);

        wallAndPlayerLayer |= (1 << 3);
        wallAndPlayerLayer |= (1 << 7);
        wallAndPlayerLayer |= (1 << 9);
    }


    public override NodeState Evaluate()
    {
        //we check if all eyes can see the player 
        //

        Vector3 targetPos = (enemy.transform.position - playerTransform.position).normalized;    

        foreach (var item in eyeArray)
        {
            if(!Physics.Raycast(item.position, targetPos, 50, wallAndPlayerLayer))
            {
                return NodeState.Failure;
            }
        }

        Debug.Log("got here");
        return NodeState.Success;


    }

}
