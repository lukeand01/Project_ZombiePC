using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class QuestData : ScriptableObject
{
    [TextArea] public string quest_Description;
    [field:SerializeField] public int quest_amountRequired {  get; private set; }
    [field: SerializeField] public int quest_blessAmount { get; private set; }
    [field: SerializeField] public QuestType quest_Type { get; private set; }
    



    public virtual void AddQuest()
    {

    }
    
    public virtual void RemoveQuest()
    {

    }

    public virtual void FinishQuest()
    {
        //give a bless here.
        PlayerHandler.instance._playerResources.Bless_Gain(quest_blessAmount);
    }

}

//
public enum QuestType
{
    City,
    Curse,
    Challenge,

}