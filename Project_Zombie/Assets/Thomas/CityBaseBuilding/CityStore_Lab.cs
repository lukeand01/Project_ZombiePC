using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityStore_Lab : CityStore
{
    [SerializeField] CityDataLab labData;


    public override CityData GetCityData => labData;
    private void Start()
    {
        
    }


    protected override void CallInteract()
    {
        base.CallInteract();
        _cityCanvas.SetAbilities(labData);
    }

    public override void IncreaseStoreLevel()
    {
        base.IncreaseStoreLevel();
        labData.GenerateAvailableAbilityList();
        _cityCanvas.SetAbilities(labData);
    }

    protected override void UpdateInteractUIName(string name)
    {
        base.UpdateInteractUIName("Lab");
    }
}
