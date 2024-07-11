using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City_House : MonoBehaviour
{

    [SerializeField] GameObject graphicHolder;
    [SerializeField] Transform especialNpcPosition;

    public Story_NpcData npc {  get; private set; }

    public bool isActive {  get; private set; }


    public void RemoveFromCity()
    {
        //we hide them underground
        graphicHolder.transform.localPosition = new Vector3(0, -4, 0);

        isActive = false;
    }

    public void AddToCity()
    {
        //we bring them to the upper level.
        graphicHolder.transform.localPosition = Vector3.zero;

        isActive = true;
    }

    public void SpawnEspecialNPC(Story_NpcData npc)
    {

        if(npc != null)
        {
            Debug.LogError("already has npc here " + gameObject.name);
        }
        //then we will spawn them always in front of the house. maybe sitting, maybe chilling, mayb standing like a soldier.
        Story_EspecialNpc newObject = Instantiate(npc.npcModel, especialNpcPosition.position, Quaternion.identity);
        newObject.transform.position = especialNpcPosition.position;

        this.npc = npc;
    }


}


//what else do i want tin the storecanvas?
//the name of the construction. its level.
//the resource canvas should show how much it requires and how much it has.
//it would be really cool if when we update we moved back and saw the new build rise. that would reward the player for improving.
//