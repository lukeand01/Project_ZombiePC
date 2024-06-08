using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StatDescriptionUnit : ButtonBase
{
    [field:SerializeField] public StatType stat {  get; private set; }
    float value;
    [SerializeField] TextMeshProUGUI statDescriptionText;
    [SerializeField] GameObject selected;

    List<ModifierClass> modifierList = new();

    Image mainImage;


    public void SetUp(StatType stat, float value)
    {
        this.stat = stat;
        this.value = value;
        statDescriptionText.text = stat.ToString() + ": " + value.ToString();
    }
    public void RemoveRaycast()
    {
        mainImage = GetComponent<Image>();
        mainImage.raycastTarget = false;
    }

    public void UpdateWithAlteredValue(float value, float alteredValue)
    {
        statDescriptionText.text = $"{stat} : {value} ({alteredValue})";
    }

    public void UpdateStat(string value, List<ModifierClass> modifierList, float totalValue)
    {
        //i just show the base here.
        //but i want to calculate with other variables 
        this.value = totalValue;
        statDescriptionText.text = value.ToString();
        this.modifierList = modifierList;
    }

    private void Update()
    {
        if(Time.timeScale > 0)
        {
            selected.SetActive(false);
        }
    }

    //if you hover over it it shows stats more precisely.

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);

        if(Time.timeScale == 0)
        {
            selected.SetActive(true);
            UIHandler.instance._pauseUI.DescribeStat(new StatClass(stat, value), transform);
        }

    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);

        if (Time.timeScale == 0)
        {
            selected.SetActive(false);
            UIHandler.instance._pauseUI.StopDescription();
        }
    }

    private void OnDisable()
    {
        selected.SetActive(false);
    }
}
