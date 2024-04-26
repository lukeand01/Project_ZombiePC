using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[CreateAssetMenu]
public class ItemTierHolder : ScriptableObject
{
    //we use this to hold the refenreces of weapons and most especifaicly to divided them into different groups for better access.

    //those divisions are: tier, type of weapons, type of damage,


    [SerializeField] List<ItemData> itemDataRefList = new();

    public Dictionary<TierType, List<ItemData>> dictionaryDividedByTier = new();

    [SerializeField] List<ItemChanceClass> currentChanceListBasedInLevel = new();


    public void ResetAllDivisions()
    {
        OrganizeTier();


    }


    void OrganizeTier()
    {
        dictionaryDividedByTier.Clear();

        foreach (var item in itemDataRefList)
        {
            if (dictionaryDividedByTier.ContainsKey(item.tierType))
            {
                dictionaryDividedByTier[item.tierType].Add(item);
            }
            else
            {
                dictionaryDividedByTier.Add(item.tierType, new List<ItemData> { item });
            }

        }

        //Debug.Log("organize has tier 1 " + dictionaryDividedByTier.ContainsKey(TierType.Tier1));
        //Debug.Log("organize has tier 2 " + dictionaryDividedByTier.ContainsKey(TierType.Tier2));



    }


    public List<ItemData> GetRandomListWithAmount(int amount)
    {
        List<ItemData> newList = new();
        List<ItemChanceClass> chanceList = currentChanceListBasedInLevel;

        while(newList.Count <= amount - 1)
        {
            int random = Random.Range(0, chanceList.Count);
            newList.Add(chanceList[random].data);
        }


        return newList;
    }

    public ItemData GetChosenItem(int level)
    {
        List<ItemChanceClass> chanceList = currentChanceListBasedInLevel;
        int roll = Random.Range(0, 101);
        ItemData chosenItem = null;

        int safeBreak = 0;

        while(chosenItem == null)
        {
            safeBreak++;

            if(safeBreak > 1000)
            {
                Debug.Log("i had to break so i will give just the first fella");
                return chanceList[0].data;
            }


            int random = Random.Range(0, chanceList.Count);

            if (chanceList[random].chance > roll)
            {
                //then this is the felal
                return chanceList[random].data;
            }
            else
            {
                roll -= 10; //reduces more by luck.
            }

        }


        return chosenItem;
    }

    //what i can do is that i build this fella everytime i change level. that way i dont need to worry about building it over and over again.



    public void GenerateNewChanceListBasedInLevel(int level)
    {
        currentChanceListBasedInLevel.Clear();
        List<ItemChanceClass> newList = GetRightChanceClass(level);

        foreach (var item in newList)
        {
            currentChanceListBasedInLevel.Add(new ItemChanceClass(item.data, item.chance));
        }
    }

    List<ItemChanceClass> GetRightChanceClass(int level)
    {
        float[] GetChanceBasedInTier = MyUtils.GetChanceForTierBasedInLevel(level);
        List<ItemChanceClass> newList = new();


        //Debug.Log(dictionaryDividedByTier.Count);

        if (GetChanceBasedInTier[0] > 0)
        {
            if (dictionaryDividedByTier.ContainsKey(TierType.Tier1))
            {
                //Debug.Log("level 1 tier");
                List<ItemData> firstList = dictionaryDividedByTier[TierType.Tier1];

                foreach (var item in firstList)
                {
                    newList.Add(new ItemChanceClass(item, (int)GetChanceBasedInTier[0]));
                }
            }
            else
            {
                //Debug.Log("no tier 1");
            }

            

        }

        if (GetChanceBasedInTier[1] > 0)
        {

            if (dictionaryDividedByTier.ContainsKey(TierType.Tier2))
            {
                //Debug.Log("level 2 tier");
                List<ItemData> firstList = dictionaryDividedByTier[TierType.Tier2];

                foreach (var item in firstList)
                {
                    newList.Add(new ItemChanceClass(item, (int)GetChanceBasedInTier[1]));
                }
            }
            else
            {
                //Debug.Log("no tier 2");
            }
               

        }

        if (GetChanceBasedInTier[2] > 0)
        {

            if (dictionaryDividedByTier.ContainsKey(TierType.Tier3))
            {
                Debug.Log("level 3 tier");
                List<ItemData> firstList = dictionaryDividedByTier[TierType.Tier3];

                foreach (var item in firstList)
                {
                    newList.Add(new ItemChanceClass(item, (int)GetChanceBasedInTier[2]));
                }
            }
            else
            {
                Debug.Log("no tier 3");
            }

            

        }

        if (GetChanceBasedInTier[3] > 0)
        {
            if (dictionaryDividedByTier.ContainsKey(TierType.Tier4))
            {
                Debug.Log("level 4 tier");
                List<ItemData> firstList = dictionaryDividedByTier[TierType.Tier4];

                foreach (var item in firstList)
                {
                    newList.Add(new ItemChanceClass(item, (int)GetChanceBasedInTier[3]));
                }
            }
            else
            {
                Debug.Log("no tier 4");
            }
            

        }

        return newList;
    }


   //actually i will just make it simple.
   //it will giev a chancec for each fella to spawn based in the myutils list
   //then we get a random fella and check if the roll is high enough. and we keep trying till we find someone.
   //each failed catch we reduce the roll by a fixed amount.
   
    //we need a way to show get this probability to the right fella.
}

[System.Serializable]
class ItemChanceClass
{
    public ItemData data;
    public int chance;

    public ItemChanceClass(ItemData data, int chance)
    {
        this.data = data;
        this.chance = chance;   
    }


}
