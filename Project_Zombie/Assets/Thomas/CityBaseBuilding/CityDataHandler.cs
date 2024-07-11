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




}

//also we will have the list of item the player currently have.