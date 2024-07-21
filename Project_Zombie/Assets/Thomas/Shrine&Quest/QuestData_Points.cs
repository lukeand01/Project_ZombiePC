using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "QuestSystem / Quest / Points")]
public class QuestData_Points : QuestData
{

    //we can control if we should how much is spent or how much is gained.
    [Separator("Points")]
    [SerializeField] bool shouldCheckForSpend;

    public override void AddQuest(QuestClass _questClass)
    {

        base.AddQuest(_questClass);

        PlayerHandler.instance._entityEvents.eventChangedPoints += (points) => ProgressQuest(_questClass, points);

    }
    public override void RemoveQuest(QuestClass _questClass)
    {

        base.RemoveQuest(_questClass);

        PlayerHandler.instance._entityEvents.eventChangedPoints -= (points) => ProgressQuest(_questClass, points);
    }

    void ProgressQuest(QuestClass _questClass, int pointChange)
    {
        //

        if(shouldCheckForSpend && pointChange < 0 || !shouldCheckForSpend && pointChange > 0)
        {
            _questClass.ProgressQuest(1);
        }


    }


}
