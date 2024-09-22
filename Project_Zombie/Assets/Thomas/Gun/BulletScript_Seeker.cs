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
        recalculationCooldown_Total = 0.4f;
    }

    protected override void UpdateFunction()
    {
        //every x seconds we update teh dir.

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

    }



}
