using MyBox;
using System;
using System.Collections;
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


        Bless_Set(initialBless);
    }

    public void ResetPlayerResource()
    {
        isDead = false;
        healthTotal = handler._entityStat.GetTotalValue(StatType.Health);
        healthCurrent = healthTotal;
        SetPoints(startingPoints);

        UIHandler.instance._playerUI.ForceUpdateHealth(healthCurrent, healthTotal);

        hasRevive = false;
        alreadyRevived = false;

        PlayerHandler.instance._rb.useGravity = true;
    }

    #region DEBUG
    [ContextMenu("DEBUG TAKE DAMAGE")]
    public void Debug_TakeDamage()
    {
        TakeDamage(new DamageClass(90));
        
    }

    [ContextMenu("DEBUG HEAL")]
    public void Debug_Heal()
    {
        RecoverHealth(30);
    }
    #endregion


    #region DAMAGEABLE

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

        if (CheckDodge() && !damage.cannotBeDodged)
        {
            //we ignore the damage and announce the dodge.
            handler._entityStat.CallDodgeFadeUI();
            handler._entityEvents.OnHasDodged();
            return;
        }

        handler._entityEvents.OnHardInput();


        bool isCrit = damage.CheckForCrit();

        float reduction = handler._entityStat.GetTotalValue(StatType.DamageReduction);


        float totalHealth = handler._entityStat.GetTotalValue(StatType.Health);


        float damageValue = damage.GetDamage(reduction, totalHealth, isCrit);

        

        handler._playerStatTracker.ChangeStatTracker(StatTrackerType.DamageTaken, damageValue);

        float damageAfterShield = handler._playerCombat.ShieldReduceDamage(damageValue);

        if (damageAfterShield == 0) return;

        healthCurrent -= damageAfterShield;
        healthCurrent = Mathf.Clamp(healthCurrent, 0, healthTotal);
        UIHandler.instance._playerUI.UpdateHealth(healthCurrent, healthTotal);
        handler._entityEvents.OnDamageTaken();

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
        handler._entityEvents.OnHealed();
    }


    public void Die(bool hasFallen = false)
    {
        if (hasRevive && !hasFallen)
        {
            CallRevive();
            return;
        }


        isDead = true;
        //stop timer
        //stop round


        UIHandler.instance._EndUI.StartDefeatUI();
        PlayerHandler.instance._rb.velocity = Vector3.zero;
        PlayerHandler.instance._rb.useGravity = false;
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
        float dodgeChance = handler._entityStat.GetTotalValue(StatType.Dodge) + handler._entityStat.GetTotalValue(StatType.Luck);

        
        int roll = UnityEngine.Random.Range(0, 80);
        dodgeChance = Math.Clamp(dodgeChance, 0, 60);


        return dodgeChance >= roll;
    }

    void CheckDamageBack(DamageClass damage)
    {

        if (damage.attacker == null) return;
        float damageBackValue = handler._entityStat.GetTotalValue(StatType.DamageBack);
        damageBackValue *= 0.01f;
        damageBackValue = damageBackValue.Clamp(0, 0.9f);


        if (damageBackValue <= 0) return;
        


        float damageBack = damage.baseDamage * damageBackValue;
        damage.attacker.TakeDamage(new DamageClass(damageBack));


    }
    public float GetTargetCurrentHealth()
    {
        return healthCurrent;
    }
    #endregion

    #region REVIVE
    bool hasRevive;
    bool alreadyRevived;
    public void AddRevive()
    {
        if (alreadyRevived) return;
        hasRevive = true;
    }
    public void RemoveRevive()
    {
        hasRevive = false;
    }
    public void CallRevive()
    {
        alreadyRevived = true;
        hasRevive = false;

        isDead = true; //we say this so enemies will stop chasing

        handler._playerController.block.AddBlock("Revive", BlockClass.BlockType.Complete);

        StartCoroutine(CallReviveProcess());
    }

    IEnumerator CallReviveProcess()
    {

        Debug.Log("revive started");
        yield return new WaitForSecondsRealtime(2);


        RecoverHealth(healthTotal * 0.25f);

        handler._playerController.block.RemoveBlock("Revive");
        BDClass bd = new BDClass("Revive", BDType.Immune, 1.5f);
        handler._entityStat.AddBD(bd);

    }

    //revive will work as follow
    //it dies and there is a cooldown timer. it deals massive damage around
    //grants the player a short immunity
    //and then gives life back.
    #endregion

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

    public bool HasEnoughPoints(int value)
    {
        return points >= value;
    }



    #endregion

    #region BLESS
    [Separator("BLESS")]
    [SerializeField] int initialBless;
    public int bless { get; private set; }

    public void Bless_Set(int value)
    {
        bless = value;
        UIHandler.instance._playerUI.Bless_ForceUpdate(bless);
    }
    public void Bless_Gain(int value)
    {
        bless += value;
        UIHandler.instance._playerUI.UpdateBless(bless, value);
    }
    public void Bless_Lose(int value)
    {
        bless -= value;
        bless = Mathf.Clamp(bless, 0, 9999);
        UIHandler.instance._playerUI.UpdateBless(bless, value);     
    }
    public bool Bless_HasEnough(int value)
    {
        return bless >= value;
    }

    [ContextMenu("Bless")]
    public void DebugBless()
    {
        Bless_Gain(10);
    }

    #endregion

    #region QUEST SYSTEM

    public void AddQuest(QuestClass quest)
    {
        //we show in the thing and we update everytime.



    }



    #endregion

}
