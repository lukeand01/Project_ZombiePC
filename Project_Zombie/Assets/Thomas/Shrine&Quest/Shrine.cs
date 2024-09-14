using DG.Tweening;
using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shrine : MonoBehaviour, IInteractable
{
    string id;
    [SerializeField] int price;
    [SerializeField] GameObject graphicHolder;
    [SerializeField] InteractCanvas _interactCanvas;
    [SerializeField] QuestClass debugQuest;
    [SerializeField] GameObject _candleGroup_Lit;
    [SerializeField] GameObject _candleGroup_Burned;
    [SerializeField] ParticleSystem _ps_whiteSmoke;
    [SerializeField] ParticleSystem _ps_blackSmoke;

    List<QuestClass> questList = new();

    Room roomItBelongsTo;

    Vector3 originalPos;

    public int index {  get; private set; }

    bool cannotBeInteracted;

    //it will go down. it will decide on spawn whyere this should go.
    //these things have a duration of two turns. then they return and 



    private void Awake()
    {
        id = Guid.NewGuid().ToString();
        originalPos = graphicHolder.transform.localPosition;
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


            questList = LocalHandler.instance._stageData.GetQuestListUsingNothing();

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

    public void SetUp(QuestType _questType, Room roomItBelongsTo)
    {
        this.roomItBelongsTo = roomItBelongsTo;

        //this will decide if its curse or not
        QuestClass quest = LocalHandler.instance._stageData.GetSingleQuestListUsingQuestType(_questType);
        questList = new List<QuestClass> { quest };
        
        _candleGroup_Lit .gameObject.SetActive(true);
        _candleGroup_Burned .gameObject.SetActive(false);

        if(_questType == QuestType.Bless)
        {
            _ps_whiteSmoke.gameObject.SetActive(true);
            _ps_blackSmoke.gameObject.SetActive(false);

            _ps_whiteSmoke.Play();
        }
        if(_questType == QuestType.Curse)
        {
            _ps_whiteSmoke.gameObject.SetActive(false);
            _ps_blackSmoke.gameObject.SetActive(true);

            _ps_blackSmoke.Play();
        }

        StartCoroutine(ShakeProcess());
        StartCoroutine(RaiseFromGroundProcess());

    }


    public void Remove()
    {
        cannotBeInteracted = true;
        StartCoroutine(ShakeProcess());
        StartCoroutine(RemoveProcess());
    }

    IEnumerator RemoveProcess()
    {

        yield return new WaitForSecondsRealtime(1);

        transform.DOMove(transform.position + new Vector3(0, -6, 0), 3);

        yield return new WaitForSecondsRealtime(3);

        if(LocalHandler.instance != null)
        {
            LocalHandler.instance.RemoveShrineFromList(index);
        }
        
        if(roomItBelongsTo != null)
        {
            roomItBelongsTo.RemoveShrine();
        }
        else
        {
            Debug.Log("this has no room " + gameObject.name);
        }


    }


    IEnumerator RaiseFromGroundProcess()
    {
        cannotBeInteracted = true;
        transform.DOMove(transform.position + new Vector3(0, 10, 0), 5);
        yield return new WaitForSecondsRealtime(3);
        cannotBeInteracted = false;
    }

    IEnumerator ShakeProcess()
    {
        float value = 0.2f;
        for (int i = 0; i < 15; i++)
        {

            float x = UnityEngine.Random.Range(-value, value);
            float z = UnityEngine.Random.Range(-value, value);
            graphicHolder.transform.localPosition = originalPos + new Vector3(x, 0, z);

            yield return new WaitForSeconds(0.2f);
        }

        graphicHolder.transform.localPosition = originalPos;

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

        bool isSuccess = PlayerHandler.instance._playerResources.AddQuest(questList[0]); 

        if(isSuccess)
        {
            PlayerHandler.instance._playerResources.SpendPoints(price);
            Remove();
            
            
        }
        else
        {
            //here we inform the player he already has enough. but i wont say here. i will say in the other place to be able to change stuff.
        }

        //we are not going to open

       
        //we get the quest directly.
       

        //UIHandler.instance._QuestUI.Shrine_OpenUI(questList, this);
    }

    public void InteractUI(bool isVisible)
    {
        _interactCanvas.gameObject.SetActive(isVisible);
        _interactCanvas.ControlInteractButton(isVisible);
        _interactCanvas.ControlPriceHolder(price);
    }

    public bool IsInteractable()
    {
        if (cannotBeInteracted) return false;
        return PlayerHandler.instance._playerResources.HasRoomForQuest();
    }
}


//once used it goes down to earth.
//when you interact it goes directly to the ui
