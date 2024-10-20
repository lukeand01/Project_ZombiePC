using DG.Tweening;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CityCanvas : MonoBehaviour
{
    //the description will all come from the small windows.

    //we update everytime we interact


    GameObject holder;

    [Separator("BASE CANVAS")]
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] CityStoreUnit cityStoreUnitTemplate;
    [SerializeField] CityStoreLevelButton levelButton;
    [SerializeField] GameObject requiredHolder;
    [SerializeField] TextMeshProUGUI requiredText;
    
    CityDataArmory armoryData;
    CityDataLab labData;


    //


    [Separator("NPC UNIT TEMPLATE")]
    [SerializeField] Story_NpcUnit npcUnitTemplate;
    [SerializeField] TextMeshProUGUI especialNpcLimitText;
    [SerializeField] GameObject especialNpcHolder;
    bool hasEspecialNpcHolder;

    [Separator("Quest")]
    [SerializeField] QuestUnit questUnitTemplate;

    [Separator("BODY")]
    [SerializeField] StatUnit_Body statUnitTemplate;

    CityStore cityStoreOwner; //where do i set this? i can just get it from parent.

    private void Awake()
    {
        holder = transform.GetChild(0).gameObject;

        if (cityStoreOwner == null)
        {
            cityStoreOwner = transform.parent.GetComponent<CityStore>();
        }


        _originalUpgradeFailureTextPosition = _upgradeFailureText.transform.localPosition;

        hasEspecialNpcHolder = especialNpcHolder.activeInHierarchy;
    }

    public bool IsTurnedOn() => holder.activeInHierarchy;

    #region SETTERS

  
    public void SetGun(CityDataArmory armoryData)
    {
        //set gun_Perma in teh start based in the 
        //this is putting all fellas all at once in this.

        nameText.text = "Armory";

       this.armoryData = armoryData;

        List<ItemGunData> gunList = armoryData.currentGunAvailableArmoryList;

        //maybe i want the cost in the thing. or do i want in teh armory?

        //set the PERMA GUN
        TryCreateContainer(0, "Perma Gun");
        ClearContainer(0);

        for (int i = 0; i < gunList.Count; i++)
        {
            var item = gunList[i];
            CityStoreUnit newObject = Instantiate(cityStoreUnitTemplate);
            PlaceUnitInContainer(newObject.transform, 0);
            newObject.SetUpGun(item, i, this, armoryData);
        }


        //set the TEMP GUN
        List<ItemGunData> gunList_Temp = armoryData.currentGunTempList;

        TryCreateContainer(1, "Temp Gun");
        ClearContainer(1);

        for (int i = 0; i < gunList_Temp.Count; i++)
        {
            var item = gunList_Temp[i];
            CityStoreUnit newObject = Instantiate(cityStoreUnitTemplate);
            PlaceUnitInContainer(newObject.transform, 1);
            newObject.SetCannotClick(true);
            newObject.SetUpGun(item, i, this, armoryData);
        }




    }

    public void SetAbilities(CityDataLab labData)
    {
        this.labData = labData;

        List<AbilityActiveData> abilityList = labData.currentActiveAbilityAvailableList;

        nameText.text = "Lab";

        TryCreateContainer(0, "Active Abilities");
        ClearContainer(0);

        for (int i = 0; i < abilityList.Count; i++)
        {
            var item = abilityList[i];
            CityStoreUnit newObject = Instantiate(cityStoreUnitTemplate);
            PlaceUnitInContainer(newObject.transform, 0);
            newObject.SetUpAbility(item, i, this, labData);
        }


        List<AbilityPassiveData> abilityList_Passive = labData.currentPassiveAbilityList;

        TryCreateContainer(1, "Passive Abilities");
        ClearContainer(1);

        for (int i = 0; i < abilityList_Passive.Count; i++)
        {
            var item = abilityList_Passive[i];
            CityStoreUnit newObject = Instantiate(cityStoreUnitTemplate);
            PlaceUnitInContainer(newObject.transform, 1);
            newObject.SetUpAbility(item, i, this, labData);
        }


        

    }

    public void SetEspecialNpcs(List<Story_NpcData> npcList, int especialNpcLimit)
    {
        nameText.text = "Main Building";

        TryCreateContainer(0, "Especial Contacts");
        ClearContainer(0);

        if(npcList == null)
        {
            Debug.Log("list is null");
        }

        foreach (var item in npcList)
        {
            Story_NpcUnit newObject = Instantiate(npcUnitTemplate);
            PlaceUnitInContainer(newObject.transform, 0);
            newObject.SetUp(item);
        }


        especialNpcLimitText.text = $"{npcList.Count} / {especialNpcLimit}";

    }

    public void SetQuests(List<QuestClass> questList_Active, List<QuestClass> questList_Done)
    {
        TryCreateContainer_Quest(1, "Active Quests");
        ClearContainer(1);

        foreach (var item in questList_Active)
        {
            QuestUnit newObject = Instantiate(questUnitTemplate);
            PlaceUnitInContainer(newObject.transform, 1);
            newObject.SetUp(item);
        }

        TryCreateContainer_Quest(2, "Completed Quests");
        ClearContainer(2);

        foreach (var item in questList_Done)
        {
            QuestUnit newObject = Instantiate(questUnitTemplate);
            PlaceUnitInContainer(newObject.transform, 2);
            newObject.SetUp(item);
        }

    }

    public void SetStats(CityData_BodyEnhancer data)
    {
        //we will create a statincreaser unit
        //how can i assign taht value_Level to the player and then remmeber

        //maybe at the start of each game we simply get the saved values and assign them to the player.
        //and it become an invisible bd. that cannot be altered. but should be able to be removed with _id.
        //

        nameText.text = "Body Enhancer";

        TryCreateContainer_Stat(0, "Stats");
        ClearContainer(0);


        foreach (var item in data.currentStatAlteredList)
        {
            StatUnit_Body newObject = Instantiate(statUnitTemplate);
            newObject.SetUp(item, data);
            PlaceUnitInContainer(newObject.transform, 0);

        }



    }

    public void SetDrop(List<DropData> dropList)
    {
        TryCreateContainer_Stat(0, "Drop");
        ClearContainer(0);


        Debug.Log(dropList.Count);
        foreach (var item in dropList)
        {
            CityStoreUnit newObject = Instantiate(cityStoreUnitTemplate);
            newObject.SetDrop(item, this);
            PlaceUnitInContainer(newObject.transform, 0);
        }

    }

    #endregion

    public void OpenUI()
    {
        holder.SetActive(true);
        PlayerHandler.instance._playerController.block.AddBlock("CityCanvas", BlockClass.BlockType.Complete);
        UIHandler.instance._EquipWindowUI.ForceCloseUI();
        UIHandler.instance._MouseUI.ControlAppear(false);

        UpdateLevelButtonText();
    }

    public void UpdateLevelButtonText()
    {
        levelButton.SetText(cityStoreOwner.GetCityData.cityStoreLevel.ToString());
    }

    public void CloseUI(bool force = false)
    {
        if (buyHolder.activeInHierarchy && !force)
        {
            buyHolder.SetActive(false);
            return;
        }
        if (isUpgradeOpen && !force)
        {
            Upgrade_Close();
            return;
        }

        holder.SetActive(false);
        UIHandler.instance._DescriptionWindow.StopDescription();
        Invoke(nameof(CallFreePlayer), 0.1f);
        UIHandler.instance._MouseUI.ControlAppear(true);
    }

    void CallFreePlayer()
    {
        PlayerHandler.instance._playerController.block.RemoveBlock("CityCanvas");
    }

    public void UpdateRequiredHolder(bool hasRequired, MainBlueprintType requiredBlueprint)
    {
        requiredHolder.SetActive(hasRequired);

        bool hasBlueprint = PlayerHandler.instance._playerInventory.HasMainBlueprint(requiredBlueprint);

        requiredText.text = requiredBlueprint.ToString();

        if (hasBlueprint)
        {
            requiredText.color = Color.green;
        }
        else
        {
            requiredText.color = Color.red;
        }
        



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
                    OpenBuy_Gun(storeUnit.gunData);
                }
                if(storeUnit.activeData != null)
                {
                    OpenBuy_Ability(storeUnit.activeData);
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
    [SerializeField] TextMeshProUGUI buyCostText;
    [SerializeField] TextMeshProUGUI buy_DescriptionText;
    [SerializeField] Transform buy_Resource_Container;
    [SerializeField] ResourceUnit buy_Resource_Template;    
    [SerializeField] Transform buy_Gun_Container;
    [SerializeField] StatDescriptionUnit buy_Gun_Template;
    //stat - what are the 
    //resource - how much it costs to buy this item. same for ability or gun_Perma
    //description - just the description. same for ability or gun_Perma


    public void CallBuy()
    {
        //i check if the player has the resource
        //then check for requirement.
        //then we need to give it to the player

        bool canBuyResource = false;

        if(currentStoreUnit.gunData != null)
        {
            canBuyResource = PlayerHandler.instance._playerInventory.HasEnoughResourceToBuy(currentStoreUnit.gunData.requirementToUnluck.requiredResourceList);
        }
        if(currentStoreUnit.activeData != null)
        {
            canBuyResource = PlayerHandler.instance._playerInventory.HasEnoughResourceToBuy(currentStoreUnit.activeData.requirementToUnluck.requiredResourceList);
        }

        Debug.Log("this is the required resource " + canBuyResource);

        if (!canBuyResource) return;

        if(currentStoreUnit.gunData != null)
        {
            //i have to call someone to update the stuff
            CityHandler.instance.BuyAndUpdateGun(currentStoreUnit.gunData);
            PlayerHandler.instance._playerInventory.SpendResourceListForCity(currentStoreUnit.gunData.requirementToUnluck.requiredResourceList);
            currentStoreUnit.SetAsBought();
        }

        if(currentStoreUnit.activeData != null)
        {
            CityHandler.instance.BuyAndUpdateAbility(currentStoreUnit.activeData);
            PlayerHandler.instance._playerInventory.SpendResourceListForCity(currentStoreUnit.activeData.requirementToUnluck.requiredResourceList);
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

    void OpenBuy_Gun(ItemGunData data)
    {
        buyHolder.SetActive(true);

        buyPortrait.sprite = data.itemIcon;
        buyNameText.text = data.itemName;
        buyTypeText.text = "Gun";

        Create_Stats_Gun(data);
        Create_ResourceCost(data.requirementToUnluck.requiredResourceList);
        Create_Description(data.itemDescription);

        CreateCostText();
    }

    void CreateCostText()
    {
        if(currentStoreUnit.gunData!= null)
        {
            List<string> stringList = currentStoreUnit.gunData.GetStringPriceList();
            buyCostText.text = "Cost:";

            foreach (var item in stringList)
            {
                buyCostText.text += item;
            }

        }

        if(currentStoreUnit.activeData!= null)
        {
            List<string> stringList = currentStoreUnit.activeData.GetStringPriceList();
            buyCostText.text = "Cost:";

            foreach (var item in stringList)
            {
                buyCostText.text += item;
            }
        }


    }

    void OpenBuy_Ability(AbilityActiveData data)
    {
        buyHolder.SetActive(true);

        buyPortrait.sprite = data.abilityIcon;
        buyNameText.text = data.abilityName;
        buyTypeText.text = "Ability";

        Create_Stat_Ability(data);
        Create_ResourceCost(data.requirementToUnluck.requiredResourceList);
        Create_Description(data.abilityDescription);

        CreateCostText();
    }

    void Create_Stats_Gun(ItemGunData data)
    {
        //just use the gunstatreflist.
        //
        StatClass[] gunBaseStatArray = data.GetGunStatList();


        for (int i = 0; i < buy_Gun_Container.childCount; i++)
        {
            Destroy(buy_Gun_Container.GetChild(i).gameObject);
        }

        foreach (var item in gunBaseStatArray)
        {
            StatDescriptionUnit newObject = Instantiate(buy_Gun_Template);
            newObject.transform.SetParent(buy_Gun_Container);
            newObject.SetUp(item.stat, item.value);
        }

    }

    void Create_Stat_Ability(AbilityActiveData data)
    {
        //what are the ability stats?
        //cooldown, Damage, 
        //this should be custom.



    }

    void Create_ResourceCost(List<ResourceClass> costList)
    {
        for (int i = 0; i < buy_Resource_Container.childCount; i++)
        {
            Destroy(buy_Resource_Container.GetChild(i).gameObject);
        }

        foreach (var item in costList)
        {
            ResourceUnit newObject = Instantiate(buy_Resource_Template);
            newObject.transform.SetParent(buy_Resource_Container);
            ItemClass newItem = new ItemClass(item.data, item.quantity);
            newObject.SetUp(newItem);
        }


    }

    void Create_Description(string description)
    {
        buy_DescriptionText.text = description;
    }

    #endregion

    #region STAGE
    //we will still spawn teh fella but with another type of unit.
    [Separator("STAGE")]
    [SerializeField] StageUnit stageUnitTemplate;
    StageUnit currentStageUnit;

    [Separator("STAGE DESCRIPTION")]
    [SerializeField] GameObject stageDescription_Holder;
    [SerializeField] TextMeshProUGUI stageDescription_NameText;
    [SerializeField] Image stageDescription_Portrait;
    [SerializeField] GameObject stageDescription_BuyButton;

    //

    void ResetStageDescription()
    {
        stageDescription_Holder.SetActive(false);

        if(currentStageUnit != null)
        {
            currentStageUnit.Unselect();
            currentStageUnit = null;
        }
    }

    public void SetStage(List<CityStageClass> cityStageClassList)
    {

        TryCreateContainer(0, "Stages");
        ClearContainer(0);

        //i need to put this in a especial
        //or maybe we do the same thing but we create another place to show the stage.

        foreach (var item in cityStageClassList)
        {
            StageUnit newObject = Instantiate(stageUnitTemplate);
            newObject.SetUp(item, this);
            PlaceUnitInContainer(newObject.transform, 0);
        }

    }

    public void DescribeStage(StageUnit stageUnit, CityStageClass cityStageClass)
    {
        stageDescription_Holder.SetActive(true);

        if (currentStageUnit != null)
        {
            currentStageUnit.Unselect();
        }

        currentStageUnit = stageUnit;
        currentStageUnit.Select();


        stageDescription_NameText.text = cityStageClass.stageData.stageName;
        stageDescription_Portrait.sprite = cityStageClass.stageData.stageSprite;

    }

    public void StartStage()
    {
        if (currentStageUnit == null) return;

        //and if we are allowed to use this thing.
        //then we are going to order the sceneloader to load the scene.
        
        GameHandler.instance._sceneLoader.LoadStage(currentStageUnit.stageData);
    }




    #endregion

    #region UPGRADE
    [Separator("UGPRADE HOLDER")]
    [SerializeField] Transform upgradeHolder;
    [SerializeField] Image buttonImage;
    [SerializeField] Transform upgradeContainer;
    [SerializeField] ResourceUnit resourceUnitTemplate;
    [SerializeField] TextMeshProUGUI _upgradeFailureText;
    Vector3 originalUpgradeHolderLocalPosition;
    Vector3 _originalUpgradeFailureTextPosition;

    bool isUpgradeOpen;

    public void SetUpgradeHolder()
    {
        //at the start we are going to set its resource cost.
        //and we are going to see the positions as well.

        if(cityStoreOwner == null )
        {
            cityStoreOwner = transform.parent.GetComponent<CityStore>();
        }

        if(upgradeHolder == null )
        {
            Debug.Log("this has no ugprade holder. was it intentional? " + cityStoreOwner.name);    
            return;
        }

        UpdateUpgradeResourceCost(cityStoreOwner.GetCityData);

        originalUpgradeHolderLocalPosition = upgradeHolder.localPosition;
        Upgrade_Close();
    }

    public void Upgrade_Open()
    {
        //we open the upgrade
        //we check if ther eis enough and change the color of the button upgrade accordingly.

        upgradeHolder.gameObject.SetActive(true);

        upgradeHolder.DOKill();
        upgradeHolder.DOLocalMoveY(originalUpgradeHolderLocalPosition.y, 0.5f).SetEase(Ease.Linear).SetUpdate(true);
        isUpgradeOpen = true;
        UpdateAvaialabilityOfUpgradeButton();

    }
    public void Upgrade_Close()
    {
        upgradeHolder.DOKill();
        upgradeHolder.DOLocalMoveY(originalUpgradeHolderLocalPosition.y - 1000, 0.5f).SetEase(Ease.Linear).SetUpdate(true);
        isUpgradeOpen = false;


        if(UIHandler.instance != null)
        {
            UIHandler.instance._DescriptionWindow.StopDescription();
        }
        
    }

    void UpdateUpgradeResourceCost(CityData data)
    {
        //we need oto call this

       if(data == null)
        {
            Debug.LogError("received no data here " + cityStoreOwner.name);
            return;
        }

        int currentLevel = data.cityStoreLevel;

        if(currentLevel >= data.requirementToIncreaseLevelList.Count)
        {
            Debug.Log("WARNING - THIS DEOST NOT HAVE UNLUCK CLASS FOR THEWIR LEVEL " + cityStoreOwner.name);
            return;
        }


        RequirementToUnluckClass requirementClass = data.requirementToIncreaseLevelList[currentLevel];

        if(requirementClass == null)
        {
            Debug.Log("WARNING - THIS DEOST NOT HAVE UNLUCK CLASS FOR THEWIR LEVEL " + cityStoreOwner.name);
            return;
        }


        for (int i = 0; i < upgradeContainer.childCount; i++)
        {
            Destroy(upgradeContainer.GetChild(i).gameObject);
        }

        ResourceUnit newObject = Instantiate(resourceUnitTemplate);
        newObject.transform.SetParent(upgradeContainer);
        newObject.SetUp(new ItemClass(data.resourcePopRef, data.popRequirement));

        foreach (var item in requirementClass.requiredResourceList)
        {
            ResourceUnit newObject2 = Instantiate(resourceUnitTemplate);
            newObject2.transform.SetParent(upgradeContainer);
            newObject2.SetUp(new ItemClass(item.data, item.quantity));
        }

       

    }

    void UpdateAvaialabilityOfUpgradeButton()
    {
           

        bool canBuy = CanBuyUpgrade();


        if (canBuy)
        {
            buttonImage.color = Color.green;
        }
        else
        {
            buttonImage.color = Color.red;
        }
    }

    public void ButtonCall_BuyUpgrade()
    {
        //

        bool hasMainUpgrade = cityStoreOwner.GetCityData.HasMainBlueprint();

        if (!hasMainUpgrade)
        {
            GameHandler.instance._soundHandler.CreateSfx(SoundType.AudioClip_Failure);
            CallUpgradeFailure("No Main blueprint. Try completing some quests.");
            Debug.Log("main upgrade ");
            return;
        }

        


        bool canBuy = CanBuyUpgrade();


        if(canBuy)
        {
            //so now we need to consume pop. should i check everything again?
            List<ResourceClass> costList = cityStoreOwner.GetCityData.GetCurrentResourceListBasedInLevel();
            cityStoreOwner.IncreaseStoreLevel();
            UpdateLevelButtonText();
            PlayerHandler.instance._playerInventory.SpendResourceListForCity(costList);
            UpdateUpgradeResourceCost(cityStoreOwner.GetCityData);
            CityHandler.instance._cityBuildingHandler.UpdatePopResource();
            UpdateAvaialabilityOfUpgradeButton();
        }
        else
        {
            Debug.Log("cannot buy it");
            CallUpgradeFailure("Not enough resources.");
            GameHandler.instance._soundHandler.CreateSfx(SoundType.AudioClip_Failure);
        }
    }

    public bool CanBuyUpgrade()
    {
        bool canUsePop = PlayerHandler.instance._playerInventory.HasPop(cityStoreOwner.GetCityData.popRequirement);

        if (!canUsePop)
        {
            return false;
        }

        List<ResourceClass> costList = cityStoreOwner.GetCityData.GetCurrentResourceListBasedInLevel();


        if (costList == null) return false;

        return PlayerHandler.instance._playerInventory.HasEnoughResourceToBuy(costList);
    }




    void CallUpgradeFailure(string failureText)
    {
        _upgradeFailureText.text = failureText;
        
        StopCoroutine(nameof(UpgradeFailureProcess));
        StartCoroutine(UpgradeFailureProcess());

    }
    IEnumerator UpgradeFailureProcess()
    {
        _upgradeFailureText.DOKill();

        _upgradeFailureText.transform.localPosition = _originalUpgradeFailureTextPosition;
        _upgradeFailureText.DOFade(0, 0);

        _upgradeFailureText.DOFade(1, 0.5f).SetEase(Ease.Linear).SetUpdate(true).OnComplete(FadeAway);

        float value = 0.3f;

        for (int i = 0; i < 15; i++)
        {
            float randomX = Random.Range(-value, value);
            float randomY = Random.Range(-value, value);

            _upgradeFailureText.transform.localPosition = _originalUpgradeFailureTextPosition + new Vector3(randomX, randomY);

            yield return new WaitForSeconds(0.03f);
        }
    }

    void FadeAway()
    {
        _upgradeFailureText.DOFade(0, 0.8f).SetEase(Ease.Linear).SetUpdate(true);
    }

    #endregion

    #region MAIN BUILDING


    #endregion

    #region CONTAINERS
    //for now i will just create the same 
    [Separator("CONTAINER")] 
    [SerializeField] Transform containerTemplate;
    [SerializeField] Transform containerTemplate_Quest;
    [SerializeField] Transform containerTemplate_Stat;
    [SerializeField] Transform containerHolder;
    [SerializeField] Transform containerForButton;
    [SerializeField] CityStore_ButtonCategory buttonCategoryTemplate;
    [SerializeField] GameObject categoryNameHolder;
    [SerializeField] TextMeshProUGUI caegoryNameText;

    List<Transform> containerList = new();
    List<CityStore_ButtonCategory> buttonCategoryList = new();

    //also we need a button for doing this. but we can create a better system here.

    public void PlaceUnitInContainer(Transform unit, int index)
    {
        if(index + 1 > containerList.Count)
        {
            //this container doesnt yet exist, so we create the container and the button.
            Debug.Log("we do not have this container");
            
        }

        unit.transform.SetParent(containerList[index]);       
    }

    void ClearContainer(int index)
    {
        if(index + 1 > containerList.Count)
        {
            Debug.Log("yo");
        }

        for (int i = 0; i < containerList[index].childCount; i++)
        {
            Destroy(containerList[index].GetChild(i).gameObject);
        }
    }

    void TryCreateContainer(int index, string name)
    {
        //if there is this container already then we dont create it.

        //need to change the category based in teh thing. so i create the


        if(buttonCategoryList.Count > 0)
        {
            SelectNewCategory(buttonCategoryList[0], 0);
        }

        if (containerList.Count >= index + 1) return;
       
            //i should not create container.

            Transform newObject = Instantiate(containerTemplate, containerTemplate.transform.position, Quaternion.identity);
            newObject.gameObject.SetActive(false);
            newObject.SetParent(containerHolder);
            newObject.position = containerTemplate.transform.position;
            newObject.transform.localScale = Vector3.one;
            containerList.Add(newObject);



            newObject.name = name;

            CityStore_ButtonCategory newObject_Button = Instantiate(buttonCategoryTemplate);
            newObject_Button.SetUp(index, this);
            newObject_Button.transform.SetParent(containerForButton);
            buttonCategoryList.Add(newObject_Button);

            if(index == 0)
            {
                SelectNewCategory(newObject_Button, 0);
            }

        
        containerForButton.gameObject.SetActive(buttonCategoryList.Count > 0);

    }

    void TryCreateContainer_Quest(int index, string name)
    {
        //if there is this container already then we dont create it.

        //need to change the category based in teh thing. so i create the
        if (buttonCategoryList.Count > 0)
        {
            SelectNewCategory(buttonCategoryList[0], 0);
        }

        if (containerList.Count >= index + 1) return;

        
            Transform newObject = Instantiate(containerTemplate_Quest, containerTemplate.transform.position, Quaternion.identity);
            newObject.gameObject.SetActive(false);
            newObject.SetParent(containerHolder);
            newObject.position = containerTemplate.transform.position;
            newObject.transform.localScale = Vector3.one;
            containerList.Add(newObject);



            newObject.name = name;

            CityStore_ButtonCategory newObject_Button = Instantiate(buttonCategoryTemplate);
            newObject_Button.SetUp(index, this);
            newObject_Button.transform.SetParent(containerForButton);
            buttonCategoryList.Add(newObject_Button);



            if (index == 0)
            {
                SelectNewCategory(newObject_Button, 0);
            }

        

        containerForButton.gameObject.SetActive(buttonCategoryList.Count > 0);

    }

    void TryCreateContainer_Stat(int index, string name)
    {
        if (buttonCategoryList.Count > 0)
        {
            SelectNewCategory(buttonCategoryList[0], 0);
        }

        if (containerList.Count >= index + 1) return;

        
            Transform newObject = Instantiate(containerTemplate_Stat, containerTemplate.transform.position, Quaternion.identity);
            newObject.gameObject.SetActive(false);
            newObject.SetParent(containerHolder);
            newObject.position = containerTemplate.transform.position;
            newObject.transform.localScale = Vector3.one;
            containerList.Add(newObject);

            newObject.name = name;
            CityStore_ButtonCategory newObject_Button = Instantiate(buttonCategoryTemplate);
            newObject_Button.SetUp(index, this);
            newObject_Button.transform.SetParent(containerForButton);
            buttonCategoryList.Add(newObject_Button);



            if (index == 0)
            {
                SelectNewCategory(newObject_Button, 0);
            }

        

        containerForButton.gameObject.SetActive(buttonCategoryList.Count > 0);
    }

    CityStore_ButtonCategory currentButton;
    public void SelectNewCategory(CityStore_ButtonCategory button, int index)
    {

        if(especialNpcLimitText != null && hasEspecialNpcHolder)
        {
            especialNpcHolder.SetActive(index == 0);
        }


        if(currentButton != null)
        {
            currentButton.UnSelect();
        }

        currentButton = button; 
        currentButton.Select();


        HideAllContainers();

        containerList[index].gameObject.SetActive(true);

        caegoryNameText.text = containerList[index].name;

    }

    void HideAllContainers()
    {
        foreach (var item in containerList)
        {
            item.gameObject.SetActive(false);
        }
    }

    #endregion

    
}

//

