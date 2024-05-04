using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemClass 
{
    public ItemData data;
    public int quantity;
    
    public ItemClass(ItemData data, int quantity)
    {
        this.data = data;
        this.quantity = quantity;
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

    ResourceUnit _resourceUnit;

    public void SetResourceUnit(ResourceUnit _resourceUnit)
    {
        this._resourceUnit = _resourceUnit;
    }

    public void UpdateUI()
    {
        if(_resourceUnit != null)
        {
            _resourceUnit.UpdateUI();
        }
    }
}
