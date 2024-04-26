using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStat : MonoBehaviour
{
    //THIS WILL TAKE CARE
    //im not handling percent values.
    //im not dealing with remove.


    //i am doing the calculation there in the moment.

    [SerializeField] List<StatClass> debugInitialList = new();

    EntityEvents _entityEvents;

    private void Awake()
    {
        if(debugInitialList.Count > 0) 
        {
            SetUp(debugInitialList);
        
        }

        _entityEvents = GetComponent<EntityEvents>();

        if(_entityEvents == null)
        {
            Debug.LogError("ENTIY EVENT NOT APPEARING HERE " + gameObject.name);
        }
    }

    private void Update()
    {

        
    }


    #region STAT
    [SerializeField] List<StatClass> debugStatList;
    protected Dictionary<StatType, float> statBaseDictionary = new Dictionary<StatType, float>();
    protected Dictionary<StatType, float> statAlteredDictionary = new Dictionary<StatType, float>();

    public void SetUp(List<StatClass> initialStatList)
    {
        List<StatType> refList = MyUtils.GetStatListRef();


        foreach (var item in initialStatList)
        {
            statBaseDictionary.Add(item.stat, item.value);
            debugStatList.Add(new StatClass(item.stat, item.value));
        }

        foreach (var item in refList)
        {
            if (!statBaseDictionary.ContainsKey(item))
            {
                statBaseDictionary.Add(item, 0);
            }

        }

        SetUpEmptyAlteredDictionary(refList);
    }

    public void SetUpWithScalingList(int round, List<StatClass> initialStatList, List<StatClass> scalingStatList)
    {
       List<StatType> refList =  MyUtils.GetStatListRef();


        //i need to create the list and i can do this later perhpass:

        foreach (var item in refList)
        {
            float value = GetValueFromList(item, round, initialStatList, scalingStatList);

            statBaseDictionary.Add(item, value);
            debugStatList.Add(new StatClass(item, value));
        }

        SetUpEmptyAlteredDictionary(refList);

    }


    void SetUpEmptyAlteredDictionary(List<StatType> refList)
    {
        foreach (var item in refList)
        {
            statAlteredDictionary.Add(item, 0);
        }
    }

    float GetValueFromList(StatType stat, int round, List<StatClass> initialStatList, List<StatClass> scalingStatList)
    {
        float value = 0;

        foreach (var item in initialStatList)
        {
            if(item.stat == stat)
            {
                value = item.value;
            }
        }

        foreach (var item in scalingStatList)
        {
            if (item.stat == stat)
            {
                value += item.value * round;
            }
        }


        return value;
    }


   

    #endregion

    #region BD
    public bool isStunned { get; private set; }

    [SerializeField] List<BDClass> tempList = new();
    [SerializeField] List<BDClass> permaList = new();
    [SerializeField] List<BDClass> tickList = new();
    Dictionary<string, BDClass> dictionaryForStacking = new Dictionary<string, BDClass>();

    void HandleBD()
    {

    }

    public void AdBD(BDClass bd)
    {
        //we should call an event to inform whoever is coonect to this 
        //also when we remove it we do stuff;
        if (dictionaryForStacking.ContainsKey(bd.id))
        {
            //then we stack
            return;
        }

        //we need to check if we already have this fella. if yes then we 

        //then we apply the thing.
        switch (bd.bdType)
        {
            case BDType.Stat:
                AddBDStat(bd);

                break;

            case BDType.Damage:
                //we dont call it now but we add and everytime it wants to do its thing it does.
                AddBDDamage(bd);

                break;

            case BDType.Stun:
                AddBDStun(bd);
                break;
        }



        if (bd.IsTemp())
        {
            tempList.Add(bd);
        }
        else
        {
            permaList.Add(bd);
        }

    }
    void AddBDStat(BDClass bd)
    {

        float flatValue = bd.statValueFlat;
        statAlteredDictionary[bd.statType] += flatValue;

        float basePercentValue = statBaseDictionary[bd.statType] * bd.statValuePercentbasedOnBaseValue;
        statAlteredDictionary[bd.statType] += basePercentValue;

        //we inform the fellas to get the right stuff
        _entityEvents.OnUpdateStat(bd.statType, GetTotalValue(bd.statType));
    }
    void AddBDDamage(BDClass bd)
    {

    }
    void AddBDStun(BDClass bd)
    {
        //
    }


    public void RemoveBdWithID(string id)
    {

        for (int i = 0; i < tempList.Count; i++)
        {
            var item = tempList[i];
            if (item.id == id)
            {
                //if its the same then we must remove this fella.
                RemoveBDWithIndex(i);
                break;
            }
        }
        for (int i = 0; i < permaList.Count; i++)
        {
            var item = permaList[i];
            if (item.id == id)
            {
                //if its the same then we must remove this fella.
                RemoveBDWithIndex(i);
                break;
            }
        }
    }

    public void RemoveBDWithIndex(int index)
    {


        BDClass bd = permaList[index];
        permaList.RemoveAt(index);

        if(bd.bdType == BDType.Stat)
        {
            RemoveStat(bd);
        }
    }

    void RemoveStat(BDClass bd)
    {
        float flatValue = bd.statValueFlat;
        statAlteredDictionary[bd.statType] -= flatValue;

        float basePercentValue = statBaseDictionary[bd.statType] * bd.statValuePercentbasedOnBaseValue;
        statAlteredDictionary[bd.statType] += basePercentValue;

        _entityEvents.OnUpdateStat(bd.statType, GetTotalValue(bd.statType));
    }




    public List<ModifierClass> GetModifierOfCertainStat(StatType stat)
    {
        List<ModifierClass> newList = new();

        foreach (var item in permaList)
        {
            if(item.statType == stat && item.bdType == BDType.Stat)
            {
                ModifierClass modifier = new ModifierClass(item.id, "BD", item.statValueFlat, item.statValuePercentbasedOnBaseValue);
                newList.Add(modifier);
            }
            
        }

        return newList;
    }

    #endregion


    #region GETTING VALUES




    public float GetTotalValue(StatType stat)
    {
        if(!statBaseDictionary.ContainsKey(stat) || !statAlteredDictionary.ContainsKey(stat))
        {
            if(stat == StatType.CritChance)
            {
                Debug.Log("no crit chance");
            }
            return 0;
        }


        return GetClampedValueForTotal(stat);
    }

    float GetClampedValueForTotal(StatType stat)
    {
       float value = statBaseDictionary[stat] + statAlteredDictionary[stat];

        if(stat == StatType.SkillCooldown)
        {
            value = Mathf.Clamp(value, 0, 0.8f);
        }

        return value;
    }
    

    #endregion

}

[System.Serializable]
public class StatClass
{
    public StatClass(StatType stat, float value)
    {
        this.stat = stat;
        this.initialValue = value;
    }


    public StatType stat;
    [SerializeField] float initialValue;
    public float value { get { return initialValue; } }    
}

public enum StatType 
{ 
    Health,
    Speed,
    Damage,
    DamageReduction,
    Tenacity,
    Pen,
    CritChance,
    CritDamage,
    ReloadSpeed,
    Magazine,
    FireRate,
    SkillCooldown,
    SkillDamage,
    Luck,
    Vampirism,


}