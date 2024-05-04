using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{

    [SerializeField] InteractCanvas _interactCanvas;
    [SerializeField] int doorPriceBase; //should multiply by some modifier.
    [SerializeField] Room[] roomToOpenArray;



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
        bool canOpen = PlayerHandler.instance._playerResources.CanSpendPoints(doorPriceBase);

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
                local.OpenRoom(item);
            }

        }

        

    }

    public void InteractUI(bool isVisible)
    {
        _interactCanvas.ControlInteractButton(isVisible);
        _interactCanvas.ControlPriceHolder(doorPriceBase);
    }

    public bool IsInteractable()
    {
        return true;
    }
}
