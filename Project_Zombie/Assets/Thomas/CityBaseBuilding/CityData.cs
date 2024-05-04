using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "City Data / Basic")]
public class CityData : ScriptableObject
{
    //we will have the basic info regarding cities.
    //how much it costs to upgrade stuff.

    public string cityStoreName; //so we can use in other stuff.
    public int currentLevel; //this is stored here. we can use as ref from other places when asked 

    [SerializeField] List<CityCostClassList> mainLevelCostList = new();
    [SerializeField] List<CityCostClassList> secondLevelCostLis = new();




}
[System.Serializable]   
public class CityCostClassList
{
    public string cityCostName;
    public List<CityCostClass> costList = new();    

    public bool IsDone(List<string> errorCollectorList)
    {
        PlayerInventory inventory = PlayerHandler.instance._playerInventory; 

        if(inventory == null)
        {
            Debug.Log("inventory is null");
            return false;
        }

        foreach (var item in costList)
        {
            if (!item.IsDone(inventory, errorCollectorList))
            {
                return false;
            }
        }

        return true;
    }

    public void SpendResources()
    {
        //
        //we need to collect all the resource.
        PlayerInventory inventory = PlayerHandler.instance._playerInventory;

        if (inventory == null)
        {
            Debug.Log("inventory is null");
            return;
        }


        foreach (var item in costList)
        {
            inventory.RemoveItemForCity(item.GetItem());
        }

    }

}

[System.Serializable]
public class CityCostClass
{
    //we need to know how much it will scale.
    //and also different resources.
    public ItemResourceData resource;
    public int cost;

    public List<CityStoreRequiredAnotherLevelClass> cityAnotherLevelList = new();

    //how do i know if you have enough resource.
    //

    public bool IsDone(PlayerInventory inventory, List<string> errorCollectorList)
    {
        bool enoughResource = inventory.HasEnoughItemForCity(GetItem());

        bool isCompleted = true;

        if(!enoughResource)
        {
            errorCollectorList.Add("Not enough " + resource.itemName);
        }

        foreach (var item in cityAnotherLevelList)
        {
            if (!item.IsDone())
            {
                isCompleted = false;
                errorCollectorList.Add("require level " + item.requiredLevel.ToString() + " for building " + item.data.cityStoreName);
            }
        }

        return enoughResource && isCompleted;
    }


    public ItemClass GetItem()
    {
        return new ItemClass(resource, cost);   
    }
}

[System.Serializable]
public class CityStoreRequiredAnotherLevelClass
{
    //this reqr
    public CityData data;
    public int requiredLevel; 

    public bool IsDone()
    {
        return data.currentLevel >= requiredLevel;
    }
}



//is this system worth it
//i need to have a cost of resource.
//prossibly a quest or the requirement of another place´s level.

