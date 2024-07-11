using DG.Tweening;
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

    [SerializeField] GameObject graphicHolder;

    [Separator("Work Spot")]
    [SerializeField] List<CityWorkSpot> workSpotList = new();

    //what do i should in the citystorecanvas?
    //


    private void Awake()
    {
        _cityCanvas.gameObject.SetActive(true);
        id = Guid.NewGuid().ToString();
        SetUI();
    }

    private void Start()
    {
        //we will check if the thing we have has the right level

       
    }

    private void Update()
    {
        if (!_cityCanvas.IsTurnedOn()) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //if its on then we turn off.
            Debug.Log("input");
            _cityCanvas.CloseUI();
        }
    }

    public virtual CityData GetCityData => null;



    void SetUI()
    {
        _cityCanvas.SetUpgradeHolder();
    }
    public virtual void UpdateBasedInDataLevel(bool shouldReset)
    {
        //

        if (shouldReset)
        {
            GetCityData.ResetCityStoreLevel();
            HideBuilding();
        }
        else
        {
            //then we brin
            if(GetCityData == null)
            {
                Debug.Log("yo " + gameObject.name);
            }

            if(GetCityData.cityStoreLevel > 0)
            {
                ShowBuilding();
            }
            else { HideBuilding();}
        }

    }


    public virtual void IncreaseStoreLevel()
    {
        //each fella does a different thing.
        //we decide on a graphic also.
        ShowBuilding();
        GetCityData.IncreaseCityStoreLevel();

    }

    public void ShowBuilding()
    {
        graphicHolder.transform.DOLocalMoveY(0, 1.5f).SetEase(Ease.Linear).SetUpdate(true);

    }
    public void HideBuilding()
    {
        graphicHolder.transform.DOLocalMoveY(-5, 1).SetEase(Ease.Linear).SetUpdate(true);
    }


   

    protected virtual void CallInteract()
    {
        _cityCanvas.OpenUI();

    }

    protected virtual void UpdateInteractUIName(string name)
    {
        _interactCanvas.ControlNameHolder(name);
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
       
            //we are going to show the build canvas.
            _interactCanvas.ControlInteractButton(isVisible);
            UpdateInteractUIName("yo");
        

        
    }

    public bool IsInteractable()
    {
        return true;
    }
    #endregion
}

//if the thing is level 0 then it will auto set as not yet built.
//then we must have different graphics for each level.
//