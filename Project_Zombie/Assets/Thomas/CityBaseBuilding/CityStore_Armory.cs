using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityStore_Armory : CityStore
{

    //at the start we will get alist of the weappons that the armory has.
    //then we need to inform what weapons the player has of those through a index list.
    [Separator("DATA")]
    [SerializeField] CityDataArmory dataForArmory;


    public override CityData GetCityData => dataForArmory;


    public override void IncreaseStoreLevel()
    {
        base.IncreaseStoreLevel();

        //we are going to update the armory and then we update the ui
        dataForArmory.GenerateAvailableGunList(); //we do this to get a new list of stuff because of the level up.
        _cityCanvas.SetGun(dataForArmory);

        //here we update the equipment we can find in this thing.

    }

    private void Start()
    {
        //set the ui attached to this.
        dataForArmory.GenerateAvailableGunList();
        
    }

    //how am i telling them apart?


    protected override void CallInteract()
    {
        base.CallInteract();
        _cityCanvas.SetGun(dataForArmory);
    }

    protected override void UpdateInteractUIName(string name)
    {
        base.UpdateInteractUIName("Armory");
    }


}
