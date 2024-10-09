using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CityData / Handler")]
public class CityDataHandler : ScriptableObject
{
    //this set up must be before we have set up the other stuff.
    [field:SerializeField] public CityDataArmory cityArmory {  get; private set; }

    [field: SerializeField] public CityData_Main cityMain { get; private set; }

    [field:SerializeField] public CityDataLab cityLab { get; private set; }

    [field: SerializeField] public CityData_DropLauncher cityDropLauncher{ get; private set; }

    [field: SerializeField] public CityData_BodyEnhancer cityBodyEnhancer { get; private set; }


    public CityDataStage cityStage;
    public List<StageData> ownedStageList = new(); //stages we can actually go

    List<CityData> cityDataList = new();
    private void OnEnable()
    {
        cityDataList = new (){ cityMain, cityArmory, cityLab, cityDropLauncher, cityBodyEnhancer};
    }

    public void RestoreState(SaveClass saveClass)
    {
        List<int> cityLevelList = saveClass._cityLevelList;

        if(cityLevelList.Count != cityDataList.Count)
        {
            Debug.Log("something wrong here. the citydatalist is not matching");
        }

        for (int i = 0; i < cityLevelList.Count; i++)
        {
            var value = cityLevelList[i];
            var item = cityDataList[i];

            item.SetCityStoreLevel(value);

        }

        //ARMORY
        cityArmory.RestoreState(saveClass);
        cityArmory.Initialize();

        //LAB
        cityLab.RestoreState(saveClass);
        cityLab.Initialize();


        Debug.Log("restore state");
    }

    public void CaptureState(SaveClass saveClass)
    {
        List<int> cityLevelList = new()
        { cityMain.cityStoreLevel, cityArmory.cityStoreLevel,cityLab.cityStoreLevel, cityDropLauncher.cityStoreLevel, cityBodyEnhancer.cityStoreLevel};

        saveClass.MakeCityLevelList(cityLevelList);

        //ARMORY
        cityArmory.CaptureState(saveClass);

        //LAB
        cityLab.CaptureState(saveClass);

    }

    public void ResetAllCityStores()
    {

    }

}

//also we will have the list of item the player currently have.