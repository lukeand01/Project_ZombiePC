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
    [SerializeField] InteractCanvas interactCanvas;


    private void Awake()
    {
        id = Guid.NewGuid().ToString(); 
    }

    public string GetInteractableID()
    {
        return id;
    }

    public virtual void Interact()
    {
        
    }

    public void InteractUI(bool isVisible)
    {
        interactCanvas.ControlInteractButton(isVisible);
    }

    public bool IsInteractable()
    {
        return !isLocked;
    }


    private void OnDisable()
    {
        interactCanvas.ControlInteractButton(false);
    }




}
