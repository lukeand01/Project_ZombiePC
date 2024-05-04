using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{

    private void Start()
    {
        interactMask |= (1 << 7);
        SetCityInventoryList();
    }
    private void Update()
    {
        CheckForInteractables();
    }


    #region INVENTORY
    //i need ref to all fellas.

    [Separator("RESOURCE ITEMS")]
    [SerializeField] List<ItemResourceData> refItemList = new();
    [SerializeField] List<ItemClass> stageInventoryList = new();
    [SerializeField]List<ItemClass> inventoryListForUI = new();
    [SerializeField] List<ItemClass> cityInventoryList = new();
    //i need an inventory for the stage and an inventory for city

    void SetCityInventoryList()
    {

        if(cityInventoryList.Count > 0)
        {
            Debug.Log("city inventory list is already set");
            return;
        }

        cityInventoryList.Clear();
        //we set the inventory.
        //


        foreach (var item in refItemList)
        {
            ItemClass newItem = new ItemClass(item, 0);
            UIHandler.instance.CityUI.CreateResourceUnitForItem(newItem);
            cityInventoryList.Add(newItem);
        }

    }

    public void AddItemForStage(ItemClass item, bool appearInUI = true)
    {
        //no slots no nothing. just stack it.

        //here we pass

        if (appearInUI)
        {
            //wthen we infomr the ui to show that it just received this thing.
            CheckItemInUI(item.data);
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
            CheckItemInUI(item.data);
        }


        int index = GetListIndex(item.data, cityInventoryList);

        if (index == -1)
        {
            cityInventoryList.Add(new ItemClass(item.data, item.quantity));
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
    void CheckItemInUI(ItemData data)
    {
      
        for (int i = 0; i < inventoryListForUI.Count; i++)
        {
            if (inventoryListForUI[i].data == data)
            {
                //then we call the ui and remove this fella.
                UIHandler.instance.InventoryUI.CallItemNotification(inventoryListForUI[i]);
                inventoryListForUI.RemoveAt(i);
                return;
            }
        }
    }

    int GetListIndex(ItemData data, List<ItemClass> targetList)
    {
        for (int i = 0; i < targetList.Count; i++)
        {
            if (stageInventoryList[i].data == data)
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
        //we keep checking aruond for anyone in the right layer


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


      

        DisableCurrentInteract();

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
                currentInteract.InteractUI(false);
                currentInteract = null;
            }


        }
    }


    #endregion


}
