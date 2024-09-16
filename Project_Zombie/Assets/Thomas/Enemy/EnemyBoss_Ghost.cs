using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss_Ghost : EnemyBoss
{
    //increases the spawn of traps.
    //throw areas of slow and throw projectiles that give blind for a short duration.
    //it is not particullary fast, but cannot be killed
    //its attacks are fast and have no indicator.
    //we will make it float. its hands should be done and so should its face.
    //it moves towards the player.




    private void Start()
    {
        UpdateTree(GetBehavior());
    }


    protected override void UpdateFunction()
    {
        base.UpdateFunction();
    }


    //we handle the projectiles and zones here.

    //if we are attacking, we should stop
    //we should be always checking for its cooldown
    //the damage area does not trigger an animation, but also has a cooldown. its always triggered on cooldown, thats why it starts on cooldown
    //need to check for distance as well.
    //then we have the teleport, which can be triggered from anywhere in the map.
    //it also has a cooldown. on cooldown and if you are doing nothing it will jump at the player. it always teleport somewhere aruond a random of the player.
    //

    Sequence2 GetBehavior()
    {
        return new Sequence2(new List<Node>
        {
            new Behavior_Boss_Chase(this, _bossData), //it simply chases the player.
            new Behavior_Boss_CheckAction(this),
            new Behavior_Boss_Ghost_SimpleAttack(this, 5, 5),
            //new Behavior_Boss_Ghost_BlindArea(this, 5, 5),
            //new Behavior_Boss_Ghost_Teleport(this, 5, 5),
        });
    }



}
