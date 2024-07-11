using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParty : MonoBehaviour
{
    //i will only be storing importnt npcs here.

    //i will assign the playerbase 

    [field: SerializeField] public List<Story_NpcData> npcList { get; private set; } = new();

    public int especialNpcLimit {  get; private set; }

    //this limit is set by the pop caluclation


    public void AddNpc(Story_NpcData newNpc)
    {
        if(npcList.Contains(newNpc))
        {
            Debug.Log("tried to add this npc but i alreayd have it");
            return;
        }

        npcList.Add(newNpc);

        //
    }

    public void SetEspecialNpcLimit(int limit)
    {
        especialNpcLimit = limit;
    }
    public void IncreaseEspecialNpcLimit(int limit)
    {
        especialNpcLimit += limit;
    }

    public bool HasSpaceForEspecialLimit()
    {
        return especialNpcLimit > npcList.Count;
    }

}
