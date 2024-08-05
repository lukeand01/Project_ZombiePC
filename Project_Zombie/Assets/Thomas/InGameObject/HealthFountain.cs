using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthFountain : ChestBase
{
    //if you have enough money regen a percent of your health. 25%
    //then it goes to cooldown. the cooldown is 5 turns.
    //and the ui should show the amount required

    [Separator("HEALTH FOUNTAIN")]
    [SerializeField] int price;
    [SerializeField] int roundsPerUse;
    int roundsPassed;

    bool CanUse { get { return roundsPassed >= roundsPerUse; } }

    //

    private void Start()
    {
        PlayerHandler.instance._entityEvents.eventPassedRound += PassedRound;

        roundsPassed = roundsPerUse;

    }

    void PassedRound()
    {
        if(roundsPassed < roundsPerUse)
        {
            roundsPassed++;
        }
    }

    public override void Interact()
    {

        if (!CanUse) return;

        if (PlayerHandler.instance._playerResources.HasEnoughPoints(price))
        {
            //heal the player and start the cooldowjn
            PlayerHandler.instance._playerResources.RestoreHealthBasedInPercent(0.25f);
            roundsPassed = 0;

            PlayerHandler.instance._playerResources.SpendPoints(price);

            interactCanvas.ControlInteractButton(false);
        }


    }
    public override void InteractUI(bool isVisible)
    {
        if (!CanUse) return;
        base.InteractUI(isVisible);
        interactCanvas.ControlPriceHolder(price);
    }

}


//