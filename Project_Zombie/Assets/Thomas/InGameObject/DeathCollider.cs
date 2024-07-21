using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCollider : MonoBehaviour
{
    [SerializeField] bool isItFall;
    private void OnTriggerEnter(Collider other)
    {


        if (other.tag != "Player") return;


        if (isItFall)
        {
            //


        }
        else
        {

        }

        PlayerHandler.instance.CallDeathByFalling();
        //
    }


}
