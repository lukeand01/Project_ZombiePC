using MyBox;
using System;
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
        SetPoints(startingPoints);

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
        if(handler._entityStat.IsImmune)
        {
            Debug.Log("its immune to damage");
            return;
        }

        //we give damage back.
        //we check for dodge. in dodge we also announce.

        CheckDamageBack(damage);

        if (CheckDodge())
        {
            //we ignore the damage and announce the dodge.
            handler._entityStat.CallDodgeFade();
            return;
        }
        


        bool isCrit = damage.CheckForCrit();

        float reduction = handler._entityStat.GetTotalValue(StatType.DamageReduction);
        float totalHealth = handler._entityStat.GetTotalValue(StatType.Health);


        float damageValue = damage.GetDamage(reduction, totalHealth, isCrit);

        healthCurrent -= damageValue;
        UIHandler.instance._playerUI.UpdateHealth(healthCurrent, healthTotal);

        if (healthCurrent <= 0)
        {
            //death
            isDead = true;
        }

    }

    public float GetTargetMaxHealth()
    {
        return handler._entityStat.GetTotalValue(StatType.Health);
    }
    public void ApplyBD(BDClass bd)
    {
        handler._entityStat.AddBD(bd);
    }

    bool CheckDodge()
    {
        float dodgeChance = handler._entityStat.GetTotalValue(StatType.Dodge);
        int roll = UnityEngine.Random.Range(0, 101);
        dodgeChance = Math.Clamp(dodgeChance, 0, 70);

        return dodgeChance > roll;
    }

    void CheckDamageBack(DamageClass damage)
    {
        float damageBackValue = handler._entityStat.GetTotalValue(StatType.DamageBack);
        damageBackValue *= 0.01f;
        damageBackValue = damageBackValue.Clamp(0, 0.9f);



        if (damageBackValue <= 0) return;
        if (damage.attacker == null) return;


        float damageBack = damage.baseDamage * damageBackValue;
        damage.attacker.TakeDamage(new DamageClass(damageBack));


    }


    #region POINTS
    public int points { get; private set; }
    [SerializeField] int startingPoints;
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
