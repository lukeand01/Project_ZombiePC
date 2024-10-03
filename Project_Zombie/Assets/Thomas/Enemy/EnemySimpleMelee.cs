using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySimpleMelee : EnemyBase
{
    [SerializeField] bool shouldOnlyMoveWhenFacing;

    [SerializeField] int attackLayer = 2;

    protected override void AwakeFunction()
    {
        base.AwakeFunction();     

    }

    protected override void UpdateFunction()
    {
        base.UpdateFunction();

        if(shouldOnlyMoveWhenFacing && isMoving)
        {
            //we check if we are facing 

            float angle = Vector3.Angle(transform.forward, currentAgentTargetPosition);

            // Check if the angle is within the threshold
            if (angle < 15)
            {
                Debug.Log("Agent is facing the target.");
            }
            else
            {
                Debug.Log("Agent is not facing the target.");
            }

        }
    }

    public override void ResetEnemyForPool()
    {
        base.ResetEnemyForPool();
        _enemyGraphicHandler.SelectRandomGraphic();
    }

    protected override void StartFunction()
    {
        UpdateTree(GetBehavior());
        _enemyGraphicHandler.SelectRandomGraphic();
        base.StartFunction();
    }

    Sequence2 GetBehavior()
    {
        return new Sequence2(new List<Node>
        {
            new BehaviorChase(this),
            new BehaviorAttack(this, attackLayer)

        });
    }


}
