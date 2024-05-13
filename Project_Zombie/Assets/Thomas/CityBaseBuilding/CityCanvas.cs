using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CityCanvas : MonoBehaviour
{
    //the description will all come from the small windows.
    GameObject holder;

    [Separator("BASE CANVAS")]
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] CityStoreUnit cityStoreUnitTemplate;
    [SerializeField] Transform containerForCityStoreUnit;

    CityDataArmory armoryData;
    CityDataLab labData;

    private void Awake()
    {
        holder = transform.GetChild(0).gameObject;
    }

    public bool IsTurnedOn() => holder.activeInHierarchy;

    public void SetGun( CityDataArmory armoryData)
    {
       this.armoryData = armoryData;    

        List<CityStoreArmoryClass> gunList = armoryData.GetAllGuns();

        for (int i = 0; i < gunList.Count; i++)
        {
            var item = gunList[i];
            CityStoreUnit newObject = Instantiate(cityStoreUnitTemplate);
            newObject.transform.SetParent(containerForCityStoreUnit);
            newObject.SetUpGun(item, i, this, armoryData);
        }

        
    }

    public void SetAbilities(CityDataLab labData)
    {
        this.labData = labData;

        List<CityStoreLabClass> abilityList = labData.GetAllAbilities();

        for (int i = 0; i < abilityList.Count; i++)
        {
            var item = abilityList[i];
            CityStoreUnit newObject = Instantiate(cityStoreUnitTemplate);
            newObject.transform.SetParent(containerForCityStoreUnit);
            newObject.SetUpAbility(item, i, this, labData);
        }
    }


    public void OpenUI()
    {
        holder.SetActive(true);
        PlayerHandler.instance._playerController.block.AddBlock("CityCanvas", BlockClass.BlockType.Complete);
        UIHandler.instance.EquipWindowUI.ForceCloseUI();
    }
    public void CloseUI()
    {
        if (buyHolder.activeInHierarchy)
        {
            buyHolder.SetActive(false);
            return;
        }

        holder.SetActive(false);
        UIHandler.instance.DescriptionWindow.StopDescription();
        Invoke(nameof(CallFreePlayer), 0.1f);
    }

    void CallFreePlayer()
    {
        PlayerHandler.instance._playerController.block.RemoveBlock("CityCanvas");
    }


    CityStoreUnit currentStoreUnit;

    public void SelectStoreUnit(CityStoreUnit storeUnit)
    {
        //if its the same store unit then we are going to open the ui.
        if(currentStoreUnit != null)
        {
            //if its the same then we call it.

            if(currentStoreUnit.id == storeUnit.id)
            {
                //open buy stuff.

                if(storeUnit.gunData != null)
                {
                    OpenBuyGun();
                }
                if(storeUnit.abilityData != null)
                {
                    OpenBuyAbility();
                }

                return;
            }
            else
            {
                currentStoreUnit.Unselect();
            }

        }



        currentStoreUnit = storeUnit;
        currentStoreUnit.Select();

    }

    #region BUY
    [Separator("BUY HOLDER")]
    [SerializeField] GameObject buyHolder;
    [SerializeField] Image buyPortrait;
    [SerializeField] TextMeshProUGUI buyNameText;
    [SerializeField] TextMeshProUGUI buyTypeText;


    //Cost holder
    //requirement holder
    //stats - like damage per shot, and stuff like that.
    //description like the description of a passive or the ability.


    //the things are not being instantited
    //the command is not working

    public void CallBuy()
    {
        //i check if the player has the resource
        //then check for requirement.
        //then we need to give it to the player

        bool canBuyResource = false;

        if(currentStoreUnit.gunData != null)
        {
            canBuyResource = PlayerHandler.instance._playerInventory.HasEnoughResourceToBuy(currentStoreUnit.armoryClass.requirementToUnluck.requiredResourceList);
        }
        if(currentStoreUnit.abilityData != null)
        {
            canBuyResource = PlayerHandler.instance._playerInventory.HasEnoughResourceToBuy(currentStoreUnit.labClass.requirementToUnluck.requiredResourceList);
        }

        Debug.Log("this is the required resource " + canBuyResource);

        if (!canBuyResource) return;

        if(currentStoreUnit.gunData != null)
        {
            //i have to call someone to update the stuff
            CityHandler.instance.BuyAndUpdateGun(currentStoreUnit.gunData);
            PlayerHandler.instance._playerInventory.SpendResourceListForCity(currentStoreUnit.armoryClass.requirementToUnluck.requiredResourceList);
            currentStoreUnit.SetAsBought();
        }

        if(currentStoreUnit.abilityData != null)
        {
            CityHandler.instance.BuyAndUpdateAbility(currentStoreUnit.abilityData);
            PlayerHandler.instance._playerInventory.SpendResourceListForCity(currentStoreUnit.labClass.requirementToUnluck.requiredResourceList);
            currentStoreUnit.SetAsBought();
        }

        CloseBuy();
    }

    void CloseBuy()
    {
        buyHolder.SetActive(false);

        if(currentStoreUnit != null)
        {
            currentStoreUnit.Unselect();
            currentStoreUnit = null;
        }
    }

    void OpenBuyGun()
    {
        buyHolder.SetActive(true);

        buyPortrait.sprite = currentStoreUnit.gunData.itemIcon;
        buyNameText.text = currentStoreUnit.gunData.itemName;
        buyTypeText.text = "Gun";
    }

    void OpenBuyAbility()
    {
        buyHolder.SetActive(true);

        buyPortrait.sprite = currentStoreUnit.abilityData.abilityIcon;
        buyNameText.text = currentStoreUnit.abilityData.abilityName;
        buyTypeText.text = "Gun";
    }

    #endregion

    #region HQ
    //we will still spawn teh fella but with another type of unit.

    public void SetStage(List<CityStageClass> cityStageClassList)
    {



    }



    #endregion


}

//FUUUCK
//citystoreunit is only when buying
//when you are equipping then its different.