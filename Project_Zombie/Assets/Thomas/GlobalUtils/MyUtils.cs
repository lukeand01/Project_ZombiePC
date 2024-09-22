

using System;
using System.Collections.Generic;
using UnityEngine;

public static class MyUtils 
{

    //it should show the base stat and when you increase 
    //also there should be a cap limit.
    
    public static List<StatType> GetStatListRef()
    {
        return new List<StatType>()
        {
            StatType.Health, 
            StatType.Speed,
            StatType.Damage,
            StatType.DamageReduction,
            StatType.Leadership,
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

    //i shouldnt be able to improve all stats. 

    public static DamageType GetDamageTypeFromBDDamageType(BDDamageType _type) 
    {
        
            switch(_type)
            {
                case BDDamageType.Burn:
                    return DamageType.Magical;
                case BDDamageType.Bleed:
                    return DamageType.Physical;
            }

            return DamageType.Physical;
        } 


    public static List<StatType> GetStatRefList_ForBodyEnhancer()
    {
        return new List<StatType>()
        {
            StatType.Health,
            StatType.Speed,
            StatType.Damage,
            StatType.DamageReduction,
            StatType.Leadership,
            StatType.Tenacity,
            StatType.CritDamage,
            StatType.CritChance,
            StatType.SkillDamage,
            StatType.ElementalPower
        };
    }

    public static List<ItemResourceType> GetResourceListRef()
    {
        return new List<ItemResourceType>()
        {
            ItemResourceType.Food,
            ItemResourceType.Iron,
            ItemResourceType.Eletrical_Component,
            ItemResourceType.Population,
            ItemResourceType.Crystals
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
            StatType.Vampirism

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
        float[] value = new float[5] { chanceTierList[0], chanceTierList[1], chanceTierList[2], chanceTierList[3], 1f};





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

        if (bd.bdType == BDType.Blind)
        {
            return "The player has been blind and cannot see far";
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
            StatTrackerType.DamageDealt_Total
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


    public static List<BDClass> GetGunListToBuffEveryValue(int value)
    {
        //it will buff damage as percent 
        //damage, firespeed, 
        return new List<BDClass>() 
        { 
            new BDClass("GenericBuff", StatType.Damage, 0, value, 0),
            new BDClass("GenericBuff", StatType.Pen, 0, value, 0),
            new BDClass("GenericBuff", StatType.CritChance, 0, value, 0),
            new BDClass("GenericBuff", StatType.CritDamage, 0, value, 0),
            new BDClass("GenericBuff", StatType.FireRate, 0, value, 0),
            new BDClass("GenericBuff", StatType.Magazine, 0, value, 0),
            new BDClass("GenericBuff", StatType.ReloadSpeed, 0, value, 0),
        };

    }


    public static int GetChanceBasedInLuckAndTier(int diff)
    {
        //if its the same then its the highest chance.
        //the lowest the most chance.

        //how the fuck is 99? enough to create three fellas.



        if (diff == 0) //meaing the tier and the luck are the same
        {
            return 50;
        }

        if (diff == 1)
        {
            return 68;
        }
        if (diff == 2)
        {
            return 95;
        }
        if (diff > 2)
        {
            return 99;
        }

        if (diff == -1)
        {
            return 55;
        }
        if (diff == -2)
        {
            return 75;
        }
        if (diff == -3)
        {
            return 98; //then it goes back to being hard.
        }
        if (diff < -3) //this cannot be accessed. 
        {
            return 102; //then it goes back to being hard.
        }

        return 0;
    }


    public static Vector3 NormalizeDirectionToInt(Vector3 vector, float threshold)
    {
        if(threshold > 0)
        {
            return new Vector3(
            Mathf.Abs(vector.x) < threshold ? 0 : Mathf.Sign(vector.x),
            Mathf.Abs(vector.y) < threshold ? 0 : Mathf.Sign(vector.y),
            Mathf.Abs(vector.z) < threshold ? 0 : Mathf.Sign(vector.z)
            );
        }
        else
        {
            return new Vector3(
            Mathf.Sign(vector.x),
            Mathf.Sign(vector.y),
            Mathf.Sign(vector.z)
            );
        }
       
    }


    public static Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180);
        return new Vector3(Mathf.Cos(angleRad), 0, Mathf.Sin(angleRad));
    }

    public static float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;

        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        if (n < 0) n += 360;

        return n;
    }


    public static string GetRandomID()
    {
        return Guid.NewGuid().ToString();
    }


    public static int GetQuantityOfSpawnIntervals(int round)
    {

        return 1;

        if (round >= 3)
        {
            return 4;
        }

        if (round > 5 && round <= 9)
        {
            return 6;
        }
        if (round > 10 && round <= 14)
        {
            return 7;
        }
        if (round > 15 && round <= 19)
        {
            return 8;
        }
        if (round > 20)
        {
            return 10;
        }
        return 3;

    }
    public static int GetAmountToSpawnPerInterval(int round, float spawnRoundModifier)
    {
        int value = 0;

        if (round <= 10)
        {
            // Linear interpolation from 5 at round 1 to 40 at round 10
            value = Mathf.RoundToInt(5 + ((40 - 5) / 9f) * (round - 1));
        }
        else if (round <= 15)
        {
            // Linear interpolation from 40 at round 10 to 80 at round 15
            value = Mathf.RoundToInt(40 + ((80 - 40) / 5f) * (round - 10));
        }
        else if (round <= 25)
        {
            // Exponential interpolation from 80 at round 15 to 150 at round 25
            float startValue = 80;
            float endValue = 150;
            float exponent = (round - 15) / 10f;
            value = Mathf.RoundToInt(startValue * Mathf.Pow((endValue / startValue), exponent));
        }
        else
        {
            // Beyond round 25, use exponential growth based on the last known values
            float startValue = 150;
            float exponent = (round - 25) / 10f;
            value = Mathf.RoundToInt(startValue * Mathf.Pow(1.5f, exponent));
        }


        float additionalSpawnValue = value * spawnRoundModifier;


        return value + (int)additionalSpawnValue;
    }
    public static float GetIntervalBetweenSpawns(int round)
    {

        if (round > 0 && round <= 5)
        {
            return 2;
        }

        if (round > 5 && round <= 10)
        {
            return 1;
        }
        if (round > 10)
        {
            return 0.5f;
        }

        return 0;
    }




    public static Vector3 GetRandomPointInAnnulus(Vector3 center, float minRadius, float maxRadius)
    {
        // Generate a random angle in radians
        float angle = UnityEngine.Random.Range(0f, Mathf.PI * 2);

        // Generate a random distance between the min and max radius
        float distance = UnityEngine.Random.Range(minRadius, maxRadius);

        // Calculate the X and Z coordinates using the angle and distance
        float x = Mathf.Cos(angle) * distance;
        float z = Mathf.Sin(angle) * distance;

        // Return the random point relative to the center position
        return new Vector3(center.x + x, center.y, center.z + z);
    }

}
