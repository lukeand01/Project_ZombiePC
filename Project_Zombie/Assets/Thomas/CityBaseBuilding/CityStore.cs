using DG.Tweening;
using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CityStore : MonoBehaviour, IInteractable
{


    string id;
    [SerializeField] protected CityCanvas _cityCanvas;
    [SerializeField] InteractCanvas _interactCanvas;

    [SerializeField] protected GameObject[] graphicArray;


    //each script decided on what to do.

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
            UpdateGraphic();
            //we reset level and so we must reset this to the original level.
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
                //if we are loading and the citystorelevel is more than 0
                UpdateGraphic();
            }
            else {

                //but if not we still hide it
                UpdateGraphic();

            }
        }

    }


    public virtual void IncreaseStoreLevel()
    {
        //each fella does a different thing.
        //we decide on a graphic also.
        //call it fade to black.

        GetCityData.IncreaseCityStoreLevel();
        UpdateGraphic();
    }


   protected virtual void UpdateGraphic()
    {
        //this get the level of the thing and 
        int lastIndex = -1;
        for (int i = 0; i < graphicArray.Length; i++)
        {    
           
            graphicArray[i].SetActive(GetCityData.cityStoreLevel == i);

            if (graphicArray[i].activeInHierarchy)
            {
                lastIndex = i;
            }
        }

        if(lastIndex != -1)
        {
            graphicArray[lastIndex].SetActive(true);
        }
    }

    protected virtual void CallInteract()
    {
        _cityCanvas.OpenUI();

    }

    protected virtual void UpdateInteractUIName(string name)
    {
        
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

        _interactCanvas.gameObject.SetActive(isVisible);

        if (!isVisible) return;

        string inputText_1 = PlayerHandler.instance._playerController.key.GetKey(KeyType.Interact).ToString();
        _interactCanvas.UpdateInteractButton_NewSystem(0, inputText_1, GetCityData.cityStoreName, GetCityData.cityStoreLevel.ToString());
        _interactCanvas.DisableInteratButton_NewSystem(1);
     

        
    }

    public bool IsInteractable()
    {
        return true;
    }
    #endregion
}

//GOAL
//each citystore has graphics that are updated by level, wont use most but its good to have as a system
//each citystore can have a character.
//when you interact with a building there are two options, to open the building options or to talk with the person.
//when you upgrade a store it should fade out and in to allow the character to appear.
//

//GOAL
//