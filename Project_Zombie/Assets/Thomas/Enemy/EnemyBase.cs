using DG.Tweening;
using MyBox;
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


    public Action<EnemyData> eventDied;
    public void OnDied(EnemyData data) => eventDied?.Invoke(data);

    [SerializeField] EnemyCanvas _enemyCanvas;
    [SerializeField] protected GameObject head;
    [SerializeField] EntityEvents _entityEvents; //these are just a bunch of events that might interest this entity.
    [SerializeField] EntityStat _entityStat; //
    //[SerializeField] MeshRenderer _rend;
    [SerializeField] protected EnemyGraphicHandler _enemyGraphicHandler;
    [field:SerializeField] public EntityAnimation _entityAnimation { get; private set; }
    [SerializeField] BoxCollider _myCollider;
    [Separator("CONTAINERS")]
    [SerializeField] Transform psContainer;

    bool isLocked;

    float healthCurrent;
    float healthTotal;

    protected Rigidbody _rb;

    public bool isAttacking {  get; private set; }

    public void SetIsAttack(bool isAttacking) => this.isAttacking = isAttacking;

    const int POINTS_PERHIT = 5;
    const int POINTS_PERKILL = 50;



    private void OnDestroy()
    {
      

        LocalHandler.instance.RemoveEnemyFromSpawnList(data);

    }
    private void Awake()
    {
        shieldedPenetrationValue = -1;
        _rb = GetComponent<Rigidbody>();
        _entityAnimation = GetComponent<EntityAnimation>();


        if(_entityAnimation != null )
        {
            _entityAnimation.SetStateUpperBody(AnimationState_UpperBody.Nothing); //I DO THIS BECAUSE THE ENEMY DOESNT HAVE UPPERBODY MOVEMENT
            _entityAnimation.SetAnimationID("Enemy");
        }



        AwakeFunction();
    }

    protected override void UpdateFunction()
    {

        if (IsAttacking_Animation)
        {

            return;
        }

        if(isLocked)
        {
            //we force it to play idle animation.
            StopAgent();

            return;
        }
        if (isDead) return;

        if (PlayerHandler.instance._playerResources.isDead)
        {
            _entityAnimation.CallAnimation_Idle();
            _entityAnimation.ControlWeight(2, 0);
            return;
        }
        else
        {
            _entityAnimation.ControlWeight(2, 1); //THIS MIGHT CREATE PROBLEMS
        }


        if (_entityStat.isStunned)
        {
            StopAgent();
            return;
        }

        if (!_agent.isStopped)
        {
            Vector3 direction = _agent.velocity;

            // If the _agent is moving
            if (direction.magnitude > 0.1f)
            {
                // Calculate the rotation required to face the direction
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                
                // Smoothly rotate towards the target rotation
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }
        }


        base.UpdateFunction();

        CheckIfShouldBeDespawned();
    }

    private void FixedUpdate()
    {

        HandleDuration();
    }

    protected virtual void AwakeFunction()
    {

        groundLayer |= (1 << 10);

        id = Guid.NewGuid().ToString();

        _agent = GetComponent<NavMeshAgent>();

        _entityEvents.eventUpdateStat += UpdateStat;

        
        gameObject.name = data.name + "; Unique_ID: " + id;

    }

    //actually what we will do instead is to simply force the respawn from this piece.

    void ControlLocked(bool isLocked)
    {
        this.isLocked = isLocked;
    }

    protected virtual void StartFunction()
    {


        PlayerHandler.instance._entityEvents.eventLockEnemies += ControlLocked;

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
        CreateKeyForAnimation_Attack();
        _entityAnimation.UpdateAttackAnimationSpeed(data.AttackAnimationSpeed);
        StartFunction();
    }

    public virtual void ResetEnemyForPool()
    {
        gameObject.SetActive(false);
        isDead = false;
        gameObject.layer = 6;
        _entityAnimation.ControlIfAnimatorApplyRootMotion(false);
        _myCollider.enabled = true;
        _agent.enabled = true;
        _enemyCanvas.UpdateDuration(0, 0);
        //but also turn on the collider.

        isLocked = false;

        _entityStat.ResetEntityStat();


        if(_enemyCanvas != null)
        {
            _enemyCanvas.ResetFadeList();
        }

        if(_abilityIndicatorCanvas != null)
        {
            _abilityIndicatorCanvas.StopCircleIndicator();
        }


        SetIsAttacking_Animation(false);
    }

    #region STAT ENTITY



    bool alreadySetStat;
    public virtual void SetStats(int round)
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
                BDClass bd_Health = new BDClass("BloodMoon_Health", StatType.Health, 0, 1f, 0);
                BDClass bd_Damage = new BDClass("BloodMoon_Damage", StatType.Damage, 0, 0.45f, 0);
                BDClass bd_Speed = new BDClass("BloodMoon_Speed", StatType.Speed, 0, 0.45f, 0);

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

    bool DoesShieldBlock(DamageClass damageRef)
    {

        if (isImmuneToExplosion && damageRef.isExplosion)
        {
            return true;
        }
        if (shieldedPenetrationValue != -1 && damageRef.attacker != null && !damageRef.isExplosion)
        {
            if (damageRef.attacker.GetID() == "Player")
            {
                //actually i want to record the position when the player shot.
                Vector3 damageDirection = (damageRef.lastPos - transform.position).normalized;

                float angle = Vector3.Angle(damageDirection, transform.forward);

                if (angle < 65)
                {

                    //and not call 

                    if (damageRef.pen < shieldedPenetrationValue)
                    {
                        CallShieldPopUp();
                        return true;
                    }

                }
                else
                {
                    Debug.Log("back");
                    
                }


            }

        }


        return false;
    }

    public void TakeDamage(DamageClass damageRef)
    {
        if (isDead) return;
         
        //this here is checking if the player can damage the shield.
        bool hasShieldBlocked = DoesShieldBlock(damageRef);

        if (hasShieldBlocked) return;


        if(damageRef.projectilTransform != null)
        {
            PSScript ps = GameHandler.instance._pool.GetPS(PSType.Blood_01, damageRef.projectilTransform);
            ps.transform.SetParent(psContainer);
            ps.StartPS();
        }
       

        DamageClass damage = new DamageClass(damageRef);
        damage.UpdateDamageList_Enemy(data); //this is checking
        _entityEvents.CallDelegate_DealDamageToEntity(ref damage);
        float totalDamage = damage.GetTotalDamage();


        //GRAPHICAL 
        _enemyGraphicHandler.MakeDamaged();

        //EVENTS
        PlayerHandler.instance._entityEvents.OnDamagedEntity(this, damage);      
        PlayerHandler.instance._playerStatTracker.ChangeStatTracker(StatTrackerType.DamageDealt_Total, totalDamage);
        if (damage.AtLeastOneDamageCrit())
        {
            PlayerHandler.instance._entityEvents.OnCrit();
        }

        //

       //UI
        _enemyCanvas.CreateDamagePopUp(damage);

        healthCurrent -= totalDamage;
        _enemyCanvas.UpdateHealth(healthCurrent, healthTotal);

        if (healthCurrent <= 0)
        {
            bool wasPlayer = false;

            if(damageRef.attacker != null)
            {
                if (damageRef.attacker.GetID() == "Player") wasPlayer = true;
            }

            //death
            Die(wasPlayer);
            GameHandler.instance._soundHandler.CreateSfx_WithAudioClip(data.audio_Dead,transform);
        }
        else
        {
            PlayerHandler.instance._playerResources.GainPoints(POINTS_PERHIT);
            GameHandler.instance._soundHandler.CreateSfx_WithAudioClip(data.audio_Hit, transform);
        }

    }

    public void CallShieldPopUp()
    {
        _enemyCanvas.CreateShieldPopUp();
    }

    protected virtual void Die(bool wasKilledByPlayer = true)
    {

        PlayerHandler.instance._entityEvents.OnKillEnemy(this, wasKilledByPlayer);
            
        PlayerHandler.instance._playerResources.GainPoints(POINTS_PERKILL);

        if (preventNextDeath)
        {
            preventNextDeath = false;
            return;
        }

        isDead = true;
        gameObject.layer = 0;
        StopAgent();
        _rb.velocity = Vector3.zero;

        _myCollider.enabled = false;
        _agent.enabled = false;

        LocalHandler.instance.RemoveEnemyFromSpawnList(data);

        HandleWhatDeathShouldDrop();

        PlayerHandler.instance._playerStatTracker.ChangeStatTracker(StatTrackerType.EnemiesKilled, 1);
        
        OnDied(data);

        StartCoroutine(DeathProcess());
        //we need to call death animation here.

        
        //Destroy(gameObject);

    }

    IEnumerator DeathProcess()
    {
        _entityAnimation.ControlWeight(2, 0);
        _entityAnimation.ControlIfAnimatorApplyRootMotion(true);
        _entityAnimation.RerollDeathAnimation();
        _entityAnimation.CallAnimation_Death();

        float clipDuration = _entityAnimation.GetDurationForDeath() + 5;

        yield return new WaitForSeconds(clipDuration);

        transform.DOMove(transform.position + new Vector3(0, -5, 0), 10f);

        yield return new WaitForSeconds(9.8f);
        Debug.Log("3");
        GameHandler.instance._pool.Enemy_Release(data, this);
    }


    void HandleWhatDeathShouldDrop()
    {
        //drop data is the last one. maybe we should do it like this we should 
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

    }
    public float GetTargetMaxHealth()
    {
        return _entityStat.GetTotalValue(StatType.Health);
    }
    #endregion

    #region PATHING
    [SerializeField]protected NavMeshAgent _agent;
    protected Vector3 currentAgentTargetPosition;
    protected bool isMoving;

    protected void SetSpeed(float value)
    {
        //we update this if we slowed.
        _agent.speed = value;    
    }

    public void SetDestinationForPathfind(Vector3 targetPos)
    {
        _agent.isStopped = false;
        _agent.destination = targetPos;

        isMoving = true;
        currentAgentTargetPosition = targetPos;
    }

    public void StopAgent()
    {
        _agent.enabled = true;
        _agent.isStopped = true;
        _agent.velocity = Vector3.zero;

        isMoving = false;

    }

    #endregion

    #region TARGETTING AND ATTACKING

    public IDamageable targetIdamageable { get; private set; }
    public GameObject targetObject {  get; private set; }

    public void SetNewtarget(GameObject target)
    {
        //the only problem is that its not reliable to use gameobject _id.

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
        _entityAnimation.RerollAttackAnimation();
        Debug.Log("yo");

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

        Debug.Log("here");

        float distanceForAttack = Vector3.Distance(targetObject.transform.position, transform.position);
        GameHandler.instance._soundHandler.CreateSfx_WithAudioClip(data.audio_Attack, transform);

        //i wont check for distance, i will check with raycast.

        //we will check any fellas in front 

        
        if(data.attackRange >= distanceForAttack) 
        {

            //if the player is still in range then we attack.
            DamageClass damage = GetDamage();
            damage.Make_Attacker(this);
            targetIdamageable.TakeDamage(damage);
        }

    }

    protected DamageClass GetDamage()
    {
        float baseDamage = _entityStat.GetTotalValue(StatType.Damage);
        float pen = _entityStat.GetTotalValue(StatType.Pen);


        return new DamageClass(baseDamage, data.damageType, pen);
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

    #region MADE ALLY

    public bool IsAlly { get; private set; }
    [Separator("ALLY")]
    [SerializeField] Material _allyMaterial;
    bool preventNextDeath;

    float duration_Current;
    float duration_Total;

    [ContextMenu("FORCE IT ALLY")]
    public void ForceAlly()
    {
        MakeAlly(5);
    }


    public void MakeAlly(float duration)
    {
        //we have to change the behavior.

        StopAgent();

        duration_Total = duration;
        duration_Current = duration_Total;

        IsAlly = true;
        gameObject.layer = 8;
        _enemyGraphicHandler.MakeAlly();

        //recover health

        healthCurrent = healthTotal;
        _enemyCanvas.UpdateHealth(healthCurrent, healthTotal);

    }

    void HandleDuration()
    {
        if (duration_Total <= 0) return;
        if(duration_Current > 0)
        {
            duration_Current -= Time.fixedDeltaTime;
            _enemyCanvas.UpdateDuration(duration_Current, duration_Total);
        }
        else
        {
            GameHandler.instance._pool.Enemy_Release(data, this);
            _enemyCanvas.UpdateDuration(duration_Current, duration_Total);
        }
    }

    public bool HasEnemyCurrentTarget()
    {
        if (targetObject == null) return false;

        return targetObject.gameObject.layer == 6;
    }

    public void PreventDeath()
    {
        Debug.Log("yo");
        preventNextDeath = true;
    }

    #endregion

    #region ANIMATION

    //i actually want to be looping rather than always calling it.
    //i want the attacks to deal damage in the half of its time.
    //all animations should have 60 fps, with 30 frames being when the damaged is dealt, so always half.
    //i want to be able to loop.

    //the zombie will not react from getting damaged. they will instead show a little red color
    //the first attack _id is random, but then it becomes a loop.
    //the zombie should have movement for slow walk and fast walk
    //



    public bool IsAttacking_Animation { get; private set; }

    public void SetIsAttacking_Animation(bool IsAttacking_Animation)
    {
        this.IsAttacking_Animation = IsAttacking_Animation;

        Debug.Log("control attack animation " + IsAttacking_Animation);
    }

    protected virtual void CreateKeyForAnimation_Attack()
    {
        _entityAnimation.AddEnemyID("Attack", 1,2);
        _entityAnimation.AddEnemyID("Death", 1, 3);
    }





    //running is just one for the meantime.

    #endregion


    [Separator("SOUND FOR ENEMY")]
    [SerializeField] SoundUnit soundUnitTemplate;
    [SerializeField] Transform container;
    //WE WILL BE IGNORING THIS FOR NOW

    protected void CreateAudioSource(AudioClip clip)
    {
        if(soundUnitTemplate == null || container == null)
        {
            Debug.Log("sound assets missing in " + gameObject.name);
            return;
        }
        SoundUnit newObject = GameHandler.instance._pool.GetSound(transform);
        newObject.transform.SetParent(container);
        newObject.transform.localPosition = Vector3.zero;
        newObject.SetUp(clip, true);
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
//we will have a script for each _enemy. its easier to do stuff like that.
//each fella will choose their behaviors for the behavior tree.

//ENEMIES
//simple melee will be the main. then we will simply have different versions that will have different stats. enemies have weak spots.
//Tanker. big and slow.
//charger. aims and charges forward. can only hit from behind. infront requires a certain amount of pen to go through.
//Mage. creates spell damages. so you need to keep moving.
//Hound. just a faster _enemy that will try to flank the player.


//all these spells should be shows as lines in the floor.
//just create an image that is placed in the floor. cannot go through wall and is affected by spell range. alos will have a fill function to show when the _enemy will attack.





//VERSION 0.7

//certain enemies can lose limbs. this has no gameplay utility it will just look cool
//