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




    private void Start()
    {
        //set the ui attached to this.
        _cityCanvas.SetGun(dataForArmory);


    }

    protected override void UpdateInteractUIName(string name)
    {
        base.UpdateInteractUIName("Armory");
    }


}
