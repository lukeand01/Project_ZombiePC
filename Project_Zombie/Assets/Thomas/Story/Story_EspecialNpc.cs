using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Story_EspecialNpc : MonoBehaviour, IInteractable
{
    [SerializeField] Story_NpcData npcData;
    

    public void SetUp()
    {
        //i need to set up. what should i set up here?


    }

    public Story_EspecialNpc GetNpcModel()
    {
        return npcData.npcModel;
    }

    //how to determine the stage?
    //we can use an astring value_Level for adding new stuff. and once we use it we put in a list that we can no longer use it.

    private void Awake()
    {
        id = Guid.NewGuid().ToString(); 
    }

    //for now it will do interactions

    #region INTERACT
    [SerializeField] InteractCanvas _interactCanvas;
    string id;

    public string GetInteractableID()
    {
        return id;
    }

    public void Interact()
    {
        //we call dialogue and pass the current dialogue for this npc
        UIHandler.instance._DialogueUI.StartDialogue(npcData);
    }

    public void InteractUI(bool isVisible)
    {
        _interactCanvas.ControlInteractButton(isVisible);
    }

    public bool IsInteractable()
    {
        return true;
    }

    #endregion
}
