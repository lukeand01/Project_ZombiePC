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


    //

    [Separator("KNIGHT")]
    [SerializeField] DamageCollider _attackDamageThrust;
    [SerializeField] PSAnimationObject _psObject_Thrust; //we call and it moves independetly.
    [SerializeField] GameObject _ps_ChargeSlashAttack;
    [SerializeField] ParticleSystem _ps_TriggerSlashAttack;


    LayerMask playerLayer;

    protected override void AwakeFunction()
    {
        playerLayer |= (1 << 3);
        DamageClass _damage = new DamageClass(attackClassArray[1].damage, DamageType.Physical, 0);
        _damage.Make_Attacker(this);
        _attackDamageThrust.SetUp(_damage);
        base.AwakeFunction();
    }



    protected override void StartFunction()
    {
        base.StartFunction();
        UpdateTree(GetBehavior());
    }

    protected override void UpdateFunction()
    {
        if (IsActing)
        {
            //this
            ControlIsShielded(false);
        }
        else
        {
            if (isShielded)
            {
                CallAnimation("ShieldIdle_New", 3);
            }
            else
            {
                CallAnimation("Idle", 3);
            }

        }

        //and also i am going to check



        base.UpdateFunction();
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

        _ps_ChargeSlashAttack.gameObject.SetActive(false);
        _ps_TriggerSlashAttack.gameObject.SetActive(true);
        _ps_TriggerSlashAttack.Clear();
        _ps_TriggerSlashAttack.Play();
        
        if (isPlayerClose)
        {
            DamageClass damage = new DamageClass(attackClassArray[0].damage, DamageType.Physical, 0);
            damage.Make_Attacker(this);
            PlayerHandler.instance._playerResources.TakeDamage(damage);
        }

        _abilityCanvas.ControlCircleFill(0, 0);
        
        //

    }





    void Calculate_Thrust()
    {
        //deal damage in a line

        //we turn the boxcollider for a frame and turn it off.
        //create an object here that moves to the position.
        //trigger an explision after its done moving

        //now we will create the thing here.

        _psObject_Thrust.gameObject.SetActive(true);
        _psObject_Thrust.CallAnimation();


        PlayerHandler.instance.TryToCallExplosionCameraEffect(transform, 0.6f);


        /*Vector3 boxCenter = _attackDamageThrust.bounds.center;
        Vector3 boxHalfExtents = _attackDamageThrust.bounds.extents;


        if (Physics.OverlapBox(boxCenter, boxHalfExtents, Quaternion.identity, playerLayer).Length > 0)
        {
            DamageClass _damage = new DamageClass(attackClassArray[1].damage, DamageType.Physical, 0);
            _damage.Make_Attacker(this);
            PlayerHandler.instance._playerResources.TakeDamage(_damage);
        }*/

    }

    public bool CanCallThrust()
    {
        return !_psObject_Thrust.isActiveAndEnabled;
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
            _ps_ChargeSlashAttack.gameObject.SetActive(true);
            return;
        }

        if(actionIndex_Current == 1)
        {
            //use a straight line as collider.
            _abilityCanvas.ControlCircleFill(0, 0);
            _abilityCanvas.ControlCustomFill(current, total);
            return;
        }

        if(actionIndex_Current == 2)
        {
            _abilityCanvas.ControlCircleFill(0, 0);
            _abilityCanvas.ControlCustomFill(0, 0);
        }


    }



    #endregion


  

    Sequence2 GetBehavior()
    {
        return new Sequence2(new List<Node>
        {
            new Behavior_Boss_Chase(this, _bossData),
            new Behavior_Boss_CheckAction(this),
            new Behavior_Boss_Knight_Shield(this, 8),
            new Behavior_Boss_Knight_Slash(this, 5, 0),
            new Behavior_Boss_Knight_Thrust(this, 15, 1),
            new Behavior_Boss_Knight_Summon(this, 75, 2),
        });
    }

}


//