using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CityData / Lab")]
public class CityDataLab : CityData
{
    //in here we are going to put all information regarding the abilities and what abilities should appear.



    [Separator("LEVEL LISTS")]
    [SerializeField] List<CityStoreLabClass_DividedByLevel> cityLab_ByLevel_List = new();
    [SerializeField] List<CityStoreLabClass_DividedByLevel_Passive> cityLabPassive_ByLevel_List = new();

    [Separator("REF LISTS")]
    [SerializeField] List<AbilityActiveData> abilityRefList_Active = new(); //this will be all abilities
    [SerializeField] List<AbilityPassiveData> abilityRefList_Passive = new(); //this will be the reference for all passive abilities.
    [SerializeField] List<AbilityPassiveData> abilityRefList_Curse = new(); //for now i will keep them here.

    [Separator("INDEX LIST")]
    [SerializeField] List<int> ownedAbilityIndexList_Active = new(); //what the player owns. we use this to create the list. but only for active.
    [SerializeField] List<int> ownedAbilityIndexList_Passive = new();
    [SerializeField] List<int> foundPassiveAbilityIndexList = new(); //we need this to tell all fellas that were already found. at the end of every stage we recrete this list checking every 

    //these are the actually lists.

    [Separator("List")]
    [SerializeField] bool sdf;
    [field:SerializeField] public List<AbilityActiveData> currentActiveAbilityOwnedList { get; private set; } = new(); //these are the list for active
    [field: SerializeField] public List<AbilityActiveData> currentActiveAbilityAvailableList { get; private set; } = new();

    [field: SerializeField] public List<AbilityPassiveData> currentPassiveAbilityList { get; private set; } = new();


    public void RestoreState(SaveClass saveClass)
    {
        ownedAbilityIndexList_Active = saveClass._lab_OwnedList_Active;
        ownedAbilityIndexList_Passive = saveClass._lab_OwnedList_Passive;

        foundPassiveAbilityIndexList = saveClass._lab_FoundList;
    }
    public void CaptureState(SaveClass saveClass)
    {
        saveClass.MakeOwnedList_Active_Lab(ownedAbilityIndexList_Active);
        saveClass.MakeOwnedList_Passive_Lab(ownedAbilityIndexList_Passive);

        saveClass.MakeFoundList_Lab(foundPassiveAbilityIndexList);

    }



    public void Initialize()
    {
        
        SetIndexOfAllAbilityDataRefs();
        GenerateOwnedActiveAbilityList();
        GenerateAvailableAbilityList();
        GeneratePassiveAbilityList();
        GeneratePassiveAbilityFoundList();
    }

    public void SetIndexOfAllAbilityDataRefs()
    {
        for (int i = 0; i < abilityRefList_Active.Count; i++)
        {
            abilityRefList_Active[i].SetIndex(i);
        }
    }

    public void GenerateOwnedActiveAbilityList()
    {
       

        currentActiveAbilityOwnedList.Clear();
        foreach (var item in ownedAbilityIndexList_Active)
        {
            //and we get this stuff and we put in the gunowned list
            abilityRefList_Active[item].SetFound(true);
            currentActiveAbilityOwnedList.Add(abilityRefList_Active[item].GetActive());
        }
    }

    public void GenerateAvailableAbilityList()
    {
        
        //available is always all the owned and by list.

        currentActiveAbilityAvailableList.Clear();

        if (cityStoreLevel == 0)
        {
            //it has no guns
            return;
        }


        List<int> indexOfGunsAddedNowToAvaialable = new();
        for (int i = 0; i < cityStoreLevel; i++)
        {
            if (i >= cityLab_ByLevel_List.Count)
            {
                Debug.Log("CUT");
                break;
            }
            //otherwise 

            foreach (var item in cityLab_ByLevel_List[i].dataList)
            {
                item.SetFound(true);
                currentActiveAbilityAvailableList.Add(item);
                indexOfGunsAddedNowToAvaialable.Add(item.storeIndex);
            }

        }

        //now we check every gun_Perma that you have but its not yet in the list and add it tot he list.
        foreach (var item in currentActiveAbilityOwnedList)
        {
            //and we are going to check who we have here but not there. and thats who we add.
            if (!indexOfGunsAddedNowToAvaialable.Contains(item.storeIndex))
            {
                currentActiveAbilityAvailableList.Add(item);
            }
        }

    }

