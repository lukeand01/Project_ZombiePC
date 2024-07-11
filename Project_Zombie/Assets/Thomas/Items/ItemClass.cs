using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemClass 
{
    public ItemData data;
    public int quantity;

    public int popUsage; //we only use this if we are using pop

    public ItemClass(ItemData data, int quantity)
    {
        this.data = data;
        this.quantity = quantity;

        popUsage = -1;
    }

    public void AddQuantity(int value)
    {
        quantity += value;
        UpdateUI();
    }
    public void RemoveQuantity(int value)
    {
        quantity -= value;

        if(quantity < 0)
        {
            Debug.Log("wrong");
        }
        UpdateUI();
    }

    public void SetPopCap(int popCap)
    {
        quantity = popCap;
       
        UpdateUI();
    }
    public void SetPopUsage(int popUsage)
    {
        this.popUsage = popUsage;
        UpdateUI();
    }

    ResourceUnit _resourceUnit;

    public void SetResourceUnit(ResourceUnit _resourceUnit)
    {
        this._resourceUnit = _resourceUnit;
    }

    public void UpdateUI()
    {
        if(_resourceUnit != null)
        {
            if(popUsage != -1)
            {
                _resourceUnit.UpdateUI_Pop();
            }
            else
            {
                _resourceUnit.UpdateUI();
            }


        }
        else
        {
            Debug.Log("no reosurce unit");
        }
    }
}
