using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BDUnit : ButtonBase
{
    //

    BDClass bd;
    [SerializeField] Image icon;
    [SerializeField] Image fillImage;
    [SerializeField] GameObject stackHolder;
    [SerializeField] TextMeshProUGUI stackText;
    [SerializeField] GameObject tickHolder;
    [SerializeField] TextMeshProUGUI tickText;
    [SerializeField] GameObject selected;

    //

    private void Update()
    {
        if(Time.timeScale > 0)
        {
            selected.SetActive(false);
        }
    }

    public void SetUp(BDClass bd)
    {
        this.bd = bd;

        UpdateStack();
        UpdateTick();

    }
    public void UpdateStack()
    {
        if (bd.IsStackable() && bd.stackCurrent > 1)
        {
            stackHolder.gameObject.SetActive(true);
            stackText.text = bd.stackCurrent.ToString();
        }
        else
        {
            stackHolder.gameObject.SetActive(false);
        }
    }

    public void UpdateTick()
    {
        tickHolder.SetActive(bd.IsTick());
        if (bd.IsTick())
        {
            //
            tickText.text = bd.tickCurrent.ToString();
        }
       
    }

    public void UpdateFill(float current, float total)
    {
        fillImage.fillAmount = current / total;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        //only if its paused.
        if(Time.timeScale == 0)
        {
            selected.SetActive(true);
            UIHandler.instance._pauseUI.DescribeBD(bd, transform);
        }
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
       if(Time.timeScale == 0)
        {
            selected.SetActive(false);
            UIHandler.instance._pauseUI.StopDescription();
        }
    }




    public void OrderOwnDestruction()
    {
        Destroy(gameObject);
    }

}
