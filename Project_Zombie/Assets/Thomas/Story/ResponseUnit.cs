using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        responseText.text = _response.responseText;
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
    }


    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        ControlSelected(true);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        ControlSelected(false);
    }

}
