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
        ResetPlayerResource();

        handler._entityEvents.eventUpdateStat += UpdateStat;
    }

    public void ResetPlayerResource()
    {
        isDead = false;
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

    void UpdateStat(StatType _stat, float _value)
    {
        if(_stat == StatType.Health)
        {

            healthTotal = _value;
            UIHandler.instance._playerUI.UpdateHealth(healthCurrent, healthTotal);
        }
    }

   


    public void TakeDamage(DamageClass damage)
    {
        if(handler._entityStat.IsImmune)
        {

            return;
        }

        //we give damage back.
        //we check for dodge. in dodge we also announce.

        CheckDamageBack(damage);

        if (CheckDodge())
        {
            //we ignore the damage and announce the dodge.
            handler._entityStat.CallDodgeFadeUI();
            handler._entityEvents.OnHasDodged();
            return;
        }

        handler._entityEvents.OnHardInput();

        bool isCrit = damage.CheckForCrit();

        float reduction = handler._entityStat.GetTotalValue(StatType.DamageReduction);

        Debug.Log("this is the player reduction " + reduction);

        float totalHealth = handler._entityStat.GetTotalValue(StatType.Health);


        float damageValue = damage.GetDamage(reduction, totalHealth, isCrit);

        
       
        handler._playerStatTracker.ChangeStatTracker(StatTrackerType.DamageTaken, damageValue);

        float damageAfterShield = handler._playerCombat.ShieldReduceDamage(damageValue);

        if (damageAfterShield == 0) return;

       healthCurrent -= damageAfterShield;
        UIHandler.instance._playerUI.UpdateHealth(healthCurrent, healthTotal);

        if (healthCurrent <= 0)
        {
            //death

            Die();
        }

    }

    public void RecoverHealth(float value)
    {
        //call a pop 

        handler._entityStat.CallRecoverHealthFadeUI(value);
        healthCurrent += value;
        healthCurrent = Mathf.Clamp(healthCurrent, 0, healthTotal);
        UIHandler.instance._playerUI.UpdateHealth(healthCurrent, healthTotal);
    }


    void Die()
    {
        isDead = true;

        //stop timer
        //stop round

        UIHandler.instance._EndUI.StartDefeatUI();


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

        
        int roll = UnityEngine.Random.Range(0, 80);
        dodgeChance = Math.Clamp(dodgeChance, 0, 60);


        return dodgeChance >= roll;
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
    public float GetTargetCurrentHealth()
    {
        return healthCurrent;
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

        handler._playerStatTracker.ChangeStatTracker(StatTrackerType.PointsGained, value);
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
