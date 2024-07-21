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

   [field:SerializeField] public float statValue_PercentbasedOnBaseValue { get; private set; }
    float statValue_PercentbasedOnBaseValue_Original;

    public float statValue_PercentbasedOnCurrentValue { get; private set; }
    float statValue_PercentbasedOnCurrentValue_Original;


    public BDClass(string id, StatType statType, float valueFlat, float valuePercentBase, float valuePercentCurrent)
    {
        debugName = $"id: {id}; stat: {statType}; valueFlat: {valueFlat}; valuePercentBase {valuePercentBase}; ValuePercentCurrent {valuePercentCurrent}";



        this.id = id;
        bdType = BDType.Stat;
        this.statType = statType;

        statValueFlat = 0;
        statValue_PercentbasedOnBaseValue = 0;
        statValue_PercentbasedOnCurrentValue = 0;

        statValueFlatOriginal = valueFlat;
        statValue_PercentbasedOnBaseValue_Original = valuePercentBase;
        statValue_PercentbasedOnCurrentValue_Original = statValue_PercentbasedOnCurrentValue;

        MakeFlatValue(valueFlat);
        MakeValuePercentBasedInBase(valuePercentBase);
        MakeValuePercentbasedInCurrent(valuePercentCurrent);


    }

    public void MakeFlatValue(float value)
    {
        statValueFlat = value;
    }
    public void MakeValuePercentBasedInBase(float value)
    {
        //Debug.Log("percent value_Level " + value_Level + " current stacks " + stackCurrent);
        statValue_PercentbasedOnBaseValue = value;
    }
    public void MakeValuePercentbasedInCurrent(float value)
    {
        statValue_PercentbasedOnCurrentValue += value;
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
        this.damageModifier = damageModifier * 0.01f;

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

    //i need the original value_Level for this otherwill we will increase the stack version.



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

    public void ResetTemp()
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

    //perphaps i want a modifier per 


    bool doesStackingRefreshTimer;
    int stackTotal;
    public int stackCurrent { get; private set; }

    public float stackScaleModifier {  get; private set; }

    public void MakeStack(int stackTotal, bool doesStackingRefreshTimer)
    {
        stackCurrent = 1;
        this.stackTotal = stackTotal;
        this.doesStackingRefreshTimer = doesStackingRefreshTimer;
    }

    public void MakeStackScaleable(float stackScaleModifier)
    {
        this.stackScaleModifier = stackScaleModifier;
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
            //Debug.Log("can no longer stack " + stackTotal);
            return;
        }

        
        stackCurrent += 1;
        UpdateBDUnitStack();
        ReapplyBD();

        //how to get the values working accordingly.

    }

    public void LoseStack()
    {
        stackCurrent -= 1;
        UpdateBDUnitStack();
        ReapplyBD();

        //each stack might represetn a value_Level so we need to calculate that as well.
    }

    void ReapplyBD()
    {
        if (bdType == BDType.Stat)
        {
            MakeFlatValue(GetValueMultipledByStackScale(statValueFlatOriginal));

            //Debug.Log(statValue_PercentbasedOnBaseValue_Original + " " + statValue_PercentbasedOnBaseValue);

            //i want it to be 0.15 - 0.3 - 0.45 - 0.3 - 0.15

            //float newValue = GetValueMultipledByStackScale(statValue_PercentbasedOnBaseValue_Original);
            //Debug.Log("new value_Level for this " + newValue + " from current stack " + stackCurrent);
            //MakeValuePercentBasedInBase(newValue);

           MakeValuePercentBasedInBase(GetValueMultipledByStackScale(statValue_PercentbasedOnBaseValue_Original));

           

           MakeValuePercentbasedInCurrent(GetValueMultipledByStackScale(statValue_PercentbasedOnCurrentValue_Original));
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

    float GetValueMultipledByStackScale(float value)
    {
        if(stackScaleModifier == 0 || stackCurrent == 0 || value == 0)
        {
            return value;
        }

        //Debug.Log("raw value_Level " + value_Level);
        float modifier = stackScaleModifier * stackCurrent;
        //Debug.Log("this is the modifier " +  modifier); 
        float result = value * modifier;
        //Debug.Log("this is the result " + result);

        return result;
    }


    public bool IsStackable() => stackTotal > 0;

    public bool LastStack() => stackCurrent <= 1;

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
        if(statValueFlat > 0|| statValue_PercentbasedOnBaseValue > 0 || statValue_PercentbasedOnCurrentValue > 0)
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

        if (statValue_PercentbasedOnBaseValue != 0)
        {
            result += "by " + statValue_PercentbasedOnBaseValue.ToString();
        }

        if (statValue_PercentbasedOnCurrentValue_Original != 0)
        {
            result += "by " + statValue_PercentbasedOnCurrentValue_Original.ToString();
        }

        return result;

    }

    #endregion

    #region PASSIVE EVENT

    //what it does is that it never runs out.
    //should i put them in a different place? or in the same as the other bd?
    //

    AbilityPassiveData dataAssignedToBD;
    AbilityClass ability;

    public BDClass(string id, AbilityClass ability)
    {
        this.id = id;
        bdType = BDType.Passive;
        dataAssignedToBD = ability.dataPassive;
        this.ability = ability;


    }

    public void CallPassiveEvent()
    {
        Debug.Log("called passive");
        dataAssignedToBD.Call(ability);
        ResetTemp();
    }

    public bool IsNeverOut()
    {
        return dataAssignedToBD != null;
    }

    #endregion

    #region ESPECIAL CONDITION
    //perphaps
    public EspecialConditionType especialConditionType { get; private set; }

    public BDClass(string id, EspecialConditionType especialConditionType, float value)
    {
        this.id = id;   
        bdType = BDType.EspecialCondition;
        this.especialConditionType = especialConditionType;
        statValueFlat = value;
    }


    #endregion

    bool IsPositive()
    {
        return statValueFlat > 0 || statValue_PercentbasedOnBaseValue > 0 || statValue_PercentbasedOnCurrentValue > 0;
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
            float baseDamage = 15;
            float percentDamage = damageable.GetTargetMaxHealth() * 0.08f;
            float total = baseDamage + percentDamage;
            float modifier = total * damageModifier;
            total += modifier;

            return total;
        }
        if(damageType == BDDamageType.Burn)
        {
            float baseDamage = 15;
            float percentDamage = damageable.GetTargetMaxHealth() * 0.09f;
            float total = baseDamage + percentDamage;
            float modifier = total * damageModifier;
            total += modifier;

            return total;
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
    SecretBulletMultipler,
    EspecialCondition //these are are mostly for the 
}

public enum BDDamageType 
{ 
    Burn,
    Bleed
}

//what is the bleed damage? percent? flat and percent. its all 5 + 5% of max health