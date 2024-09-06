using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{

    [SerializeField] InteractCanvas _interactCanvas;
    [SerializeField] int doorPriceBase; //should multiply by some _value.
    int currentPrice;
    [SerializeField] Room[] roomToOpenArray;

    [Separator("CONDITIONS TO OPEN DOOR")]
    [SerializeField] Door[] doorRequiredArray;


    string id;
    private void Awake()
    {
        id = Guid.NewGuid().ToString();
    }
    public string GetInteractableID()
    {
        return id;
    }

    public void Interact()
    {
        //i check if you ahve the money.


        if(doorRequiredArray.Length > 0)
        {
            Debug.Log("door required array ");
            foreach(var room in doorRequiredArray)
            {
                Debug.Log("ye");
                if (room != null)
                {
                    Debug.Log("still has the door");
                    return;
                }
            }
        }


        bool canOpen = PlayerHandler.instance._playerResources.HasEnoughPoints(doorPriceBase);

        if(canOpen)
        {
            PlayerHandler.instance._playerResources.SpendPoints(doorPriceBase);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("not enough");
        }

        LocalHandler local = LocalHandler.instance;


        if(local != null) 
        {
            foreach (var item in roomToOpenArray)
            {
                item.OpenRoom_Room();
            }

        }

        

    }

    public void InteractUI(bool isVisible)
    {


       

        float modifier = PlayerHandler.instance._entityStat.GetTotalEspecialConditionValue(EspecialConditionType.GatePriceModifier);
        float reduction = doorPriceBase * modifier;
        currentPrice = (int)(doorPriceBase - reduction);
        currentPrice = Mathf.Clamp(currentPrice, 0, 9999);

        _interactCanvas.ControlInteractButton(isVisible);
        _interactCanvas.ControlPriceHolder(currentPrice);

    }

    public bool IsInteractable()
    {
        return true;
    }
}
