using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestGun : ChestBase
{

    //we open another ui
    //we slow the game to bascially stop.
    //we can animate stuff here and
    //i can use time.unscaledtime to not deal with the mess of scale time.

    //the player can get any gun from this place
    //but the guns arent as unique as they might seen
    //you will have a bunch of guns here that look alike but with different stats.
    //there are different levels of gun and skill tier. and you can increase your chances in the base.
    //there is 4 tiers.
    //and at the first level:
    //1 tier -> 80%
    //2 tier -> 15%
    //3 tier -> 5%
    //4 tier -> 0%

    //every level improves the chance of higher tier and lowers the chance of lower tiers.


    //so now we ask the following.
    //for a list 



    public override void Interact()
    {
        base.Interact();


        //we get a bunch of random fellas. but those random fellas need to be from places we can get them
        //then we roll for the 
        List<ItemData> spinningGunList = PlayerHandler.instance.GetGunSpinningList();
        ItemData chosenGun = PlayerHandler.instance.GetGunChosen();
        UIHandler.instance.ChestUI.SetChest(this);
        UIHandler.instance.ChestUI.CallChestGun(spinningGunList, chosenGun);

    }

    public override void ProgressChest()
    {
        if(LocalHandler.instance != null)
        {
            LocalHandler.instance.ChestGunUse();
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
