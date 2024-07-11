using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest_Drop : MonoBehaviour
{
    //on touching the player

    [SerializeField] DropData data;



    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;

        data.CallDrop();

        PlayerHandler.instance._entityStat.CallDropFadedUI(data.dropName);

        Destroy(gameObject);
    }

}
