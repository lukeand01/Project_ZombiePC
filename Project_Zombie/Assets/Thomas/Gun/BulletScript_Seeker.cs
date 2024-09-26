using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript_Seeker : BulletScript
{
    //this tries to correct its path to follow the player.


    float recalculationCooldown_Current;
    float recalculationCooldown_Total;

    private void Start()
    {
        recalculationCooldown_Total = 0.15f;
    }

    private void OnDisable()
    {
        //we will create a smoke here
    }


    protected override void UpdateFunction()
    {
        //every x seconds we update teh dir.

        

        //rotate to always face where its going.

        Quaternion targetRotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 50 * Time.deltaTime);


        if (dir == Vector3.zero)
        {
            Debug.Log("dir is zero");
        }


        Debug.Log("this is the speed " + speed);

        if(recalculationCooldown_Current > recalculationCooldown_Total)
        {
            //recalculate pathing.
            RecalculateDir();
            recalculationCooldown_Current = 0;
        }
        else
        {
            recalculationCooldown_Current += Time.deltaTime;
        }


        transform.position += dir.normalized * speed * Time.fixedDeltaTime;
    }

    void RecalculateDir()
    {
        Vector3 shootDir = PlayerHandler.instance.transform.position - transform.position;
        dir = shootDir;
    }

    protected override void TriggerEnterFunction(Collider other)
    {

        if (other.gameObject.layer == 10)
        {
            GameHandler.instance._pool.Bullet_Release(1, this); //its index one because its enemy
            return;
        }
        if (other.gameObject.layer == 9)
        {
            GameHandler.instance._pool.Bullet_Release(1, this);
            return;
        }

        if (other.tag != "Player") return;


        IDamageable damageable = other.GetComponent<IDamageable>();

        //Debug.Log("deal damage to " + other.gameObject.name + " " + damage.baseDamage);

        if (damage == null)
        {
            //Debug.Log("no damage here");
        }

        if (damageable != null)
        {
            CalculateDamageable(damageable);
        }

        //if it touches a wall 



    }

}
