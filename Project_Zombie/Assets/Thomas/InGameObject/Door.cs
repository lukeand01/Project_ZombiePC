using DG.Tweening;
using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{

    [SerializeField] InteractCanvas _interactCanvas;
    [SerializeField] int doorPriceBase; //should multiply by some _value.
    int currentPrice;
    [SerializeField] Room[] roomToOpenArray;

    [Separator("CONDITIONS TO OPEN DOOR")]
    [SerializeField] Door[] doorRequiredArray;

    [Separator("GRAPHICAL")]
    [SerializeField] GameObject graphicHolder;
    [SerializeField] GameObject wallHolder;
    [SerializeField] GameObject gateHolder;
    [SerializeField] GameObject doorLeft;
    [SerializeField] GameObject doorRight;

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


        if (isChallenge) return;

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
            GameHandler.instance._soundHandler.CreateSfx(SoundType.AudioClip_GateOpen, transform);
            OpenDoor();
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
        _interactCanvas.gameObject.SetActive(isVisible);
        if (isChallenge)
        {
            //we inform that you cannot open till challenge is done.
            _interactCanvas.ControlNameHolder("Complete the Challenge");
            return;
        }

        

        float modifier = PlayerHandler.instance._entityStat.GetTotalEspecialConditionValue(EspecialConditionType.GatePriceModifier);
        float reduction = doorPriceBase * modifier;
        currentPrice = (int)(doorPriceBase - reduction);
        currentPrice = Mathf.Clamp(currentPrice, 0, 9999);

        _interactCanvas.ControlInteractButton(isVisible);
        _interactCanvas.ControlPriceHolder(currentPrice);

    }

    public bool IsInteractable()
    {


        return !IsOpen && !isChallenge;
    }


    //we cannot destroy this door.

    public bool IsOpen { get; private set; }
    bool isChallenge;

    public void OpenDoor()
    {
        gateHolder.SetActive(false);
        wallHolder.SetActive(false);

        IsOpen = true;

        float timer = 1;

        doorLeft.transform.DOKill();
        doorLeft.transform.DOLocalRotate(new Vector3(0, -340, 0), timer).SetEase(Ease.Linear);

        doorRight.transform.DOKill();
        doorRight.transform.DOLocalRotate(new Vector3(0, 160, 0), timer).SetEase(Ease.Linear);

    }

   
    public void CloseDoor()
    {
        gateHolder.SetActive(true);
        wallHolder.SetActive(true);

        IsOpen = false;

        float timer = 1;

        doorLeft.transform.DOKill();
        doorLeft.transform.DOLocalRotate(new Vector3(0, -180, 0), timer).SetEase(Ease.Linear);

        doorRight.transform.DOKill();
        doorRight.transform.DOLocalRotate(new Vector3(0, 0, 0), timer).SetEase(Ease.Linear);

    }

  
    public void OpenDoor_Challenge()
    {
        if (IsOpen)
        {
            OpenDoor();
            
        }

        isChallenge = false;

    }
    public void CloseDoor_Challenge()
    {
        gateHolder.SetActive(true);
        wallHolder.SetActive(true);

        float timer = 1;

        doorLeft.transform.DOKill();
        doorLeft.transform.DOLocalRotate(new Vector3(0, -180, 0), timer).SetEase(Ease.Linear);

        doorRight.transform.DOKill();
        doorRight.transform.DOLocalRotate(new Vector3(0, 0, 0), timer).SetEase(Ease.Linear);

        isChallenge = true;
    }
    


}
