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




    protected override void StartFunction()
    {
        UpdateTree(GetBehavior());
        base.StartFunction();
    }

    protected override void UpdateFunction()
    {
        if (isLocked)
        {
            CallAnimation("Idle", 1);
            CallAnimation("Idle", 2);
            return;
        }

        if (!_agent.isStopped)
        {
            CallAnimation("Idle", 2);
        }

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

    public override void CalculateAttack()
    {

        if (actionIndex_Current == 0)
        {
            Calculate_SimpleAttack();
            return;
        }

        if(actionIndex_Current == 1)
        {
            //call 
        }

        if( actionIndex_Current == 2)
        {
            Calculate_Teleport();
            return;
        }

    }


    void Calculate_SimpleAttack()
    {

        bool isPlayerClose = Vector3.Distance(transform.position, PlayerHandler.instance.transform.position) <= attackClassArray[0].range;

        if (isPlayerClose)
        {
            DamageClass damage = new DamageClass(attackClassArray[0].damage, DamageType.Physical, 0);
            damage.Make_Attacker(this);
            PlayerHandler.instance._playerResources.TakeDamage(damage);
        }

        GameHandler.instance._soundHandler.CreateSfx_WithAudioClip(attackClassArray[0].sound_Release);
    }

    void Calculate_Teleport()
    {
        Debug.Log("teleport");
        StartCoroutine(TeleportProcess());
        GameHandler.instance._soundHandler.CreateSfx_WithAudioClip(attackClassArray[1].sound_Release);
        attackClassArray[1].ControlPSAttackCharge(false);
    }
    IEnumerator TeleportProcess()
    {
        ControlIsLocked(true);
        _graphicHolder.gameObject.SetActive(false);
        GameHandler.instance._pool.GetPS(PSType.Dash_02, transform);

        Vector3 targetPos = MyUtils.GetRandomPointInAnnulus(PlayerHandler.instance.transform.position, 4,5);
        
       
        yield return new WaitForSeconds(0.5f);

        transform.position = targetPos;
        GameHandler.instance._pool.GetPS(PSType.Dash_02, transform);
        _graphicHolder.gameObject.SetActive(true);

        behaviorSimpleAttack.ResetCooldown();

        yield return new WaitForSeconds(0.15f);

        StopAction();
        ControlIsLocked(false);

        Debug.Log("go to the end");
    }


    Behavior_Boss_Ghost_SimpleAttack behaviorSimpleAttack;
    Sequence2 GetBehavior()
    {

        behaviorSimpleAttack = new Behavior_Boss_Ghost_SimpleAttack(this, attackClassArray[0].range, 0.3f, 0);
        return new Sequence2(new List<Node>
        {
            new Behavior_Boss_Chase(this, _bossData), //it simply chases the player.
            new Behavior_Boss_CheckAction(this),
            behaviorSimpleAttack,
            new Behavior_Boss_Ghost_BlindArea(this, 6, 5),
            new Behavior_Boss_Ghost_Teleport(this, 10, 2),
            
            
        });
    }



    public override void TakeDamage(DamageClass damageRef)
    {
        if (isDead) return;

        //this here is checking if the player can damage the shield.
       
        if (damageRef.projectilTransform != null)
        {
            PSScript ps = GameHandler.instance._pool.GetPS(PSType.Blood_01, damageRef.projectilTransform);
            ps.transform.SetParent(psContainer);
            ps.StartPS();
        }



        DamageClass damage = new DamageClass(damageRef);

        damage.UpdateDamageList_Enemy(_bossData); //this is checking   
        _entityEvent.CallDelegate_DealDamageToEntity(ref damage);

        float corruptDamage = damage.GetTotalDamage_Especific(DamageType.Corrupt);

        if (corruptDamage <= 0)
        {
            DamageClass newDamageClass = new DamageClass(0, DamageType.Pure, 0);
            _enemyCanvas.CreateDamagePopUp(newDamageClass);
            return;
        }

        float totalDamage = damage.GetTotalDamage();     

        //GRAPHICAL 
        _enemyGraphicHandler.MakeDamaged();

        //EVENTS
        PlayerHandler.instance._entityEvents.OnDamagedEntity(this, damage);
        PlayerHandler.instance._playerStatTracker.ChangeStatTracker(StatTrackerType.DamageDealt_Total, totalDamage);
        if (damage.AtLeastOneDamageCrit())
        {
            PlayerHandler.instance._entityEvents.OnCrit();
        }

        //

        //UI
        _enemyCanvas.CreateDamagePopUp(damage);

        healt_Current -= totalDamage;
        _enemyCanvas.UpdateHealth(healt_Current, health_Total);



        if (healt_Current <= 0)
        {

            //death
            Die();

        }
    }

}
