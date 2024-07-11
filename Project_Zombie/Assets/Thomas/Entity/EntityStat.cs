using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

    [SerializeField] protected Dictionary<StatType, List<StatAlteredClass>> statAltered_Flat_Dictionary = new();
    [SerializeField] protected Dictionary<StatType, List<StatAlteredClass>> statAltered_PercentBase_Dictionary = new();
    [SerializeField] protected Dictionary<StatType, List<StatAlteredClass>> statAltered_PercentCurrent_Dictionary = new();

    
    public Dictionary<StatType, float> GetStatBaseDictionary { get {  return statBaseDictionary; } }

    //the list for this should be unique for each thing.



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

            statAltered_Flat_Dictionary.Add(item, new List<StatAlteredClass>());
            statAltered_PercentBase_Dictionary.Add(item, new List<StatAlteredClass>());
            statAltered_PercentCurrent_Dictionary.Add(item, new List<StatAlteredClass>());
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
        statAlteredDictionary.Clear();
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


            //we check here before.

           

            if (item.IsTempDone())
            {
                if (item.IsNeverOut())
                {
                    //and we inform to call the even
                    item.CallPassiveEvent();
                    continue;
                }

                if (item.IsStackable() && !item.LastStack())
                {

                    RemoveStat(item);                               
                    item.LoseStack();
                    item.ResetTemp();
                    AddBDStat(item);

                    //i need to inform the player of this update here.

                    continue;
                }
                
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

        //i want a stat

        if (dictionaryForStacking.ContainsKey(bd.id))
        {
            //then we stack
            
            BDClass stackBD = dictionaryForStacking[bd.id];
            RemoveStat(stackBD);
            stackBD.Stack(bd);
            AddBDStat(stackBD);
            Debug.Log("it stacked");
            return;
        }

        //we need to check if we already have this fella. if yes then we 

        if (bd.IsStackable())
        {

            dictionaryForStacking.Add(bd.id, bd);
        }

        //we need to check if we already have this fella.

        bool shouldRefreshInstead = CheckIfShouldRefreshCooldownInstead(bd.id);

        if (shouldRefreshInstead)
        {
            return;
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
     
    }

    bool CheckIfShouldRefreshCooldownInstead(string id)
    {
        //we are only going to check in the temp list for now
        foreach (var item in tempList)
        {
            if (item.id == id)
            {
                item.ResetTemp();
                return true;
            }
        }
        return false;
    }

    float GetValueForAlteredDictionary(StatType _type)
    {
        float value = 0;

        if (!statAltered_Flat_Dictionary.ContainsKey(_type))
        {
            statAltered_Flat_Dictionary.Add(_type, new List<StatAlteredClass>());
        }
        

        List<StatAlteredClass> statAltered_Flat_List = statAltered_Flat_Dictionary[_type];

        foreach (var item in statAltered_Flat_List)
        {
            value += item.value_Original;
        }


        if (!statAltered_PercentBase_Dictionary.ContainsKey(_type))
        {
            statAltered_PercentBase_Dictionary.Add(_type, new List<StatAlteredClass>());
        }

        List<StatAlteredClass> statAltered_PercentBase_List = statAltered_PercentBase_Dictionary[_type];
        float baseValue = statBaseDictionary[_type]; 
        foreach (var item in statAltered_PercentBase_List)
        {
            value += baseValue * item.value_Original;
        }


        if (!statAltered_PercentCurrent_Dictionary.ContainsKey(_type))
        {
            statAltered_PercentCurrent_Dictionary.Add(_type, new List<StatAlteredClass>());
        }

        List<StatAlteredClass> statAltered_PercentCurrent_List = statAltered_PercentCurrent_Dictionary[_type];
        float totalValue = GetTotalValue(_type);
        foreach (var item in statAltered_PercentCurrent_List)
        {
            value += totalValue * item.value_Original;
        }

        return value;
    }

    void AddBDStat(BDClass bd)
    {
        //we instead add to the list.

        if(bd.statValueFlat != 0)
        {
            if (!statAltered_Flat_Dictionary.ContainsKey(bd.statType))
            {
                statAltered_Flat_Dictionary.Add(bd.statType, new List<StatAlteredClass>());
            }
           
            statAltered_Flat_Dictionary[bd.statType].Add(new StatAlteredClass(bd.id, bd.statValueFlat));
        }

        if (bd.statValue_PercentbasedOnBaseValue != 0)
        {
            if (!statAltered_PercentBase_Dictionary.ContainsKey(bd.statType))
            {
                statAltered_PercentBase_Dictionary.Add(bd.statType, new List<StatAlteredClass>());
            }
            statAltered_PercentBase_Dictionary[bd.statType].Add(new StatAlteredClass(bd.id, bd.statValue_PercentbasedOnBaseValue));
        }

        float value = GetValueForAlteredDictionary(bd.statType);

        statAlteredDictionary[bd.statType] = value;
        _entityEvents.OnUpdateStat(bd.statType, GetTotalValue(bd.statType));
        //we update the right dictionary in the right place.


        //we inform the fellas to get the right stuff

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
        //this is flawed because many variable can change and fuck up everything.
        //what we should do i form a list that carries a value_Level and an id.

        if(bd.statValueFlat != 0)
        {
            List<StatAlteredClass> statAltered_Flat_List = statAltered_Flat_Dictionary[bd.statType];

            for (int i = 0; i < statAltered_Flat_List.Count; i++)
            {
                if(bd.id == statAltered_Flat_List[i].id)
                {
                    statAltered_Flat_List.RemoveAt(i);
                }
            }
        }

        if(bd.statValue_PercentbasedOnBaseValue != 0)
        {
            List<StatAlteredClass> statAltered_PercentBase_List = statAltered_PercentBase_Dictionary[bd.statType];

            for (int i = 0; i < statAltered_PercentBase_List.Count; i++)
            {
                if (bd.id == statAltered_PercentBase_List[i].id)
                {
                    statAltered_PercentBase_List.RemoveAt(i);
                }
            }
        }

        float value = GetValueForAlteredDictionary(bd.statType);

        
        statAlteredDictionary[bd.statType] = value;
        _entityEvents.OnUpdateStat(bd.statType, GetTotalValue(bd.statType));


        return;

        float flatValue = bd.statValueFlat;
        statAlteredDictionary[bd.statType] -= flatValue;

        float basePercentValue = statBaseDictionary[bd.statType] * bd.statValue_PercentbasedOnBaseValue;
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
                ModifierClass modifier = new ModifierClass(item.id, "BD", item.statValueFlat, item.statValue_PercentbasedOnBaseValue);
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
        if(stat == StatType.Dodge)
        {
            value = Mathf.Clamp(value, 0, 0.7f);
        }
        
        if(stat == StatType.CritChance)
        {
            value = Mathf.Clamp(value, 0, 100);
        }

        return value;
    }


    #endregion

    #region ESPECIAL CONDITION

    EspecialConditionType _especialConditionType;

    public float GetTotalEspecialConditionValue(EspecialConditionType especialCondition)
    {
        float value = 0;
        foreach (var item in permaList)
        {
            if(item.bdType == BDType.EspecialCondition && item.especialConditionType == especialCondition)
            {
                value += item.statValueFlat;
            }
        }
        foreach (var item in tempList)
        {
            if (item.bdType == BDType.EspecialCondition && item.especialConditionType == especialCondition)
            {
                value += item.statValueFlat;
            }
        }

        return value;
    }

    #endregion

    public void ResetEntityStat()
    {
        //remove all bds 
        for (int i = 0; i < permaList.Count; i++)
        {
            RemoveBDWithIndex(i, permaList);
        }
        for (int i = 0; i < tempList.Count; i++)
        {
            RemoveBDWithIndex(i, tempList);
        }
        for (int i = 0; i < tickList.Count; i++)
        {
            RemoveBDWithIndex(i, tickList);
        }


        
        
    }

    public void CallDodgeFadeUI()
    {
        if (_entityCanvas == null) return;
        _entityCanvas.CreateFadeUIForDodge();
    }
    public void CallRecoverHealthFadeUI(float value)
    {
        if (_entityCanvas == null) return;
        _entityCanvas.CreateFadeUIForRecoverHealth(value);
    }

    public void CallPowerFadeUI(string powerName)
    {
        _entityCanvas.CreateFadeUIForPower(powerName);
    }

    public void CallDropFadedUI(string dropName) => _entityCanvas.CreateFadeUIForDrop(dropName);
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

    public void AddToValue(float newValue)
    {
        initialValue += newValue;
    }

}

[System.Serializable]
public class StatAlteredClass
{

    //its doble for some reason.

    public StatAlteredClass(string id, float value)
    {
        this.id = id;
        value_Original = value;
    }

    public void SetValueAltered(float value_Altered)
    {
        this.value_Altered = value_Altered;
    }

    [field:SerializeField] public string id { get; private set; }
    [field: SerializeField] public float value_Original { get; private set; }
    public float value_Altered { get; private set; }



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
    Dodge,
    ElementalPower, //this affects how strong bleeding or fire is
    ElementalChance //this affects the chance of applying fire and 



}