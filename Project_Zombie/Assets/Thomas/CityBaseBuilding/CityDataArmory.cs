using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CityData / Armory")]
public class CityDataArmory : CityData
{

    //we use this list this list to inform.

    [Separator("Armory_LeveLists")]
    [SerializeField] List<CityStoreArmoryClass_DividedByArmoryLevel> cityArmoryList_DividedByLevel_Perma = new(); //this is just for division.
    [SerializeField] List<CityStoreArmoryClass_DividedByArmoryLevel_WithChance> cityArmoryList_DividedByLevel_Temp = new(); //
    [Separator("Armory_AllRef")]
    [SerializeField] List<ItemGunData> allGunDataRef = new(); //these are all items. how toel perma from temp?

    [Separator("Armory_INDEXLISTS")]
    [SerializeField] List<int> ownedGunIndexList = new();
    [SerializeField] List<int> foundPermaGunIndexList = new();
    int cannotBuyLimit; //there should be a limit

    //we dont need to know whats owned because the temp we will just show 
    [Separator("Armory_Current owned lists")]
    [SerializeField] bool nothing;
    [field:SerializeField]public List<ItemGunData> currentGunAvailableArmoryList { get; private set; } = new(); //this is the list of all guns that should be avaialble in the store.
    [field: SerializeField] public List<ItemGunData> currentGunOwnedList { get; private set; } = new();
    [field: SerializeField] public List<ItemGunData> currentGunTempList { get; private set; } = new();


    //now lets check the 


    public void Initialize()
    {
        SetIndexOfAllGunDataRefs();
        GenerateAvailableGunList();
        GenerateGunPermaList();
        GenerateFoundGunList();
        GenerateGunTempList();
    }

    //how am i going to handle this thing?
    //how to make guns have different spawn chances?
    //maybe the chance should be in the itemgundata itself?
    //actually how it works is that there is a modifier.
    //the problem is that there is the same chance for later guns. thats a ingame. when you have luck you have a higher chance of getting weapons of higher 
    //

    //how does luck inlfuence it?
    //each luck should increase the chance of certain things.



    public void SetIndexOfAllGunDataRefs()
    {
        for (int i = 0; i < allGunDataRef.Count; i++)
        {
            allGunDataRef[i].SetIndex(i);
            allGunDataRef[i].SetHasBeenFound(false);
        }
    }


    public void AddGunWithIndex(int index)
    {
        //use this to add a gun to the currentBulletIndex.
        if(!ownedGunIndexList.Contains(index))
        {
            ownedGunIndexList.Add(index);


            if (allGunDataRef[index].isTemp)
            {
                GenerateGunTempList();
            }
            else
            {
                GenerateGunPermaList();
                GenerateAvailableGunList();
            }

            


        }

        




        //we need to somehow inform everyone.
        //we can do so that everytime you open the ui it updates.

    }

    #region PERMA
    //THIS MUST BE CALLED AT THE START

    public void GenerateGunPermaList()
    {
        //we check the int of both those versions and
        //

        currentGunOwnedList.Clear();
        foreach (var item in ownedGunIndexList)
        {
            //and we get this stuff and we put in the gunowned list
            if (!allGunDataRef[item].isTemp)
            {
                currentGunOwnedList.Add(allGunDataRef[item]);
            }
            
        }

        if(UIHandler.instance != null)
        {
            UIHandler.instance._EquipWindowUI.UpdateOptionForGunContainer(currentGunOwnedList);
        }


    }

