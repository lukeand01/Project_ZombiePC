using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Room : MonoBehaviour
{
    //the room will hold spawn ref.
    [SerializeField] Transform portalHolder;
    [SerializeField] Transform chestGunPos;
    public List<Portal> portalList = new();
    [SerializeField] Transform[] gapWalls; //
    [SerializeField] bool checkDebug;
    [SerializeField] Shrine[] shrineArray; //we will do through this to check the shrines
    Shrine currentShrine;
    public string id {  get; private set; }


    private void Start()
    {
       ResetCallShrine();
    }

    [ContextMenu("DEBUG CALL SHRINE")]
    public void DebugCallShrine()
    {
        bool isSuccess = CallShrine();

        Debug.Log("call shrine was success " + isSuccess);
    }

    void ResetCallShrine()
    {
        for (int i = 0; i < shrineArray.Length; i++)
        {
            var item = shrineArray[i];

            item.transform.localPosition += new Vector3(0, -10, 0);
            item.gameObject.SetActive(false);

        }
    }

    public bool CallShrine()
    {
        //if there is already an active shrine it will return false.

        if (shrineArray.Length == 0) return false;
        if (currentShrine != null) return false;

        int random = UnityEngine.Random.Range(0, shrineArray.Length);
        currentShrine = shrineArray[random];

        currentShrine.gameObject.SetActive(true); 

        int random_QuestType = UnityEngine.Random.Range(0, 101);

        if(random_QuestType <= 80)
        {
            currentShrine.SetUp(QuestType.Bless, this);
        }
        else if(random_QuestType > 80)
        {
            currentShrine.SetUp(QuestType.Curse, this);
        }


        //then we set it up.


        return true;
    }

    public void RemoveShrine()
    {
        currentShrine = null;
    }

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


    public void OpenRoom_Room()
    {
        LocalHandler.instance.OpenRoom_LocalHandler(this, "From room");
        foreach (var item in portalList)
        {
            item.OpenForSpawn();       
        }

        foreach (var item in gapWalls)
        {
            //move all gapwalls down.
            item.DOLocalMoveY(transform.localPosition.y - 8, 3);
            item.DOScale(0, 5);
        }
    }

    public Transform GetChestGunSpawnPos() => chestGunPos;


}

//the spawn system must decided which room he wants to spawn the fella.
//



//at what spawn level are we are 
