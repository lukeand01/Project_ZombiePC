using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityStore_DropLauncher : CityStore
{
    [SerializeField] CityData_DropLauncher data;


    public override void IncreaseStoreLevel()
    {
        base.IncreaseStoreLevel();
        _cityCanvas.SetDrop(data.currentAvailableDropList);
    }

    protected override void CallInteract()
    {
        base.CallInteract();
        _cityCanvas.SetDrop(data.currentAvailableDropList);
    }

    protected override void UpdateInteractUIName(string name)
    {
        base.UpdateInteractUIName("Drop Launcher");
    }

    public override CityData GetCityData => data;

}
