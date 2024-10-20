using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CityData / Main")]
public class CityData_Main : CityData
{

    [Separator("ESPECIAL NPC REF")]
    [SerializeField] List<Story_NpcData> especialNpcList_Ref = new();
    List<int> indexList_FoundNpc = new();

    public List<Story_NpcData> especialNpcList_Current { get; private set; } = new();

    [Separator("STORY QUEST REF")]
    [SerializeField] List<QuestClass> storyQuestList_Ref = new();


    public List<QuestClass> storyQuestList_Active { get; private set; } = new();
    public List<QuestClass> storyQuestList_Completed { get; private set; } = new();

    [Separator("INDEX LIST")]
    [SerializeField] List<int> indexList_ActiveQuests = new();
    [SerializeField] List<int> indexList_CompletedQuests = new();

    public void Initialize()
    {
        //put stuff here.

        GenerateList_Npc();
        GenerateList_Quest();
    }

    void GenerateList_Quest()
    {
        storyQuestList_Active.Clear();
        foreach (var item in indexList_ActiveQuests)
        {
            storyQuestList_Active.Add(storyQuestList_Ref[item]);
        }

        //then we get these quests to assign them to the player.

        foreach (var item in storyQuestList_Ref)
        {
            item.questData.AddQuest(item);
        }

        storyQuestList_Completed.Clear();
        foreach (var item in indexList_CompletedQuests)
        {
            storyQuestList_Completed.Add(storyQuestList_Ref[item]);
        }
    }
    void GenerateList_Npc()
    {
        foreach (var item in indexList_FoundNpc)
        {
            especialNpcList_Current.Add(especialNpcList_Ref[item]);
        }
    }

    public void AddQuestWithIndex(int index)
    {
        //we add to the list
        //and we 
        //first check if alçready have that item.

        if(indexList_ActiveQuests.Contains(index) || indexList_CompletedQuests.Contains(index))
        {
            Debug.Log("this quest is already present " );
            return;
        }

        //how to update this thing? i need to update
        Debug.Log("added quest");

        

        indexList_ActiveQuests.Add(index);
        storyQuestList_Active.Add(storyQuestList_Ref[index]);
        storyQuestList_Ref[index].questData.AddQuest(storyQuestList_Ref[index]);


        //i can just call them from here? instead of updating everytime it opens 
        //but i dont have reference for the ui so i 

        if (UIHandler.instance == null) return;

        UIHandler.instance._EquipWindowUI.UpdateOptionForStoryQuest(storyQuestList_Active);

    }

}
