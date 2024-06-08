using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyCharger : EnemyBase
{
    [Separator("CHARGE")]
    [SerializeField] Transform[] eyeArray;
    [SerializeField] Transform feet;
    LayerMask wallLayer;
    bool isCharging;
    Vector3 lastChargePosition;

    protected override void AwakeFunction()
    {
        base.AwakeFunction();

        wallLayer |= (1 << 9);
    }

    private void Start()
    {
        UpdateTree(GetBehavior());
    }

    protected override void UpdateFunction()
    {
        

        if (isCharging)
        {
            RotateTarget(PlayerHandler.instance.transform.position);
            StopAgent();
            return;
        }

        Debug.Log("udpate");
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
            new BehaviorAttack(this)

        });
    }


    public override void CallAttack()
    {


        float distanceForSimpleAttack = 2.8f;

        if(Vector3.Distance(transform.position, PlayerHandler.instance.transform.position) < distanceForSimpleAttack)
        {
            base.CallAttack();
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(ChargeProcess());
        }
        //quickly rotate to the target.

    }

    //rotation is shaking for some reason.
    //its never toating well.
    //


    IEnumerator ChargeProcess()
    {
        Vector3 direction = PlayerHandler.instance.transform.position - transform.position;
        Vector3 directionNormalized = direction.normalized;

        isCharging = true;

        Quaternion targetRotation = Quaternion.LookRotation(directionNormalized);
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 50 * Time.deltaTime);
        transform.rotation = targetRotation;


       
        Debug.Log("completed");
        yield return new WaitForSecondsRealtime(2);

        isCharging = false;

        

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

        isCharging = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (isCharging && other.gameObject.layer == 3)
        {
            Debug.Log("dash found the player");


        }
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