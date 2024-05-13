using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CityStore : MonoBehaviour, IInteractable
{
    //the city store is anything in the base
    //what things are we going to have in the base
    //each of them will have a level that will dictate what you can do
    //the buildins are:
    //armory: Perma guns and player roll chance
    //Training: Active abilities and ability roll
    //Headquarter: where you choose a mission


    //i can just hardcode this.
    //i can create a generic class that takes resources.
    //it needs data so it easy to change. the data inform how much it costs and what it costs to up things

    //City data. armory has cost of ugprading.
    //there should be a bar in the top showing that stuff,

    string id;
    [SerializeField] protected CityCanvas _cityCanvas;
    [SerializeField] InteractCanvas _interactCanvas;
     
    private void Awake()
    {
        id = Guid.NewGuid().ToString();
    }

    private void Update()
    {
        if (!_cityCanvas.IsTurnedOn()) return;

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            //if its on then we turn off.

            _cityCanvas.CloseUI();
        }
    }

    protected virtual void CallInteract()
    {
        _cityCanvas.OpenUI();
    }

    #region INTERACT
    public string GetInteractableID()
    {
        return id;
    }

    public void Interact()
    {
        CallInteract(); 
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
