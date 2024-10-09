using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveClass 
{

    public SaveClass()    
    {
        //if i have to create a new one then we will do the following

        _hasSaveData = true;

        _cityLevelList = new() { 0, 0, 0, 0, 0 };

        _equip_GunIndex = -1;

        _abilitySlot = 1;
    }


    [field:SerializeField]public bool _hasSaveData { get; private set; }
    public void MakeHasSaveData(bool hasSaveData)
    {
        _hasSaveData = hasSaveData;
    }

    #region DATA FOR CITY
    public List<int> _cityLevelList { get; private set; } = new();
    public void MakeCityLevelList(List<int> citylevelList)
    {
        _cityLevelList = citylevelList;
    }

    public List<int> _armory_OwnedList { get; private set; } = new();
    public void MakeOwnedList_Armory(List<int> armory_OwnedList)
    {
        _armory_OwnedList = armory_OwnedList;
    }

    public List<int> _armory_FoundList { get; private set; } = new();
    public void MakeFoundList_Armory(List<int> armory_FoundList)
    {
        _armory_FoundList = armory_FoundList;
    }



    public List<int> _lab_OwnedList_Active { get; private set; } = new();
    public void MakeOwnedList_Active_Lab(List<int> lab_OwnedList)
    {
        _lab_OwnedList_Active = lab_OwnedList;
    }

    public List<int> _lab_OwnedList_Passive { get; private set; } = new();
    public void MakeOwnedList_Passive_Lab(List<int> lab_OwnedList)
    {
        _lab_OwnedList_Passive = lab_OwnedList;
    }

    public List<int> _lab_FoundList { get; private set; } = new();
    public void MakeFoundList_Lab(List<int> lab_FoundList)
    {
        _lab_FoundList = lab_FoundList;
    }
    #endregion

    #region DATA FOR PLAYER

    //we will use the ref list 
    public List<int> _playerInventoryList { get; private set; } = new();

    //i need a re
    public void MakePlayerInventory(List<int> playerInventoryList)
    {
        _playerInventoryList = playerInventoryList;
    }


    public int _equip_GunIndex { get; private set; }

    //i need a re
    public void MakeEquipGunIndex(int equip_GunIndex)
    {
        _equip_GunIndex = equip_GunIndex;
    }

    public List<int> _equip_AbilityList { get; private set; } = new();

    //i need a re
    public void MakeEquipAbilityList(List<int> equip_AbilityList)
    {
        _equip_AbilityList = equip_AbilityList;
    }

    public List<int> _equip_DropList { get; private set; } = new();

    //i need a re
    public void MakeEquipDropList(List<int> equip_DropList)
    {
        _equip_DropList = equip_DropList;
    }


    [field:SerializeField]public int _abilitySlot { get; private set; }
    public void MakeEquipAbilitySlot(int abilitySlot)
    {
        _abilitySlot = abilitySlot;
    }

    public int _dropSlot { get; private set; }
    public void MakeEquipDropSlot(int dropSlot)
    {
        _dropSlot = dropSlot;
    }

    #endregion

    //but for now we will be testing just the level stuff.

}


//instead of a saveclass will be the SO directly.

//i TWO int lists for teh equipment.
//i need a int list for each building and the building itself will adjust the stuff the player has
//