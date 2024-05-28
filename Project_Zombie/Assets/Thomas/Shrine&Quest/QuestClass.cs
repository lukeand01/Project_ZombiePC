using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class QuestClass 
{
    //the quest creates a questclass.
    //it will store the progress.

    public QuestData questData {  get; private set; }

    public int amountTotal {  get; private set; }
    public int amountCurrent { get; private set; }

    public QuestClass(QuestData data)
    {

        amountTotal = data.quest_amountRequired;
        amountCurrent = 0;

    }

}
