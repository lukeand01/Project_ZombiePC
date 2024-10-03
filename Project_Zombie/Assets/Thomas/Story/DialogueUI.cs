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
    [SerializeField] List<ResponseUnit> responseUnitList;
    [SerializeField] GameObject flashingObject;

    [Separator("HOLDERS")]
    [SerializeField] Transform dialogue_Holder;
    [SerializeField] GameObject response_Holder;


    [Separator("SPEED")]
    [SerializeField] float speed_Normal;
    [SerializeField] float speed_Fast;
    float speed_Current;

    Vector3 originalPos_Dialogue;
    Vector3 originalPos_Response;


    private void Awake()
    {
        

        holder = transform.GetChild(0).gameObject;

        originalPos_Dialogue = dialogue_Holder.transform.localPosition;
        originalPos_Response = response_Holder.transform.localPosition;
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
    Story_EspecialNpc _npcObject;

    public void StartDialogue(Story_NpcData npc, Story_EspecialNpc npcObject = null)
    {
        DisableResponseHolder();

        _npcObject = npcObject;

        PlayerHandler.instance._playerController.block.AddBlock("Dialogue", BlockClass.BlockType.Complete);

        dialogueData = npc.GetDialogueData();

        holder.SetActive(true);

        dialogueText.text = "";
        nameText.text = npc.npcName;

        UIHandler.instance._MouseUI.ControlMouseUI(true);

        PlayerHandler.instance._playerCamera.SetCamera(CameraPositionType.Dialogue, 1, 1);

        GameHandler.instance._soundHandler.CreateSfx(SoundType.AudioClip_PlayerStarDialogue);


        StartCoroutine(ShowDialogueProcess());
    }

    public void CloseDialogue()
    {
        StopAllCoroutines();
        StartCoroutine(HideDialogueProcess());

        PlayerHandler.instance._playerController.block.RemoveBlock("Dialogue");
        PlayerHandler.instance._playerCamera.SetCamera(CameraPositionType.Default, 1, 1);

        UIHandler.instance._MouseUI.ControlMouseUI(false);

        holder.SetActive(false);

        if(_npcObject != null)
        {
            _npcObject.EndDialogue();
        }
    }

    IEnumerator ShowDialogueProcess()
    {
        //we bring the fellas from boths sides. first we need to set them to the sides.

        dialogue_Holder.transform.localPosition = originalPos_Dialogue + new Vector3(0, Screen.height * -0.5f, 0);
        response_Holder.transform.localPosition = originalPos_Response + new Vector3(Screen.width * 0.5f, 0, 0);


        float timer = 0.5f;


        dialogue_Holder.transform.DOKill();
        dialogue_Holder.transform.DOLocalMove(originalPos_Dialogue, timer).SetUpdate(true).SetEase(Ease.Linear);


        yield return new WaitForSecondsRealtime(timer);


        StartCoroutine(StartDialogueProcess(dialogueData.dialogueList[0]));

    }

    IEnumerator HideDialogueProcess()
    {
        float timer = 0.5f;

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

            dialogueText.text = "  ";

            //now we need to indetify it


            string colorValue_Red = "<color=red>"; //THIS IS THE RED COLOR. I WILL USE THIS FOR EVERYTHING FOR NOW
            string colorValue_Green = "<color=green>";
            string colorValue_Blue = "<color=#44bcd8>";
            string colorValue_Yellow = "<color=yellow>";

            string colorFinalIncrement = "</color>";

            string sizeValue = "<size=130%>"; //
            string sizeFinalIncrement = "</size>";

            string iterationsFirst = "";
            string iterationsLast = "";


            bool mustCheckForNext = false;
            bool waitingToClose = false;
            for (int y = 0; y < item.ToCharArray().Length; y++)
            {
               
                var letter = item[y];

                if (letter == '*')
                {

                    if (waitingToClose)
                    {
                        waitingToClose = false;
                        mustCheckForNext = false;

                        iterationsFirst = "";
                        iterationsLast = "";

                    }
                    else
                    {
                        mustCheckForNext = true;
                        waitingToClose = true;
                    }
                    
                    continue;
                }

                if(mustCheckForNext )
                {
                    if (letter == '0')
                    {
                        iterationsFirst = sizeValue + colorValue_Red;
                        iterationsLast = colorFinalIncrement + sizeFinalIncrement;

                    }
                    if ( letter == '1')
                    {
                        iterationsFirst = sizeValue + colorValue_Green;
                        iterationsLast = colorFinalIncrement + sizeFinalIncrement;
                        
                    }
                    if (letter == '2')
                    {
                        iterationsFirst = sizeValue + colorValue_Blue;
                        iterationsLast = colorFinalIncrement + sizeFinalIncrement;

                    }
                    if (letter == '3')
                    {
                        iterationsFirst = sizeValue + colorValue_Yellow;
                        iterationsLast = colorFinalIncrement + sizeFinalIncrement;

                    }

                    mustCheckForNext = false;
                    continue;
                }
                


                bool isSpeeding = speed_Current == speed_Fast;

                if (isSpeeding)
                {

                }
                else
                {
                    GameHandler.instance._soundHandler.CreateSFX_DialogueLetter();
                }

                dialogueText.text += iterationsFirst + letter + iterationsLast;

                yield return new WaitForSecondsRealtime(speed_Current);

            }

            //or maybe i can read it afterwards
            

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

    void DisableResponseHolder()
    {
        response_Holder.transform.DOLocalMove(originalPos_Response + new Vector3(Screen.width * 0.5f, 0, 0), 0.2f).SetEase(Ease.Linear);
    }

    void EnableResponseHolder()
    {
        response_Holder.transform.DOLocalMove(originalPos_Response, 0.2f).SetEase(Ease.Linear);
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
        //we will look for the right _id here and

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
            case ReponseTriggerType.TriggerUniqueToThisNPC:
                //need to have a reference to the npc model.
                _npcObject.CallFunctionUnique(index);
                return;

        }
    }
}



//

//i need to be abkle to grant the player a quest here.
//i also need to inform the player when he receives a new quest