    public void GenerateAvailableGunList()
    {
        //this is based in teh gun level and also the especial gunlist. what i will first add the level guns, and then i will add

        //its here we will tell it to always show if its a perma weapon

        //we generating 
        currentGunAvailableArmoryList.Clear();

        if (cityStoreLevel == 0)
        {
            //it has no guns
            return;
        }

      
        List<int> indexOfGunsAddedNowToAvaialable = new();
        for (int i = 0; i < cityStoreLevel; i++)
        {
            if(i >= cityArmoryList_DividedByLevel_Perma.Count)
            {
                Debug.Log("CUT");
                break;
            }
            //otherwise 

            foreach (var item in cityArmoryList_DividedByLevel_Perma[i].gunList)
            {
                item.SetHasBeenFound(true);
                currentGunAvailableArmoryList.Add(item);

                indexOfGunsAddedNowToAvaialable.Add(item.storeIndex);
            }

        }

        //now we check every gun that you have but its not yet in the list and add it tot he list.
        foreach (var item in currentGunOwnedList)
        {
            //and we are going to check who we have here but not there. and thats who we add.
            if (!indexOfGunsAddedNowToAvaialable.Contains(item.storeIndex))
            {
                //then we add.
                //Debug.Log("we dont have it in the list");
                item.SetHasBeenFound(true);
                currentGunAvailableArmoryList.Add(item);
            }
            else
            {
                Debug.Log("we have it in the list");
            }
        }

    }

    public void BuyGun(ItemGunData data)
    {
        //
        if (HasGun(data.storeIndex))
        {
            Debug.LogError("ALREADY HAS THIS GUN IN LIST " + data.itemName);
            return;
        }

        ownedGunIndexList.Add(data.storeIndex);
        currentGunOwnedList.Add(data);

        GenerateGunPermaList();

    }
   
    public bool HasGun(int index)
    {
        //but the problem is that we can confuse things between the two lists. the best would be to have all in one list.
 

        return ownedGunIndexList.Contains(index);
    }
    #endregion

    #region TEMP

    //what are my problme here?

    void GenerateFoundGunList()
    {
        //we use the currentBulletIndex gun to tell what temp guns have already been found.
        //we inform the right weapons that they have already been found.
        foreach (var item in foundPermaGunIndexList)
        {
            allGunDataRef[item].SetHasBeenFound(true);
        }
    }

    void GenerateGunTempList()
    {
        currentGunTempList.Clear();

        //i need to add those fellas

        
        for (int i = 0; i < cityStoreLevel; i++)
        {
            if (i > cityArmoryList_DividedByLevel_Temp.Count)
            {
                continue;
            }

            foreach (var item in cityArmoryList_DividedByLevel_Temp[i].gunList)
            {
                currentGunTempList.Add(item.data.GetGun());
            }
        }

        foreach (var item in ownedGunIndexList)
        {
            //but here everytime i am to add this fella i need to check if i havent already.

            if (allGunDataRef[item].isTemp)
            {
                currentGunTempList.Add(allGunDataRef[item]);
            }

        }

    }




    public void FoundNewGun(int foundIndex)
    {
        if(foundPermaGunIndexList.Contains(foundIndex))
        {
            Debug.Log("trying to add a fella that alreayd has here");
            return;
        }

        foundPermaGunIndexList.Add(foundIndex);
    }

