using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurretDamage : Turret
{


    //it will get damage from the modifier that i will put here.
    public override void SetUp()
    {
        //we scale the damage based in the list.
        //we scale based in the player´s stat.



        base.SetUp();
    }

    protected override void FixedUpdateFunction()
    {
        base.FixedUpdateFunction();

        LookForTarget1();
        return;

        if (targetObject != null)
        {
            //
        }
        else
        {
            Debug.Log("look for targets");
            graphic.transform.Rotate(new Vector3(0, 0, 0.2f));
            
        }



        if (targetObject == null)
        {
            //in this case we just keep moving around

            return;
        }

        Vector3 direction = targetObject.transform.position - transform.position;
        Debug.DrawLine(head.transform.position, direction * 5000, Color.red);
    }


    //change this
    //i dont want to be checking every frame for it.
    //we check only if we find no one, but when we do we only become interested on it.
    //we arealdyu doing but there is no coinsistence
    //

    void LookForTarget1()
    {
        //we will check t
        RaycastHit[] targetsAround = Physics.SphereCastAll(transform.position, range, Vector2.up, 50, enemyLayer);

        foreach (var item in targetsAround)
        {
            //we will get the first to be the right one because its generaly the closest.
            if (IsInSight(item.collider.transform))
            {

                Debug.Log("something caught in sight");
                IDamageable damageable = item.collider.GetComponent<IDamageable>();

                if (damageable == null) continue;

                Debug.Log("caught right");
                target = damageable;
                targetObject = item.collider.gameObject;
                return;

            }
            else
            {
                Debug.Log("not caught");
            }

        }

    }




    void LookForTarget()
    {
        RaycastHit[] targetsAround = Physics.SphereCastAll(transform.position, range, Vector2.up, 50, targetLayer);

        //Debug.Log("number of fellas around the turret " + targetsAround.Length);

        foreach (var item in targetsAround)
        {
            //we check if these target is an enemy. and then we check 
            //then we check. otherwise we continue.


            Vector3 direction = (item.collider.transform.position - transform.position).normalized;

            Ray ray = new Ray(transform.position, direction);
            
            

            if (Physics.Raycast(ray, out RaycastHit hit, range, targetLayer))
            {
                

                if(hit.collider.gameObject.layer == 9)
                {
                    //Debug.Log("WALL: aiming for this  " + item.collider.name + " and got this " + hit.collider.name);
                    continue;
                }
                else
                {
                    //Debug.Log("this is not a wall " + item.collider.name);
                }
                

                IDamageable damageable = item.collider.gameObject.GetComponent<IDamageable>();

                if (damageable == null)
                {
                    continue;
                }

                target = damageable;
                targetObject = item.collider.gameObject;
                return;
            }

        }
    }

    void RotateToTarget()
    {
        Vector3 direction = targetObject.transform.position - transform.position;
        Vector3 directionNormalized = direction.normalized;

        //transform.LookAt(targetObject.transform.position);

        Quaternion targetRotation = Quaternion.LookRotation(directionNormalized);
        graphic.transform.rotation = Quaternion.Slerp(graphic.transform.rotation, targetRotation, 15 * Time.deltaTime);


        if (Quaternion.Angle(graphic.transform.rotation, targetRotation) <= 5)
        {
            //can now shoot
            Shoot();
        }
    }

    void CheckIfTargetIsNull()
    {

        float distanceFromTarget = Vector3.Distance(transform.position, targetObject.transform.position);

        if(distanceFromTarget > range)
        {
            target = null;
            targetObject = null;
            return;
        }


        if (IsInSight(null))
        {
            Debug.Log("still on sight");
        }
        else
        {
            Debug.Log("not on sight anymore");
            target = null;
            targetObject = null;
            return;
        }


        if (target.IsDead())
        {
            target = null;
            targetObject = null;
        }

        return;
        Vector3 direction = targetObject.transform.position - transform.position;

        Ray ray = new Ray(head.transform.position, direction);

        //i think this raycast is screwing it, i need more relibable 
        //i need to shioot a bunch of raycast to make sure. one iss not reliable enough.

        if (Physics.Raycast(ray, out RaycastHit hit, range, targetLayer))
        {

            if(hit.collider.gameObject.layer == 9)
            {
                Debug.Log("target got behind a wall");
                target = null;
                targetObject = null;
                return;
            }         

        }

        if(target == null)
        {
            Debug.Log("yo?");
        }

        if (target.IsDead())
        {
            target = null;
            targetObject = null;
        }

    }

    void Shoot()
    {
        if(attackCooldownCurrent > 0)
        {
            Vector3 direction = (targetObject.transform.position - transform.position).normalized;
            BulletScript newBullet = Instantiate(bulletTemplate, gunPointTransform.position, Quaternion.identity);
            newBullet.SetUp("Turret Ally", direction);

            newBullet.MakeDamage(new DamageClass(5), 0, 0);
            newBullet.MakeSpeed(5, 0, 0);

            attackCooldownCurrent = attackCooldownTotal;
        }
        else
        {
            attackCooldownCurrent -= Time.deltaTime;
        }
    }


   

}
