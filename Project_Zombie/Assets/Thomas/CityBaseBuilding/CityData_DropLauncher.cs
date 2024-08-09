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

    List<int> dropIndexList = new();

    [field: SerializeField] public List<DropData> currentAvailableDropList { get; private set; } = new();


    public void Initalize()
    {
        SetIndexForRefList();
        GenerateAvailableList();
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


    //we check each to see if any of them was possible.
    //

    //the number of tries are based in the level
    public DropData GetDropData()
    {

        int tries = cityStoreLevel * 2;

        for (int i = 0; i < tries; i++)
        {
            int roll = Random.Range(0, 500);
            int random = Random.Range(0, currentAvailableDropList.Count);

            var item = currentAvailableDropList[random]; 

            if(item.rollRequired > roll)
            {
                return item;
            }
        }

        return null;
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