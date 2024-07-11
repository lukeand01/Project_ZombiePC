using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CityData / BodyEnhancer")]
public class CityData_BodyEnhancer : CityData
{
    //need a int list 
    //each increase is the same however, increase is different for each
    //we can get the value_Level here.

    List<StatType> statRefList = new();
    [SerializeField]List<int> indexList = new(); //they should be created based in the statreflist. each item holds 

    [field: SerializeField] public List<StatClass> currentStatAlteredList { get; private set; } = new();


    //there are the all the stat.
    //we will use that list to ref the valuye for the stuff.


    public void Initalize()
    {
        statRefList = MyUtils.GetStatRefList_ForBodyEnhancer();
        
        if(indexList.Count != statRefList.Count || indexList.Count == 0)
        {

            indexList.Clear();

            foreach (var item in statRefList)
            {
                indexList.Add(0);
            }

        }

        GenerateList();

    }



    public void IncreaseStat(StatType stat)
    {
        for (int i = 0; i < statRefList.Count; i++)
        {
            if (statRefList[i] == stat)
            {
                //then we update the new fella.
                indexList[i] += 1;
                currentStatAlteredList[i].AddToValue(GetValueForStat(stat));
            }
        }


        //everytime we increase the stat.

        GenerateList();
    }

    //so here we have a problem. perpahsp

    void GenerateList()
    {
        Dictionary<StatType, float> statBaseDictionary = PlayerHandler.instance._entityStat.GetStatBaseDictionary;
        currentStatAlteredList.Clear();
        EntityStat statHandler = PlayerHandler.instance._entityStat; 

        for (int i = 0; i < statRefList.Count; i++)
        {
            StatType stat = statRefList[i];
            int level = indexList[i];
            float baseStat = statBaseDictionary[stat];
            float increment = level * GetValueForStat(stat);

            //we give the increment to teh player as bd.

            if(increment > 0)
            {

                //we are going to do stuff
                string id = "Body-" + stat.ToString();
                statHandler.RemoveBdWithID(id);

                BDClass bd = new BDClass(id, stat, increment, 0, 0);
                statHandler.AddBD(bd);

            }


            currentStatAlteredList.Add(new StatClass(stat, baseStat + increment));
        }

    }


   public float GetValueForStat(StatType stat)
    {
        //we get each value_Level.

        switch (stat) 
        {
            case StatType.Health:
                return 5;
            case StatType.Speed:
                return 0.05f;
            case StatType.Damage:
                return 1;
            case StatType.DamageReduction:
                return 0.01f;
            case StatType.Tenacity:
                return 0.1f;
            case StatType.CritDamage:
                return 0.5f;
            case StatType.CritChance:
                return 0.05f;
            case StatType.SkillDamage:
                return 0.1f;
            case StatType.ElementalPower:
                return 0.1f;
        }

        Debug.LogError("should never get here " + stat);
        return -1;
    }

    public int GetLevelForStat(StatType stat)
    {
        for (int i = 0; i < statRefList.Count; i++)
        {
            if (statRefList[i] == stat)
            {
                return indexList[i];
            }
        }

        return -1;
    }

}
