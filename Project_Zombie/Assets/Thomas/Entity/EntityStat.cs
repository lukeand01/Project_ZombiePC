using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EntityStat : MonoBehaviour
{
    //THIS WILL TAKE CARE
    //im not handling percent values.
    //im not dealing with remove.


    //i am doing the calculation there in the moment.

    [SerializeField] List<StatClass> debugInitialList = new();
    [SerializeField] EntityStatCanvas _entityCanvas;
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
        //if (gameObject.tag == "Player") return;

        if (Input.GetKeyDown(KeyCode.T))
        {
            DebugBleed();
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            DebugStun();
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            DebugSpeed();
        }
    }

    private void FixedUpdate()
    {
        HandleBD();
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
            if (!statBaseDictionary.ContainsKey(item))
            {
                float value = GetValueFromList(item, round, initialStatList, scalingStatList);
                statBaseDictionary.Add(item, value);
                debugStatList.Add(new StatClass(item, value));
            }
            else
            {
                //Debug.Log("key already present in " + gameObject.name + " " + item.ToString());

            }

           
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
    [field:SerializeField]public bool isStunned { get; private set; }
    [field: SerializeField] public bool IsImmune {  get; private set; }

    [field:SerializeField] public bool IsInvisible {  get; private set; }


    [SerializeField] List<BDClass> tempList = new();
    [SerializeField] List<BDClass> permaList = new();
    [SerializeField] List<BDClass> tickList = new();
    Dictionary<string, BDClass> dictionaryForStacking = new Dictionary<string, BDClass>();

    
    void HandleBD()
    {

        for (int i = 0; i < tempList.Count; i++)
        {
            var item = tempList[i];

            item.HandleTemp();

            if (item.IsTempDone())
            {
                RemoveBDWithIndex(i, tempList);
            }

        }

        for (int i = 0; i < tickList.Count; i++)
        {
            var item = tickList[i];

            item.HandleTick();

            if (item.IsTickDone())
            {
                RemoveBDWithIndex(i, tickList);
            }
        }
    }

    public void AddBD(BDClass bd)
    {
        //we should call an event to inform whoever is coonect to this 
        //also when we remove it we do stuff;



        if (dictionaryForStacking.ContainsKey(bd.id))
        {
            //then we stack

            dictionaryForStacking[bd.id].Stack(bd);
            return;
        }

        //we need to check if we already have this fella. if yes then we 

        if (bd.IsStackable())
        {

            dictionaryForStacking.Add(bd.id, bd);
        }

        

        //then we apply the thing.
        switch (bd.bdType)
        {
            case BDType.Stat:
                AddBDStat(bd);
                break;

            case BDType.Damage:
                //we dont call it now but we add and everytime it wants to do its thing it does. 
                break;

            case BDType.Stun:
                AddBDStun();
                break;

            case BDType.Immune:
                AddBDImmune();
                break;
            case BDType.Invisible:
                AddBDInvisibility();
                break;

            case BDType.SecretBulletMultipler:
                PlayerHandler.instance._playerCombat.MakeSecretStatMultipleBulletPercent(1);
                break;
        }


        if (gameObject.tag == "Player")
        {
            bd.CreateBDUnit();
        }
        else
        {

            bd.CreateBDUnit(_entityCanvas);
        }


        if (bd.IsTick())
        {
            //we add to the ticklist.
            tickList.Add(bd);
            return;
        }

        if (bd.IsTemp())
        {
            tempList.Add(bd);
        }
        else
        {
            permaList.Add(bd);
        }

        //the bd will have a permission.
        //we always ask the bd to build.
       
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
        //we add the damage. but there is nothing to do.
    }
    void AddBDStun()
    {

        isStunned = true;

        if(_entityCanvas != null) 
        {
            _entityCanvas.ControlStunned(true);
        }
    }

    void AddBDInvisibility()
    {
        IsInvisible = true;
    }
    void AddBDImmune()
    {
        IsImmune = true;
    }



    public void RemoveBdWithID(string id)
    {

        for (int i = 0; i < tempList.Count; i++)
        {
            var item = tempList[i];
            if (item.id == id)
            {
                //if its the same then we must remove this fella.
                RemoveBDWithIndex(i, tempList);
                break;
            }
        }
        for (int i = 0; i < permaList.Count; i++)
        {
            var item = permaList[i];
            if (item.id == id)
            {
                //if its the same then we must remove this fella.
                RemoveBDWithIndex(i, permaList);
                break;
            }
        }
    }

    public void RemoveBDWithIndex(int index, List<BDClass> targetList)
    {
        BDClass bd = targetList[index];
        targetList.RemoveAt(index);
        bd.RemoveBDUnit();

        if (dictionaryForStacking.ContainsKey(bd.id))
        {
            dictionaryForStacking.Remove(bd.id);
        }

        switch (bd.bdType)
        {
            case BDType.Stat:
                RemoveStat(bd);
                break;

            case BDType.Damage:
                //we dont call it now but we add and everytime it wants to do its thing it does.
                //AddBDDamage(bd);

                break;

            case BDType.Stun:
                RemoveStun();
                break;

            case BDType.Immune:
                if (!HasAnotherOfType(BDType.Immune)) IsImmune = false;
                break;
            case BDType.Invisible:
                if(!HasAnotherOfType(BDType.Invisible)) IsInvisible = false;
                break;
            case BDType.SecretBulletMultipler:
                PlayerHandler.instance._playerCombat.MakeSecretStatMultipleBulletPercent(0);
                break;
        }

    }

    void RemoveStat(BDClass bd)
    {
        float flatValue = bd.statValueFlat;
        statAlteredDictionary[bd.statType] -= flatValue;

        float basePercentValue = statBaseDictionary[bd.statType] * bd.statValuePercentbasedOnBaseValue;
        statAlteredDictionary[bd.statType] -= basePercentValue;

        _entityEvents.OnUpdateStat(bd.statType, GetTotalValue(bd.statType));
    }
    
    void RemoveStun()
    {
        if (HasAnotherOfType(BDType.Stun)) return;
        
        isStunned = false;
        if (_entityCanvas != null)
        {
            _entityCanvas.ControlStunned(false);
        }
    }
    void RemoveInvisibility()
    {

    }

    bool HasAnotherOfType(BDType bd)
    {
        foreach (var item in tempList)
        {
            if (item.bdType == bd) return true;
        }
        foreach (var item in permaList)
        {
            if (item.bdType == bd) return true;
        }
        return false;
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

    #region DEBUG
    [ContextMenu("DEBUG STUN")]
    public void DebugStun()
    {
        BDClass bd = new BDClass("DebugStun", BDType.Stun, 3.5f);
        AddBD(bd);
    }

    [ContextMenu("DEBUG BLEED")]
    public void DebugBleed()
    {
        IDamageable damageable = GetComponent<IDamageable>();   
        BDClass bd = new BDClass("DebugBleed", BDDamageType.Bleed, damageable, 1, 5, 1.5f);
        bd.MakeStack(50, true);
        AddBD(bd);
    }

    public void DebugSpeed()
    {
        BDClass bd = new BDClass("DebugSpeed", StatType.Speed, 0, 0.5f, 0);
        bd.MakeTemp(5);
        AddBD(bd);
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


    public void CallDodgeFade()
    {
        if (_entityCanvas == null) return;
        _entityCanvas.CreateFadeUIForDodge();
    }

}

//it can rece

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
    DamageBack,
    Dodge



}