using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CityData / Armory")]
public class CityDataArmory : CityData
{
    [Separator("Armory")]
    [SerializeField] List<CityStoreArmoryClass> cityArmoryList = new();
    [SerializeField] List<RequirementToUnluckClass> requirementToIncreaseLevelList = new();
    [SerializeField] List<int> ownedGunIndexList = new(); //what the player owns. we use this to create the list.
    
    public List<ItemGunData> GetGunList()
    {
        List<ItemGunData> gunList = new();

        foreach (var item in ownedGunIndexList)
        {
            ItemGunData data = cityArmoryList[item].data;

            gunList.Add(data);

        }

        return gunList;
    }

    public List<CityStoreArmoryClass> GetAllGuns() => cityArmoryList;


    public void BuyGun(ItemData boughtData)
    {
        for (int i = 0; i < cityArmoryList.Count; i++)
        {
            ItemData data = cityArmoryList[i].data;

            if (data.name != boughtData.name) continue;

            if (ownedGunIndexList.Contains(i))
            {
                Debug.Log("this item was already bought");
                return;
            }

            Debug.Log("we got tot he end");
            ownedGunIndexList.Add(i);
            return;          
        }
    }


    public bool HasGun(int index) => ownedGunIndexList.Contains(index);
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