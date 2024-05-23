using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StageUnit : ButtonBase
{
    //this fella receives stagedata and once activated will send the player to the mission

    CityStageClass stageClass;
    public StageData stageData {  get; private set; }

    CityCanvas handler;

    [SerializeField] Image portrait;
    [SerializeField] GameObject selected;
    [SerializeField] GameObject hover;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] GameObject locked;

    public void SetUp(CityStageClass stageClass, CityCanvas handler)
    {
        //here we will check about the 

        this.stageClass = stageClass;
        stageData = stageClass.stageData;
        this.handler = handler;

        nameText.text = stageData.stageName;
        portrait.sprite = stageData.stageSprite;

    }

    public void Select()
    {
        selected.SetActive(true);
    }
    public void Unselect()
    {
        selected.SetActive(false);
    }


    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        hover.SetActive(true);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        hover.SetActive(false);
    }
    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        handler.DescribeStage(this, stageClass);
    }

}
