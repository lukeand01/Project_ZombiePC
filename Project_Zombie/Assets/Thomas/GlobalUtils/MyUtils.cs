using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyUtils 
{
    
    public static List<StatType> GetStatListRef()
    {
        return new List<StatType>()
        {
            StatType.Health, 
            StatType.Speed,
            StatType.Damage,
            StatType.Resistance,
            StatType.Tenacity

        };
    }

}
