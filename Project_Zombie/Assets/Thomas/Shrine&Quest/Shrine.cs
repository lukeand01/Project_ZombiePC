using DG.Tweening;
using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrine : MonoBehaviour, IInteractable
{
    string id;
    [SerializeField] int price;
    [SerializeField] InteractCanvas _interactCanvas;
    [SerializeField] QuestClass debugQuest;

    List<QuestClass> questList = new();

    public int index {  get; private set; }

    bool cannotBeInteracted;


    private void Awake()
    {
        id = Guid.NewGuid().ToString();
    }

    private void Start()
    {

        _interactCanvas.ControlPriceHolder(price);

        if(LocalHandler.instance != null)
        {
            if (debugQuest.questData != null)
            {
                questList = new List<QuestClass>() { debugQuest, null, null };
                return;
            }


            questList = LocalHandler.instance._stageData.GetQuestList();

            if(questList == null)
            {
                Debug.Log("something wrong with this shrine " + gameObject.name);
                return;
            }

            if(questList.Count < 3)
            {
                Debug.Log("not enough in this quest list " + gameObject.name);
            }

        }
    }

    public void Remove()
    {
        cannotBeInteracted = true;
        StartCoroutine(RemoveProcess());
    }

    IEnumerator RemoveProcess()
    {
        //shake a bit. 
        //then it falls under the ground.
        Vector3 originalPos = transform.position;

        for (int i = 0; i < 20; i++)
        {
            //it will move randomly to x and y
            float value = 0.05f;
            float x = UnityEngine.Random.Range(-value, value);
            float z = UnityEngine.Random.Range(-value, value);
            transform.position = originalPos + new Vector3(x, 0, z);
            yield return new WaitForSecondsRealtime(0.05f);
        }

        transform.position = originalPos;

        yield return new WaitForSecondsRealtime(1);

        transform.DOMove(transform.position + new Vector3(0, -6, 0), 3);

        yield return new WaitForSecondsRealtime(3);

        if(LocalHandler.instance != null)
        {
            LocalHandler.instance.Shrine_Remove(index);
        }
        
        Destroy(gameObject);

    }


    public void RaiseFromGround()
    {
        StartCoroutine(RaiseFromGroundProcess());
    }
    IEnumerator RaiseFromGroundProcess()
    {
        cannotBeInteracted = true;
        transform.DOMove(transform.position + new Vector3(0, 6, 0), 3);
        yield return new WaitForSecondsRealtime(3);
        cannotBeInteracted = false;
    }

    public void SetIndex(int index)
    {
        this.index = index;
    }

    public string GetInteractableID()
    {
        return id;
    }

    //i will do the calculation here to discover from where i should get this things.

    public void Interact()
    {
        if (!PlayerHandler.instance._playerResources.HasEnoughPoints(price))
        {
            return;
        }

        PlayerHandler.instance._playerResources.SpendPoints(price);
        UIHandler.instance._QuestUI.Shrine_OpenUI(questList, this);
    }

    public void InteractUI(bool isVisible)
    {
        _interactCanvas.ControlInteractButton(isVisible);
    }

    public bool IsInteractable()
    {
        if (cannotBeInteracted) return false;
        return PlayerHandler.instance._playerResources.HasRoomForQuest();
    }
}


//once used it goes down to earth.
//when you interact it goes directly to the ui
