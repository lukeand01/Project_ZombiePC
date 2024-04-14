using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStat : MonoBehaviour
{
    //THIS WILL TAKE CARE

    [SerializeField] List<StatClass> debugInitialList = new();

    private void Awake()
    {
        if(debugInitialList.Count > 0) 
        {
            SetUp(debugInitialList);
        
        }
    }


    public bool isStunned { get; private set; }

    protected Dictionary<StatType, float> statBaseDictionary = new Dictionary<StatType, float>();
    protected Dictionary<StatType, float> statAlteredDictionary = new Dictionary<StatType, float>();

    public void SetUp(List<StatClass> initialStatList)
    {
        List<StatType> refList = MyUtils.GetStatListRef();


        foreach (var item in initialStatList)
        {
            statBaseDictionary.Add(item.stat, item.value);
        }

        foreach (var item in refList)
        {
            if (!statBaseDictionary.ContainsKey(item))
            {
                statBaseDictionary.Add(item, 0);
            }

            statAlteredDictionary.Add(item, 0);
        }
    }

    public void SetUpWithScalingList(List<StatClass> initialStatList, List<StatClass> scalingStatList)
    {

    }


    #region BD

    [SerializeField] List<BDClass> tempList = new();
    [SerializeField] List<BDClass> permaList = new();
    [SerializeField] List<BDClass> tickList = new();
    Dictionary<string, BDClass> dictionaryForStacking = new Dictionary<string, BDClass>();

    void HandleBD()
    {

    }

    public void AdBD()
    {

 
    }


    #endregion


    #region GETTING VALUES


    public float GetTotalValue(StatType stat)
    {
        return statBaseDictionary[stat] + statAlteredDictionary[stat];
    }

    #endregion

}

[System.Serializable]
public class StatClass
{
    public StatClass(StatType stat, float value)
    {
        this.stat = stat;
        this.value = value;
    }

    public StatType stat;
    public float value;
}

public enum StatType 
{ 
    Health,
    Speed,
    Damage,
    Resistance,
    Tenacity,


}