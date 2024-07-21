using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.Image;

public class EnemyGiant : EnemyBase
{

    LayerMask targetLayer;

    const float ICE_STORM_VALUE = 12;
    


    protected override void StartFunction()
    {
        base.StartFunction();

        UpdateTree(GetBehavior());
        _abilityIndicatorCanvas.StartCircleIndicator(ICE_STORM_VALUE);
        _abilityIndicatorCanvas.ControlCircleFill(0, 0);

        targetLayer |= (1 << 3);
    }

    protected override void UpdateFunction()
    {
        base.UpdateFunction();

        ApplyEffectToPlayerAround();


    }

    void ApplyEffectToPlayerAround()
    {
        Debug.Log(ICE_STORM_VALUE);

        RaycastHit[] targets = Physics.SphereCastAll(transform.position, ICE_STORM_VALUE - 1.5f, Vector3.up, 0, targetLayer);


        if (targets.Length > 0)
        {
            PlayerHandler.instance._playerMovement.GiantInRange();
        }
        

    }

    public override void CallAttack()
    {
        base.CallAttack();

        //if it attacks you it causes a short stun
        if (targetObject == null)
        {
            //Debug.Log("1");
            return;
        }

        if (targetIdamageable == null)
        {
            //Debug.Log("2");
            return;
        }

        if (targetIdamageable.IsDead())
        {
            //Debug.Log("3");
            return;
        }

        Debug.Log("called attack");

        float distanceForAttack = Vector3.Distance(targetObject.transform.position, transform.position);

        if(data.attackRange >= distanceForAttack)
        {
            //then it causes damage and causes stun in player.
            DamageClass damage = GetDamage();
            targetIdamageable.TakeDamage(damage);

            BDClass bd = new BDClass("Giant_Stun", BDType.Stun, 1.2f);
            targetIdamageable.ApplyBD(bd);

        }
        

    }



    Sequence2 GetBehavior()
    {
        return new Sequence2(new List<Node>
        {
            new BehaviorChase(this),
            new BehaviorAttack(this)

        });
    }

    //when a player is around him.


}
