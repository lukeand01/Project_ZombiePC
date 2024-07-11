using DG.Tweening;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class DialogueUI : MonoBehaviour
{

    Story_DialogueData dialogueData;
    GameObject holder;


    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] Image portrait;
    [SerializeField] List<ResponseUnit> responseUnitList;
    [SerializeField] GameObject responseBlackScreen;
    [SerializeField] GameObject flashingObject;

    [Separator("HOLDERS")]
    [SerializeField] Transform portrait_Holder;
    [SerializeField] Transform dialogue_Holder;
    [SerializeField] GameObject response_Holder;


    [Separator("SPEED")]
    [SerializeField] float speed_Normal;
    [SerializeField] float speed_Fast;
    float speed_Current;

    Vector3 originalPos_Portrait;

    Vector3 originalPos_Dialogue;


    private void Awake()
    {
        holder = transform.GetChild(0).gameObject;

        originalPos_Portrait = portrait_Holder.transform.position;
        originalPos_Dialogue = dialogue_Holder.transform.position;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            //we will hurry up

            speed_Current = speed_Fast;
        }
        else
        {

            speed_Current = speed_Normal;
        }
    }

    public void StartDialogue(Story_NpcData npc)
    {
        HideResponseUnit();

        PlayerHandler.instance._playerController.block.AddBlock("Dialogue", BlockClass.BlockType.Complete);

        dialogueData = npc.GetDialogueData();

        holder.SetActive(true);

        dialogueText.text = "";
        nameText.text = npc.npcName;
        portrait.sprite = npc.npcSprite;

        StartCoroutine(ShowDialogueProcess());
    }

    public void CloseDialogue()
    {
        StopAllCoroutines();
        StartCoroutine(HideDialogueProcess());

        PlayerHandler.instance._playerController.block.RemoveBlock("Dialogue");

        holder.SetActive(false);
    }

    IEnumerator ShowDialogueProcess()
    {
        //we bring the fellas from boths sides. first we need to set them to the sides.

        portrait_Holder.transform.position = originalPos_Portrait + new Vector3(Screen.width * -0.3f, 0, 0); //it goes to the left.
        dialogue_Holder.transform.position = originalPos_Dialogue + new Vector3(Screen.width * 0.3f, 0, 0);

        float timer = 0.5f;

        portrait_Holder.transform.DOKill();
        portrait_Holder.transform.DOMove(originalPos_Portrait, timer).SetUpdate(true).SetEase(Ease.Linear);

        dialogue_Holder.transform.DOKill();
        dialogue_Holder.transform.DOMove(originalPos_Dialogue, timer).SetUpdate(true).SetEase(Ease.Linear);

        yield return new WaitForSecondsRealtime(timer);


        StartCoroutine(StartDialogueProcess(dialogueData.dialogueList[0]));

    }

    IEnumerator HideDialogueProcess()
    {
        float timer = 0.5f;
        portrait_Holder.transform.DOKill();
        portrait_Holder.transform.DOMove(originalPos_Portrait + new Vector3(-Screen.width, 0, 0), timer).SetUpdate(true).SetEase(Ease.Linear);

        dialogue_Holder.transform.DOKill();
        dialogue_Holder.transform.DOMove(originalPos_Dialogue + new Vector3(Screen.width, 0, 0), timer).SetUpdate(true).SetEase(Ease.Linear);

        yield return new WaitForSecondsRealtime(timer);
    }

    IEnumerator StartDialogueProcess(DialogueClass _dialogueClass)
    {
        //in here we will keep writting



        List<string> dialogueList = _dialogueClass.dialogueStringList;

        dialogueText.text = "";
        DisableResponseHolder();

        for (int i = 0; i < dialogueList.Count; i++)
        {
            flashingObject.SetActive(false);
            var item = dialogueList[i];

            dialogueText.text = "";

            foreach (char letter in item.ToCharArray())
            {
                //we are going to put each fella here.
                dialogueText.text += letter;

                yield return new WaitForSecondsRealtime(speed_Current);
            }

            if(i + 1 < dialogueList.Count)
            {

                flashingObject.SetActive(true);
                yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
                flashingObject.SetActive(false);
            }
                  
        }


        flashingObject.SetActive(false);

        //if we got here we need to check if there is a response

        if(_dialogueClass.responseList.Count > 0)
        {
            //we bring forth the response holder and we spawn teh fekllas
            UpdateResponseUnit(_dialogueClass.responseList);

        }
        else
        {
            //otherwise we wait for input as well.
            flashingObject.SetActive(true);
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            flashingObject.SetActive(false);
            CloseDialogue();
        }

    }

    void HideResponseUnit()
    {
        foreach (var item in responseUnitList)
        {
            item.gameObject.SetActive(false);
        }
    }
    void DisableResponseHolder()
    {
        UIHandler.instance._MouseUI.ControlVisibility(false);
        responseBlackScreen.SetActive(true);
        response_Holder.transform.DOScale(new Vector3(8.5f, 3.5f, 0), 0.15f).SetUpdate(true).SetEase(Ease.Linear);
    }

    void EnableResponseHolder()
    {
        UIHandler.instance._MouseUI.ControlVisibility(true);
        responseBlackScreen.SetActive(false);
        response_Holder.transform.DOScale(new Vector3(9, 4, 0), 0.15f).SetUpdate(true).SetEase(Ease.Linear);
    }
    void UpdateResponseUnit(List<ResponseClass> responseList)
    {
        //we updat the list and those we dont update we 

        EnableResponseHolder();

        for (int i = 0; i < responseUnitList.Count; i++)
        {
            if (i >= responseList.Count)
            {
                responseUnitList[i].gameObject.SetActive(false);
            }
            else
            {
                responseUnitList[i].gameObject.SetActive(true);
                responseUnitList[i].SetUp(responseList[i], this);
            }


        }

    }

    public void ChooseResponse(ResponseClass _response)
    {
        //we will look for the right id here and

        //we check 
        CheckResponseTrigger(_response.response_trigger, _response.responseTrigger_Index);

        if(_response.keyForResponse == "")
        {
            //if this is the case then the response closes the dialogue.
            CloseDialogue();
            return;
        }

        Debug.Log("chose this id " + _response.keyForResponse);
        foreach (var item in dialogueData.dialogueList)
        {
            if(item.dialogueKeyForResponse == _response.keyForResponse)
            {
                //then we will load this one next.
                StopAllCoroutines();
                StartCoroutine(StartDialogueProcess(item));
                return;
            }
        }

        Debug.LogError("error. could not find a new dialogue");
        CloseDialogue();
        //

    }


    public void CheckResponseTrigger(ReponseTriggerType triggerType, int index)
    {
        switch(triggerType)
        {
            case ReponseTriggerType.Quest:
                //we inform teh player resource to add this fella
                GameHandler.instance.cityDataHandler.cityMain.AddQuestWithIndex(index);
                return;
            case ReponseTriggerType.Gun:
                GameHandler.instance.cityDataHandler.cityArmory.AddGunWithIndex(index);
                return;

        }
    }
}


//i need to be abkle to grant the player a quest here.
//i also need to inform the player when he receives a new quest