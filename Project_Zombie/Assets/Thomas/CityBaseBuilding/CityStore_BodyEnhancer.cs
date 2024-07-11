using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityStore_BodyEnhancer : CityStore
{
    [SerializeField] CityData_BodyEnhancer data;

    protected override void CallInteract()
    {
        base.CallInteract();

        _cityCanvas.SetStats(data);

    }

    public override void IncreaseStoreLevel()
    {
        base.IncreaseStoreLevel();

        //then we need to update all fellas.

        _cityCanvas.SetStats(data);
    }

    protected override void UpdateInteractUIName(string name)
    {
        base.UpdateInteractUIName("Body Enhancer");
    }

    public override CityData GetCityData => data;


    
}
    
