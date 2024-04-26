using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbility : MonoBehaviour
{
    [SerializeField] List<AbilityClass> abilityActiveList = new();
    [SerializeField] List<AbilityClass> abilityPassiveList = new();

    [SerializeField] List<AbilityActiveData> debugAbilityActiveStartingList = new();
    [SerializeField] List<AbilityPassiveData> debugPassiveStartingList = new();
    private void Awake()
    {
        SetAbility();
    }

    private void FixedUpdate()
    {
        HandleAbilityActiveCooldown();
    }

    #region SET ABILITIES
    void SetAbility()
    {
        for (int i = 0; i < 3; i++)
        {
            abilityActiveList.Add(new AbilityClass());
        }

        foreach (var item in debugAbilityActiveStartingList)
        {
            AddAbility(item);
        }

        foreach(var item in abilityActiveList)
        {
            UIHandler.instance.AbilityUI.SetActiveAbility(abilityActiveList);
        }

        foreach (var item in debugPassiveStartingList)
        {
            AddAbility(item);
        }

    }

    public void AddAbility(AbilityBaseData data)
    {

        AbilityActiveData dataActive = data.GetActive();

        if(dataActive != null)
        {
            AddAbilityActive(dataActive);
            return;
        }


        AbilityPassiveData dataPassive = data.GetPassive();

        if (dataPassive != null)
        {
            AddAbilityPassive(dataPassive);
            return;
        }

    }

    void AddAbilityActive(AbilityActiveData data)
    {
        int index = GetAbilityFreeSlot(abilityActiveList);

        if(index != -1)
        {
            abilityActiveList[index] = new AbilityClass(data);
        }
        else
        {
            Debug.Log("couldnt add");
        }

    }
    void AddAbilityPassive(AbilityPassiveData data)
    {
        //we simply add
        //but we have to check if there is already one and if we need to add.
        int index = GetTargetAbility(data, abilityPassiveList);


        if(index != -1)
        {
            //
            ReplaceSameAbilityByIndex(index);
        }
        else
        {
            //we s imply add to teh list
            AbilityClass passiveAbility = new AbilityClass(data);
            passiveAbility.AddPassive();
            abilityPassiveList.Add(passiveAbility);
        }
        



    }


    public int GetAbilityFreeSlot(List<AbilityClass> abilityList)
    {
        for (int i = 0; i < abilityList.Count; i++)
        {
            if (abilityList[i].IsEmpty())
            {
                return i;
            }
        }
        return -1;
    }

    int GetTargetAbility(AbilityBaseData target, List<AbilityClass> abilityList)
    {
        for (int i = 0; i < abilityList.Count; i++)
        {
            if (abilityList[i].IsEmpty()) continue;
            if (abilityList[i].dataActive == target)
            {
                return i;
            }
            if (abilityList[i].dataPassive == target)
            {
                return i;
            }

        }
        return -1;
    }


    public void ReplaceAbility(AbilityBaseData data, int index)
    {

    }
    public void ReplaceSameAbility(AbilityClass ability)
    {

        if(ability.dataPassive == null)
        {
            Debug.Log("ability sent has nothing");
            return;
        }

        //we look for someone to replace this.
        foreach (var item in abilityPassiveList)
        {
            if (item.IsEmpty()) continue;
            if (item.dataPassive == null) continue;
            if(item.dataPassive == ability.dataPassive)
            {
                item.IncreaseLevel();
                return;
            }

        }
    }

    public void ReplaceSameAbilityByIndex(int index)
    {
        var item = abilityPassiveList[index];
        if (item.IsEmpty()) return;
        if (item.dataPassive == null) return;
        item.IncreaseLevel();

        
    }


    public void ClearPassiveList()
    {
        foreach (var item in abilityPassiveList)
        {
            item.RemovePassive();
        }

        abilityPassiveList.Clear();
    }


    public void DebugIncreaseLevel()
    {
        if(abilityPassiveList.Count > 0)
        {
            abilityPassiveList[0].IncreaseLevel();
        }
    }

    #endregion

    public void UseAbilityActive(int index)
    {
        if (!abilityActiveList[index].IsEmpty())
        {
            //it first needs to be empty.
            abilityActiveList[index].UseActive();
        }
    }

    void HandleAbilityActiveCooldown()
    {
        foreach (var item in abilityActiveList)
        {
            item.HandleActiveCooldown();
        }
    }



}
