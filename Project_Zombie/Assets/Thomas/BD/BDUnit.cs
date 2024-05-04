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
    [SerializeField] TextMeshProUGUI stackText;

    public void SetUp(BDClass bd)
    {
        this.bd = bd;

        UpdateStack();
               

    }
    public void UpdateStack()
    {
        if (bd.IsStackable())
        {
            stackText.gameObject.SetActive(true);
            stackText.text = bd.stackCurrent.ToString();
        }
        else
        {
            stackText.gameObject.SetActive(false);
        }
    }

    public void UpdateFill(float current, float total)
    {
        fillImage.fillAmount = current / total;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
    }

    public void OrderOwnDestruction()
    {
        Destroy(gameObject);
    }

}
