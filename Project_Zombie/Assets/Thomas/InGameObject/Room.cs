using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    //the room will hold spawn ref.
    [SerializeField] Transform portalHolder;
    [SerializeField] Transform chestGunPos;
    public List<Portal> portalList = new();


    public string id {  get; private set; }



    private void Awake()
    {
        id = Guid.NewGuid().ToString();


        if (portalHolder == null) return;
        for (int i = 0; i < portalHolder.transform.childCount; i++)
        {
            Portal portal = portalHolder.transform.GetChild(i).GetComponent<Portal>();

            if (portal == null) continue;

            portalList.Add(portal);
        }
    }


    public void OpenRoom()
    {
        LocalHandler.instance.OpenRoom(this);
        foreach (var item in portalList)
        {
            item.OpenForSpawn();
            
        }
    }

    public Transform GetChestGunSpawnPos() => chestGunPos;


}

//the spawn system must decided which room he wants to spawn the fella.
//



//at what spawn level are we are 
