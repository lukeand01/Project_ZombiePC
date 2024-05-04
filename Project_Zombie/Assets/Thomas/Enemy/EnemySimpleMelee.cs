using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySimpleMelee : EnemyBase
{

    protected override void AwakeFunction()
    {
        base.AwakeFunction();

        

    }

    private void Start()
    {
        UpdateTree(GetBehavior());
    }

    Sequence2 GetBehavior()
    {
        return new Sequence2(new List<Node>
        {
            new BehaviorChase(this),
            new BehaviorAttack(this)

        });
    }

}
