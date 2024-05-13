using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BDClass 
{
    //the types of bd
    //

    public string id { get; private set; }
    public BDType bdType { get; private set; }

    public string debugName;


    #region STAT
    public StatType statType { get; private set; } //this is only relevant if the bd is stat.
    float statValueFlatOriginal;
    public float statValueFlat { get; private set; }

    public float statValuePercentbasedOnBaseValue { get; private set; }
    float statValuePercentbasedOnBaseValueOriginal;

    public float statValuePercentbasedOnCurrentValue { get; private set; }
    float statValuePercentbasedOnCurrentValueOriginal;


    public BDClass(string id, StatType statType, float valueFlat, float valuePercentBase, float valuePercentCurrent)
    {
        debugName = $"id: {id}; stat: {statType}; valueFlat: {valueFlat}; valuePercent {valuePercentBase}; ";



        this.id = id;
        bdType = BDType.Stat;
        this.statType = statType;

        statValueFlat = 0;
        statValuePercentbasedOnBaseValue = 0;
        statValuePercentbasedOnCurrentValue = 0;

        statValueFlatOriginal = valueFlat;
        statValuePercentbasedOnBaseValueOriginal = statValuePercentbasedOnBaseValue;
        statValuePercentbasedOnCurrentValueOriginal = statValuePercentbasedOnCurrentValue;

        MakeFlatValue(valueFlat);
        MakeValuePercentBasedInBase(valuePercentBase);
        MakeVavluePercentbasedInCurrent(valuePercentCurrent);


    }

    public void MakeFlatValue(float value)
    {
        statValueFlat += value;
    }
    public void MakeValuePercentBasedInBase(float value)
    {
        statValuePercentbasedOnBaseValue += value;
    }
    public void MakeVavluePercentbasedInCurrent(float value)
    {
        statValuePercentbasedOnCurrentValue += value;
    }

    #endregion

    #region DAMAGE

    public BDDamageType damageType { get; private set; }
    DamageClass damage;
    IDamageable damageable;
    float damageModifier;
    public BDClass(string id, BDDamageType damageType, IDamageable damageable, float damageModifier, int tickTotal, float tickTimeTotal)
    {
        bdType = BDType.Damage;

        debugName = "Damage Tick " + damageType.ToString();

        this.id = id;
        this.damageable = damageable;
        this.damageType = damageType;
        this.damageModifier = damageModifier;

        float damageValue = GetDamage();
        damage = new DamageClass(damageValue);



        MakeShowInUI();
        //CreateBDUnit();
        CreateTick(tickTotal, tickTimeTotal);

    }

    void HandleDamageTick()
    {
        damageable.TakeDamage(damage);

    }


    #endregion

    #region TICK 

    public int tickTotal {  get; private set; }
    public int tickCurrent {  get; private set; }

    float tickTimerTotal;
    float tickTimerCurrent;

    public void CreateTick(int tickTotal, float tickTimerTotal)
    {
        this.tickTotal = tickTotal;
        tickCurrent = tickTotal;

        this.tickTimerTotal = tickTimerTotal;
        tickTimerCurrent = tickTimerTotal;
        MakeShowInUI();
    }


    //the stack refresh the tick. and also the stacks only affect it.
    public void HandleTick()
    {
        if (tickTimerCurrent <= 0)
        {
            //we proc the damage here.
            //but it needs to be influenced bystacks.

            if (bdType == BDType.Damage)
            {

                HandleDamageTick();
            }


            tickCurrent -= 1;
            UpdateBDUnitTick();
            tickTimerCurrent = tickTimerTotal;
        }
        else
        {

            tickTimerCurrent -= Time.fixedDeltaTime;
            UpdateFill(tickTimerCurrent, tickTimerTotal);
        }

    }

    void ResetTick()
    {
        tickCurrent = tickTotal;
    }

    public bool IsTick() => tickTotal > 0;

    public bool IsTickDone()
    {
        return tickCurrent <= 0;
    }

    //i need the original value for this otherwill we will increase the stack version.



    #endregion

    #region TEMP

    float tempCurrent;
    float tempTotal;

    public void MakeTemp(float total)
    {
        
        tempTotal = total;
        tempCurrent = tempTotal;
        MakeShowInUI();
        //CreateBDUnit();
        debugName += "IsTemp: " + total.ToString() + ";";
    }

    public void HandleTemp()
    {
        if (tempCurrent > 0)
        {
            tempCurrent -= Time.fixedDeltaTime;
            UpdateFill(tempCurrent, tempTotal); 
        }
        
    }

    void ResetTemp()
    {
        tempCurrent = tempTotal;

    }

    public float GetTempDurationForDescription()
    {
        if (IsTick())
        {
            //we will get the duration between tick, and put them together in a total.
            return tickTotal * tickTimerTotal;
        }
        if (IsTemp())
        {
            return tempTotal;
        }
        return -1;
    }
    public bool IsTemp() => tempTotal > 0;

    public bool IsTempDone()
    {
        return tempCurrent <= 0;
    }

    #endregion

    #region STUN, IMMUNE, INVISIBILITY, SECRET BULLETMULTIPLIER
    public BDClass(string id, BDType _bdType, float duration)
    {
        //this means its the stun.
        this.id = id;
        bdType = _bdType;

        if(duration != 0) MakeTemp(duration);
        
    }

    #endregion

    #region STACKING

    bool doesStackingRefreshTimer;
    int stackTotal;
    public int stackCurrent { get; private set; }

    public void MakeStack(int stackTotal, bool doesStackingRefreshTimer)
    {
        stackCurrent = 1;
        this.stackTotal = stackTotal;
        this.doesStackingRefreshTimer = doesStackingRefreshTimer;
    }

    public void Stack(BDClass bd)
    {
        //we can stack more than just stat.

        if (doesStackingRefreshTimer)
        {
            ResetTemp();
            ResetTick();
        }

        if (stackCurrent >= stackTotal)
        {
            Debug.Log("can no longer stack");
            return;
        }

        stackCurrent += 1;
        UpdateBDUnitStack();
        ReapplyBD();



    }


    void ReapplyBD()
    {
        if (bdType == BDType.Stat)
        {
            MakeFlatValue(statValueFlatOriginal);
            MakeValuePercentBasedInBase(statValuePercentbasedOnBaseValueOriginal);
            MakeVavluePercentbasedInCurrent(statValuePercentbasedOnCurrentValueOriginal);
            return;
        }

        if (bdType == BDType.Damage)
        {
            //icnrease or not the damage.
            //we need to get the damage and multiply it.

            if(damage == null)
            {
                float damageValue = GetDamage();
                damage = new DamageClass(damageValue);               
            }

            damage.MakeStack(stackCurrent);

            return;
        }

        if (bdType == BDType.Passive)
        {
            //then we ask the passive skill for what to do.
            return;
        }
    }
    public bool IsStackable() => stackTotal > 0;

    #endregion


    #region UI
    BDUnit bd;
    bool showInUI; //no perma i want to show. only temp
    //we can another 

    public void CreateBDUnit(EntityStatCanvas canvas = null)
    {
        if (!showInUI) return;
        if (bd != null) return;

        if(canvas == null)
        {
            bd = UIHandler.instance.bdUI.CreateBDUnit(this);
        }
        else
        {
            bd = canvas.CreateBDUnit(this);
        }

        
    }


    void UpdateFill(float current, float total)
    {
        if(bd != null)
        {
            bd.UpdateFill(current, total);
        }
    }

    void UpdateBDUnitStack()
    {
        if(bd != null)
        {
            bd.UpdateStack();
        }
    }

    void UpdateBDUnitTick()
    {
        if (bd != null)
        {
            bd.UpdateTick();
        }
    }

    public void RemoveBDUnit()
    {
        if(bd != null)
        {
            bd.OrderOwnDestruction();
        }
    }

    public void MakeShowInUI()
    {
        if (bdType == BDType.Stun) return;
        showInUI = true;
    }

    public string GetFirstForStat()
    {
        if(statValueFlat > 0|| statValuePercentbasedOnBaseValue > 0 || statValuePercentbasedOnCurrentValue > 0)
        {
            return "Increase";
        }
        else
        {
            return "Decrease";
        }
    }

    public string GetSecondForStat()
    {
        string result = "";

        if(statValueFlat != 0)
        {
            result += "by " + statValueFlat.ToString();
        }

        if (statValuePercentbasedOnBaseValue != 0)
        {
            result += "by " + statValuePercentbasedOnBaseValue.ToString();
        }

        if (statValuePercentbasedOnCurrentValueOriginal != 0)
        {
            result += "by " + statValuePercentbasedOnCurrentValueOriginal.ToString();
        }

        return result;

    }

    #endregion


    bool IsPositive()
    {
        return statValueFlat > 0 || statValuePercentbasedOnBaseValue > 0 || statValuePercentbasedOnCurrentValue > 0;
    }

    public string GetTypeForDescription()
    {
        if(bdType == BDType.Stat)
        {
            if (IsPositive())
            {
                return "Buff";
            }
            else
            {
                return "Debuff";
            }
        }
        if(bdType == BDType.Damage)
        {
            if(damageType == BDDamageType.Burn)
            {
                return "Damage Tick - Burn";
            }
            if (damageType == BDDamageType.Bleed)
            {
                return "Damage Tick - Bleed";
            }
        }

        if(bdType == BDType.Immune)
        {
            return "Immunity";
        }

        if(bdType == BDType.Stun)
        {
            return "Stun";
        }

        return "Error";
    }

    public string GetDamageDescription()
    {
        if (bdType != BDType.Damage) return "";

        float damage = GetDamage();
        
        if(damageType == BDDamageType.Bleed)
        {
            return $"5 base damage + 2% of target max health = {damage} physical damage per tick"; 
        }
        if (damageType == BDDamageType.Burn)
        {
            return $"5 base damage + 2% of target max health = {damage} pure damage per tick";
        }

        return "";
    }

    float GetDamage()
    {
        if(damageType == BDDamageType.Bleed)
        {
            float baseDamage = 5;
            float percentDamage = damageable.GetTargetMaxHealth() * 0.02f;
            return baseDamage + percentDamage;
        }
        if(damageType == BDDamageType.Burn)
        {
            float baseDamage = 5;
            float percentDamage = damageable.GetTargetMaxHealth() * 0.05f;
            return baseDamage + percentDamage;
        }


        return 0;
    }
}

public enum BDType
{
    Undecided,
    Stat, //change stats by flat or percent
    Passive, //a passive that can be triggered while the bd is active
    Damage, //damage over a duration of time while the bd is active.
    Stun, //it used to be a boolean but now itsd the stun.
    Immune,
    Invisible,
    SecretBulletMultipler
}

public enum BDDamageType 
{ 
    Burn,
    Bleed
}

//what is the bleed damage? percent? flat and percent. its all 5 + 5% of max health