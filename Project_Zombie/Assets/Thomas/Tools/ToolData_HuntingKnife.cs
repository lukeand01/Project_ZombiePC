using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tool / Tool_HuntingKnife")]
public class ToolData_HuntingKnife : ToolData
{

    //we need to check for _enemy killed.
    //and for 

    [SerializeField] bool shouldCheckForBoss; //otherwise we connect to the other variable.
    [SerializeField] IngredientClass_Enemy[] ingredientClassArray;

    public override void OnAdded()
    {
        if (shouldCheckForBoss)
        {
            PlayerHandler.instance._entityEvents.eventKillBoss += RollForHarvest_Boss;
        }
        else
        {
            PlayerHandler.instance._entityEvents.eventKilledEnemy += RollForHarvest_Enemy;
        }
    }

    void RollForHarvest_Enemy(EnemyBase enemy, bool wasPlayer)
    {
        if (!wasPlayer) return;

        int random = Random.Range(0, 101);

        if(random > 92)
        {

            for (int i = 0; i < ingredientClassArray.Length; i++)
            {
                var item = ingredientClassArray[i];

                if (item.enemyData == enemy.GetData())
                {
                    item.Ingredientdata.OnHarvested(this);
                    PlayerHandler.instance._playerInventory.AddIngredient(this, item.Ingredientdata, 1);
                    return;
                }
            }

        }

    }

    void RollForHarvest_Boss(EnemyBoss boss)
    {
        //you always get something.
        
        for (int i = 0; i < ingredientClassArray.Length; i++)
        {
            var item = ingredientClassArray[i];

            if(item.enemyData == boss.GetBossData)
            {
                PlayerHandler.instance._playerInventory.AddIngredient(this, item.Ingredientdata, 1);
                return;
            }
        }

    }

    public override void OnRemoved()
    {
        if (shouldCheckForBoss)
        {
            PlayerHandler.instance._entityEvents.eventKillBoss -= RollForHarvest_Boss;
        }
        else
        {
            PlayerHandler.instance._entityEvents.eventKilledEnemy -= RollForHarvest_Enemy;
        }
    }
}


public class IngredientClass_Enemy
{
    public IngredientData Ingredientdata;
    public EnemyData enemyData;
}

//you ahve especial