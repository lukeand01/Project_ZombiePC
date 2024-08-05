using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPassiveData : AbilityBaseData
{

    //do i want to create a passive for each thing? 
    [Separator("PASSIVE")]
    [SerializeField] protected float _firstValue;
    [SerializeField] protected float _secondValue;   

    public virtual void Add(AbilityClass ability)
    {
        
    }
    public virtual void Remove(AbilityClass ability) 
    { 
    
    }

    public virtual void Call(AbilityClass ability)
    {

    }

    protected void AddBDToPlayer(BDClass bd)
    {
        PlayerHandler.instance._entityStat.AddBD(bd);   
    }
    protected void RemoveBDFromPlayer(string id)
    {
        PlayerHandler.instance._entityStat.RemoveBdWithID(id);
    }

    //i can use bullet behavior

    protected float GetFirstValue(List<AbilityPassiveData> stackList)
    {
        //add the first fella that is itself.
        float value = 0;

        foreach (var item in stackList)
        {
            value += item._firstValue;
        }

        return value;
    }
    protected float GetSecondValue(List<AbilityPassiveData> stackList)
    {
        float value = _secondValue;

        foreach (var item in stackList)
        {
            if(item._secondValue > value)
            {
                value = item._secondValue;
            }
        }


        int level = stackList.Count;
        int secondSkillModifier = MyUtils.GetSecondPassiveModifier(level);


        if(secondSkillModifier == 0)
        {
            return 0;
        }

        float secondSkillValue = secondSkillModifier * value;


        return secondSkillValue;
    }

    public virtual List<float> GetBaseValues()
    {
        return new List<float>() { _firstValue, _secondValue};
    }



    public virtual bool IsCursed() => false;
    public override AbilityPassiveData GetPassive() => this;   
}


//we can ad stuff and just handle stuff somewhere else.
//