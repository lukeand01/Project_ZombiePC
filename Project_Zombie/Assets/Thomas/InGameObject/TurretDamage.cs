using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurretDamage : Turret
{


    //it will get damage from the modifier that i will put here.

    [Separator("TURRET DAMAGE")]
    [SerializeField] List<BulletBehavior> bulletBehaviorList;

    public override void SetUp()
    {
        //we scale the damage based in the list.
        //we scale based in the player´s stat.



        base.SetUp();
    }

    protected override void FixedUpdateFunction()
    {
        base.FixedUpdateFunction();

        


        if (targetObject != null)
        {
            //         
            Quaternion targetRotation = RotateToTarget();
            CheckIfShouldShoot(targetRotation);
            CheckIfTargetIsNull();
        }
        else
        {

            graphic.transform.Rotate(new Vector3(0, 0.5f, 0));     
            LookForTarget();
        }


    }



    void CheckIfShouldShoot(Quaternion targetRotation)
    {

        if (Quaternion.Angle(graphic.transform.rotation, targetRotation) <= 5)
        {
            //can now shoot

            Shoot();

        }
    }

   
    void Shoot()
    {
        Debug.Log("yo");
        if(attackCooldownCurrent <= 0)
        {
            Vector3 direction = (targetObject.transform.position - transform.position).normalized;
            BulletScript newBullet = Instantiate(bulletTemplate, gunPointTransform.position, Quaternion.identity);
            newBullet.SetUp("Turret Ally", direction);

            newBullet.MakeBulletBehavior(bulletBehaviorList);
            newBullet.MakeDamage(damage, 0, 0);
            newBullet.MakeSpeed(25, 0, 0);

            attackCooldownCurrent = attackCooldownTotal;
        }
        else
        {
            attackCooldownCurrent -= Time.deltaTime;
        }
    }


   

}
