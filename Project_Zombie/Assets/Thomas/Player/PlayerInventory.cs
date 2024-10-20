using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class PlayerInventory : MonoBehaviour
{

    PlayerHandler handler;

    private void Awake()
    {
        handler = GetComponent<PlayerHandler>();
        
    }

    
    private void Start()
    {
        interactMask |= (1 << 7);
        SetCityInventoryList();

        foreach (var item in initialCityInventoryList)
        {
            AddItemForCity(item);
        }

        foreach (var item in initialStageInventoryList)
        {
            AddItemForStage(item);
        }


        SetTools();
        DebugAddSigil();

    }
    private void Update()
    {
        CheckForInteractables();
    }


    private void FixedUpdate()
    {
        HandleResourceGather();
    }

    #region INVENTORY
    //i need ref to all fellas.

    [Separator("RESOURCE ITEMS")]
    [SerializeField]List<ItemResourceData> refItemList = new();
    [SerializeField] List<ItemClass> stageInventoryList = new();
    List<ItemClass> inventoryListForUI = new();
    [SerializeField] List<ItemClass> cityInventoryList = new();
    Dictionary<ItemData, ItemClass> cityInventoryDictionary = new();
    //i need an inventory for the stage and an inventory for city

    [Separator("ITENS FOR THE START")]
    [SerializeField] List<ItemClass> initialCityInventoryList = new();
    [SerializeField] List<ItemClass> initialStageInventoryList = new();

    public List<ItemClass> GetStageInventoryList() => stageInventoryList;

    void SetCityInventoryList()
    {

        if(cityInventoryList.Count > 0)
        {
            return;
        }

        cityInventoryList.Clear();
        //we set the inventory.



        foreach (var item in refItemList)
        {

            ItemClass newItem = new ItemClass(item, 0);
            UIHandler.instance._CityUI.CreateResourceUnitForItem(newItem);

            if (item.resourceType == ItemResourceType.Population)
            {
                newItem.SetPopUsage(0);
            }

            cityInventoryList.Add(newItem);
            cityInventoryDictionary.Add(newItem.data, newItem); 
        }

    }

   
   
    public void AddItemForStage(ItemClass item, bool appearInUI = true)
    {
        //no slots no nothing. just stack it.

        //here we pass

        if (appearInUI)
        {
            //wthen we infomr the ui to show that it just received this thing.
            CheckItemInUI(item);
        }


        int index = GetListIndex(item.data, stageInventoryList);

        if(index == -1)
        {
            stageInventoryList.Add(new ItemClass(item.data, item.quantity));
        }
        else
        {
            stageInventoryList[index].AddQuantity(item.quantity);
        }


    }

    public void AddItemForCity(ItemClass item, bool appearInUI = true)
    {
        if (appearInUI)
        {
            //wthen we infomr the ui to show that it just received this thing.
            CheckItemInUI(item);
        }


        int index = GetListIndex(item.data, cityInventoryList);



        if (index == -1)
        {
            ItemClass newItem = new ItemClass(item.data, item.quantity);
            cityInventoryList.Add(newItem);
            UIHandler.instance._CityUI.CreateResourceUnitForItem(newItem);
        }
        else
        {
            cityInventoryList[index].AddQuantity(item.quantity);
        }
    }

    public void RemoveItemForCity(ItemClass _itemClass)
    {
        int index = GetListIndex(_itemClass.data, cityInventoryList);

        if(index != -1)
        {
            cityInventoryList[index].RemoveQuantity(_itemClass.quantity);
        }
    }

    //the only problem that is that resounrce pop works differently.

    public bool HasEnoughItemForCity(ItemClass _itemClass)
    {
        int index = GetListIndex(_itemClass.data, cityInventoryList);

        if (index == -1) return false;

        return cityInventoryList[index].quantity >= _itemClass.quantity;

    }

    public void AddItemInUI(ItemClass item)
    {
        //we add items to the list and once we receive the item we show it all in ui.
        inventoryListForUI.Add(item);   
    }

    
    void CheckItemInUI(ItemClass item)
    {
        //this list is the pauselist.
        //the item must be added before being called.
 
        for (int i = 0; i < inventoryListForUI.Count; i++)
        {
            if (inventoryListForUI[i].data == item.data)
            {
                //then we call the ui and remove this fella.
                UIHandler.instance.InventoryUI.CallItemNotification(inventoryListForUI[i]);
                inventoryListForUI.RemoveAt(i);
                return;
            }
        }

        UIHandler.instance.InventoryUI.CallItemNotification(item);

    }

    int GetListIndex(ItemData data, List<ItemClass> targetList)
    {


        for (int i = 0; i < targetList.Count; i++)
        {
            if (targetList[i].data == data)
            {
                return i;   
            }
        }

        return -1;
    }


    public void PassStageInventoryToCityInventory()
    {
        //we do that when we go to city.

        foreach (var item in stageInventoryList)
        {
            AddItemForCity(item, false);
        }

        GameHandler.instance._saveHandler.CaptureStateUsingCurrentSaveSlot();

        stageInventoryList.Clear();
    }

    void UpdateCityInventory()
    {
        //iu dont need to update this because i will do everything in the other thing


        
    }


    public bool HasEnoughResourceToBuy(List<ResourceClass> resourceCostList)
    {

        foreach (var resource in resourceCostList)
        {

            ItemClass itemClass = resource.GetItem();

            if (!cityInventoryDictionary.ContainsKey(itemClass.data))
            {
                Debug.Log("returned false because found nothing");
                return false;
            }

            ItemClass rightClass = cityInventoryDictionary[itemClass.data];

            if (itemClass.quantity > rightClass.quantity)
            {

                return false;
            }

        }

        return true;
    }

    public bool HasResourceWithName_City(string name)
    {
        foreach (var item in cityInventoryList)
        {
            if(item.data.name == name && item.quantity > 0)
            {
                return true;
            }
        }

        return false;
    }
    
    public void SpendResourceWithName_City(string name)
    {
        foreach (var item in cityInventoryList)
        {
            if(item.data.name == name)
            {
                item.RemoveQuantity(1);
            }
        }
    }

    public void SpendResourceListForCity(List<ResourceClass> resourceCostList)
    {
        foreach (var item in resourceCostList)
        {
            RemoveItemForCity(item.GetItem());
        }
    }

    #endregion

    #region INTERACT

    IInteractable currentInteract;
    [SerializeField] LayerMask interactMask;

    public void InteractWithCurrentInteractable()
    {
        if (currentInteract != null)
        {

            currentInteract.Interact();
        }


    }
    public bool HasCurrentInteract()
    {
        return currentInteract != null;
    }

    void CheckForInteractables()
    {
        //we keep checking aruond for anyone in the right enemyLayer


        RaycastHit[] interactList = Physics.SphereCastAll(transform.position, 3, Vector3.forward, 1, interactMask);


        if (interactList.Length <= 0)
        {
            DisableCurrentInteract();
            currentInteract = null;
            return;
        }

        //we always only care about the first.

        IInteractable interact = null;

        for (int i = 0; i < interactList.Length; i++)
        {
            IInteractable ThisInteract = interactList[i].collider.GetComponent<IInteractable>();

            if (ThisInteract == null) continue;

            if (!ThisInteract.IsInteractable()) continue;

            interact = ThisInteract;
            break;  

        }


        if(interact == null)
        {
            DisableCurrentInteract();
            return;
        }


        if(currentInteract != null)
        {
            if(interact.GetInteractableID() != currentInteract.GetInteractableID())
            {
                DisableCurrentInteract();
            }
        }

   
       

        currentInteract = interact;
        currentInteract.InteractUI(true);
    }

    public void DisableCurrentInteract()
    {
        if (currentInteract != null)
        {
            currentInteract.InteractUI(false);
            currentInteract = null;

        }
    }

    public void DisableCurrentInteractWithID(string id)
    {
        if (currentInteract != null)
        {

            if (currentInteract.GetInteractableID() == id)
            {
                Debug.Log("disable with id");
                currentInteract.InteractUI(false);
                currentInteract = null;
            }


        }
    }


    #endregion

    #region RESOURCE GATHER

    ResourceGather currentResourceGather;

    void HandleResourceGather()
    {
        if (currentResourceGather == null) return;

        currentResourceGather.HandleResourceGather(this);
    }

    public void SetResourceGather(ResourceGather newResourceGather)
    {
        currentResourceGather = newResourceGather;
        handler._entityEvents.eventHardInput += RemoveResourceGather;

        handler._playerController.block.AddBlock("Resourcegather", BlockClass.BlockType.Rotation);

    }

    public void RemoveResourceGather()
    {
        //if there is any movement then we should remove resource gather. combat, combat

        if (currentResourceGather != null)
        {
            currentResourceGather.RemoveResourceGather();
        }

        Debug.Log("remove resource");
        handler._entityEvents.eventHardInput -= RemoveResourceGather;
        handler._playerController.block.RemoveBlock("Resourcegather");

        currentResourceGather = null;
    }


    #endregion

    #region POPULATION

    public int popMax { get; private set; }
    public int popUse { get; private set; }

    //pop is always the first in the city inventory.

    public void SetPopulation(int newPopulation)
    {
        

        popMax = newPopulation;
        //then we update the ui.
        cityInventoryList[0].SetPopCap(popMax);

    }
    public void SetPopulationUsage(int popUsage)
    {
        popUse = popUsage;
        cityInventoryList[0].SetPopUsage(popUsage);

    }

    public bool HasPop(int cost)
    {
        ItemClass popItem = cityInventoryList[0];
        int surplus = popItem.quantity - popItem.popUsage ;


        return surplus >= cost;
    }


    #endregion

    #region BOSS SIGILS

    public Action<List<BossSigilType>> eventUpdateBossSigilUI;

    public void OnUpdateBossSigilUI(List<BossSigilType> bossSigilType) => eventUpdateBossSigilUI?.Invoke(bossSigilType);

    [field:SerializeField] public List<BossSigilType> bossSigilList { get; private set; } = new();
    
    public List<BossSigilType> readyToBeUsedSigilList { get; private set; } = new();
    //when we use i want to sepdn it.

    [ContextMenu("ADD KNIGHT")]
    public void DebugAddSigil()
    {
        AddBossSigil(BossSigilType.MiniBoss_Knight);
        AddBossSigil(BossSigilType.MiniBoss_Ghost);
        AddBossSigil(BossSigilType.MiniBoss_Mage);
    }

    public void AddBossSigil(BossSigilType bossSigil)
    {
        bossSigilList.Add(bossSigil);
        readyToBeUsedSigilList = GetBossSigilForPortal();
        OnUpdateBossSigilUI(readyToBeUsedSigilList);
        //everytime we do this we update the ui and we are close we simply show it.
    }

    public bool HasEnoughSigil()
    {
        for (int i = 0; i < bossSigilList.Count; i++)
        {
            var item = bossSigilList[i];
            if (item == BossSigilType.Nothing) return false;
        }

        return true;
    }

    //first we will check if any of them have three.
    List<BossSigilType> GetBossSigilForPortal()
    {

       //we first try to get them into three unique
       //then we try to build them into three of the same
       //then we just take what we can.

    
        Dictionary<BossSigilType, int> quantitySigilDictionary = new();
        List<BossSigilType> uniqueList = new();
        List<BossSigilType> defaultList = new();
        BossSigilType bossWithThreeCopies = BossSigilType.Nothing;

        //we first to detect if there are three fellas. 
        for (int i = 0; i < bossSigilList.Count; i++)
        {
            var item = bossSigilList[i];

            if(defaultList.Count < 3)
            {
                defaultList.Add(item);
            }

            if (!quantitySigilDictionary.ContainsKey(item))
            {
                quantitySigilDictionary.Add(item, 1);
                uniqueList.Add(item);
            }
            else
            {
                quantitySigilDictionary[item] += 1;
            }
      
        }


        //otherwise 
        int safeBreak = 0;

        if(uniqueList.Count >= 3)
        {
            return new List<BossSigilType>() { uniqueList[0], uniqueList[1], uniqueList[2] };
        }

        if(bossWithThreeCopies != BossSigilType.Nothing)
        {
            new List<BossSigilType>() { bossWithThreeCopies, bossWithThreeCopies, bossWithThreeCopies };
        }

        //then we get random fellas.

        while(defaultList.Count < 3)
        {
            defaultList.Add(BossSigilType.Nothing);
        }

        return defaultList;
    }
    public void SpendSigil()
    {
        for (int i = 0; i < readyToBeUsedSigilList.Count; i++)
        {
            var item = readyToBeUsedSigilList[i];
            if (bossSigilList.Contains(item))
            {
                bossSigilList.Remove(item);
            }
        }

        readyToBeUsedSigilList = GetBossSigilForPortal();
        OnUpdateBossSigilUI(readyToBeUsedSigilList);
    }


    #endregion

    #region TOOLS

    [Separator("TOOLS")]
    [SerializeField] ToolData[] _initialToolArray;
    [SerializeField] List<ToolClass> _toolList = new();
    Dictionary<ToolType, List<ToolClass>> _toolDictionary = new();
    void SetTools()
    {
        for (int i = 0; i < _initialToolArray.Length; i++)
        {
            var item = _initialToolArray[i];

            AddTool(item);

        }
    }

    public void AddTool(ToolData data)
    {
        for (int i = 0; i < _toolList.Count; i++)
        {
            var item = _toolList[i];
            if (item._data == data) return;
        }

        ToolClass toolClass = new ToolClass(data);
        UIHandler.instance._pauseUI.CreateToolUnit(toolClass);
        _toolList.Add(toolClass);
        data.OnAdded();

        if (_toolDictionary.ContainsKey(data._toolType))
        {
            _toolDictionary[data._toolType].Add(toolClass);
        }
        else
        {
            _toolDictionary.Add(data._toolType, new List<ToolClass>() { toolClass});
        }


    }

    //but we need to get a random one.
    public bool HasTool(ToolType _toolType)
    {
        return _toolDictionary.ContainsKey(_toolType);
    }

    public ToolData GetRandomTool(ToolType _toolType)
    {
        if (!_toolDictionary.ContainsKey(_toolType)) return null;


        List<ToolClass> toolList = _toolDictionary[_toolType];

        if(toolList.Count <= 0)
        {
            Debug.Log("the list has nothing");
            return null;
        }

        int random = UnityEngine.Random.Range(0,toolList.Count);
        return toolList[random]._data;


    }


    public void AddIngredient(ToolData toolData, IngredientData ingredientData, int quantity = 1)
    {
        for (int i = 0; i < _toolList.Count; i++)
        {
            var item = _toolList[i];

            if (item._data != toolData) continue;

            
            item.AddIngredient(ingredientData, quantity);

            GameHandler.instance._soundHandler.CreateSfx_WithAudioClip(item._data._harvestAudioClip);
            return;
        }
    }
    public void ConsumeIngredient(ToolData toolData, IngredientData ingredientData, int quantity = 1)
    {
        for (int i = 0; i < _toolList.Count; i++)
        {
            var item = _toolList[i];

            if (item._data != toolData) continue;

            item.RemoveIngredient(ingredientData, quantity);

            return;
        }
    }
    public void ResetToolList()
    {
        for (int i = 0; i < _toolList.Count; i++)
        {
            var item = _toolList[i];
            item._data.OnRemoved();
        }

        _toolList.Clear();
        _toolDictionary.Clear();
    }

    #endregion

    #region SAVE DATA

    public void CaptureState(SaveClass saveClass)
    {
        List<int> inventoryList = new();

        for (int i = 0; i < cityInventoryList.Count; i++)
        {
            var item = cityInventoryList[i];
            inventoryList.Add(item.quantity);
        }

        saveClass.MakePlayerInventory(inventoryList);


        //MAIN BLUEPRINT

        List<int> mainBlueprintNewList = new();

        for (int i = 0; i < _mainBlueprintList.Count; i++)
        {
            var item = _mainBlueprintList[i];

            mainBlueprintNewList.Add((int)item);
        }

        saveClass.MakeMainBlueprintList(mainBlueprintNewList);

    }

    public void RestoreState(SaveClass saveClass)
    {
        List<int> inventoryList = saveClass._playerInventoryList;

        //Debug.Log("restore the state " + inventoryList.Count);
        //we start at 0 because we dont need to save pop.
        //

        if(cityInventoryList.Count <= 0)
        {
            SetCityInventoryList();
        }

        for (int i = 1; i < inventoryList.Count; i++)
        {
            var value = inventoryList[i];
            var item = cityInventoryList[i];



            item.quantity = value;
            item.UpdateUI();
        }


        //MAIN BLUEPRINT

        List<int> mainBlueprintList = saveClass._mainBlueprintList;


        if(mainBlueprintList != null)
        {
            List<MainBlueprintType> refList = new()
        {
                MainBlueprintType.Main_Blueprint,
            MainBlueprintType.Armory_Blueprint,
            MainBlueprintType.Lab_Blueprint,
            MainBlueprintType.Drop_Launcher_Blueprint,
            MainBlueprintType.Body_Enhancer_Blueprint
        };

            _mainBlueprintList.Clear();

            for (int i = 0; i < mainBlueprintList.Count; i++)
            {
                var item = mainBlueprintList[i];

                _mainBlueprintList.Add(refList[item]);
            }
        }

        


    }


    #endregion


    #region MAIN BLUEPRINTS

    [SerializeField] List<MainBlueprintType> _mainBlueprintList = new();


    [ContextMenu("DEBUG ADD BLUEPRINT")]
    public void DebugAddBlueprint()
    {
        AddMainBlueprint(MainBlueprintType.Main_Blueprint);
    }

    public void AddMainBlueprint(MainBlueprintType newBlueprint)
    {
        if (_mainBlueprintList.Contains(newBlueprint))
        {
            Debug.Log("it contains");
            return;
        }
        Debug.Log("yo");

        _mainBlueprintList.Add(newBlueprint);
        GameHandler.instance._saveHandler.CaptureStateUsingCurrentSaveSlot();
    }

    public bool HasMainBlueprint(MainBlueprintType newBlueprint)
    {
        return _mainBlueprintList.Contains(newBlueprint);
    }


    #endregion

}

public enum MainBlueprintType
{
    Main_Blueprint = 0,
    Armory_Blueprint = 1,
    Lab_Blueprint = 2,
    Drop_Launcher_Blueprint = 3,
    Body_Enhancer_Blueprint = 4
}


public enum BossSigilType 
{ 
    MiniBoss_Knight,
    MiniBoss_Ghost,
    MiniBoss_Mage,
    MiniBoss_Artillery,
    Nothing,
    Boss_Devil,
    Boss_Tree,
    Boss_Twin

}


//i need something for pop here.
//the way it works is that it sets a limit and then we calculate the cost of everyone else.