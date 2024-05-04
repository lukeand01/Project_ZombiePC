using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "City Data / Armory")]
public class CityDataArmory : CityData
{

    [SerializeField] List<GunCostClass> gunCostList = new();
    [SerializeField] List<int> liberatedGunList = new(); //we use this to tell who we liberated.

}
[System.Serializable]
public class GunCostClass
{
    public string nameClass;
    public ItemGunData gunData;
    public int levelRequired; //at what level can you do stuff?
    public List<CityCostClass> costList = new();
}