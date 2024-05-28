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


    private void FixedUpdate()
    {
        //there is a strong inconsitency in turret raycast. need to find another way to solve this. because the thing is failiing to shoot right.


        //it will go as following:
        //the turret checks if there are enemies close enough.
        //then we use that list. we check everyone to find someone we have a clear line of sight. if we do, we set target
        //once we have the target we rotate to the target.
        //once the turret is lined to the turret then we start shooting
        //we keep checking if the target is dead or if its out of sight
        //we loop


        if(targetObject != null)
        {
            //
            Debug.Log("spotted fella");
            RotateToTarget();
            CheckIfTargetIsNull();

        }
        else
        {
            graphic.transform.Rotate(new Vector3(0, 0, 0.2f));
            LookForTarget();
        }


       
        if (targetObject == null)
        {
            //in this case we just keep moving around

            return;
        }

        Vector3 direction = targetObject.transform.position - transform.position;
        Debug.DrawLine(head.transform.position, direction * 5000, Color.red);

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
                    Debug.Log("WALL: aiming for this  " + item.collider.name + " and got this " + hit.collider.name);
                    continue;
                }
                else
                {
                    Debug.Log("this is not a wall " + item.collider.name);
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

        Vector3 direction = targetObject.transform.position - transform.position;

        Ray ray = new Ray(head.transform.position, direction);

        //here we try to check 

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
