using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBomber : EnemyBase
{

    //the behavioor is the same but just the attack
    //

    LayerMask targetLayers;

    private void Start()
    {
        UpdateTree(GetBehavior());
    }




    public override void CallAttack()
    {


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


        float distanceForAttack = Vector3.Distance(targetObject.transform.position, transform.position);
        GameHandler.instance._soundHandler.CreateSfx(data.audio_Attack, transform);

        //we will check any fellas in front 
        //its the same thing but in the end it explodes. creates effect.
        //and we cause damage to anyone who is player or who is ally around.

        targetLayers  |= (1 << 3);
        targetLayers |= (1 << 8);

        RaycastHit[] targets = Physics.SphereCastAll(transform.position, distanceForAttack, Vector3.up, 0, targetLayers);

        DamageClass damage = GetDamage();




        foreach (var item in targets)
        {
            IDamageable targetDamageable = item.collider.GetComponent<IDamageable>();

            if (targetIdamageable == null) continue;
            targetDamageable.TakeDamage(damage);
            //push it from teh palyer too
        }

        Destroy(gameObject);
       
    }

    public override void CallAbilityIndicator(float current, float total)
    {
        base.CallAbilityIndicator(current, total);

        if(total <= 0)
        {
            _abilityIndicatorCanvas.StopCircleIndicator();
        }
        else
        {
            _abilityIndicatorCanvas.StartCircleIndicator(data.attackRange);
            _abilityIndicatorCanvas.ControlCircleFill(current, total);
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

}
