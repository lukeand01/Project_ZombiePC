using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeCollider : MonoBehaviour
{
    //this will check
    //we activate this while charging. and remove it when we get out. also importantly this should turn off 


    

    private void OnTriggerEnter(Collider other)
    {
        //now i want to throw the player to the side and leave it stunned for a moment.

        if (other.tag == "Player")
        {

            //we need to apply a force relate to the forward vector.
            Vector3 pushDirection = (other.transform.position - transform.position).normalized;

            // Apply the force to the player's Rigidbody

            BDClass newBd = new BDClass("PushPlayer", BDType.Stun, 1.2f);
            PlayerHandler.instance._entityStat.AddBD(newBd);

            PlayerHandler.instance.PushPlayer(pushDirection, 500);



        }
    }


}
