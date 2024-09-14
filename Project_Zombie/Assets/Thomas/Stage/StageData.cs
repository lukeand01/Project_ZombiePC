using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class StageData : ScriptableObject
{
    //this dicatates what enemies can round at this stage.
    //each champion has a chance.
    //its always between 1 and 25.
    //we will use this thing also teo tell what resources we can get here.
    //this will also inform events.like events in certain levels. for example:> round a certain fella. round a boss. shower of meteors. gas.


    [Separator("Description")]
    public string stageName;
    public Sprite stageSprite;
    public int stageIndex;

    [Separator("ROUND SYSTEM")]
    public float enemyPerIntervalModifier = 1; //the amount of fellas per interval.
    public List<RoundClass> especialRoundList = new();



    #region  ENEMIES
    [Separator("Enemy")]
    public List<EnemyChanceSpawnClass> classList = new();
    //the enemies that can be spawned in this thing.

    //then we need to update them.
    public List<EnemyChanceSpawnClass> GetCompleteSpawnList(int round)
    {
        List<EnemyChanceSpawnClass> newList = GetAllowedEnemySpawnList(round);


        return newList;
    }


    List<EnemyChanceSpawnClass> GetAllowedEnemySpawnList(int round)
    {
        //we use this list to get only those who are even allowed to be round at the moment basde in the max cap
        
        //this we fomr the list.


        List<EnemyChanceSpawnClass> newList = new();

        foreach (EnemyChanceSpawnClass item in classList)
        {

            item.UpdateRound(round);

            //we only refuse to put this fella here when its 0 because -1 means that there is no limit.
            if (item.GetChanceToSpawn() <= 0) continue;

            newList.Add(item);
        }



        return newList;
    }

    //we dont need this anymore. because this is updating the modifier but the animation curve is already doing that.

    #endregion

    #region ITEMS
    [Separator("Item")]
    [SerializeField] List<ItemChanceClass> itemChanceList = new();

    public ItemClass GetItem()
    {
        int roll = Random.Range(0, 101);
        int luck = 0; //

        int safeBreak = 0;


        if (itemChanceList.Count <= 0)
        {
            return new ItemClass(null, 0);
        }

        while (safeBreak < 1000)
        {
            safeBreak++;

            int randomItem = Random.Range(0, itemChanceList.Count);
            ItemChanceClass itemChance = itemChanceList[randomItem];

            if (itemChance.chance >= roll)
            {
                int quantity = Random.Range(itemChance.minAmount, itemChance.maxAmount);
                return new ItemClass(itemChance.data, quantity);
            }
            else
            {
                roll -= 10 + luck;
            }

        }

        Debug.Log("it didnt work");
        return new ItemClass(null, 0);
    }


    #endregion

    #region QUESTS FOR BLESS
    [Separator("QUEST FOR BLESS")]
    [SerializeField] List<QuestClass> questList_Bless = new();
    [SerializeField] List<QuestClass> questList_Challenge = new();
    [SerializeField] List<QuestClass> questList_Curse = new(); //all curses allowed in this thing 

    public List<QuestClass> GetQuestListUsingNothing()
    {
        
        

        int roll = Random.Range(0, 101);

        if(questList_Curse.Count <= 0)
        {
            roll += 11;

        }
        if (questList_Challenge.Count <= 0)
        {
            roll += 11;
 
        }

        if (roll >= 0 && roll <= 10 )
        {
            //then its curse

            int roll_Curse = Random.Range(0, questList_Curse.Count);
            return new List<QuestClass>() { questList_Curse[roll_Curse], null, null };

        }
        if(roll > 10 && roll <= 20)
        {
            int roll_Challenge = Random.Range(0, questList_Challenge.Count);
            return new List<QuestClass>() { questList_Challenge[roll_Challenge], null, null };
        }
        if(roll > 20)
        {
            //hten its normal bless
            List<int> alreadyUsedIndexList = new();
            List<QuestClass> questList = new();
            int brakeLimit = 0;

            

            while(questList.Count < 3)
            {
                brakeLimit++;

                if(brakeLimit > 10000)
                {
                    //then we will just fill in the list as better as we can.

                    Debug.Log("the list to find quest bless broke off");
                    return null;
                }


                int roll_Bless = Random.Range(0, questList_Bless.Count);

                if (alreadyUsedIndexList.Contains(roll_Bless))
                {
                    continue;
                }

                questList.Add(questList_Bless[roll_Bless]);
                alreadyUsedIndexList.Add(roll_Bless);

            }

            return questList;
        }


        return null;
    }

    public QuestClass GetSingleQuestListUsingQuestType(QuestType _type)
    {
        List<QuestClass> rightList = new();

        if(_type == QuestType.Bless)
        {
            rightList = questList_Bless;
        }
        if(_type == QuestType.Curse)
        {
            rightList = questList_Curse;
        }

        int random = Random.Range(0, rightList.Count);
        return rightList[random];
    }

    #endregion

}
[System.Serializable]
public class EnemyChanceSpawnClass
{

    [SerializeField] string Id;

    public EnemyData data;


    public float GetChanceToSpawn()
    {
        return current_SpawnChance;
    }

    //allowed to round

    public int GetMaxAllowedToSpawn()
    {
        return current_MaxSpawn;
    }

    //the spawn chance will now envolve a number, when i want the 

    //i dont want to iterate everytime

    [SerializeField] ValuePerRoundClass[] _spawnChanceArray;
    [SerializeField] ValuePerRoundClass[] _maxSpawnCapArray;


    //
    int current_SpawnChance;
    int current_MaxSpawn;

    //how do we tell when this start spawning
    //and how do we tell thatthere is no limit to spawn
    //if its zero its because 

    public void UpdateRound(int round)
    {
        //get the values here.
        //we check every value.
        //we get the closest  value that it is lower.

        int storedValue_SpawnRound = 0;
        float storedValue_SpawnChance = 0;

        for (int i = 0; i < _spawnChanceArray.Length; i++)
        {
            var item = _spawnChanceArray[i];

            if(round >= item.round && item.round > storedValue_SpawnRound)
            {
                storedValue_SpawnRound = item.round;
                storedValue_SpawnChance = item.value;
            }

        }

        current_SpawnChance = (int)storedValue_SpawnChance;


        int storedValue_spawnCapRound = 0;
        float storedValue_SpawnCapMax = 0;


        for (int i = 0; i < _maxSpawnCapArray.Length; i++)
        {
            var item = _maxSpawnCapArray[i];

            if (round >= item.round && item.round > storedValue_spawnCapRound)
            {
                storedValue_spawnCapRound = item.round;
                storedValue_SpawnCapMax = item.value;
            }

        }



        current_MaxSpawn = (int)storedValue_SpawnCapMax;

    }


}

[System.Serializable]
public class ValuePerRoundClass
{
    [Range(1, 30)] public int round = 1;
    public float value;
}