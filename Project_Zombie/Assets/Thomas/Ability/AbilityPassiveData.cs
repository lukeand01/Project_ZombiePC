using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPassiveData : AbilityBaseData
{

    //do i want to create a passive for each thing? 

    public virtual void Add(AbilityClass ability)
    {

    }
    public virtual void Remove(AbilityClass ability) 
    { 
    
    }

    //i can use bullet behavior


    public override AbilityPassiveData GetPassive() => this;   
}


//we can ad stuff and just handle stuff somewhere else.
//