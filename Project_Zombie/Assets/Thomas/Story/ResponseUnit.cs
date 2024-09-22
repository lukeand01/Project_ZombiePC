using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;

public class ResponseUnit : ButtonBase
{

    [Separator("Response")]
    [SerializeField] TextMeshProUGUI responseText;

    ResponseClass _response;

    DialogueUI _dialogueUIHandler;

    public void SetUp(ResponseClass _response, DialogueUI _dialogueUIHandler)
    {
        this._response = _response;
        this._dialogueUIHandler = _dialogueUIHandler;
        CheckForTriggers(_response.responseText);
    }

    void CheckForTriggers(string newStringValue)
    {

        string colorValue_Red = "<color=red>"; //THIS IS THE RED COLOR. I WILL USE THIS FOR EVERYTHING FOR NOW
        string colorValue_Green = "<color=green>";
        string colorValue_Blue = "<color=blue>";
        string colorValue_Yellow = "<color=yellow>";

        string colorFinalIncrement = "</color>";

        string sizeValue = "<size=130%>"; //
        string sizeFinalIncrement = "</size>";

        string iterationsFirst = "";
        string iterationsLast = "";


        bool mustCheckForNext = false;
        bool waitingToClose = false;

        responseText.text = "";

        for (int i = 0; i < newStringValue.ToCharArray().Length; i++)
        {
            var letter = newStringValue.ToCharArray()[i];

            if (letter == '*')
            {
                Debug.Log("this phrase was a trigger");

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

            if (mustCheckForNext)
            {
                if (letter == '0')
                {
                    iterationsFirst = sizeValue + colorValue_Red;
                    iterationsLast = colorFinalIncrement + sizeFinalIncrement;

                }
                if (letter == '1')
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

            responseText.text += iterationsFirst + letter + iterationsLast;

        }
    }

    private void Start()
    {
        ControlSelected(false);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        //we just inform the dialogue UI
        _dialogueUIHandler.ChooseResponse(_response);
        GameHandler.instance._soundHandler.CreateSfx(SoundType.AudioClip_ResponseChoice);
    }


    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        ControlSelected(true);
        GameHandler.instance._soundHandler.CreateSfx(SoundType.AudioClip_ButtonHover);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        ControlSelected(false);
    }


    private void OnDisable()
    {
        ControlSelected(false);
    }
}
