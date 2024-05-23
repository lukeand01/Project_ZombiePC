using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResourceUnit : ButtonBase
{
    [Separator("RESOURCE UNIT")]
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI quantityText;
    [SerializeField] GameObject selected;



    public ItemClass item {  get; private set; }
    
    public void SetUp(ItemClass item)
    {
        this.item = item;

        item.SetResourceUnit(this);
        UpdateUI();
       
    }




    private void Update()
    {
        
    }


    public void UpdateUI()
    {
        if(item.data == null)
        {
            return;
        }
        icon.sprite = item.data.itemIcon;
        quantityText.text = item.quantity.ToString();
    }


    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        selected.SetActive(true);



        UIHandler.instance._DescriptionWindow.DescribeResource(item, transform);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        selected.SetActive(false);

        UIHandler.instance._DescriptionWindow.StopDescription();
    }

    private void OnDisable()
    {
        selected.SetActive(false);
        
    }

    

}
