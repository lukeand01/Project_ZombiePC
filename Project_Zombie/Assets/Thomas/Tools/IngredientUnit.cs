using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IngredientUnit : ButtonBase
{
    public IngredientData _data {  get; private set; }


    [SerializeField] TextMeshProUGUI _quantityText;
    [SerializeField] Image _icon;
    [SerializeField] TextMeshProUGUI _nameText;

    public void Set(IngredientData data)
    {
        _nameText.text = data._name;
        _icon.sprite = data._icon;
    }
    public void UpdateValue(int value)
    {
        _quantityText.text = value.ToString();
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
    }


    //call description

}
