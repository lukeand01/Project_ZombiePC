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

        //there is a time between attacks so we dont span attacks
        //also 



        //when we call the animation we only stop this after two things
        //but we might need to reset this.
        //

        //WE DO THIS ALWAYS TO CONFIRM THAT THE ATTACKS STARTS FROM 0
        if (!enemy.IsAttacking_Animation)
        {
            current = 0;
            //stop animation if that becomes a problem
        }

        

        if (enemy._entityAnimation.IsAttacking(0))
        {
            Debug.Log("attacking");
            //enemy._entityAnimation.CallAnimation_Idle(0, 1);
            enemy._entityAnimation.CallAnimation_Idle(0, 0);
        }
        else if(!enemy.IsAttacking_Animation)
        {

            enemy._entityAnimation.CallAnimation_Attack(2);
            enemy.SetIsAttacking_Animation(true);
            current = 0;
        }
        else if (enemy.IsAttacking_Animation)
        {
            enemy.SetIsAttacking_Animation(false);
            return NodeState.Success;

        }

        //we call the attack but we can stop       


        if (enemy.IsAttacking_Animation)
        {
            current += Time.deltaTime;
            return NodeState.Running;
        }
        else
        {
            return NodeState.Success;
        }

    }




}
