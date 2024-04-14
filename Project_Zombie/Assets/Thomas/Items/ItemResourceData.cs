using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item / Resource")]
public class ItemResourceData : ItemData
{
    public ItemResourceType resourceType;


}

public enum ItemResourceType
{
    Iron,
    Food,
    Copper,
    Eletrical_Components,
    Uranium,
    Rare_Cristals,
    Zyo,
    Anti_Matter
}