using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : Tree, IDamageable
{

    [SerializeField] protected EnemyData data;
    public EnemyData GetData() { return data; }


    [SerializeField] EnemyCanvas _enemyCanvas;
    [SerializeField] GameObject head;
    EntityEvents _entityEvents; //these are just a bunch of events that might interest this entity.
    EntityStat _entityStat; //

    ChestAbility _chestAbility; //if you have it

    float healthCurrent;
    float healthTotal;

    


    private void Awake()
    {

        AwakeFunction();

    }

    protected override void UpdateFunction()
    {

        if (_entityStat.isStunned)
        {
            StopAgent();
            return;
        }
        base.UpdateFunction();
    }

    protected virtual void AwakeFunction()
    {

        groundLayer |= (1 << 10);

        id = Guid.NewGuid().ToString();

        agent = GetComponent<NavMeshAgent>();   

        SetEntity();
        SetStats(5);

        SetSpeed(_entityStat.GetTotalValue(StatType.Speed));

        healthTotal = _entityStat.GetTotalValue(StatType.Health);
        healthCurrent = healthTotal;

        _enemyCanvas.UpdateHealth(healthCurrent, healthTotal);

        gameObject.name = data.name + "; Unique_ID: " + id;

    }

    private void Start()
    {
        
    }

    

    public void SetChest(ChestAbility chestAbilityTemplate)
    {
        _chestAbility = chestAbilityTemplate;
    }

    #region STAT ENTITY

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

    #endregion

    #region  DAMAGEABLE
    string id;
    bool isDead;
    LayerMask groundLayer;

    public void ApplyBD(BDClass bd)
    {
        _entityStat.AddBD(bd);
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

        _enemyCanvas.UpdateHealth(healthCurrent, healthTotal);


        if(healthCurrent <= 0)
        {
            //death
            Die();
        }
        else
        {
            PlayerHandler.instance._playerResources.GainPoints(1);
        }


    }

    void Die()
    {
        isDead = true;
        PlayerHandler.instance._playerResources.GainPoints(5);

        if(_chestAbility != null)
        {
            //we spawn at the exact position
            //we are goingt to raycast down to get the 
          
            RaycastHit hit;

           bool success = Physics.Raycast(head.transform.position, Vector3.down , out hit, 250, groundLayer);


            if(success && hit.collider != null)
            {
                Instantiate(_chestAbility, hit.point, Quaternion.identity);
            }
            else
            {
                Debug.Log("yo");
            }

        }

        //when this fella dies we remove the the canvas 
        _enemyCanvas.transform.SetParent(null); //no parent. but it should be ordered to destroy itself once there are no more childnre in the damagepopcontainer.
        _enemyCanvas.MakeDestroyItself(transform);
        Destroy(gameObject);
    }

    public float GetTargetMaxHealth()
    {
        return _entityStat.GetTotalValue(StatType.Health);
    }
    #endregion


    #region PATHING
    NavMeshAgent agent;



    void SetSpeed(float value)
    {
        //we update this if we slowed.
        agent.speed = value;    
    }

    public void SetDestinationForPathfind(Vector3 targetPos)
    {
        agent.isStopped = false;
        agent.destination = targetPos;
    }

    public void StopAgent()
    {
        agent.isStopped = true;
    }

    #endregion

    #region TARGETTING

    //
    public IDamageable targetIdamageable { get; private set; }
    public GameObject targetObject {  get; private set; }

    public void SetNewtarget(GameObject target)
    {
        //the only problem is that its not reliable to use gameobject id.

        if(targetObject != null)
        {
            if(targetObject.name == target.name)
            {
                return;
            }
        }

        targetObject = target;  
        targetIdamageable = target.GetComponent<IDamageable>(); 

    }
    
    public virtual void CallAttack()
    {
        if(targetObject == null)
        {
            return;
        }

        if (targetIdamageable == null)
        {
            return;
        }

        if (targetIdamageable.IsDead())
        {
            return;
        }


        float distanceForAttack = Vector3.Distance(targetObject.transform.position, transform.position);


        //we will check any fellas in front 

     
        if(data.attackRange >= distanceForAttack) 
        {
            //if the player is still in range then we attack.
            DamageClass damage = GetDamage();
            damage.MakeAttacker(this);
            targetIdamageable.TakeDamage(damage);
        }

    }

    protected DamageClass GetDamage()
    {
        float baseDamage = _entityStat.GetTotalValue(StatType.Damage);
        float pen = _entityStat.GetTotalValue(StatType.Pen);

        return new DamageClass(baseDamage, pen);
    }

    #endregion

    public bool IsStunned()
    {
        return _entityStat.isStunned;
    }

    #region ABILITY INDICATOR

    [SerializeField] protected AbilityIndicatorCanvas _abilityIndicatorCanvas;
    public virtual void CallAbilityIndicator(float current, float total)
    {
        //nothing happens for basic fellas.
        
    }


    #endregion



    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawSphere(transform.position, 5);
    }

    
}

//how should i go about doing this?
//we will have a script for each enemy. its easier to do stuff like that.
//each fella will choose their behaviors for the behavior tree.

//ENEMIES
//simple melee will be the main. then we will simply have different versions that will have different stats. enemies have weak spots.
//Tanker. big and slow.
//charger. aims and charges forward. can only hit from behind. infront requires a certain amount of pen to go through.
//Mage. creates spell damages. so you need to keep moving.
//Hound. just a faster enemy that will try to flank the player.


//all these spells should be shows as lines in the floor.
//just create an image that is placed in the floor. cannot go through wall and is affected by spell range. alos will have a fill function to show when the enemy will attack.
