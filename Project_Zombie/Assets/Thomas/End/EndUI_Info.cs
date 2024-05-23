using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndUI_Info : MonoBehaviour
{
    //this will receive the inform and put in the rihgt place.

    [SerializeField] List<Transform> containerList = new();
    [SerializeField] List<ButtonBase> containerButtonList = new();
    [SerializeField] AbilityUnit abilityUnitTemplate;
    [SerializeField] ResourceUnit resourceUnitTemplate;
    [SerializeField] StatTrackerUnit statTrackerUnitTemplate;
    public void ButtonCall_ChangeContainer(int index)
    {

        foreach (var item in containerButtonList)
        {
            item.ControlSelected(false);
        }

        foreach (Transform t in containerList)
        {
            t.gameObject.SetActive(false);
        }

        containerButtonList[index].ControlSelected(true);
        containerList[index].gameObject.SetActive(true);

    }

    public void Open()
    {
        containerList[0].gameObject.SetActive(true);
        containerButtonList[0].ControlSelected(true);
    }

    public void SetStatTracker(Dictionary<StatTrackerType, float> statTrackerDictionary)
    {
        //this only receives the info and pass to ui units.
        List<StatTrackerType> refList = MyUtils.GetStatTrackerRefList();
        int indexForStatTracker = 0;
        ClearContainer(containerList[indexForStatTracker]);

        foreach (var item in refList)
        {
            float value = 0;

            if (statTrackerDictionary.ContainsKey(item))
            {
                value = statTrackerDictionary[item];
            }


            StatTrackerUnit newObject = Instantiate(statTrackerUnitTemplate);
            newObject.SetUp(item, value);
            newObject.transform.SetParent(containerList[indexForStatTracker]);
        }


    }
    
    public void SetAbility(List<AbilityClass> abilityList)
    {
        int indexForAbility = 1;

        ClearContainer(containerList[indexForAbility]);

        foreach (var ability in abilityList)
        {
            AbilityUnit newObject = Instantiate(abilityUnitTemplate);
            newObject.SetUpPassive(ability);
            newObject.transform.SetParent(containerList[indexForAbility]);
        }
    }

    public void SetResource(List<ItemClass> resourceList)
    {
        int indexForResource = 3;
        ClearContainer(containerList[indexForResource]);
        foreach (var item in resourceList)
        {
            ResourceUnit newObject = Instantiate(resourceUnitTemplate);
            newObject.transform.SetParent(containerList[indexForResource]);
            newObject.SetUp(item);
        }
    }


    void ClearContainer(Transform targetContainer)
    {
        for (int i = 0; i < targetContainer.childCount; i++)
        {
            Destroy(targetContainer.GetChild(i).gameObject);
        }
    }
}
