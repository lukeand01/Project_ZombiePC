using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "QuestSystem / Quest / Mine Resource")]
public class QuestData_MineResource : QuestData
{
    public override void AddQuest(QuestClass _questClass)
    {

        base.AddQuest(_questClass);

        PlayerHandler.instance._entityEvents.eventMinedResource += () => ProgressQuest(_questClass);

    }
    public override void RemoveQuest(QuestClass _questClass)
    {

        base.RemoveQuest(_questClass);

        PlayerHandler.instance._entityEvents.eventMinedResource -= () => ProgressQuest(_questClass);
    }

    void ProgressQuest(QuestClass _questClass)
    {
        //
        _questClass.ProgressQuest(1);

    }
}
