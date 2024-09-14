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

    bool isAttacking;

    public BehaviorAttack(EnemyBase enemy)
    {
        this.enemy = enemy;
        enemyData = enemy.GetData();
        total = enemy.GetData().attackRest;
    }

    //instead of waiting, we will attack and call it in the end of the animation.

    //what if the animation is too slow? i just need to speed it up for each one.

    //


    public override NodeState Evaluate()
    {
        //and time we want to attack. we need to complete the animation for attack and then we can move.
        //only after we call it do we stop.
        enemy.StopAgent();

        enemy.CallAbilityIndicator(current, total);

        if (enemy._entityAnimation.IsAttacking(0))
        {
            Debug.Log("attacking");
            //enemy._entityAnimation.CallAnimation_Idle(0, 1);
            enemy._entityAnimation.CallAnimation_Idle(0, 0);
            return NodeState.Running;
        }

        if (!enemy.IsAttacking_Animation)
        {

            if (isAttacking)
            {
                isAttacking = false;
                return NodeState.Success;
            }
            else
            {
                current = 0;
                enemy._entityAnimation.CallAnimation_Attack(2);
                enemy.SetIsAttacking_Animation(true);
                isAttacking = true;

                
            }

           
        }


        return NodeState.Running;
    }




}
