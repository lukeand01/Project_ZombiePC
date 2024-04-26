using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : ScriptableObject
{
    ///BASE OF ALL ITEMS

    public string itemName;
    public Sprite itemIcon;
    public TierType tierType;



    public virtual ItemGunData GetGun() {  return null; }

}

public enum TierType
{
    Tier1,
    Tier2,
    Tier3,
    Tier4
}



//ITEMS CAN BE:
//Resources - which has no utility but in the base
//Guns - also an item
//Keys - so i can give them skin and proper names.

//but should abilities be as welll? so i can put them in chests. but i dont need to put them in the same chests.