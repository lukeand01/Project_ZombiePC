using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DashUnit : ButtonBase
{
    [SerializeField] Image fillImage;
    [SerializeField] TextMeshProUGUI cooldownText;
    [SerializeField] GameObject selected;
    [SerializeField] GameObject cannotUse;

    public void UpdateCooldown(float current, float total)
    {
        fillImage.fillAmount = current / total;


        cooldownText.gameObject.SetActive(current > 0);
        cooldownText.text = current.ToString("f1");


    }
    
    public void ControlCannotUse(bool choice)
    {
        cannotUse.SetActive(choice);
    }

    private void Update()
    {
        if(Time.timeScale > 0)
        {
            selected.SetActive(false);
        }


    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);

        if(Time.timeScale == 0)
        {
            selected.SetActive(true);
            UIHandler.instance._pauseUI.DescribeDash(transform);
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

}
