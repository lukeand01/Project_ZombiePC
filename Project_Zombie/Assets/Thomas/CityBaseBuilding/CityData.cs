using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CityData / Basic")]
public class CityData : ScriptableObject
{

    //reset this only means that the citystore level is reduced to 0;

    [field: SerializeField] public bool _requireMainBlueprint { get; private set; }
    [field: SerializeField] public MainBlueprintType _mainBlueprint { get; private set; }

    public void ResetCityStoreLevel()
    {
        cityStoreLevel = 0;
    }
    [ContextMenu("DBEUG INCREASE LEVEL BY ONE")]
    public void IncreaseCityStoreLevel()
    {
        cityStoreLevel += 1;
    }

    public void SetCityStoreLevel(int level)
    {
        cityStoreLevel = level;
    }

    [field: SerializeField] public string cityStoreName { get; private set; }
    [field: SerializeField] public int cityStoreLevel { get; private set; }
    [field:SerializeField] public int popRequirement { get; private set; }
    [field:SerializeField] public ItemResourceData resourcePopRef {  get; private set; }
    [field:SerializeField] public List<RequirementToUnluckClass> requirementToIncreaseLevelList { get; private set; } = new();


    public List<ResourceClass> GetCurrentResourceListBasedInLevel()
    {
        if(cityStoreLevel >= requirementToIncreaseLevelList.Count)
        {
            return null;
        }

        return requirementToIncreaseLevelList[cityStoreLevel].requiredResourceList;
    }


    public bool HasMainBlueprint()
    {
        if (!_requireMainBlueprint) return true;

        bool hasBlueprint = PlayerHandler.instance._playerInventory.HasMainBlueprint(_mainBlueprint);

        return hasBlueprint;
    }
}

[System.Serializable]
public class RequirementToUnluckClass
{
    [field: SerializeField] public List<ResourceClass> requiredResourceList = new();
    [field: SerializeField] public List<CityStoreLevelRequirement> requiredCityStoreLevelList = new();
}

//but how do i know the level of thet arget. cannot be monobehavior. 
[System.Serializable]
public class CityStoreLevelRequirement
{

}

[System.Serializable]   
public class ResourceClass
{

    public ResourceClass(ItemResourceData data, int quantity) 
    {
        this.data = data;
        this.quantity = quantity;
    }

    [field: SerializeField] public ItemResourceData data { get; private set; }
    [field: SerializeField] public int quantity{ get; private set; }


    public ItemClass GetItem() => new ItemClass(data, quantity);

}
