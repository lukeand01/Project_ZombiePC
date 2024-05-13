using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipWindowOptionButton : ButtonBase
{
    //THIS IS THE BUTTON TO OPEN DIFFERENT OPTIONS

    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] GameObject selected;

    EquipWindowType equipType;
    EquipWindowUI handler;

    public void SetUp(EquipWindowType equipType, EquipWindowUI handler)
    {
        this.equipType = equipType;
        this.handler = handler;

        nameText.text = equipType.ToString();
    }


    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        handler.OpenOption(equipType);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        selected.SetActive(true);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        selected.SetActive(false);  
    }
    private void OnDisable()
    {
        selected.SetActive(false);
    }
}
