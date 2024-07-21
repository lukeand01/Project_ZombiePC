using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "QuestSystem / Quest / KillEnemy")]
public class QuestData_KillEnemy : QuestData
{

    //maybe we randomize the value_Level?
    //or give a list of possible values.
    //should i set the reward here? 
    //what if i want to give random attributes tot he player?
    //


    [SerializeField] EnemyData[] enemyAllowed;

    public override void AddQuest(QuestClass _questClass)
    {

        if (PlayerHandler.instance == null) return;

        if(_questClass == null)
        {
            Debug.Log("the quest was null here");
            return;
        }

        base.AddQuest(_questClass);



        //we assign the right event here.
        PlayerHandler.instance._entityEvents.eventKilledEnemy += (enemy, isPlayer) => ProgressQuest(_questClass, enemy, isPlayer);

    }
    public override void RemoveQuest(QuestClass _questClass)
    {
        base.RemoveQuest(_questClass);

        PlayerHandler.instance._entityEvents.eventKilledEnemy -= (enemy, isPlayer) => ProgressQuest(_questClass, enemy, isPlayer);
    }

   

    void ProgressQuest(QuestClass quest, EnemyBase enemy, bool isPlayer)
    {
        
        EnemyData data = enemy.GetData();

        if (data == null) return;

        bool hasPassed = false;

        if (enemyAllowed.Length == 0) hasPassed = true;

        foreach (var item in enemyAllowed)
        {
            if (item.ToString() == data.ToString())
            {
                hasPassed = true;
            }
        }

        if (hasPassed) quest.ProgressQuest(1);
    }


}
