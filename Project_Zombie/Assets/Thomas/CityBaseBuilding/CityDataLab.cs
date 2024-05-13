using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CityData / Lab")]
public class CityDataLab : CityData
{
    [Separator("Lab")]
    [SerializeField] List<CityStoreLabClass> cityLabList = new();
    [SerializeField] List<RequirementToUnluckClass> requirementToIncreaseLevelList = new();
    [SerializeField] List<int> ownedAbilityIndexList = new(); //what the player owns. we use this to create the list.


    public List<AbilityActiveData> GetAbilityList()
    {
        List<AbilityActiveData> abilityList = new();

        foreach (var item in ownedAbilityIndexList)
        {
            AbilityActiveData data = cityLabList[item].data;

            abilityList.Add(data);

        }

        return abilityList;
    }




    public List<CityStoreLabClass> GetAllAbilities() => cityLabList;


    public void BuyAbility(AbilityActiveData boughtData)
    {
        for (int i = 0; i < cityLabList.Count; i++)
        {
            AbilityActiveData data = cityLabList[i].data;

            if (data.name != boughtData.name) continue;

            if (ownedAbilityIndexList.Contains(i))
            {
                Debug.Log("this item was already bought");
                return;
            }

            Debug.Log("we got tot he end");
            ownedAbilityIndexList.Add(i);
            return;
        }
    }


    public bool HasAbility(int index) => ownedAbilityIndexList.Contains(index);

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