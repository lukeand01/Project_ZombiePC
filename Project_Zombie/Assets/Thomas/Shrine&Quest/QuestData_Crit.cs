using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "QuestSystem / Quest / Crit")]
public class QuestData_Crit : QuestData
{
    public override void AddQuest(QuestClass _questClass)
    {

        base.AddQuest(_questClass);

        PlayerHandler.instance._entityEvents.eventCrit += () => ProgressQuest(_questClass);

    }
    public override void RemoveQuest(QuestClass _questClass)
    {

        base.RemoveQuest(_questClass);

        PlayerHandler.instance._entityEvents.eventCrit -= () => ProgressQuest(_questClass);
    }

    void ProgressQuest(QuestClass _questClass)
    {
        //
        
        _questClass.ProgressQuest(1);

    }
}
