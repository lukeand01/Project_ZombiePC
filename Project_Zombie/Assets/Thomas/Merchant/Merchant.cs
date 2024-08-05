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
        CreateCurseMerchant();

        return;
        int roll = Random.Range(0, 101);


        if(roll > 0 && roll <= 75)
        {
            CreateAbilityMerchant();
            return;
        }

        if(roll > 75 && roll <= 95)
        {
            CreateWeaponMerchant();
            return;
        }

        if(roll > 95)
        {
            CreateCurseMerchant();
            return;
        }

    }

    void CreateAbilityMerchant()
    {
        List<AbilityPassiveData> abilityList = GameHandler.instance.cityDataHandler.cityLab.GetPassiveAbilityList();
        //actually dont create it, just send the information.

        isCurseMerchant = false;

        int price = GetPrice();

        for (int i = 0; i < abilityList.Count; i++)
        {
            itemArray[i].SetUp(abilityList[i], this, price, color_Ability);
        }
    }
    void CreateWeaponMerchant()
    {
        


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


    


}


//curses are removed once one is bought. it costs no price.
