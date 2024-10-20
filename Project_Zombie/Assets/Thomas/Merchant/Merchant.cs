using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : Story_EspecialNpc
{
    //we will get a random list.
    //everytime the teleporter is called we roll for merchant again

    [Separator("MERCHANT")]
    [SerializeField] MerchantItem[] itemArray; //these are the possible items. they are disable once used, not perfoamcen spend
    [SerializeField] int basePrice;
    [SerializeField] int scalePrice;
    [SerializeField] float modifierPriceForCurse;
    [SerializeField] List<AbilityPassiveData> abilityList = new();
    //everything is scaled for curses.

    [Separator("COLORS")]
    [SerializeField] Color color_Ability;
    [SerializeField] Color color_Cursed;
    Color color_Current;

    bool isCurseMerchant;
    int itemsBought;


    //maybe the 

    private void Start()
    {
        GenerateMerchant();
    }

    int GetPrice()
    {
        if (isCurseMerchant) return 0;


        float newBasePrice = Random.Range(basePrice * 0.7f, basePrice * 1.5f);


        int price = (int)newBasePrice + (scalePrice * itemsBought);

        return price;   
    }

    public void NewItemBought()
    {
        itemsBought += 1;

     
       foreach (var item in itemArray)
       {
          if (isCurseMerchant)
          {
             item.ForceDisable();
          }
          else
          {
             item.IncreasePrice(scalePrice);
          }
                
       }
        
    }



    public void GenerateMerchant()
    {
        //we roll here.

        //CreateAbilityMerchant();


        int roll = Random.Range(0, 101);

        if(roll < 85)
        {
            CreateAbilityMerchant();
        }
        else
        {
            CreateCurseMerchant();
        }

    }

    void CreateAbilityMerchant()
    {
        List<AbilityPassiveData> abilityList = GameHandler.instance.cityDataHandler.cityLab.GetPassiveAbilityList(3, "Merchant");
        //actually dont create it, just send the information.


        isCurseMerchant = false;

        int price = GetPrice();

        for (int i = 0; i < abilityList.Count; i++)
        {
            itemArray[i].SetUp(abilityList[i], this, price, color_Ability);
        }
    }
    void CreateCurseMerchant()
    {
        List<AbilityPassiveData> abilityList = GameHandler.instance.cityDataHandler.cityLab.GetCurseAbilities();
        //actually dont create it, just send the information.


        this.abilityList = abilityList;

        isCurseMerchant = true;

        int price = GetPrice();

        for (int i = 0; i < abilityList.Count; i++)
        {
            itemArray[i].SetUp(abilityList[i], this, price, color_Cursed);
        }

    }


    //we change a bit the grpahic.
    //then we prepare to spawn the fellas.
    //it first sell abilities.

    void RerollItems()
    {
        //check for the price here.

        if (!PlayerHandler.instance._playerResources.Bless_HasEnough(1))
        {

            return;
        }

        PlayerHandler.instance._playerResources.Bless_Lose(1);

        //but we cannot allow there to be the same
        if (isCurseMerchant)
        {
            CreateCurseMerchant();
        }
        else
        {
            GenerateMerchant();
        }
    }

    void ShowEspecialItems()
    {
        //we replace the items with the other stuff
        if (!PlayerHandler.instance._playerResources.Bless_HasEnough(10))
        {

            return;
        }

        PlayerHandler.instance._playerResources.Bless_Lose(10);
        

        Debug.Log("got here");
    }

    public override void CallFunctionUnique(int triggerIndex)
    {
        switch(triggerIndex)
        {
            case 0:
                RerollItems();
                break;

            case 1:
                ShowEspecialItems();
                break;
        }
        
    }


}


//curses are removed once one is bought. it costs no price.
