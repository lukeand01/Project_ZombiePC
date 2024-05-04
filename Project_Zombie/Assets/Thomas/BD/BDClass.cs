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

    public BDClass(string id, BDDamageType damageType, IDamageable damageable, DamageClass damage, int tickTotal, float tickTimeTotal)
    {
        this.id = id;
        this.damageType = damageType;
        this.damageable = damageable;
        this.damage = damage;

        CreateTick(tickTotal, tickTimeTotal);
    }

    void HandleDamageTick()
    {
        if (damageType == BDDamageType.Burn)
        {
            damageable.TakeDamage(damage);
        }
    }


    #endregion

    #region TICK 

    int tickTotal;
    int tickCurrent;

    float tickTimerTotal;
    float tickTimerCurrent;

    public void CreateTick(int tickTotal, float tickTimerTotal)
    {
        this.tickTotal = tickTotal;
        tickTimerCurrent = 0;
        this.tickTimerTotal = tickTimerTotal;
        MakeShowInUI();
    }


    //the stack refresh the tick. and also the stacks only affect it.
    public void HandleTick()
    {
        if (tickTimerCurrent > tickTimerTotal)
        {
            //we proc the damage here.
            //but it needs to be influenced bystacks.

            if (bdType == BDType.Damage)
            {
                HandleDamageTick();
            }


            tickCurrent += 1;
            tickTimerCurrent = 0;
        }
        else
        {
            tickTimerCurrent += Time.fixedDeltaTime;
        }

    }

    void ResetTick()
    {
        tickCurrent = 0;
    }

    public bool IsTick() => tickTotal > 0;

    public bool IsTickDone()
    {
        return tickCurrent >= tickTotal;
    }

    //i need the original value for this otherwill we will increase the stack version.



    #endregion

    #region TEMP

    float tempCurrent;
    float tempTotal;

    public void MakeTemp(float total)
    {
        tempCurrent = 0;
        tempTotal = total;
        MakeShowInUI();
        debugName += "IsTemp: " + total.ToString() + ";";
    }

    public void HandleTemp()
    {
        if (tempTotal > tempCurrent)
        {
            tempCurrent += Time.fixedDeltaTime;
        }
        else
        {

        }
    }

    void ResetTemp()
    {
        tempCurrent = 0;

    }

    public bool IsTemp() => tempTotal > 0;

    public bool IsTempDone()
    {
        return tempCurrent >= tempTotal;
    }

    #endregion

    #region STUN
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
        this.stackTotal = stackTotal;
        this.doesStackingRefreshTimer = doesStackingRefreshTimer;
    }

    public void Stack(BDClass bd)
    {
        //we can stack more than just stat.
        Debug.Log("stack");
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

    public void CreateBDUnit()
    {
        if (!showInUI) return;
        bd = UIHandler.instance.bdUI.CreateBDUnit(this);
    }

    void UpdateBDUnitStack()
    {
        if(bd != null)
        {
            bd.UpdateStack();
        }
    }

    public void MakeShowInUI()
    {
        showInUI = true;
    }
    #endregion


}

public enum BDType
{
    Undecided,
    Stat, //change stats by flat or percent
    Passive, //a passive that can be triggered while the bd is active
    Damage, //damage over a duration of time while the bd is active.
    Stun, //it used to be a boolean but now itsd the stun.
    Immune
}

public enum BDDamageType 
{ 
    Burn,
    Bleed
}