    public List<ItemGunData> GetGunSpinningList()
    {
        //spinning is just a combinaation of weapons that i can get.
        //

        List<ItemGunData> spinningList = new();
        List<int> alreadyUsedGunIndexList = new();

        int safeBreak = 0;

        while(spinningList.Count < 5)
        {
            safeBreak++;
            if(safeBreak > 1000)
            {
                return spinningList;
            }

            int random = Random.Range(0, currentGunTempList.Count);

            if (alreadyUsedGunIndexList.Contains(random)) continue;

            spinningList.Add(currentGunTempList[random]);
            alreadyUsedGunIndexList.Add(random);
        }


        return spinningList;
    }
    public ItemGunData GetGunChosen()
    {
        //when i get a gun it needs to take into considreation the following:
        //each gun will have a different chance of being spawned
        //luck increases the chance of higher level spawning by lowering the chance of any weapons beloiw its level.

        //int luck = (int)PlayerHandler.instance._entityStat.GetTotalValue(StatType.Luck);
        int luck = debugLuck;
        //so thats how we are going to do. each level under the luck level you get penalty of -25
        //
        ItemGunData chosenGun = null;

        if(currentGunTempList.Count == 0)
        {
            Debug.Log("no gun temp list");
            return null;
        }

        int safeBreak = 0;

        while(chosenGun == null)
        {
            int random = Random.Range(0, currentGunTempList.Count);
            Debug.Log("this was the roll");

            ItemGunData data = currentGunTempList[random];


            safeBreak++;

            if(safeBreak > 1000)
            {
                Debug.Log("it broke safely");
                break;
            }

            //((int)data.tierType - luck + 1) * 40;

            //diff = mathf.ceil
            //

            //they cannot all be equally bad. 
            //if the luck is too high 

            int diff = ((int)data.tierType - luck);
            int luckRequired = MyUtils.GetChanceBasedInLuckAndTier(diff);

            int luckRoll = Random.Range(0, 101);

            //Debug.Log("this was the luck modifier " + luckRequired + " for " + data.name);
            //Debug.Log("this was the diff " + diff + " for " + data.name);

            if (luckRoll > luckRequired)
            {
                chosenGun = data;
            }
            else
            {
                continue;
            }



            if (data.additionalRollRequired > 0)
            {

                Debug.Log("it required another roll");
                int secondRoll = Random.Range(0, 101);

                if(secondRoll > data.additionalRollRequired)
                {
                    return data;
                }
                else
                {
                    chosenGun = null;
                }

            }





            //the problem is taht tier 2 should have more when you increase but thats not happening.
        }


        return chosenGun;
    }


    #endregion


    [SerializeField] List<ItemChanceClass> debug_ItemList = new();
    [SerializeField] int debugLuck;

    [ContextMenu("DEBUG CALL GUN ALGO")]
    public void Debug_CallGunAlgo()
    {

        //something isnt making me happy about the chances.



        debug_ItemList.Clear();
        for (int i = 0; i < 100; i++)
        {
            ItemGunData chosenGun = GetGunChosen();
            bool hasFound = false;

            for (int y = 0; y < debug_ItemList.Count; y++)
            {
                //if it finds 
                if (debug_ItemList[y].data == chosenGun)
                {
                    debug_ItemList[y].DebugIncreaseChance(1);
                    hasFound = true;
                }
            }

            if (!hasFound)
            {
                debug_ItemList.Add(new ItemChanceClass(chosenGun, 1));
            }

        }

    }

}

//so two important things
//first is that there should be a list of guns obtained by upgrading the 
//second is that guns should be obtainable by other means. so we are going to have two lists. 
//one list should tell when a guy is allowed to buy
//this tells the cost but i also need to tell 

[System.Serializable]
public class CityStoreArmoryClass_DividedByArmoryLevel
{
    public List<ItemGunData> gunList = new();

}

[System.Serializable]
public class CityStoreArmoryClass_DividedByArmoryLevel_WithChance
{
    public List<ItemChanceClass> gunList = new();
   

}




[System.Serializable]
public class CityStoreArmoryClass
{
    //this has the data for the weapon which we can use.
    //then we have a cost to unluck it 
    //and we have conditions for unluckint it
    //those conditions might be: level of a certain place, or a certain quest, or an achievement.
    [SerializeField] string GunName;
    [field: SerializeField] public ItemGunData data { get; private set; }
    [field: SerializeField] public RequirementToUnluckClass requirementToUnluck { get; private set; }

    public List<string> GetStringPriceList()
    {
        List<ResourceClass> resourceList = requirementToUnluck.requiredResourceList;
        List<string> stringList = new();
        

        foreach (var item in resourceList)
        {
            string text = "";

            text += item.data.itemName + ": ";
            text += item.quantity.ToString();

            stringList.Add(text);
        }

        return stringList;
    }


}