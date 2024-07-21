using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public  class QuestData : ScriptableObject
{
    [TextArea] public string quest_Description;
    



    public virtual void AddQuest(QuestClass _questClass)
    {
        if (PlayerHandler.instance == null) return;

        if (_questClass == null)
        {
            Debug.Log("the quest was null here");
            return;
        }
    }
    
    public virtual void RemoveQuest(QuestClass _questClass)
    {
        if (PlayerHandler.instance == null) return;

        if (_questClass == null)
        {
            Debug.Log("the quest was null here");
            return;
        }
    }

    public virtual bool CanAddQuests(QuestClass _questClass)
    {
        return true;
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