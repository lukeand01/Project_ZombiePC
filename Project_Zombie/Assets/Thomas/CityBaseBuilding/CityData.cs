using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CityData : ScriptableObject
{

    [field: SerializeField] public int cityStoreLevel { get; private set; }

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
    [field: SerializeField] public ItemResourceData data { get; private set; }
    [field: SerializeField] public int quantity{ get; private set; }


    public ItemClass GetItem() => new ItemClass(data, quantity);

}
