using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUI : MonoBehaviour
{
    [SerializeField] AbilityUnit abilityUnitTemplate;
    [SerializeField] Transform container;
    [SerializeField]List<AbilityUnit> abilityActiveUnitList = new();
    public void SetActiveAbility(List<AbilityClass> abilityList)
    {
        ClearUI(container);
        abilityActiveUnitList.Clear();

        for (int i = 0; i < abilityList.Count; i++)
        {
            var item = abilityList[i];
            AbilityUnit newObject = Instantiate(abilityUnitTemplate);
            newObject.transform.SetParent(container);
            newObject.SetUpActive(item, i);
            abilityActiveUnitList.Add(newObject);
        }

        foreach (AbilityClass ability in abilityList)
        {
           
        }
        
    }

    public void UpdateAbilityActiveUnit(AbilityClass ability, int index)
    {
        if(abilityActiveUnitList.Count >= index)
        {
            abilityActiveUnitList[index].SetUpActive(ability, index);
        }
    }

    


    void ClearUI(Transform targetContainer)
    {
        for (int i = 0; i < targetContainer.childCount; i++)
        {
            Destroy(targetContainer.GetChild(i).gameObject);
        }
    }

}
