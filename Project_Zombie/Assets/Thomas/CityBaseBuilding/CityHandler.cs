using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityHandler : MonoBehaviour
{
    //this will control stuff events and quests.


    public static CityHandler instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UIHandler.instance.ControlUI(true);
        PlayerHandler.instance._playerController.block.AddBlock("City", BlockClass.BlockType.Combat);
        
    }


    //i need to store the items i already have.
    //i should the info about the progress in the data?
    //only the armoy needs to know if i own the place or not.

}
