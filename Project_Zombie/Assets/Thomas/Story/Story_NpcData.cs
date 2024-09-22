using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Story / NPC_Especial")]
public class Story_NpcData : ScriptableObject
{
    //these npcs are the unique ones.
    //

    [field: SerializeField] public string npcName { get; private set; }
    [field: SerializeField] public Sprite npcSprite { get; private set; }
    //i will put the dialogue here but also it needs to have triggers 
    [field: SerializeField] public Story_EspecialNpc npcModel { get; private set; }

    [field: SerializeField] public int houseIndex { get; private set; } = -1;
    //we use this to put the fella in the house. if its -1 then we are going to reset it.

    //[field: SerializeField] public AudioClip houseIndex { get; private set; } = -1;


    public void SetHouseIndex(int houseIndex)
    {
        this.houseIndex = houseIndex;
    }

    //alson i want different types of default.perphaps?
    //for now we will just use the first one.

    [SerializeField] List<Story_DialogueData> dialogueList = new();



    public Story_DialogueData GetDialogueData()
    {
        return dialogueList[0];
    }

}

