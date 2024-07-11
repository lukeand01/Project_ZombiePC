using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class EnemyCharger : EnemyBase
{
    [Separator("CHARGE")]
    [SerializeField] Transform[] eyeArray;
    [SerializeField] Transform feet;
    [SerializeField] ChargeCollider _chargeCollider;
    [SerializeField] GameObject shieldObject;
    LayerMask wallLayer;

    Vector3 lastChargePosition;

    public override void ResetEnemyForPool()
    {
        base.ResetEnemyForPool();
        _chargeCollider.gameObject.SetActive(false);
        gameObject.layer = 6;
    }

    protected override void AwakeFunction()
    {
        base.AwakeFunction();

        wallLayer |= (1 << 9);
    }

    protected override void StartFunction()
    {
        UpdateTree(GetBehavior());
        base.StartFunction();
    }


    protected override void UpdateFunction()
    {
      
        base.UpdateFunction();
    }

    //behavior we have is walk.
    //then when in range then we check if we can see the player.
    //

    Sequence2 GetBehavior()
    {
        return new Sequence2(new List<Node>
        {
            new BehaviorChase(this),
            new BehaviorCheckSight(this, eyeArray),
            new BehaviorAttack_WaitForAttackToComplete(this)

        });
    }


    public override void CallAttack()
    {
        //when we call attack we want to check if the enemy is near or if its far.
        //if we get here we check the distance


        float distance = Vector3.Distance(transform.position, PlayerHandler.instance.transform.position);

        SetIsAttack(true);

        StopAllCoroutines();
        StartCoroutine(ChargeProcess1());
       



    }

    //what should it do?
    //what we do is that we want it moving but tilting only a bit.
    //
    IEnumerator AttackProcess()
    {
        //waits a moment and play an animation of attacks.
        //
        //
        yield return new WaitForSecondsRealtime(data.attackSpeed); //

        //we check if the player is close enough.

        //we play the animation and check if the player is still close.

        yield return null;
    }
    IEnumerator ChargeProcess1()
    {
        //if thats the case then we wait a bit and we start charging forward.
        //we quickly look at the target.


        StopAgent();
        SetIsAttack(true);



        Vector3 direction = PlayerHandler.instance.transform.position - transform.position;
        Vector3 directionNormalized = direction.normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionNormalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1 * Time.fixedDeltaTime);

        //then we are going to be pushed along the distance. we update the 

        yield return new WaitForSeconds(1.5f);

        gameObject.layer = 12;
        _chargeCollider.gameObject.SetActive(true);

        Debug.Log("started moving");
        //we are going to rotate while we stop the agent.
        float startTime = Time.time;
        float dashTime = 2.5f;
        float dashSpeed = 10;

        //and now i want the player to be pushed aside.
        //and for the dude to continue.

        //actuallçy i want it first to be slow, then to starting speed up and then slowing down in the end again.


        while (Time.time < startTime + dashTime && !IsWallAhead())
        {
            Vector3 movement = head.transform.forward * dashSpeed;
            //_rb.AddForce(movement  , ForceMode.Force);
            _rb.velocity = movement;
            //we are trusting that this is rotate towards the real target. lets try this for now.

            yield return new WaitForSeconds(0.01f);
        }

        _rb.velocity = Vector3.zero;   
        
        gameObject.layer = 6;
        _chargeCollider.gameObject.SetActive(false);

        yield return new WaitForSeconds(1.5f);

        SetIsAttack(false);

    }

    IEnumerator ChargeProcess()
    {
        Debug.Log("yo");
        yield break;    

        Vector3 direction = PlayerHandler.instance.transform.position - transform.position;
        Vector3 directionNormalized = direction.normalized;


        Quaternion targetRotation = Quaternion.LookRotation(directionNormalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 50 * Time.deltaTime);
        transform.rotation = targetRotation;


       
        Debug.Log("completed");
        yield return new WaitForSecondsRealtime(2);

        

        yield break;
        Debug.Log("done rotating");

        

        float startTime = Time.time;
        float dashTime = 0.08f;
        float dashSpeed = 15;

        while (Time.time < startTime + dashTime && !IsWallAhead())
        {
            Vector3 movement = head.transform.forward * dashSpeed;
            _rb.velocity = movement;
            Debug.Log("yo");
            yield return null;
        }

        //if you hit the player while in charge then it deals damage.
        //applies stun.
        //pushes. to the side. and if the enmy hit it stops at the moment of the hit.


        if (IsWallAhead())
        {
            //if there is a wall ahead then the it stunned f
            Debug.Log("wall ahead");
            BDClass bd = new BDClass("WallStunned", BDType.Stun, 0.5f);
            ApplyBD(bd);

        }


    }


    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision name " + collision.gameObject.name);
    }





    bool IsWallAhead()
    {
        //either of those.
        bool isHeadWall = Physics.Raycast(head.transform.position, Vector3.forward, 1.5f, wallLayer);
        bool isFeetWall = Physics.Raycast(feet.position, Vector3.forward, 1.5f, wallLayer);



        return isFeetWall || isHeadWall;


    }
}


//show a ability ui?
//will not show
//will make a sound queue that its going to start.
//it has a big range
//when we call attack we stop all al behavior.
//then we charge to the position the player was.
//wont the delay make it too easy to dodge?
//