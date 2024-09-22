using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss_Mage : EnemyBoss
{

    [Separator("EYE")]
    [SerializeField] Transform[] _eyesArray;

    [Separator("MAGE")]
    [SerializeField] MageBarrier _mageBarrier;

    public bool CanCallMageBarrier { get { return !_mageBarrier.gameObject.activeInHierarchy; } }


    public void CallMageBarrier()
    {
        _mageBarrier.gameObject.SetActive(true);
        //call an effect
        //sound 
    }
    void HandleMageBarrier()
    {
        //keep placing it between player and object.

        if (!_mageBarrier.gameObject.activeInHierarchy) return;




    }

    protected override void UpdateFunction()
    {

        HandleMageBarrier();

        base.UpdateFunction();
    }

    protected override void AwakeFunction()
    {
        
        base.AwakeFunction();
    }

    protected override void StartFunction()
    {
        UpdateTree(GetBehavior());
        base.StartFunction();
    }

    Sequence2 GetBehavior()
    {
        return new Sequence2(new List<Node>
        {
            new Behavior_Boss_Chase(this, _bossData), //it simply chases the player.           
            new Behavior_Boss_Mage_Meteor(this, _eyesArray),
            new Behavior_Boss_Mage_Shoot(this),
            //new Behavior_Boss_Mage_Barrage(this),

        });
    }
}


//this fellas goes after the player. she keeps on moving
//if the player is on sight she throws projectiles.
//if the player is not in sight but its in range then she will throw meteors at the player. same as magic, but bigger and deal more damage.
//at cooldown the mage raises a wall that is always rotate between the mage and the player. it can be destroyed though.
//

//then the next is shooting projectiles.
//it has two projectiles. one that follows the player, and the other that goes straight.
//
