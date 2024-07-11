using MyBox;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : Tree, IDamageable
{

    [SerializeField] protected EnemyData data;
    public EnemyData GetData() { return data; }


    public Action<EnemyData> eventDied;
    public void OnDied(EnemyData data) => eventDied?.Invoke(data);

    [SerializeField] EnemyCanvas _enemyCanvas;
    [SerializeField] protected GameObject head;
    [SerializeField] EntityEvents _entityEvents; //these are just a bunch of events that might interest this entity.
    [SerializeField] EntityStat _entityStat; //


    float healthCurrent;
    float healthTotal;

    protected Rigidbody _rb;

    public bool isAttacking {  get; private set; }

    public void SetIsAttack(bool isAttacking) => this.isAttacking = isAttacking;

    private void Awake()
    {
        shieldedPenetrationValue = -1;
        _rb = GetComponent<Rigidbody>();
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

        CheckIfShouldBeDespawned();
    }

    protected virtual void AwakeFunction()
    {

        groundLayer |= (1 << 10);

        id = Guid.NewGuid().ToString();

        agent = GetComponent<NavMeshAgent>();   


        _entityEvents.eventUpdateStat += UpdateStat;

        
        gameObject.name = data.name + "; Unique_ID: " + id;

    }

    protected virtual void StartFunction()
    {

        if (!alreadySetStat)
        {
            SetStats(1); //
        }
    }
    
    public void UpdateStat(StatType stat, float value)
    {
        if(stat == StatType.Speed)
        {
            SetSpeed(value);
        }
    }

    private void Start()
    {
        StartFunction();
    }

    public virtual void ResetEnemyForPool()
    {
        gameObject.SetActive(false);
        isDead = false;
        //but also turn on the collider.

        if(_abilityIndicatorCanvas != null)
        {
            _abilityIndicatorCanvas.StopCircleIndicator();
        }
       
    }

    #region STAT ENTITY



    bool alreadySetStat;
    public void SetStats(int round)
    {
        //so we need to set the stats of each felal here.
        //we will use the data already inside.


        alreadySetStat = true;

        List<StatClass> baseStatList = data.initialStatList;
        List<StatClass> scaleStatList = data.scaleStatList;

        


        if(_entityStat == null)
        {
            Debug.Log("something wrong here " + gameObject.name);
        }

        //now we need to put these fellas into the thing.
        _entityStat.SetUpWithScalingList(round, baseStatList, scaleStatList);

        //if its blood moon we give speed an additional value and increase health


        if(LocalHandler.instance != null)
        {
            bool isBloodMoon = LocalHandler.instance.IsBloodMoon;

            if (isBloodMoon)
            {
                BDClass bd_Health = new BDClass("BloodMoon_Health", StatType.Health, 0, 0, 0.25f);
                BDClass bd_Damage = new BDClass("BloodMoon_Damage", StatType.Damage, 0, 0.35f, 0);
                BDClass bd_Speed = new BDClass("BloodMoon_Speed", StatType.Speed, 5, 0, 0);

                _entityStat.AddBD(bd_Health);
                _entityStat.AddBD(bd_Damage);
                _entityStat.AddBD(bd_Speed);
            }


        }


        SetHealth();
        SetSpeed(_entityStat.GetTotalValue(StatType.Speed));
    }

    void SetHealth()
    {
        healthTotal = _entityStat.GetTotalValue(StatType.Health);
        healthCurrent = healthTotal;

        _enemyCanvas.UpdateHealth(healthCurrent, healthTotal);

    }

    #endregion

    #region  DAMAGEABLE
    string id;
    bool isDead;
    LayerMask groundLayer;
    bool isImmuneToExplosion;

    public void ControlEnemyImmunityToExplosion(bool isImmune)
    {
        isImmuneToExplosion = isImmune;
    }

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

    [SerializeField] float shieldedPenetrationValue = -1;
    public void SetShieldedPenetrationValue(float penetrationValue)
    {
        shieldedPenetrationValue = penetrationValue;
    }

    public void TakeDamage(DamageClass damageRef)
    {
        //Debug.Log("took damage");
        if (isDead) return;

       
        if (isImmuneToExplosion && damageRef.isExplosion)
        {
            return;
        }
        if (damageRef.isExplosion)
        {
            Debug.Log("is explosion damage " + gameObject.name);
        }

       if(shieldedPenetrationValue != -1 && damageRef.attacker != null && !damageRef.isExplosion)
        {


            if(damageRef.attacker.GetID() == "Player")
            {
                //actually i want to record the position when the player shot.
                Vector3 damageDirection = (damageRef.lastPos - transform.position).normalized;

                float angle = Vector3.Angle(damageDirection, transform.forward);

                if(angle < 65)
                {
                    Debug.Log("front");

                    //and not call 

                    if (damageRef.pen < shieldedPenetrationValue)
                    {
                        CallShieldPopUp();
                        return;
                    }                  
                    
                }
                else
                {
                    Debug.Log("back");
                }

                
            }

        }
        


        DamageClass damage = new DamageClass(damageRef);


        bool isCrit = damage.CheckForCrit();

        if (isCrit)
        {
            PlayerHandler.instance._entityEvents.OnCrit();
        }

        PlayerHandler.instance._entityEvents.OnDamagedEntity(this, damage);


        float reduction = _entityStat.GetTotalValue(StatType.DamageReduction);
        float totalHealth = _entityStat.GetTotalValue(StatType.Health);

        float damageValue = damage.GetDamage(reduction, totalHealth, isCrit);


        healthCurrent -= damageValue;
        PlayerHandler.instance._playerStatTracker.ChangeStatTracker(StatTrackerType.DamageDealt, damageValue);
        _enemyCanvas.CreateDamagePopUp(damageValue, DamageType.Physical, isCrit);

        _enemyCanvas.UpdateHealth(healthCurrent, healthTotal);
      

        if(healthCurrent <= 0)
        {
            //death
            Die();
            GameHandler.instance._soundHandler.CreateSfx(data.audio_Dead,transform);
        }
        else
        {
            PlayerHandler.instance._playerResources.GainPoints(1);
            GameHandler.instance._soundHandler.CreateSfx(data.audio_Hit, transform);
        }


    }

    public void CallShieldPopUp()
    {
        _enemyCanvas.CreateShieldPopUp();
    }

    protected void Die()
    {
        isDead = true;
        PlayerHandler.instance._playerResources.GainPoints(5);


        LocalHandler.instance.RemoveEnemyFromSpawnList(data);

        HandleWhatDeathShouldDrop();

        PlayerHandler.instance._playerStatTracker.ChangeStatTracker(StatTrackerType.EnemiesKilled, 1);
        PlayerHandler.instance._entityEvents.OnKillEnemy(this);
        OnDied(data);

        //when this fella dies we remove t
        //he the canvas 



        Vector3 posBeforeInde = _enemyCanvas.transform.position;
        _enemyCanvas.transform.SetParent(null); //no parent. but it should be ordered to destroy itself once there are no more childnre in the damagepopcontainer.
        _enemyCanvas.MakeDestroyItself(posBeforeInde);


        GameHandler.instance._pool.Enemy_Release(data, this);
        //Destroy(gameObject);

    }


    void HandleWhatDeathShouldDrop()
    {

        //

        ChestAbility _chestAbility = LocalHandler.instance.GetChestAbility();

        if (_chestAbility != null)
        {
            //we spawn at the exact position
            //we are goingt to raycast down to get the 

            RaycastHit hit;

            bool success = Physics.Raycast(head.transform.position, Vector3.down, out hit, 250, groundLayer);


            if (success && hit.collider != null)
            {
                Instantiate(_chestAbility, hit.point, Quaternion.identity);
            }
            else
            {
                Debug.Log("yo");
            }

            return;

        }

        Chest_Ammo _chestAmmo = LocalHandler.instance.GetChestAmmo();

        if(_chestAmmo != null )
        {
            RaycastHit hit;

            bool success = Physics.Raycast(head.transform.position, Vector3.down, out hit, 250, groundLayer);

            if (success && hit.collider != null)
            {
                Instantiate(_chestAmmo, hit.point, Quaternion.identity);
            }
            else
            {
                Debug.Log("yo");
            }

            return;
        }

        DropData _dropData = GameHandler.instance.cityDataHandler.cityDropLauncher.GetDropData();

        if(_dropData != null)
        {
            //we spawn the box. the spawn box is inside the data.
            RaycastHit hit;

            bool success = Physics.Raycast(head.transform.position, Vector3.down, out hit, 250, groundLayer);

            if (success && hit.collider != null)
            {
                Instantiate(_dropData.dropModel, hit.point, Quaternion.identity);
            }
            else
            {
                Debug.Log("yo");
            }

            return;
        }

        //then we check if the roll was sucess.

        Debug.Log("dropped nothing");
    }
    public float GetTargetMaxHealth()
    {
        return _entityStat.GetTotalValue(StatType.Health);
    }
    #endregion

    #region PATHING
    [SerializeField]protected NavMeshAgent agent;
    protected Vector3 currentAgentTargetPosition;
    protected bool isMoving;

    void SetSpeed(float value)
    {
        //we update this if we slowed.
        agent.speed = value;    
    }

    public void SetDestinationForPathfind(Vector3 targetPos)
    {
        agent.isStopped = false;
        agent.destination = targetPos;

        isMoving = true;
        currentAgentTargetPosition = targetPos;
    }

    public void StopAgent()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;

        isMoving = false;

    }

    #endregion

    #region TARGETTING AND ATTACKING

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
        GameHandler.instance._soundHandler.CreateSfx(data.audio_Attack, transform);

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

    protected bool RotateTarget(Vector3 target, float rotationSpeed = 15, float rotationAngle = 10)
    {
        if (PlayerHandler.instance == null) return false;

        Vector3 direction = target - transform.position;
        Vector3 directionNormalized = direction.normalized;


        Quaternion targetRotation = Quaternion.LookRotation(directionNormalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        return Quaternion.Angle(transform.rotation, targetRotation) <= rotationAngle;
    }


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

    #region VISIBLE FOR CAMERA AND CHECK FOR DESPAWNS IF TOO FAR AWAY
    [Separator("GRAPHIC")]
    [SerializeField] EnemySightedByCameraScript _enemySightedByCamera;

    void CheckIfShouldBeDespawned()
    {
        if (data.shouldNotDespawnBecauseOfDistance) return;
        if (_enemySightedByCamera.IsVisibleByCamera) return;

        Transform playerTransform = PlayerHandler.instance.transform;
        float distance = Vector3.Distance(transform.position, playerTransform.position);



        if(distance > 80)
        {
            Debug.Log("i am too far away and should despawn");
            LocalHandler.instance.GetEnemyAndSpawninAnotherPortal(this);
        }

    }

    #endregion



    [Separator("SOUND FOR ENEMY")]
    [SerializeField] SoundUnit soundUnitTemplate;
    [SerializeField] Transform container;

    protected void CreateAudioSource(AudioClip clip)
    {
        if(soundUnitTemplate == null || container == null)
        {
            Debug.Log("sound assets missing in " + gameObject.name);
            return;
        }
        SoundUnit newObject = Instantiate(soundUnitTemplate);
        newObject.transform.SetParent(container);
        newObject.transform.localPosition = Vector3.zero;
        newObject.SetUp(clip);
    }


    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawSphere(transform.position, 5);
    }

    public float GetTargetCurrentHealth()
    {
        return healthCurrent;
    }

    public void RestoreHealth(float value)
    {
        healthCurrent += value;
        healthCurrent = UnityEngine.Mathf.Clamp(healthCurrent, 0f, healthTotal);
    }

    public GameObject GetObjectRef()
    {
        return gameObject;
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
