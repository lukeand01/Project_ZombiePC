using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUI : MonoBehaviour
{

    GameObject holder;

    private void Awake()
    {
        holder = transform.GetChild(0).gameObject;
    }

    public void ControlUI(bool isVisible)
    {
        holder.SetActive(isVisible);
    }


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

    [Separator("DASH")]
    [SerializeField] GameObject dashHolder;
    [SerializeField] Image dashFill;

    public void UpdateDashFill(float current, float total)
    {
        dashFill.fillAmount = current / total;
    }


    void ClearUI(Transform targetContainer)
    {
        for (int i = 0; i < targetContainer.childCount; i++)
        {
            Destroy(targetContainer.GetChild(i).gameObject);
        }
    }

}
