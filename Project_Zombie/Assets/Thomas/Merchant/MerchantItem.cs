using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MerchantItem : ChestBase
{
    //when walk close show ability description, price and button to interact.
    //this needs to fit different kinds of rewards.

    [Separator("MERCHANT COMPONENTS")]
    [SerializeField] GameObject graphicHolder;

    int price;

    AbilityPassiveData abilityPassiveData;

    Merchant _merchant;

    Color _color;


    public void SetUp(AbilityPassiveData abilityPassiveData, Merchant _merchant, int price, Color _color)
    {
        this.abilityPassiveData = abilityPassiveData;
        this.price = price;
        this._merchant = _merchant;
        this._color = _color;   

        graphicHolder.SetActive(true);
    }

    
    public void IncreasePrice(int increment)
    {
        price += increment;
    }
    public void ForceDisable()
    {
        graphicHolder.SetActive(false);
    }

    #region GETTERS
    string GetName()
    {
        if(abilityPassiveData != null)
        {
            return abilityPassiveData.abilityName;
        }

        return "";
    }
    string GetDescription()
    {
        if (abilityPassiveData != null)
        {
            return abilityPassiveData.abilityDescription;
        }

        return "";
    }
    Sprite GetIcon()
    {
        if (abilityPassiveData != null)
        {
            return abilityPassiveData.abilityIcon;
        }

        return null;
    }
    #endregion

    #region BUY FUNCTIONS

    void Buy()
    {
        //we get whatever we have here.
        PlayerHandler.instance._playerResources.SpendPoints(price);
        graphicHolder.SetActive(false);
        _merchant.NewItemBought();

        if (abilityPassiveData != null)
        {
            PlayerHandler.instance._playerAbility.AddAbility(abilityPassiveData);
            return;
        }


    }

    #endregion


    #region INTERACT
    public override void Interact()
    {

        if (!PlayerHandler.instance._playerResources.HasEnoughPoints(price)) return;
        
        Buy();
        

        base.Interact();
    }
    public override void InteractUI(bool isVisible)
    {

        //we will start the merchant and if it leaves we close it.

        if (!isVisible)
        {
            interactCanvas.StopMerchant();
            return;
        }

        if (!graphicHolder.activeInHierarchy)
        {
            interactCanvas.ControlInteractButton(false);
            return;
        }

        interactCanvas.StartMerchant(price, GetName(), GetDescription(), GetIcon(), _color);

    }
    #endregion
}
