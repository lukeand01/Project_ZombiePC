using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestBase : MonoBehaviour, IInteractable
{
    string id;
    protected bool isLocked;
    [Separator("CHESTBASE")]
    [SerializeField] protected InteractCanvas interactCanvas;
    [SerializeField] protected Transform graphic_Body;
    [SerializeField] protected Transform graphic_Lid;
    [SerializeField] protected AudioClip openChestClip;
    private void Awake()
    {
        id = Guid.NewGuid().ToString(); 
    }


    public virtual void ResetForPool()
    {
        graphic_Body.gameObject.SetActive(true);
        graphic_Lid.gameObject.SetActive(true);
        interactCanvas.gameObject.SetActive(false);
    }

    public string GetInteractableID()
    {
        return id;
    }

    public virtual void Interact()
    {
        
    }

    public virtual void InteractUI(bool isVisible)
    {
        interactCanvas.gameObject.SetActive(isVisible);
        interactCanvas.ControlInteractButton(isVisible);

        //this makes the gunchest update the price.



    }

    public virtual bool IsInteractable()
    {
        return !isLocked;
    }


    private void OnDisable()
    {
        interactCanvas.ControlInteractButton(false);
    }


    [SerializeField] int amountAllowed;
    int amountUsed;

    public virtual void ProgressChest()
    {
        amountUsed++;

        if (amountUsed >= amountAllowed)
        {
            Destroy(gameObject);
        }
    }
}
