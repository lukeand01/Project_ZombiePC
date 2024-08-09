using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu]
public class StageData : ScriptableObject
{
    //this dicatates what enemies can spawn at this stage.
    //each champion has a chance.
    //its always between 1 and 25.
    //we will use this thing also teo tell what resources we can get here.
    //this will also inform events.like events in certain levels. for example:> spawn a certain fella. spawn a boss. shower of meteors. gas.


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
    public List<EnemyChanceSpawnClass> GetCompleteSpawnList(int level)
    {
        List<EnemyChanceSpawnClass> newList = GetAllowedEnemySpawnList(level);
        newList = GetUpdateEnemySpawnList(level, newList);



        return newList;
    }


    List<EnemyChanceSpawnClass> GetAllowedEnemySpawnList(int spawn)
    {
        //we use this list to get only those who are even allowed to be spawn at the moment basde in the max cap
        //
        List<EnemyChanceSpawnClass> newList = new();

        foreach (EnemyChanceSpawnClass item in classList)
        {
            if (item.max != 0 && spawn > item.max)
            {
                //we dont add this fella
                continue;
            }
            if (item.min != 0 && spawn < item.min)
            {
                //we dont add this fella
                continue;
            }
            newList.Add(item);
        }

        return newList;
    }

    List<EnemyChanceSpawnClass> GetUpdateEnemySpawnList(int level, List<EnemyChanceSpawnClass> targetList)
    {
        //we use this list to alter the spawn cahnce values based in the level.
        List<EnemyChanceSpawnClass> newList = new();

        //we calculate the thing.

        foreach (var item in targetList)
        {

            float chanceScale = (item.chancePerLevelToSpawn * level);
            chanceScale = Mathf.Clamp(chanceScale, item.chanceSpawnMinCap, item.chanceSpawnMaxCap);

            float chance = item.baseChanceToSpawn + chanceScale;

            EnemyChanceSpawnClass enemySpawn = new EnemyChanceSpawnClass(item.data, chance, item.maxAllowedAtAnyTime);
            newList.Add(enemySpawn);
        }


        return newList;

    }
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

    public List<QuestClass> GetQuestList()
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



    #endregion

}
[System.Serializable]
public class EnemyChanceSpawnClass
{
    public EnemyChanceSpawnClass(EnemyChanceSpawnClass refClass)
    {
        data = refClass.data;

        min = refClass.min;
        max = refClass.max;

        baseChanceToSpawn = refClass.baseChanceToSpawn;
        chancePerLevelToSpawn = refClass.chancePerLevelToSpawn;

        chanceSpawnMaxCap = refClass.chanceSpawnMaxCap;
        chanceSpawnMinCap = refClass.chanceSpawnMinCap;

        maxAllowedAtAnyTime = refClass.maxAllowedAtAnyTime;
    }

    public EnemyChanceSpawnClass(EnemyData data, float totalChanceSpawn, int maxAllowedAtAnyTime)
    {
        this.data = data;
        this.totalChanceSpawn = totalChanceSpawn;
        this.maxAllowedAtAnyTime = maxAllowedAtAnyTime;
    }


    [SerializeField] string Id;

    public EnemyData data;
    [Range(0, 25)] public int min = 1;
    [Range(0, 25)] public int max = 25; //max 

    public float totalChanceSpawn { get; private set; }
    [Range(0,100)]public float baseChanceToSpawn;
    [Range(-100,100)]public float chancePerLevelToSpawn;

    [Range(0, 100)] public float chanceSpawnMaxCap;
    [Range(0,100)] public float chanceSpawnMinCap;  

    [Range(0, 100)]public int maxAllowedAtAnyTime; //you can never spawn more creatures than the allowed. 0 means there is no need to check this.
}
