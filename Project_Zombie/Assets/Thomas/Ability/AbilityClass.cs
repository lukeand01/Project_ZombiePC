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
            UpdateLevel();
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

        bool isSuccess = dataActive.Call(this);


        if (!isSuccess) return;

        float reducer = GetCooldownReducer();
        activeCooldownTotal = dataActive.abilityCooldown - reducer;
        activeCooldownCurrent = activeCooldownTotal;

        

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

    public void DestroyUI()
    {
        if (_abilityUnit != null)
        {
            _abilityUnit.DestroyItself();
        }
    }

    void UICooldown(float current, float total)
    {
        if(_abilityUnit != null)
        {
            _abilityUnit.UpdateCooldown(current, total);
        }
    }

    void UpdateLevel()
    {
        if(_abilityUnit != null)
        {
            _abilityUnit.UpdatePassiveLevel();
        }
    }

    #endregion

    #region FOR DESCRIPTION

    public string GetNameForDescription()
    {
        if(dataActive != null)
        {
            return dataActive.abilityName;
        }
        if (dataPassive != null)
        {
            return dataPassive.abilityName;
        }
        return "No Data";
    }
    public string GetTypeForDescription()
    {
        if (dataActive != null)
        {
            return "Active Ability";
        }
        if (dataPassive != null)
        {
            return "Passive Ability";
        }
        return "No Data";
    }
    public string GetTierForDescription()
    {
        if (dataActive != null)
        {
            return dataActive.abilityTier.ToString();
        }
        if (dataPassive != null)
        {
            return dataPassive.abilityTier.ToString();
        }
        return "No Data";
    }
    public string GetDescriptionForDescription()
    {
        if (dataActive != null)
        {
            return dataActive.abilityDescription;
        }
        if (dataPassive != null)
        {
            return dataPassive.abilityDescription;
        }
        return "No Data";
    }
    public string GetDamageForDescription()
    {
        //i will use the damage part for damage or stat.
        if (dataActive != null)
        {
            return dataActive.GetDamageDescription(level);
        }
        if (dataPassive != null)
        {
            return dataPassive.GetDamageDescription(level);
        }
        return "No Data";
    }
    public string GetCooldownForDescription()
    { 
        //for active it shows cooldown
        //for passive it shows stack


        if (dataActive != null)
        {
            return "Cooldown: " + activeCooldownTotal;
        }
        if (dataPassive != null)
        {
            return "Stack: " + level;
        }
        return "No Data";
    }
    #endregion


    public bool IsEmpty()
    {
        return dataPassive == null && dataActive == null;
    }



}
