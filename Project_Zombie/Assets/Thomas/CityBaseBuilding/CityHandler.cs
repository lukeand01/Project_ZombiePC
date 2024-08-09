using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityHandler : MonoBehaviour
{
    //this will control stuff events and quests.

    //where should i hold this ref? i cant hold in city because i might want to use.
    //



    public static CityHandler instance;

    [SerializeField] List<CityStore> cityStoreList = new(); //i will use this list to give the save data to them once the game starts or to force an update.
    [SerializeField] Transform spawnPos;

    [SerializeField] CityDataHandler cityDataHandler;

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

    }

    private void Start()
    {

        UIHandler.instance.ControlUI(true); //
        UIHandler.instance._MouseUI.ControlAppear(true); //
        PlayerHandler.instance._playerInventory.PassStageInventoryToCityInventory(); //we get the inventory from teh player to teh city
        PlayerHandler.instance.transform.position = spawnPos.position; //we put the player in the right position
        PlayerHandler.instance._playerController.block.AddBlock("City", BlockClass.BlockType.Combat); //and we block combat

        //now i need to 

        UpdateAbilityListUsingCurrentAbilities();
        UpdateGunListUsingCurrentPermaGun(); 


        Invoke(nameof(ThingsToStartWithADelay), 0.1f);
    }

    void ThingsToStartWithADelay()
    {
        CityBuilding_Start(); //does it break in awake?
        NpcHandler_Start();


    }


    public void StartCity()
    {
        PlayerHandler.instance._playerCombat.ControlGunHolderVisibility(false);

        //ready the equip here and i get ref from the player

        UpdateGunListUsingCurrentPermaGun();

        UpdateAbilityListUsingCurrentAbilities();
        //hide the gun from the player.



    }

    //cant i call this directly from somewhere else?


    


    public void UpdateGunListUsingCurrentPermaGun()
    {
        List<ItemGunData> withoutCurrentList = new();
        ItemGunData currentPermaGun = PlayerHandler.instance._playerCombat.GetCurrentPermaGun();

        


        List<ItemGunData> ownedGunList = cityDataHandler.cityArmory.currentGunOwnedList;

        //this update is updating only the owned gun.

        foreach (var item in ownedGunList)
        {
            if(item.name != currentPermaGun.name)
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
        //we call this when we buy a weapon.
        CityDataArmory armory = cityDataHandler.cityArmory;

        armory.BuyGun(gunData);
        armory.GenerateGunPermaList(); //with this we update what guns do we have.

        UpdateGunListUsingCurrentPermaGun();
    }

    //i need to store the items i already have.
    //i should the info about the progress in the data?
    //only the armoy needs to know if i own the place or not.
    public void UpdateAbilityListUsingCurrentAbilities()
    {
        List<AbilityActiveData> withoutCurrentList = new();

        List<AbilityClass> currentList = PlayerHandler.instance._playerAbility.GetActiveAbiltiyList();
        List<AbilityActiveData> abilityActiveList = GameHandler.instance.cityDataHandler.cityLab.currentActiveAbilityOwnedList;

        for (int i = 0; i < abilityActiveList.Count; i++)
        {
            var item = abilityActiveList[i];


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

        UpdateAbilityListUsingCurrentAbilities();
    }

    #region NPC HANDLER
    CityHandler_NpcHandler _npcHandler;

    void NpcHandler_Start()
    {
        _npcHandler = new CityHandler_NpcHandler();

        List<Story_NpcData> npcList = PlayerHandler.instance._playerParty.npcList;


        _npcHandler.NpcHandler_Start(_cityBuildingHandler.housingCityStoreList, npcList);
    }

    #endregion

    #region BUILDING HANDLER
    public CityHandler_CityBuildingHandler _cityBuildingHandler { get; private set; }

    [Separator("CITY BUILDING HANDLER")]
    [SerializeField] List<CityStore> especialCityStoreList = new();
    [SerializeField] List<City_House> housingCityStoreList = new(); //these are just for the houses.

    void CityBuilding_Start()
    {
        _cityBuildingHandler = new();

        _cityBuildingHandler.CityBuildingStart(especialCityStoreList, housingCityStoreList);      

        //after taking a look at the city we take take a look at npcs.


    }


    #endregion

}

public class CityHandler_NpcHandler
{
    List<GameObject> graphicList = new(); //the different aseet we will use to spawn different npcs
    List<CityWorkSpot> workSpotList = new();

    //so i want to spawn the especila npc here.



    public void UpdateWorkSpotList(List<CityWorkSpot> workSpotList)
    {
        //the idea here is that we will check the list of buildings avaialble and get from them spots. each spot is an actual palce that carries info about how they work.

        this.workSpotList = workSpotList;


    }

    //so bascially we will start and we will get the list of espeicla npc
    //first thing we will do is put only the npc that has the place
    //then we get the npcs that do not have 

    public void NpcHandler_Start(List<City_House> houseList, List<Story_NpcData> especialNpcList)
    {
        //we spawn half of these fellas in all spots we can.
        //we spawn the especila npc as well.

        if(houseList.Count == 0)
        {
            //thre are no houses to use. i should 
            return;
        }

        List<Story_NpcData> npcWithoutHouseList = new();

        foreach (var item in especialNpcList)
        {
            int houseIndex = item.houseIndex;

            if(houseIndex == -1)
            {
                npcWithoutHouseList.Add(item);
            }
            else
            {
                //then we put in that position.
                houseList[houseIndex].SpawnEspecialNPC(item);
            }
        }

        foreach (var item in npcWithoutHouseList)
        {
            int houseIndex = GetFreeHouseIndex(houseList);

            if(houseIndex == -1)
            {
                //then we just ignore it because we couldnt find a stop for it.
                Debug.Log("had to ignore this fella");
            }
            else
            {
                item.SetHouseIndex(houseIndex);
                houseList[houseIndex].SpawnEspecialNPC(item);
            }
        }


    }

    int GetFreeHouseIndex(List<City_House> houseList)
    {
        int index = -1;
        int safeBreak = 0;

        if(houseList.Count == 0)
        {
            Debug.Log("no house here");
        }

        while(index == -1)
        {
            int random = Random.Range(0, houseList.Count);

            if (houseList[random].npc == null)
            {
                index = random;
                return random;
            }
            else
            {
                Debug.Log("random was not it " + random);
            }

            safeBreak++;

            if(safeBreak > 1000)
            {
                break;
            }
        }
       
        
        return -1;
    }



    public void NpcHandler_Handle()
    {

    }

    void SpawnNpc()
    {
        //we choose a random skin
        //npcs that are fropm the start have no schedule
        //npcs 
    }


    public void AddWorkSpot(CityWorkSpot workSpot)
    {
        workSpotList.Add(workSpot);
    }
}

public class CityHandler_CityBuildingHandler
{
    //at teh start we check every store in the list. and we check their state.

    List<CityStore> especialCityStoreList = new();
   public List<City_House> housingCityStoreList { get; private set; } = new();



    #region GAME START FUNCTIONS
    public void CityBuildingStart(List<CityStore> especialList, List<City_House> houseList)
    {
        especialCityStoreList = especialList;
        housingCityStoreList = houseList;

        if (especialCityStoreList.Count == 0)
        {
            Debug.LogError("NO ESPECIAL STORE ASSIGNED");
            return;
        }
        if (especialCityStoreList[0] == null)
        {
            Debug.LogError("THE FIRST LIST HAS NOT ASSIGNED");
            return;
        }

        bool shouldResetEveryone = especialCityStoreList[0].GetCityData.cityStoreLevel == 0;



        for (int i = 0; i < especialCityStoreList.Count; i++)
        {
            especialCityStoreList[i].UpdateBasedInDataLevel(shouldResetEveryone);
        }

        //we remove all houses and then bring back them back based in the center level. 
        //every level we add three houses.

        foreach (var item in houseList)
        {
            item.RemoveFromCity();
        }

        if (!shouldResetEveryone)
        {
            InCaseNotReset();

        }
        else
        {
            InCaseDoReset();   
        }
      
    }

    void InCaseNotReset()
    {
        //and now we need to inform the other buildings to update if we have not reset.


        UpdatePopResource();
    }

    void InCaseDoReset()
    {
        PlayerInventory playerInventory = PlayerHandler.instance._playerInventory;

 
        playerInventory.SetPopulation(0);
        playerInventory.SetPopulationUsage(0);

        UpdatePopResource();

    }

    #endregion



    public List<CityWorkSpot> GetWorkSpotFromAllOpenBuildings()
    {
        List<CityWorkSpot> workSpotList = new();



        return workSpotList;
    }

    //this is the only way to set the pop as resource for the player.
    public void UpdatePopResource()
    {
        int mainBaseLevel = especialCityStoreList[0].GetCityData.cityStoreLevel;
        int popFromMainBuilding = mainBaseLevel * 10; //for now its one fella per thing and i want to show this in the maincity ui.
        int especialPopFromMainBuilding = mainBaseLevel;
        int popSpend = 0;


        UpdatePopGraphic_RegularHouses();

        for (int i = 1; i < especialCityStoreList.Count; i++)
        {
            int popCost = especialCityStoreList[i].GetCityData.popRequirement * especialCityStoreList[i].GetCityData.cityStoreLevel;
            popSpend += popCost;
        }
        //and also we will calculate how much pop we are using. based in every especial pop

        if (popSpend > popFromMainBuilding)
        {
            //


            Debug.LogError("pop spend should never be more than the current pop");
            return;
        }

        PlayerInventory playerInventory = PlayerHandler.instance._playerInventory;
        
        playerInventory.SetPopulation(popFromMainBuilding);
        playerInventory.SetPopulationUsage(popSpend);

        PlayerHandler.instance._playerParty.SetEspecialNpcLimit(especialPopFromMainBuilding);

    }

    void UpdatePopGraphic_RegularHouses()
    {
        //here we update the number of houses that should be showing.

        //3 for each fella.
        int houseQuantityBasedInLevel = especialCityStoreList[0].GetCityData.cityStoreLevel * 3;

        //Debug.Log("update pop " + houseQuantityBasedInLevel);
        
        //need to clear everyone

        foreach (var item in housingCityStoreList)
        {
            item.RemoveFromCity();
        }

        if (houseQuantityBasedInLevel == 0)
        {
            return;
        }

        for (int i = 0; i < houseQuantityBasedInLevel; i++)
        {
            if (i >= housingCityStoreList.Count)
            {
                return;
            }

            housingCityStoreList[i].AddToCity();

        }
    }
}

//


//the system will work on two parts
//as the scene loads. we instantly load half of your population limit in random positions. these npc have fixwed routine meaning they dont return back to their house.
//as time progresses. we occasionally add a npc if not over the limit and its only remove when teh routine of the npc is completed.

//how do i add workspots? there should be new spots based in active buildings so therefore they should be added as the building are built.
//the workspots are added by the buidlings. once bought they call the cityhandler.



//when we load the city we must load the npc.
//i want the npc to simply be thrown in the map and handle where they shoul go by themselvs.
//how i define spot and how do i add spots as the city grows? 
//everyn npc when born has at least one goal, like head to this place and pretend to talk.
//i need to find a system where 

//how the system for building teh city will work?
//Cetner which is the main building. it caps all other buildings.
//there will be a lot of areas with empty buildings. you interact with tha place to say if you wanna build it.
//upgrading and increasing the things is based on the pop resource.



//