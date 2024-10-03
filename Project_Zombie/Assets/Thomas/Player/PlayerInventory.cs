using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            Debug.Log("city inventory list is already set");
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

    public List<BossSigilType> bossSigilList { get; private set; } = new();
    public List<BossSigilType> readyToBeUsedSigilList { get; private set; } = new();
    //when we use i want to sepdn it.



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

       


        //we check if any of them has three.
        //
        Dictionary<BossSigilType, int> quantitySigilDictionary = new();

        //we first to detect if there are three fellas. 
        for (int i = 0; i < bossSigilList.Count; i++)
        {
            var item = bossSigilList[i];

            if (!quantitySigilDictionary.ContainsKey(item))
            {
                quantitySigilDictionary.Add(item, 1);
            }
            else
            {
                quantitySigilDictionary[item] += 1;
            }

            if (quantitySigilDictionary[item] >= 3)
            {
                //we will return a list of three of this fella
                return new List<BossSigilType> { item, item, item };

            }
      
        }

        List<BossSigilType> sigilList = new();
        //otherwise 
        int safeBreak = 0;
        int number = 0;
        while(sigilList.Count < 3)
        {
            safeBreak++;

            if(safeBreak > 1000)
            {
                break;
            }

            if(number >= sigilList.Count)
            {
                sigilList.Add(BossSigilType.Nothing);
            }
            else
            {
                sigilList.Add(sigilList[number]);
                number++;
            }



        }

        return sigilList;


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
    [SerializeField] ToolData[] initialToolArray;
    [SerializeField] List<ToolClass> toolList = new();

    void SetTools()
    {
        for (int i = 0; i < initialToolArray.Length; i++)
        {
            var item = initialToolArray[i];

            AddTool(item);

        }
    }

    public void AddTool(ToolData data)
    {
        for (int i = 0; i < toolList.Count; i++)
        {
            var item = toolList[i];
            if (item._data == data) return;
        }

        ToolClass toolClass = new ToolClass(data);
        UIHandler.instance._pauseUI.CreateToolUnit(toolClass);
        toolList.Add(toolClass);
    }

    #endregion

}

public enum BossSigilType 
{ 
    MiniBoss_Knight,
    MiniBoss_Ghost,
    MiniBoss_Mage,
    MiniBoss_Artillery,
    Nothing,
    Boss_Devil

}


//i need something for pop here.
//the way it works is that it sets a limit and then we calculate the cost of everyone else.