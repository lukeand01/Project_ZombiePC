using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour, IDamageable
{

    [SerializeField] EnemyData data;
    [SerializeField] EnemyCanvas _enemyCanvas;
    EntityEvents _entityEvents; //these are just a bunch of events that might interest this entity.
    EntityStat _entityStat; //

    NavMeshAgent _agent; //this is for movement.

    float healthCurrent;
    float healthTotal;
    private void Awake()
    {
        id = Guid.NewGuid().ToString();


        SetEntity();
        SetStats(5);

        healthTotal = _entityStat.GetTotalValue(StatType.Health);
        healthCurrent = healthTotal;    
    }

    void SetEntity()
    {
        _entityEvents = GetComponent<EntityEvents>();
        if (_entityEvents == null)
        {
            Debug.LogError("THIS ENEMY IS LACKINGT ENTIY EVENTS " + gameObject.name);
        }

        _entityStat = GetComponent<EntityStat>();
        if (_entityStat == null)
        {
            Debug.LogError("THIS ENEMY IS LACKINGT ENTIY STAT " + gameObject.name);
        }
    }
    public void SetStats(int round)
    {
        //so we need to set the stats of each felal here.
        //we will use the data already inside.

        List<StatClass> baseStatList = data.initialStatList;
        List<StatClass> scaleStatList = data.scaleStatList;


        //now we need to put these fellas into the thing.
        _entityStat.SetUpWithScalingList(round, baseStatList, scaleStatList);



    }


    #region  DAMAGEABLE
    string id;
    bool isDead;

    public void ApplyBD(BDClass bd)
    {
        _entityStat.AdBD(bd);
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
        if (isDead) return;

        bool isCrit = damage.CheckForCrit();

        float reduction = _entityStat.GetTotalValue(StatType.DamageReduction);
        float totalHealth = _entityStat.GetTotalValue(StatType.Health);


        float damageValue = damage.GetDamage(reduction, totalHealth, isCrit);

        healthCurrent -= damageValue;
        _enemyCanvas.CreateDamagePopUp(damageValue, DamageType.Physical, isCrit);

        if(healthCurrent <= 0)
        {
            //death
            isDead = true;
        }


    }
    #endregion


}
