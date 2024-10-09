using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City_House : MonoBehaviour
{
    [SerializeField] GameObject graphicHolder;
    [SerializeField] GameObject[] graphicArray;
    [SerializeField] Transform especialNpcPosition;

    public Story_NpcData npc {  get; private set; }

    public bool isActive {  get; private set; }

    void UpdateGraphic()
    {
        //we check the main one and increase the level

        int mainLevel = GameHandler.instance.cityDataHandler.cityMain.cityStoreLevel / 2;
        int cappedLevel = Mathf.Clamp(mainLevel, 1, 5) - 1;


        for (int i = 0; i < graphicArray.Length; i++)
        {
            graphicArray[i].SetActive(cappedLevel == i - 1);
        }
    }

    public void RemoveFromCity()
    {
        //we hide them underground
        UpdateGraphic();
        graphicHolder.SetActive(false);
        isActive = false;
    }

    public void AddToCity()
    {
        //we bring them to the upper level.
        UpdateGraphic();
        graphicHolder.SetActive(true);

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