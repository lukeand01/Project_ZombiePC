using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : ScriptableObject
{
    ///BASE OF ALL ITEMS

    public string itemName;
    [TextArea] public string itemDescription;
    public Sprite itemIcon;
    public TierType tierType;



    public virtual ItemGunData GetGun() {  return null; }

    public virtual ItemResourceData GetResource() { return null; }
}

public enum TierType
{
    Tier1 = 0,
    Tier2 = 1,
    Tier3 = 2,
    Tier4 = 3
}



//ITEMS CAN BE:
//Resources - which has no utility but in the base
//Guns - also an item
//Keys - so i can give them skin and proper names.

//but should abilities be as welll? so i can put them in chests. but i dont need to put them in the same chests.