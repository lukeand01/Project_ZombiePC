using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest_Ammo : MonoBehaviour
{
    [SerializeField] [Range(0,1)] float ammoRecoveryPercent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;

        PlayerHandler.instance._playerCombat.RecoverReserveAmmoByPercent(ammoRecoveryPercent);
        PlayerHandler.instance._entityStat.CallDropFadedUI("Small Ammo Box");

        Destroy(gameObject);

        //might want to put this in the pooling but for now i wont.
    }

}
