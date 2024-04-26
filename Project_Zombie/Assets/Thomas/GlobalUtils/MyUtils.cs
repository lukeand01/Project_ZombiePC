
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
            StatType.DamageReduction,
            StatType.Tenacity,
            StatType.Pen,
            StatType.CritDamage,
            StatType.CritChance,
            StatType.ReloadSpeed,
            StatType.Magazine,
            StatType.FireRate,
            StatType.SkillCooldown,
            StatType.SkillDamage,
            StatType.Luck,
            StatType.Vampirism,

        };
    }

    public static List<StatType> GetStatForGunListRef()
    {
        return new List<StatType>()
        {
            StatType.Damage,
            StatType.FireRate,
            StatType.Pen,
            StatType.ReloadSpeed,
            StatType.Magazine,
            StatType.CritDamage,
            StatType.CritChance,

        };
    }


    public static int GetSecondPassiveModifier(int level)
    {
        int value = 0;
        int progress = 0;
        for (int i = 0; i < level; i++)
        {
            progress++;

            if(progress >= 3)
            {
                progress = 0;
                value++;
            }
        }

        return value;
    }

    //this works for everyone.
    public static float[] GetChanceForTierBasedInLevel(int level)
    {
        List<float> chanceTierList = new();

        switch (level)
        {
            case 0:

                chanceTierList = new()
                {
                    90,
                    10,
                    0,
                    0
                };

                break;

            case 1:
                chanceTierList = new()
                {
                    80,
                    20,
                    0,
                    0
                };

                break;

            case 2:
                chanceTierList = new()
                {
                    65,
                    35,
                    0,
                    0
                };

                break;

            case 3:
                chanceTierList = new()
                {
                    40,
                    55,
                    5,
                    0
                };

                break;

            case 4:
                chanceTierList = new()
                {
                    30,
                    60,
                    10,
                    0
                };

                break;

            case 5:
                chanceTierList = new()
                {
                    10,
                    65,
                    15,
                    0
                };

                break;
            case 6:
                chanceTierList = new()
                {
                    5,
                    70,
                    25,
                    0
                };

                break;

            case 7:
                chanceTierList = new()
                {
                    0,
                    60,
                    40,
                    0
                };

                break;

            case 8:
                chanceTierList = new()
                {
                    0,
                    40,
                    50,
                    10
                };

                break;

            case 9:
                chanceTierList = new()
                {
                    0,
                    15,
                    60,
                    25
                };

                break;

            case 10:
                chanceTierList = new()
                {
                    0,
                    0,
                    65,
                    35
                };

                break;

            default:

                Debug.Log("what happeend tot the level?");

                break;
        }


        float[] value = new float[4] { chanceTierList[0], chanceTierList[1], chanceTierList[2], chanceTierList[3]};





        return value;

    }

    


}
