using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityNpcBase : MonoBehaviour
{
    //i want this thing to have a basic routine. it goes to the target, pretends to work, or pretends to talk with another npc and keeps on looping.

    [SerializeField] List<CityNpc_TaskClass> taskList = new();




}

[System.Serializable]
public class CityNpc_TaskClass
{
    //this is the loop of things the npc is will do.

    public CityWorkSpot workSpot;
    public float duration;


}

//the place needs to hold a reference to the npc as well. 
//i think only the stop should reference to the duration and the npc decides if he is looping that.

//how to handle the workspots? 
//the workspot will tell the player if its done doing its activity.



//the ui of the mainbase should only show the thing ready for an upgrade and stats about the base.
//need a way to get the store and the ui to be more standardized.
//