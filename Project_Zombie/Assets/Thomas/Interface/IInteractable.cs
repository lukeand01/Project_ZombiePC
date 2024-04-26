using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable 
{
    public bool IsInteractable();
    public void InteractUI(bool isVisible);
    public void Interact();

    public string GetInteractableID();
}
