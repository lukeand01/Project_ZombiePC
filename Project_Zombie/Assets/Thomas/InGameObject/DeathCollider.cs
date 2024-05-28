using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCollider : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {


        if (other.tag != "Player") return;

        PlayerHandler.instance._playerResources.Die(true);
        //
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("gsdgd");
    }

}
