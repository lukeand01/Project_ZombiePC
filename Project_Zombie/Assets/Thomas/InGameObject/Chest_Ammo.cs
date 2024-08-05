using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest_Ammo : MonoBehaviour
{
    [SerializeField] [Range(0,1)] float ammoRecoveryPercent;
    [SerializeField] GameObject graphic;

    //this should always be a bit above the 

    private void FixedUpdate()
    {
        //we just rotate the fella ehre
        graphic.transform.Rotate(new Vector3(0, 0.5f, 0));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;

        PlayerHandler.instance._playerCombat.RecoverReserveAmmoByPercent(ammoRecoveryPercent);
        PlayerHandler.instance._entityStat.CallDropFadedUI("Small Ammo Box");

        Destroy(gameObject);

        //might want to put this in the pooling but for now i wont.
    }

}
