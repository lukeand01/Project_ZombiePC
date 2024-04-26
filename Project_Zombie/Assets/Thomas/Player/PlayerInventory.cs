using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{

    private void Start()
    {
        interactMask |= (1 << 7);

    }
    private void Update()
    {
        CheckForInteractables();
    }


    #region INVENTORY



    [SerializeField] List<ItemClass> inventoryList = new();
    [SerializeField]List<ItemClass> inventoryListForUI = new();
    public void AddItem(ItemClass item, bool appearInUI = true)
    {
        //no slots no nothing. just stack it.

        //here we pass

        if (appearInUI)
        {
            //wthen we infomr the ui to show that it just received this thing.
            CheckItemInUI(item.data);
        }


        int index = GetListIndex(item.data);

        if(index == -1)
        {
            inventoryList.Add(new ItemClass(item.data, item.quantity));
        }
        else
        {
            inventoryList[index].AddQuantity(item.quantity);
        }


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


    int GetListIndex(ItemData data)
    {
        for (int i = 0; i < inventoryList.Count; i++)
        {
            if (inventoryList[i].data == data)
            {
                return i;   
            }
        }

        return -1;
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
