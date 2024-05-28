

using System;
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
            StatType.DamageBack,
            StatType.Dodge,
            StatType.ElementalPower, 
            StatType.ElementalChance

        };
    }

    public static List<ItemResourceType> GetResourceListRef()
    {
        return new List<ItemResourceType>()
        {
            ItemResourceType.Food,
            ItemResourceType.Iron,
            ItemResourceType.Copper,
            ItemResourceType.Eletrical_Components,
            ItemResourceType.Uranium,
            ItemResourceType.Rare_Cristals,
            ItemResourceType.Zyo,
            ItemResourceType.Anti_Matter
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
                    0,
                };

                break;

            case 1:
                chanceTierList = new()
                {
                    80,
                    20,
                    0,
                    0,
                };

                break;

            case 2:
                chanceTierList = new()
                {
                    65,
                    35,
                    0,
                    0,
                };

                break;

            case 3:
                chanceTierList = new()
                {
                    40,
                    55,
                    5,
                    0,
                };

                break;

            case 4:
                chanceTierList = new()
                {
                    30,
                    60,
                    10,
                    0,
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


        //the last one is for the especial items.
        float[] value = new float[5] { chanceTierList[0], chanceTierList[1], chanceTierList[2], chanceTierList[3], 4 + level};





        return value;

    }

    public static string GetDescriptionForBD(BDClass bd)
    {

        if(bd.bdType == BDType.Damage)
        {
            return $"Deals damage for {bd.tickTotal} ticks";
        }

        if(bd.bdType == BDType.Stat)
        {
            string first = bd.GetFirstForStat();
            string second = bd.GetSecondForStat();
            return $"{first} the stat {bd.statType} {second}";
        }

        if(bd.bdType == BDType.Immune)
        {
            return "The player is immune to any damage for the duration";
        }

        if (bd.bdType == BDType.Stun)
        {
            return "The player cannot do anything for the duration";
        }

        return "";

    }

    public static string GetStatDescription(StatType stat)
    {
        switch(stat)
        {
            case StatType.Health:
                return "This represents how much damage you can take. once it reaches 0 the player dies";
            case StatType.Damage:
                return "This influences your damage done by weapons and certain abilities.";
            case StatType.SkillDamage:
                return "This influences all of your spells. it increases as a percent";
            case StatType.CritDamage:
                return "Influnces the additional damage you deal when you crit with spell or gun";
            case StatType.DamageBack:
                return "Every amount of damage dealt to the player, to the health or shield, is dealt back to the dealer.";
            case StatType.CritChance:
                return "Increases your chance to crit with guns or spells";
            case StatType.DamageReduction:
                return "Its a percent of how much damage is reduced. it goes up to 90%";
            case StatType.FireRate:
                return "Influences how fast you shoot guns and some spells";
            case StatType.Luck:
                return "Influences the chance to get items of higher tier, be them guns, abilities or resources";
            case StatType.Magazine:
                return "Influence the size of gun magazine";
            case StatType.ReloadSpeed:
                return "Influences how fast you reload";
            case StatType.Speed:
                return "Influences how fast you move";
            case StatType.Tenacity:
                return "Influences your resistance against Debuffs";
            case StatType.Pen:
                return "Influences how much you ignore of damage reduction";
            case StatType.Vampirism:
                return "Influences how much you heal from all the damage you deal";
            case StatType.ElementalPower:
                return "Increases the damage dealt by bleeding or burning";
            case StatType.ElementalChance:
                return "Increases the chance of applying bleeding or burning";

        }

        return "";
    }


    public static List<StatTrackerType> GetStatTrackerRefList()
    {
        return new List<StatTrackerType>()
        {
            StatTrackerType.TimeSpent,
            StatTrackerType.EnemiesKilled,
            StatTrackerType.PointsGained,
            StatTrackerType.PassiveAbilitiesFound,
            StatTrackerType.GunChestsUsed,
            StatTrackerType.ResourceChestsFound,
            StatTrackerType.DamageTaken,
            StatTrackerType.DamageDealt
        };
    }

    public static int GetSpawnQuantityBasedInRound(int level)
    {
        if (level > 0 && level <= 5)
        {
            return level;
        }
        if (level > 5 && level <= 10)
        {
            return 5;
        }
        if (level > 10 && level <= 15)
        {
            return (int)(level * 0.5f);
        }
        if (level > 20)
        {
            return 15;
        }

        return 0;
    }
    public static float GetSpawnTimerBasedInRound(int level)
    {
        if(level > 0 && level <= 5)
        {
            return UnityEngine.Random.Range(3, 4);
        }
        if (level > 5 && level <= 10)
        {
            return UnityEngine.Random.Range(2, 3);
        }
        if (level > 10 && level <= 20)
        {
            return UnityEngine.Random.Range(1.5f, 2);
        }
        if (level > 20)
        {
            return UnityEngine.Random.Range(0.8f, 1f);
        }

        return 1;
    }



    public static float GetTimerForRoundTotal(int level)
    {
              
        if(level > 0 && level <= 5)
        {
            return 10 + (level * 1.5f);
        }

        if (level > 5 && level <= 15)
        {
            return 25 + (level * 2);
        }

        if (level > 15)
        {
            return 35 + (level * 2.5f);
        }

        return level * 5;
    }
}
