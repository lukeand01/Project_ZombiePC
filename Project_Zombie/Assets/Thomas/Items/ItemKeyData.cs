using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item / Key")]
public class ItemKeyData : ItemData
{
    public ItemKeyType KeyType;



}

public enum ItemKeyType
{
    Basic,
    Advanced,
    Mystical
}