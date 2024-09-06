using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorAttack_WaitForAttackToComplete : Sequence2
{


    EnemyBase enemy;
    EnemyData enemyData;


    float current;
    float total;

    bool alreadyCalledAttack;

    public BehaviorAttack_WaitForAttackToComplete(EnemyBase enemy)
    {
        this.enemy = enemy;
        enemyData = enemy.GetData();

        total = enemyData.AttackAnimationSpeed;
    }

    public override NodeState Evaluate()
    {
        //this is the same thing but it after calling it only passes after the attack itself is completed.



        if(enemy.isAttacking)
        {
            return NodeState.Running;
        }

        if (!alreadyCalledAttack)
        {

            enemy.CallAttack();
            alreadyCalledAttack = true;
            return NodeState.Running;
        }

        alreadyCalledAttack = false;
        return NodeState.Success;


    }
}