    public void BuyAbility(AbilityActiveData data)
    {

        if (HasAbility_Active(data.storeIndex))
        {
            Debug.LogError("ALREADY HAS THIS GUN IN LIST " + data.abilityName);
            return;
        }

        ownedAbilityIndexList_Active.Add(data.storeIndex);
        currentActiveAbilityOwnedList.Add(data);


        GenerateAvailableAbilityList();
        GenerateOwnedActiveAbilityList();

    }
    
    public void BuyAbilityWithIndex_Passive(int index)
    {
        if (HasAbility_Passive(index))
        {
            Debug.LogError("ALREADY HAS THIS GUN IN LIST " + abilityRefList_Passive[index].abilityName);
            return;
        }

        ownedAbilityIndexList_Passive.Add(index);
        currentPassiveAbilityList.Add(abilityRefList_Passive[index]);


        GeneratePassiveAbilityList();
    }
    public void BuyAbilityWithIndex_Active(int index)
    {
        if (HasAbility_Active(index))
        {
            Debug.LogError("ALREADY HAS THIS GUN IN LIST " + abilityRefList_Active[index].abilityName);
            return;
        }

        ownedAbilityIndexList_Active.Add(index);
        currentActiveAbilityOwnedList.Add(abilityRefList_Active[index]);


        GenerateAvailableAbilityList();
        GenerateOwnedActiveAbilityList();
    }
    public bool HasAbility_Active(int index) => ownedAbilityIndexList_Active.Contains(index);
    public bool HasAbility_Passive(int index) => ownedAbilityIndexList_Passive.Contains(index);
    #region PASSIVE

    //we can receive new items.
    //what about the chances?
    //should i do the same that i did with guns?
    //and what about especial abilities that are even rarer? maybe we can just do that they are rarer than the rest.


    public void ResetFoundList()
    {
        foundPassiveAbilityIndexList.Clear();
    }

    void GeneratePassiveAbilityFoundList()
    {

    }

    public void GeneratePassiveAbilityList()
    {
        currentPassiveAbilityList.Clear();
        //then we add based,

        List<int> indexOfAbilityAddedNowToAvaialable = new();

        for (int i = 0; i < cityStoreLevel; i++)
        {
            if (i > cityLabPassive_ByLevel_List.Count)
            {
                continue;
            }

            foreach (var item in cityLabPassive_ByLevel_List[i].dataList)
            {
                indexOfAbilityAddedNowToAvaialable.Add(item.storeIndex);
                currentPassiveAbilityList.Add(item);
            }
        }

        foreach (var item in ownedAbilityIndexList_Passive)
        {
            if (!indexOfAbilityAddedNowToAvaialable.Contains(item))
            {
                currentPassiveAbilityList.Add(abilityRefList_Passive[item]);
            }
            
        }


    }

    public void FoundNewGun(int foundIndex)
    {
        if (foundPassiveAbilityIndexList.Contains(foundIndex))
        {
            Debug.Log("trying to add a fella that alreayd has here");
            return;
        }

        foundPassiveAbilityIndexList.Add(foundIndex);
        GeneratePassiveAbilityFoundList();
    }


    

    #endregion

    public AbilityActiveData GetActiveAbilityWithIndex(int index)
    {
        return abilityRefList_Active[index];
    }


