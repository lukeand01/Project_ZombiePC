using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "TierHolder / Ability")]
public class AbilityTierHolder : ScriptableObject
{
    //these are the passive
    //i want to have passive that are always possible. but they rare.

    [SerializeField] List<AbilityPassiveData> passiveDataRefList = new();
    [SerializeField] List<AbilityPassiveData> passiveDataRefListEspecial = new();
    float spawnChanceForEspecial;
    [SerializeField] List<AbilityActiveData> activeDataRefList = new();


    Dictionary<TierType, List<AbilityPassiveData>> passiveDataDictionary = new();
    Dictionary<TierType, List<AbilityActiveData>> activeDataDictionary = new();

    //this list is based in the player gaining rolllevel. 
    [SerializeField] List<AbilityChanceClass> currentChanceListBasedInLevel = new();

    public void SetHolder()
    {
        OrganizeActiveTier();
        OrganizePassiveTier();
    }

    #region ORGANIZE
    void OrganizePassiveTier()
    {
        passiveDataDictionary.Clear();


        foreach (var item in passiveDataRefList)
        {
            if (passiveDataDictionary.ContainsKey(item.abilityTier))
            {
                passiveDataDictionary[item.abilityTier].Add(item);
            }
            else
            {
                passiveDataDictionary.Add(item.abilityTier, new List<AbilityPassiveData> { item });
            }

        }
    }

    void OrganizeActiveTier()
    {
        activeDataDictionary.Clear();


        foreach (var item in activeDataRefList)
        {
            if (activeDataDictionary.ContainsKey(item.abilityTier))
            {
                activeDataDictionary[item.abilityTier].Add(item);
            }
            else
            {
                activeDataDictionary.Add(item.abilityTier, new List<AbilityActiveData> { item });
            }

        }

        //Debug.Log("organize has tier 1 " + dictionaryDividedByTier.ContainsKey(TierType.Tier1));
        //Debug.Log("organize has tier 2 " + dictionaryDividedByTier.ContainsKey(TierType.Tier2));



    }
    #endregion

    //

    public List<AbilityPassiveData> GetPassiveChosenList(int amount)
    {
        //tweaks to the logic
        //there should be a higher chance of getting something the player already has.
        //also should not be be able to show the player something he can no longer stack.
        float luck = 0;

        if(PlayerHandler.instance != null)
        {
            luck = PlayerHandler.instance._entityStat.GetTotalValue(StatType.Luck);
        }


        List<int> indexList = new();
        List<int> indexListForEspecial = new();
        int safeBreak = 0;
        int roll = Random.Range(0, 101) + (int)(luck * 2);
        List<AbilityPassiveData> newList = new();


        List<AbilityPassiveData> forbiddenAbilityList = PlayerHandler.instance._playerAbility.abilityList_CannotStack;
        List<AbilityPassiveData> higherChanceAbilityList = PlayerHandler.instance._playerAbility.abilityList_HigherChance;


        if (currentChanceListBasedInLevel.Count <= 0)
        {
            Debug.Log("there is no list of chances");
            return newList;
        }


        while(amount > newList.Count)
        {
            safeBreak++;
            if(safeBreak > 1000)
            {
                Debug.Log("it broke off");
                return newList;
            }

            //right here in the beggining we check the chances to get an item.
            int rollForEspecial = Random.Range(0, 101);

            if(rollForEspecial > spawnChanceForEspecial && passiveDataRefListEspecial.Count > 0 && indexListForEspecial.Count <= 0)
            {
                //then we are going to pick a random from the espcial list to add.
                int randomEspecial = Random.Range(0, passiveDataRefListEspecial.Count);
                newList.Add(passiveDataRefListEspecial[randomEspecial]);
                indexListForEspecial.Add(randomEspecial);
                continue;
            }



            int random = Random.Range(0, currentChanceListBasedInLevel.Count);

            if (indexList.Contains(random))
            {
                continue;
            }

            AbilityChanceClass abilityChanceClass = currentChanceListBasedInLevel[random];
            AbilityPassiveData ability = abilityChanceClass.data.GetPassive();

            if (ability == null)
            {
                Debug.Log("this item is not pasive");
                continue;
            }
            
            if(forbiddenAbilityList.Contains(ability))
            {
                Debug.Log("blocked");
                continue;
            }

            if (higherChanceAbilityList.Contains(ability))
            {

                newList.Add(ability);
                indexList.Add(random);
                continue;
            }

            if (abilityChanceClass.chance > roll)
            {
                newList.Add(ability);
                indexList.Add(random);
            }
            else
            {
                roll -= 10; //reduces more by luck.
            }

        }

        return newList;

    }




    public void GenerateNewChanceListBasedInLevel(int level)
    {
        currentChanceListBasedInLevel.Clear();
        List<AbilityChanceClass> newList = GetRightChanceClass(level);

        foreach (var item in newList)
        {
            currentChanceListBasedInLevel.Add(new AbilityChanceClass(item.data, item.chance));
        }
    }


    List<AbilityChanceClass> GetRightChanceClass(int level)
    {
        float[] GetChanceBasedInTier = MyUtils.GetChanceForTierBasedInLevel(level);
        List<AbilityChanceClass> newList = new();


        //Debug.Log(dictionaryDividedByTier.Count);

        if (GetChanceBasedInTier[0] > 0)
        {
            if (passiveDataDictionary.ContainsKey(TierType.Tier1))
            {
                //Debug.Log("level 1 tier");
                List<AbilityPassiveData> firstList = passiveDataDictionary[TierType.Tier1];

                foreach (var item in firstList)
                {
                    newList.Add(new AbilityChanceClass(item, (int)GetChanceBasedInTier[0]));
                }
            }
            else
            {
                //Debug.Log("no tier 1");
            }



        }

        if (GetChanceBasedInTier[1] > 0)
        {

            if (passiveDataDictionary.ContainsKey(TierType.Tier2))
            {
                //Debug.Log("level 2 tier");
                List<AbilityPassiveData> firstList = passiveDataDictionary[TierType.Tier2];

                foreach (var item in firstList)
                {
                    newList.Add(new AbilityChanceClass(item, (int)GetChanceBasedInTier[1]));
                }
            }
            else
            {
                //Debug.Log("no tier 2");
            }


        }

        if (GetChanceBasedInTier[2] > 0)
        {

            if (passiveDataDictionary.ContainsKey(TierType.Tier3))
            {
                Debug.Log("level 3 tier");
                List<AbilityPassiveData> firstList = passiveDataDictionary[TierType.Tier3];

                foreach (var item in firstList)
                {
                    newList.Add(new AbilityChanceClass(item, (int)GetChanceBasedInTier[2]));
                }
            }
            else
            {
                Debug.Log("no tier 3");
            }



        }

        if (GetChanceBasedInTier[3] > 0)
        {
            if (passiveDataDictionary.ContainsKey(TierType.Tier4))
            {
                Debug.Log("level 4 tier");
                List<AbilityPassiveData> firstList = passiveDataDictionary[TierType.Tier4];

                foreach (var item in firstList)
                {
                    newList.Add(new AbilityChanceClass(item, (int)GetChanceBasedInTier[3]));
                }
            }
            else
            {
                Debug.Log("no tier 4");
            }


        }


        spawnChanceForEspecial = GetChanceBasedInTier[4];

        return newList;
    }

    

}

public class AbilityChanceClass
{
    public AbilityBaseData data;
    public int chance;

    public AbilityChanceClass(AbilityBaseData data, int chance)
    {
        this.data = data;
        this.chance = chance;
    }
}