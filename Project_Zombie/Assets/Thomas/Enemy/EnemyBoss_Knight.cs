using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class EnemyBoss_Knight : EnemyBoss
{
    //i will create behavior for it.
    //

    //
    //BEHAVIOR
    //Move towards the player, it always know where the player is.
    //check for ability, we do a little check that returns if the player has something he can do.
    //if the player is too far and is shooting at the knight it will raise its shield and start blocking it. increase move speed
    //if the player is another range and he has not being attack then it attacks
    //if the player is close then do a swap
    //randomy decide if it should spawn the blades. the blades have two random moves.
    //

    [Separator("KNIGHT")]
    [SerializeField] BoxCollider _attackThrustCollider;
    LayerMask playerLayer;

    protected override void AwakeFunction()
    {
        playerLayer |= (1 << 3);
        base.AwakeFunction();
    }

    private void Start()
    {
        UpdateTree(GetBehavior());
    }

    #region CALCULATE ATTACK
    public override void CalculateAttack()
    {

        if(actionIndex_Current == 0)
        {
            Calculate_Slash();
            return;
        }
        if(actionIndex_Current == 1)
        {
            Calculate_Thrust();
            return;
        }
        if (actionIndex_Current == 2)
        {
            Calculate_Summon();
            return;
        }
    }
    void Calculate_Slash()
    {
        //dela damage based in range.
        bool isPlayerClose = Vector3.Distance(transform.position, PlayerHandler.instance.transform.position) <= attackClassArray[0].range;

        if (isPlayerClose)
        {
            DamageClass damage = new DamageClass(attackClassArray[0].damage, DamageType.Physical, 0);
            damage.Make_Attacker(this);
            PlayerHandler.instance._playerResources.TakeDamage(damage);
        }

    }
    void Calculate_Thrust()
    {
        //deal damage in a line

        //we turn the boxcollider for a frame and turn it off.
        Vector3 boxCenter = _attackThrustCollider.bounds.center;
        Vector3 boxHalfExtents = _attackThrustCollider.bounds.extents;


        if (Physics.OverlapBox(boxCenter, boxHalfExtents, Quaternion.identity, playerLayer).Length > 0)
        {
            DamageClass _damage = new DamageClass(attackClassArray[1].damage, DamageType.Physical, 0);
            _damage.Make_Attacker(this);
            PlayerHandler.instance._playerResources.TakeDamage(_damage);
        }



    }
    void OnDrawGizmosSelected()
    {
        
    }

    void Calculate_Summon()
    {
        //summon two blades that move around the knight
        //summon one blade that moves randomly in the room.


    }
    #endregion

    #region CALCULATE UI
    //we call here to do the ui.

    protected override void CallUI(float current, float total)
    {
        if(actionIndex_Current == 0)
        {
            //use the circle ui.
            _abilityCanvas.StartCircleIndicator(attackClassArray[0].range);
            _abilityCanvas.ControlCircleFill(current, total);
            return;
        }

        if(actionIndex_Current == 1)
        {
            //use a straight line as collider.
            _abilityCanvas.ControlCustomFill(current, total);
            return;
        }


    }

    #endregion


    Sequence2 GetBehavior()
    {
        return new Sequence2(new List<Node>
        {
            new Behavior_Boss_Chase(this, _bossData),
            new Behavior_Boss_CheckAction(this),
            new Behavior_Boss_Knight_Shield(this, 5),
            new Behavior_Boss_Knight_Slash(this, 5, 0),
            new Behavior_Boss_Knight_Thrust(this, 15, 1),
            new Behavior_Boss_Knight_Summon(this, 75, 2),
        });
    }

}


//