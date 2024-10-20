using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerEquipmentData : ScriptableObject
{
    //in here we will organize the information about the equipped stuff.

    [field: SerializeField] public int _gunStored { get; private set; } = 0;

    public void MakeGunStored(int gunStored)
    {
        _gunStored = gunStored;
    }


    [field: SerializeField] public List<int> _abilitiesStoredList { get; private set; } = new(); //null items have -1

    public void MakeAbilitiesStoredList(List<int> abilitiesStoredList)
    {
        _abilitiesStoredList = abilitiesStoredList;
    }


    [field: SerializeField] public List<int> _dropStoredList { get; private set; } = new();

    public void MakeDropStoredList(List<int> dropStoredList)
    {
        _dropStoredList = dropStoredList;
    }

    public void CaptureState(SaveClass saveClass)
    {


        saveClass.MakeEquipGunIndex(_gunStored);

        saveClass.MakeEquipAbilityList(_abilitiesStoredList);

        saveClass.MakeEquipDropList(_dropStoredList);




    }
    public void RestoreState(SaveClass saveClass)
    {
        //here its a bit more complicated.
        //we must get those stuff from the city, put it here and call the ui.

        _gunStored = saveClass._equip_GunIndex;
        _abilitiesStoredList = saveClass._equip_AbilityList;
        _dropStoredList = saveClass._equip_DropList;



        UIHandler.instance._EquipWindowUI.RestoreState();
    }

}
