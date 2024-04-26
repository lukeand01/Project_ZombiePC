using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StatDescriptionUnit : ButtonBase
{
    [SerializeField] StatType stat;
    [SerializeField] TextMeshProUGUI statDescriptionText;

    List<ModifierClass> modifierList = new();

    public void UpdateStat(string value, List<ModifierClass> modifierList)
    {
        //i just show the base here.
        //but i want to calculate with other variables 
        statDescriptionText.text = value;
        this.modifierList = modifierList;
    }


    //if you hover over it it shows stats more precisely.

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
    }

    private void OnDisable()
    {
        
    }
}
