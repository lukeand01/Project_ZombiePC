using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class StatUnit_Body : ButtonBase
{
    //this thing will check for price and increase stat.
    StatType statType;
    float value_Level;
    float value_Total;

    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] GameObject button_Increase;
    [SerializeField] TextMeshProUGUI maxLevelWarningText;

    CityData_BodyEnhancer data;
    public void SetUp(StatClass stat, CityData_BodyEnhancer data)
    {
        statType = stat.stat;
        value_Total = stat.value;

        this.data = data;
        value_Level = data.GetLevelForStat(statType);

        UpdateUI();
    }

    void UpdateUI()
    {
        SetText($"{statType}: {value_Total}");

        
        levelText.text = value_Level.ToString();
        bool canIncrease = data.cityStoreLevel * 5 > value_Level;

        button_Increase.SetActive(canIncrease);
        maxLevelWarningText.gameObject.SetActive(!canIncrease);
    }

    //


    public void Call_BuyStat()
    {
        //check for resource.
        //then we allow it to increase by one. always by one.

        Debug.Log("call it");

        bool hasResource = PlayerHandler.instance._playerInventory.HasResourceWithName_City("ItemResourceBodyEnhancer");

        if(hasResource) 
        {
            //increase and decrease
            StopAllCoroutines();
            StartCoroutine(IncreaseProcess());

            GameHandler.instance.cityDataHandler.cityBodyEnhancer.IncreaseStat(statType);
            PlayerHandler.instance._playerInventory.SpendResourceWithName_City("ItemResourceBodyEnhancer");
            value_Level += 1;

           float newValue = data.GetValueForStat(statType);
           value_Total += newValue;

            UpdateUI();
        }
        else
        {
            //flash red
            //
        }

    }

    IEnumerator ShineRedProcess()
    {

        yield return null;
    }

    IEnumerator IncreaseProcess()
    {
        float timer = 0.15f;
        transform.DOKill();
        transform.DOScale(new Vector3(1.5f, 1.4f), timer).SetUpdate(true).SetEase(Ease.Linear);

        yield return new WaitForSecondsRealtime(timer);

        StartCoroutine(DecreaseProcess());
    }

    IEnumerator DecreaseProcess()
    {
        float timer = 0.15f;
        transform.DOKill();
        transform.DOScale(new Vector3(1.4f, 1.3f), timer).SetUpdate(true).SetEase(Ease.Linear);
        yield return new WaitForSecondsRealtime(timer);
    }

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
