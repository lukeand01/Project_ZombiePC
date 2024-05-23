using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityStore_Stage : CityStore
{
    [SerializeField] CityDataStage cityStageData;


    private void Start()
    {
        //city 
        _cityCanvas.SetStage(cityStageData.cityStageClassList);


    }

    protected override void UpdateInteractUIName(string name)
    {
        base.UpdateInteractUIName("Stage Store");
    }
}
