using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunUpgradeStation : MonoBehaviour, IInteractable
{
    [SerializeField] InteractCanvas _interactCanvas;
    [SerializeField] GunUpgradeStationCanvas _gunUpgradeStationCanvas;
    [SerializeField] int pointCost; 
    string id;

    GunClass gun;
    bool hasGun;
    bool isActive;

    float upgradeTimerCurrent;
    float upgradeTimerTotal;


    private void Awake()
    {
        id = Guid.NewGuid().ToString();

        upgradeTimerTotal = 10;
    }

    private void FixedUpdate()
    {
        if (isActive && !hasGun)
        {
            _gunUpgradeStationCanvas.ControlDebugIsReady(true);
        }
        else
        {
            _gunUpgradeStationCanvas.ControlDebugIsReady(false);
            _gunUpgradeStationCanvas.UpdateGunProgress(0, 0);
        }



        if (!hasGun)
        {
            //_gunUpgradeStationCanvas.UpdateGunProgress(0, 0);
            upgradeTimerCurrent = 0;
            return;
        }

        _gunUpgradeStationCanvas.UpdateGunProgress(upgradeTimerCurrent, upgradeTimerTotal);

        if (upgradeTimerCurrent > upgradeTimerTotal)
        {
            MakeGunAvailable();
        }
        else
        {
            upgradeTimerCurrent += Time.fixedDeltaTime;
        }
    }

    void MakeGunAvailable()
    {
        hasGun = false;

        //do the thing for the gun.

    }

    //i need to know the index of the weapon.
    void StartStation()
    {
        hasGun = true;
        isActive = true;


        gun = PlayerHandler.instance._playerCombat.GetCurrentGun();
        gun.AddUpgradeStation();

        PlayerHandler.instance._playerCombat.RemoveCurrentGun_ForUpgradeStation();

    }
    void GiveGunToPlayer()
    {
        PlayerHandler.instance._playerCombat.ReceiveTempGun_FromUpgradeStation();
        gun.RemoveUpgradeStation();
        Debug.Log("gave gun");

        hasGun = false;
        isActive = false;
    }


    public string GetInteractableID()
    {
        return id;
    }

    public void Interact()
    {
        //need to remove the gun from the player and force the player equip

        if (isActive)
        {
            if (hasGun)
            {
                //then we should do nothing
            }
            else
            {
                //then we should pick gun because its ready
                GiveGunToPlayer();
            }
        }
        else
        {
            //then we should give the gun. but only if it has enough thing
            if (PlayerHandler.instance._playerResources.HasEnoughPoints(pointCost))
            {
                StartStation();
            }

        }


    }

    public void InteractUI(bool isVisible)
    {
        _interactCanvas.ControlInteractButton(isVisible);
    }

    public bool IsInteractable()
    {
        if (isActive)
        {
            if (hasGun)
            {
                return false;
            }
            else
            {
                return true;
            }

             //if its doing its thing it should not be interactable
        }


        return PlayerHandler.instance._playerCombat.CanUseGunStation();
    }

    
}

//what about a station for that uses points and simply increase stats.
//what about a station for blesses that gives a random passive. can have all the passives. but buff increases.

//for now only points.

//first we check if we are not equipped with a perma gun
//then we check if we have the resources.
//then we need to remove the gun from player
//wait a moment
//then we give back
//te player must head there and do interact to get it back.