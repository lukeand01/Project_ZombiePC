using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class QuestData : ScriptableObject
{
    [TextArea] public string quest_Description;
    



    public virtual void AddQuest(QuestClass _questClass)
    {

    }
    
    public virtual void RemoveQuest(QuestClass _questClass)
    {

    }

    public virtual void FinishQuest(QuestClass _questClass)
    {
        //give a bless here.
        //PlayerHandler.instance._playerResources.Bless_Gain(quest_blessAmount);
    }

}

//
public enum QuestType
{
    Bless,
    Curse,
    Challenge,

}