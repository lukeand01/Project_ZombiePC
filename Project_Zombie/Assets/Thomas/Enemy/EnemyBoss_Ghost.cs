using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss_Ghost : EnemyBoss
{
    //increases the spawn of traps.
    //throw areas of slow and throw projectiles that give blind for a short duration.
    //it is not particullary fast, but cannot be killed
    //its attacks are fast and have no indicator.

    private void Start()
    {
        UpdateTree(GetBehavior());
    }


    protected override void UpdateFunction()
    {
        base.UpdateFunction();
    }


    //we handle the projectiles and zones here.



    Sequence2 GetBehavior()
    {
        return new Sequence2(new List<Node>
        {
            new Behavior_Boss_Chase(this, _bossData), //it simply chases the player.
        });
    }


}
