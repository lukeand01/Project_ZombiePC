using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharger : EnemyBase
{

    private void Start()
    {
        UpdateTree(GetBehavior());
    }

    //behavior we have is walk.
    //then when in range then we check if we can see the player.
    //


    Sequence2 GetBehavior()
    {
        return new Sequence2(new List<Node>
        {
            

        });
    }


}