    #region GETTING PASSIVE FOR STAGE
    //as we progress the game we should have a higher chance to get better passives rather than the same for all.
    //
    public List<AbilityPassiveData> GetPassiveAbilityList(int amountRequired = 3)
    {
        //       
        int luck = (int)PlayerHandler.instance._entityStat.GetTotalValue(StatType.Luck);
        List<AbilityPassiveData> abilityList_CannotStack = PlayerHandler.instance._playerAbility.abilityList_CannotStack;
        List<AbilityPassiveData> abilityList_HigherChance = PlayerHandler.instance._playerAbility.abilityList_HigherChance;

        List<AbilityPassiveData> abilityList = new();
        List<string> AbilityNameList = new();
        List<int> alreadyUsedIndexList = new(); //cannot vie the same ability passive

        int safeBreak = 0;

        //we need to give a higher chance for abilities that already exist.
        //that we get from the player
        //and also. i dont want to spawn any fella with the same nanme. because they might be


        while (amountRequired > abilityList.Count )
        {
            int random = Random.Range(0, currentPassiveAbilityList.Count);

            safeBreak++;

            if(safeBreak > 1000)
            {
                return abilityList;
            }

            if (alreadyUsedIndexList.Contains(random))
            {
                continue;
            }

            AbilityPassiveData data = currentPassiveAbilityList[random];

            if (abilityList_CannotStack.Contains(data)) continue;

            if (AbilityNameList.Contains(data.abilityName)) continue;

            int additional = 0;

            if (abilityList_HigherChance.Contains(data))
            {
                additional = 20;
            }

            int secondRoll = Random.Range(0, 101);
            int requiredLuck = 30 + additional;

            if(data.GetAdditionalRollRequired > 0)
            {
                requiredLuck = data.GetAdditionalRollRequired + additional;
            }

            if (secondRoll > requiredLuck)
            {
                AbilityNameList.Add(data.abilityName);
                abilityList.Add(data);
            }
            
        }


        return abilityList;


    }

    [SerializeField] List<AbilityPassiveData> debug_PassiveList = new();

    [ContextMenu("Debug Passive Ability")]
    public void Debug_GetPassiveAbility()
    {
        debug_PassiveList.Clear();

        debug_PassiveList = GetPassiveAbilityList(3);
    }

    #endregion

    #region CURSE

    public List<AbilityPassiveData> GetCurseAbilities(int quantity = 3)
    {
        //teh player always has all abilities.

        List<AbilityPassiveData> curseList = new();
        List<int> alreadyChosenIndexList = new();


        List<AbilityClass> alreadyOnwedList = PlayerHandler.instance._playerAbility.abilityCurseList;

        //also have to check if 

        int safeBreak = 0;

        while(curseList.Count < quantity)
        {
            if(safeBreak > 1000)
            {
                return curseList;
            }

            safeBreak++;

            int random = Random.Range(0, abilityRefList_Curse.Count);

            if (alreadyChosenIndexList.Contains(random)) continue;

            if (HasCurseInThisList(alreadyOnwedList, abilityRefList_Curse[random])) continue;

            alreadyChosenIndexList.Add(random);
            curseList.Add(abilityRefList_Curse[random]);

        }

        return curseList;

    }

    bool HasCurseInThisList(List<AbilityClass> abilityList, AbilityPassiveData data)
    {
        foreach (var item in abilityList)
        {
            if (item.dataPassive.name == data.name) return true;
        }
        return false;
    }

    #endregion

}
[System.Serializable]
public class CityStoreLabClass_DividedByLevel
{
    [field:SerializeField]public List<AbilityActiveData> dataList { get; private set; } = new();
}

[System.Serializable]
public class CityStoreLabClass_DividedByLevel_Passive
{
    [field: SerializeField] public List<AbilityPassiveData> dataList { get; private set; } = new();

}

[System.Serializable]
public class CityStoreLabClass
{
    //this has the data for the weapon which we can use.
    //then we have a cost to unluck it 
    //and we have conditions for unluckint it
    //those conditions might be: level of a certain place, or a certain quest, or an achievement.
    [SerializeField] string GunName;
    [field: SerializeField] public AbilityActiveData data { get; private set; }
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

    public List<string> GetStringCityStoreLevelList()
    {
        List<CityStoreLevelRequirement> resourceList = requirementToUnluck.requiredCityStoreLevelList;
        List<string> stringList = new();


        foreach (var item in resourceList)
        {
            string text = "";



            stringList.Add(text);
        }

        return stringList;
    }

}