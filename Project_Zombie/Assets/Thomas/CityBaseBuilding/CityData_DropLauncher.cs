using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CityData / Drop")]
public class CityData_DropLauncher : CityData
{
    //we show options per thing.
    [Separator("Drop_LeveLists")]
    [SerializeField] List<CityStoreDropLauncherClass_DividedByArmoryLevel> cityDropDividedByLevelList = new(); //this is just for division.
    [Separator("Drop_AllRef")]
    [SerializeField] List<DropData> dropDataRefList = new(); //these are all items. how toel perma from temp?

    List<int> dropIndexList = new(); //what is this for?
    [SerializeField]List<int> equippedDropIndexList = new(); //this is the list for saving the drops you have.


    [field: SerializeField] public int dropSlot;
    [field: SerializeField] public List<DropData> currentAvailableDropList { get; private set; } = new();
    [field:SerializeField] public List<DropData> currentEquippedDropList { get; private set; } = new();



    public void Initalize()
    {
        SetIndexForRefList();
        GenerateAvailableList();
        GenerateListForEquipContainer();
        SendEquippedDropForEquipUnits();
    }

    void SetIndexForRefList()
    {
        for (int i = 0; i < dropDataRefList.Count; i++)
        {
            dropDataRefList[i].SetIndex(i);
        }
    }

    bool HasDrop_AvailableList(int index)
    {
        foreach (var item in currentAvailableDropList)
        {
            if (item.storeIndex == index) return true;
        }
        return false;
    }

    void GenerateAvailableList()
    {

        currentAvailableDropList.Clear();

        //avai
        for (int i = 0; i < cityStoreLevel; i++)
        {
            if (i >= cityDropDividedByLevelList.Count)
            {
                Debug.Log("CUT");
                break;
            }
            //otherwise 

            foreach (var item in cityDropDividedByLevelList[i].dropList)
            {
                currentAvailableDropList.Add(item);
            }

        }

        foreach (var item in dropIndexList)
        {
            //if i have something here that i do not yet have in the available list then we add it
            if (!HasDrop_AvailableList(item))
            {
                currentAvailableDropList.Add(dropDataRefList[item]);
            }
        }


    }

    public void GenerateListForEquipContainer()
    {
        //we get a list and remove those that are in the currentequipped.

        List<DropData> dropRefList = new();


        for (int i = 0; i < currentAvailableDropList.Count; i++)
        {
            var item = currentAvailableDropList[i];

            if (currentEquippedDropList.Contains(item))
            {

            }
            else
            {
                dropRefList.Add(item);
            }
        }


        


        UIHandler.instance._EquipWindowUI.UpdateOptionForDrops(dropRefList);
    }

    public void SendEquippedDropForEquipUnits()
    {
        //we are going to send the information there.
        UIHandler.instance._EquipWindowUI.ReceiveDropList(currentEquippedDropList);
    }



    public DropData GetDropDataFromIndex(int index)
    {
        return dropDataRefList[index];
    }

    public void SetEquippedDropList(List<DropData> dropList)
    {
        //we will get this list to create a new indexlist
        currentEquippedDropList = dropList;



    }

    public DropData GetDropData()
    {

        //the chance to spawn each should be the same?
        //

        if (currentEquippedDropList.Count == 0) return null;

        int tries = cityStoreLevel * 2;

        for (int i = 0; i < tries; i++)
        {
            int roll = Random.Range(0, 500);
            int random = Random.Range(0, currentEquippedDropList.Count);

            var item = currentEquippedDropList[random]; 

            if(item.rollRequired > roll)
            {
                return item;
            }
        }

        return null;
    }



    public void RestoreState(SaveClass saveClass)
    {
        equippedDropIndexList = saveClass._equip_DropList;

        //and we will use this 
        currentEquippedDropList.Clear();

        for (int i = 0; i < equippedDropIndexList.Count; i++)
        {
            var item = equippedDropIndexList[i];
            currentEquippedDropList.Add(dropDataRefList[item]);
        }

    }
    public void CaptureState(SaveClass saveClass)
    {
        equippedDropIndexList.Clear();

        for (int i = 0; i < currentEquippedDropList.Count; i++)
        {
            var item = currentEquippedDropList[i];

            equippedDropIndexList.Add(item.storeIndex);
        }

        saveClass.MakeEquipDropList(equippedDropIndexList);

    }


}
[System.Serializable]
public class CityStoreDropLauncherClass_DividedByArmoryLevel
{
    public List<DropData> dropList = new();

}

[System.Serializable]
public class DropChanceClass
{
    public DropData data;
    [Range(1,100)]public int chance;
}