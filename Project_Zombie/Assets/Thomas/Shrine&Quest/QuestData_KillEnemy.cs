using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest / KillEnemy")]
public class QuestData_KillEnemy : QuestData
{

    //maybe we randomize the value?
    //or give a list of possible values.
    //should i set the reward here? 
    //what if i want to give random attributes tot he player?
    //

    public override void AddQuest()
    {
        base.AddQuest();
    }
    public override void RemoveQuest()
    {
        base.RemoveQuest();
    }
    public override void FinishQuest()
    {
        base.FinishQuest();
    }
}
