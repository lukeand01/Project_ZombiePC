using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerResources : MonoBehaviour, IDamageable
{
    PlayerHandler handler;


    float healthCurrent;
    float healthTotal;

    string id;
    bool isDead;
    private void Awake()
    {
        id  =Guid.NewGuid().ToString();
        handler = GetComponent<PlayerHandler>();    
    }
    private void Start()
    {
        healthTotal = handler._entityStat.GetTotalValue(StatType.Health);
        healthCurrent = healthTotal;
        SetPoints(0);

        UIHandler.instance._playerUI.ForceUpdateHealth(healthCurrent, healthTotal);
    }

    public string GetID()
    {
        return id;
    }

    public bool IsDead()
    {
        return isDead;
    }

    public void TakeDamage(DamageClass damage)
    {
        bool isCrit = damage.CheckForCrit();

        float reduction = handler._entityStat.GetTotalValue(StatType.DamageReduction);
        float totalHealth = handler._entityStat.GetTotalValue(StatType.Health);


        float damageValue = damage.GetDamage(reduction, totalHealth, isCrit);

        healthCurrent -= damageValue;


        if (healthCurrent <= 0)
        {
            //death
            isDead = true;
        }

    }

    public void ApplyBD(BDClass bd)
    {
        handler._entityStat.AdBD(bd);
    }


    #region POINTS
    public int points { get; private set; } 

    public void SetPoints(int value)
    {
        points = value;
        UIHandler.instance._playerUI.ForceUpdatePoint(points);
    }

    public void GainPoints(int value)
    {
        points += value;
        UIHandler.instance._playerUI.UpdatePoint(points, value);
    }
    public void SpendPoints(int value)
    {
        points -= value;
        UIHandler.instance._playerUI.UpdatePoint(points, -value);
    }

    public bool CanSpendPoints(int value)
    {
        return points >= value;
    }

    #endregion
}
