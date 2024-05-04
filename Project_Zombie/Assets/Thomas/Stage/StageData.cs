using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StageData : ScriptableObject
{
    //this dicatates what enemies can spawn at this stage.
    //each champion has a chance.
    //its always between 1 and 25.
    //we will use this thing also teo tell what resources we can get here.
    //this will also inform events.like events in certain levels. for example:> spawn a certain fella. spawn a boss. shower of meteors. gas.

    #region  ENEMIES
    [Separator("Enemy")]
    public List<EnemyChanceSpawnClass> classList = new(); 

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
            if(item.max != 0 && spawn > item.max)
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
            float chance = item.baseChanceToSpawn + item.chancePerLevelToSpawn * level;
            chance = Mathf.Clamp(chance, item.chanceSpawnMinCap, item.chanceSpawnMaxCap);
            EnemyChanceSpawnClass enemySpawn = new EnemyChanceSpawnClass(item.data,chance, item.maxAllowedAtAnyTime);
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


        if(itemChanceList.Count <= 0)
        {
            return new ItemClass(null, 0);
        }

        while (safeBreak < 1000)
        {
            safeBreak++;

            int randomItem = Random.Range(0, itemChanceList.Count);
            ItemChanceClass itemChance = itemChanceList[randomItem];

            if(itemChance.chance >= roll)
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




    public EnemyData data;
    [Range(0, 25)] public int min = 1;
    [Range(0, 25)] public int max = 25; //max 

    public float totalChanceSpawn;
    [Range(0,100)]public float baseChanceToSpawn;
    [Range(-100,100)]public float chancePerLevelToSpawn;

    [Range(0, 100)] public float chanceSpawnMaxCap;
    [Range(0,100)] public float chanceSpawnMinCap;  

    [Range(0, 15)]public int maxAllowedAtAnyTime; //you can never spawn more creatures than the allowed. 0 means there is no need to check this.
}
