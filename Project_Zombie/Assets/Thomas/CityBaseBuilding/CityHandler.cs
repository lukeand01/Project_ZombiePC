using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityHandler : MonoBehaviour
{
    //this will control stuff events and quests.


    public static CityHandler instance;
    [SerializeField] CityDataHandler cityDataHandler;
    [SerializeField] List<CityStore> cityStoreList = new(); //i will use this list to give the save data to them once the game starts or to force an update.




    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }


        cityDataHandler.UpdateGunList(); //now we can use this list of guns owned.
        cityDataHandler.UpdateAbilityList();
    }




    private void Start()
    {
        UIHandler.instance.ControlUI(true);
        PlayerHandler.instance._playerController.block.AddBlock("City", BlockClass.BlockType.Combat);
        //we ask the inventory to pass any item to here
        PlayerHandler.instance._playerInventory.PassStageInventoryToCityInventory();

    }



    public void UpdateGunListUsingCurrentPermaGun(ItemGunData gunData)
    {
        List<ItemGunData> withoutCurrentList = new();

        foreach (var item in cityDataHandler.ownedGunList)
        {
            if(item.name != gunData.name)
            {

                withoutCurrentList.Add(item);
            }
            else
            {

            }
        }



        UIHandler.instance._EquipWindowUI.UpdateOptionForGunContainer(withoutCurrentList);
    }


    public void BuyAndUpdateGun(ItemGunData gunData)
    {
        Debug.Log("call this");
        ItemGunData currentPerma = PlayerHandler.instance._playerCombat.GetCurrentPermaGun();
        cityDataHandler.cityArmory.BuyGun(gunData);
        cityDataHandler.UpdateGunList();

        UpdateGunListUsingCurrentPermaGun(currentPerma);
    }

    //i need to store the items i already have.
    //i should the info about the progress in the data?
    //only the armoy needs to know if i own the place or not.


    public void UpdateAbilityListUsingCurrentAbilities()
    {
        List<AbilityActiveData> withoutCurrentList = new();

        List<AbilityClass> currentList = PlayerHandler.instance._playerAbility.GetActiveAbiltiyList();


        for (int i = 0; i < cityDataHandler.ownedAbilityList.Count; i++)
        {
            var item = cityDataHandler.ownedAbilityList[i];


            if(!HasAbilityInList(item, currentList))
            {
                withoutCurrentList.Add(item);
            }
                           
        }

       
        UIHandler.instance._EquipWindowUI.UpdateOptionForAbilityContainer(withoutCurrentList);
    }

    
    bool HasAbilityInList(AbilityActiveData data, List<AbilityClass> abilitiyList)
    {
        foreach (var item in abilitiyList)
        {
            if (item.dataActive == null) continue;
            if (item.dataActive.name == data.name) return true;
        }
        return false;
    }



    public void BuyAndUpdateAbility(AbilityActiveData data)
    {


        cityDataHandler.cityLab.BuyAbility(data);
        cityDataHandler.UpdateAbilityList();

        UpdateAbilityListUsingCurrentAbilities();
    }
    



}
