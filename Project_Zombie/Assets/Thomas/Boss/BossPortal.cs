using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPortal : MonoBehaviour, IInteractable
{
    string _id;
    [SerializeField] InteractCanvas _interactCanvas;

    //when we interact we create a list.
    //each sigil has its graphic that i will give it here.





    private void Awake()
    {
        _id = MyUtils.GetRandomID();
    }

    public string GetInteractableID()
    {
        return _id;
    }

    public void Interact()
    {
        

    }

    public void InteractUI(bool isVisible)
    {
        


    }

    public bool IsInteractable()
    {
        return true;
    }

    //cannot interact if there 
}


//i need 3 to call it
//if there are three similar you get something else in the end. but you cant control the chances through it
//

//to use show