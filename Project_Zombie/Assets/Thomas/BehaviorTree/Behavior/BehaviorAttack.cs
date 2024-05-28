using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorAttack : Sequence2
{

    //we need a target.
    //we can get the target from the enemybase.
    //how do we call attacks.
    //the target will be in the enemybase.

    EnemyBase enemy;
    EnemyData enemyData;


    float current;
    float total;

    public BehaviorAttack(EnemyBase enemy)
    {
        this.enemy = enemy;
        enemyData = enemy.GetData();

        total = enemyData.attackSpeed;
    }

    public override NodeState Evaluate()
    {
        //and time we want to attack. we need to complete the animation for attack and then we can move.
        //only after we call it do we stop.
        enemy.StopAgent();

        enemy.CallAbilityIndicator(current, total);


        if(current > total)
        {
            //then we call the attack against the target. but only if the target is still close to the player.
            enemy.CallAttack();
            current = 0;
            return NodeState.Success;
        }
        else
        {
            current += Time.deltaTime;
            return NodeState.Running;
        }


        return base.Evaluate();
    }


}
