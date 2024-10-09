using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item / Resource")]
public class ItemResourceData : ItemData
{
    public ItemResourceType resourceType;

    public override ItemResourceData GetResource() => this;
    
}

public enum ItemResourceType
{
    Iron,
    Food,
    Eletrical_Component,
    Population,
    Crystals,
    Body_Enhancer, //this is usedd for improving your body.
    Blueprint
}