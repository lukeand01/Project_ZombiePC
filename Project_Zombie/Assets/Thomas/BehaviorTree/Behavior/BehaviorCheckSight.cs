using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
        Debug.Log("eye");
        if(eyeArray.Length == 0)
        {
            Debug.Log("no eye array");
        }

        for (int i = 0; i < eyeArray.Length; i++)
        {
            var item = eyeArray[i];

            Vector3 targetPos = (playerTransform.position - item.position).normalized;
            Ray ray = new Ray(item.position, targetPos);

            if (Physics.Raycast(ray, out RaycastHit hit, 500, wallAndPlayerLayer))
            {
                if (hit.collider.tag != "Player")
                {
                    Debug.Log("failure");
                    return NodeState.Failure;
                }
            }
            else
            {
                Debug.Log("failure 1");
                return NodeState.Failure;
            }

        }




        return NodeState.Success;


    }

}
