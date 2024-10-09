using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRunAway : EnemyBase
{
    //this _enemy runs away from the player
    //on death it grants a especial resource.

    [SerializeField] Transform[] eyeArray;

    protected override void StartFunction()
    {
        UpdateTree(GetBehavior());
        _enemyGraphicHandler.SelectRandomGraphic();
        base.StartFunction();
    }

    protected override void Die(bool wasKilledByPlayer = true)
    {
        base.Die(wasKilledByPlayer);

        if(wasKilledByPlayer)
        {
            //then we give important resource.
            Debug.Log("killed");
        }

    }

    Sequence2 GetBehavior()
    {
        return new Sequence2(new List<Node>
        {
            new BehaviorCheckSight(this, eyeArray),
            new BehaviorRunAway(this)

        });
    }

}
