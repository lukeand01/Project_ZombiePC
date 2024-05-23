using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGather : MonoBehaviour, IInteractable
{
    string id;
    [Separator("UI")]
    [SerializeField] InteractCanvas _interactCanvas;
    [SerializeField] ResourceGatherCanvas _resourceGatherCanvas;


    //just click and they start doing things, but if the player moves then it stops.

    [Separator("ITEM")]
    [SerializeField] ItemResourceData resourceData;
    [SerializeField] int minResource = 1;
    [SerializeField] int maxResource = 1;

    [SerializeField] int resourceQuantity = 1; //how many times can we harvest this fella.
    
    float resourceCurrent;
    float resourceTotal;

    bool isInteracting;

    public void RemoveResourceGather()
    {
        Debug.Log("this was called");
        resourceCurrent = 0;
        isInteracting = false;
        _resourceGatherCanvas.Close();
    }

    public void HandleResourceGather(PlayerInventory inventory)
    {
        //we update ui
        //

        isInteracting = true;

        _resourceGatherCanvas.UpdateResourceGather(resourceQuantity, resourceCurrent, resourceTotal);
        _interactCanvas.ControlInteractButton(false);

        if(resourceCurrent > resourceTotal)
        {
            resourceCurrent = 0;
            //we get an item.
            int itemAmount = UnityEngine.Random.Range(minResource, maxResource);
            inventory.AddItemForStage(new ItemClass(resourceData, itemAmount));

            resourceQuantity -= 1;

            if(resourceQuantity <= 0)
            {
                inventory.RemoveResourceGather();
            }
        }
        else
        {
            resourceCurrent += Time.fixedDeltaTime;
        }

    }


    private void Awake()
    {
        id = Guid.NewGuid().ToString();
        resourceTotal = 5;
    }

    public string GetInteractableID()
    {
        return id;
    }

    public void Interact()
    {
        //we stop
        if (!isInteracting)
        {
            PlayerHandler.instance._playerInventory.SetResourceGather(this);
            Debug.Log("interacted");
        }
        
    }

    public void InteractUI(bool isVisible)
    {
        _interactCanvas.ControlInteractButton(isVisible);
    }

    public bool IsInteractable()
    {
        if (isInteracting)
        {
            return false;
        }

        return resourceQuantity > 0;
    }
}
