using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "QuestSystem / Quest / Find Box")]
public class QuestData_FindBox : QuestData
{
    [Separator("FIND BOX")]
    [SerializeField] ChestType targetChestType;

    public override void AddQuest(QuestClass _questClass)
    {



        base.AddQuest(_questClass);

        PlayerHandler.instance._entityEvents.eventOpenChest += (_chest) => ProgressQuest(_questClass, _chest);

    }
    public override void RemoveQuest(QuestClass _questClass)
    {
        
        base.RemoveQuest(_questClass);

        PlayerHandler.instance._entityEvents.eventOpenChest -= (_chest) => ProgressQuest(_questClass, _chest);
    }

    void ProgressQuest(QuestClass _questClass, ChestType _chestType)
    {
        //
        if(_chestType != targetChestType)
        {
            return;
        }
        _questClass.ProgressQuest(1);

    }
}
