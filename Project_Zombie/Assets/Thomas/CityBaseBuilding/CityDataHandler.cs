using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CityData / Handler")]
public class CityDataHandler : ScriptableObject
{

    public CityDataArmory cityArmory;
    public List<ItemGunData> ownedGunList { get; private set; } = new(); //gunlcass bcause i might want to addd levels to this stuff.



    public void UpdateGunList()
    {
        ownedGunList.Clear(); //
        ownedGunList = cityArmory.GetGunList();

    }


    public CityDataLab cityLab;
    public List<AbilityActiveData> ownedAbilityList {  get; private set; } = new();

    public void UpdateAbilityList()
    {
        ownedAbilityList.Clear();
        ownedAbilityList = cityLab.GetAbilityList();
    }


    public CityDataStage cityStage;
    public List<StageData> ownedStageList = new(); //stages we can actually go




}

//also we will have the list of item the player currently have.