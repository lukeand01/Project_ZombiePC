using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IngredientUnit : ButtonBase
{
    [SerializeField] IngredientData debugData;
    public IngredientData _data {  get; private set; }


    [SerializeField] TextMeshProUGUI _quantityText;
    [SerializeField] Image _icon;
    [SerializeField] TextMeshProUGUI _nameText;

    private void Awake()
    {
        if (debugData != null)
        {
            Set(debugData);
        }
        
    }


    public void Set(IngredientData data)
    {
        _data = data;
        _nameText.text = data._name;
        _icon.sprite = data._icon;
    }
    public void UpdateValue(int value)
    {
        _quantityText.text = value.ToString();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {

        base.OnPointerClick(eventData);
        
        UIHandler.instance._DescriptionWindow.Describe_IngredientData(_data, _quantityText.text, transform);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        UIHandler.instance._DescriptionWindow.StopDescription(); 
    }


    //call description

}
