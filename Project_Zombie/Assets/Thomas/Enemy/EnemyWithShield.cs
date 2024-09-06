using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Runtime.CompilerServices;
using UnityEngine;


public class EnemyWithShield : EnemyBase
{
    [SerializeField] bool shouldOnlyMoveWhenFacing;

    protected override void AwakeFunction()
    {
        base.AwakeFunction();

    }

    

    [SerializeField] float oldSpeed;


    private void FixedUpdate()
    {
        if (shouldOnlyMoveWhenFacing && isMoving && oldSpeed != 0)
        {

            //we check if we are facing 
            Vector3 damageDirection = (currentAgentTargetPosition - transform.position).normalized;
            float angle = Vector3.Angle(damageDirection, transform.forward);

            // Check if the angle is within the threshold
            if (angle < 20)
            {
                agent.angularSpeed = 30;
                agent.speed = oldSpeed;
            }
            else
            {
                Debug.Log("Agent is not facing the target.");
                agent.speed = 50f;
                agent.angularSpeed = 999;
                agent.velocity = Vector3.zero;
            }
        }
    }


    protected override void UpdateFunction()
    {
        base.UpdateFunction();

        
    }



    protected override void StartFunction()
    {
        UpdateTree(GetBehavior());
        base.StartFunction();

        oldSpeed = agent.speed;
    }

    Sequence2 GetBehavior()
    {
        return new Sequence2(new List<Node>
        {
            new BehaviorChase(this),
            new BehaviorAttack(this)

        });
    }

    public override void CallAttack()
    {
        if (targetObject == null)
        {
            return;
        }

        if (targetIdamageable == null)
        {
            return;
        }

        if (targetIdamageable.IsDead())
        {
            return;
        }


        float distanceForAttack = Vector3.Distance(targetObject.transform.position, transform.position);
        GameHandler.instance._soundHandler.CreateSfx(data.audio_Attack, transform);



        //we will check any fellas in front 

        if (data.attackRange >= distanceForAttack)
        {

            //if the player is still in range then we attack.
            DamageClass damage = GetDamage();
            damage.Make_Attacker(this);
            targetIdamageable.TakeDamage(damage);

            Vector3 pushDirection = (PlayerHandler.instance.transform.position - transform.position).normalized;
            // Apply the force to the player's Rigidbody

            BDClass newBd = new BDClass("PushPlayer", BDType.Stun, 1.2f);
            PlayerHandler.instance._entityStat.AddBD(newBd);

            PlayerHandler.instance.PushPlayer(pushDirection, 500);
        }



    }
}
