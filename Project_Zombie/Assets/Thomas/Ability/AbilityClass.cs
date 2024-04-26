using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]   
public class AbilityClass 
{

    //this is used for passive and active.
    //in passive we register how much of an ability it has
    //in active we register cooldowns. 
    //actives also can contain level to detect how strong is a fella. we use this fella to communicate with data.

    public AbilityClass() 
    { 
        
    
    }

    public AbilityClass(AbilityClass refClass)
    {
        dataActive = refClass.dataActive;
        dataPassive = refClass.dataPassive;

    }


    public int level;
    public string debugName;

    public void IncreaseLevel()
    {
        //if its a passive then we need to remove and add again.
        //if its active we do nothing.

        if(dataPassive != null)
        {
            //so we need to remove this fella. increase level and add.
            RemovePassive();
            level++;
            AddPassive();
            return;
        }



    }


    #region ACTIVE
    public AbilityActiveData dataActive {  get; private set; }

    float activeCooldownCurrent;
    float activeCooldownTotal;

    public AbilityClass(AbilityActiveData data)
    {
        dataActive = data;
        debugName = data.abilityName;
        activeCooldownTotal = data.abilityCooldown;
    }

    public void UseActive()
    {

        //set new cooldown.
        if (activeCooldownCurrent > 0) return;

        float reducer = GetCooldownReducer();
        activeCooldownTotal = dataActive.abilityCooldown - reducer;
        activeCooldownCurrent = activeCooldownTotal;

        dataActive.Call(this);

    }

    float GetCooldownReducer()
    {
        EntityStat stat = PlayerHandler.instance._entityStat;
        if(stat == null) return 0;
        float modifier = stat.GetTotalValue(StatType.SkillCooldown);
        return dataActive.abilityCooldown * modifier;
    }

    public void HandleActiveCooldown()
    {
        if(activeCooldownCurrent > 0)
        {
            activeCooldownCurrent -= Time.fixedDeltaTime;
            activeCooldownCurrent = Mathf.Clamp(activeCooldownCurrent, 0, activeCooldownTotal);
            UICooldown(activeCooldownCurrent, activeCooldownTotal);
        }
    }


    #endregion


    #region PASSIVE
    public AbilityPassiveData dataPassive { get; private set; }
    public AbilityClass(AbilityPassiveData data, int level = 1)
    {
        dataPassive = data;
        this.level = level;
        debugName = data.abilityName;
        
    }

    public void AddPassive()
    {
        dataPassive.Add(this);
    }
    public void RemovePassive()
    {
        dataPassive.Remove(this);
    }

    #endregion

    #region UI

    public AbilityUnit _abilityUnit {  get; private set; }
    public void SetUI(AbilityUnit _abilityUnit)
    {
        this._abilityUnit = _abilityUnit;
    }

    void UICooldown(float current, float total)
    {
        if(_abilityUnit != null)
        {
            _abilityUnit.UpdateCooldown(current, total);
        }
    }

    #endregion

    public bool IsEmpty()
    {
        return dataPassive == null && dataActive == null;
    }



}
