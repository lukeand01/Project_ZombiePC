using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest_Drop : MonoBehaviour
{
    //on touching the player

    [SerializeField] DropData data;
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

        data.CallDrop();

        PlayerHandler.instance._entityStat.CallDropFadedUI(data.dropName);

        Destroy(gameObject);
    }

}
