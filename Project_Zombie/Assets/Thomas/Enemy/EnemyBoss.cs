using DG.Tweening;
using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBoss : Tree, IDamageable
{
    //bosses should have multiple attacks.
    //we will create our own stuff here. instead of using the enemyboss class but that would be an additional problem.
    //we can still use the  enemydata for some information
    
    //

    protected string id;
    protected bool isDead;

    [Separator("BOSS")]
    [SerializeField] protected EnemyData _bossData;
    [SerializeField] protected BossSigilType _sigilType;
    [SerializeField] protected AttackClass[] attackClassArray;
    [SerializeField] bool _healthShouldUpdateBossUI;
    public EnemyData GetBossData { get { return _bossData; }  }

    [Separator("SCRIPTS")]
    [SerializeField] protected EntityEvents _entityEvent;
    [SerializeField] protected EntityStat _entityStat;
    [SerializeField] protected EnemyGraphicHandler _enemyGraphicHandler;

    [Separator("COMPONENTS")]
    [SerializeField] protected NavMeshAgent _agent;
    [SerializeField] protected Rigidbody _rb;
    [SerializeField] protected BoxCollider _boxCollider;
    [SerializeField] public Animator _animator;

    [Separator("CANVAS")]
    [SerializeField] protected EnemyCanvas _enemyCanvas;
    [SerializeField] protected AbilityIndicatorCanvas _abilityCanvas;

    [Separator("CONTAINERS")]
    [SerializeField] protected Transform psContainer;

    [Separator("PS")]
    [SerializeField] ParticleSystem _deathPS;
    [SerializeField] public Transform _graphicHolder;
    protected float health_Total;
    protected float health_Current;

    float _speed_Base;

    protected const int POINTS_PERKILL = 1500;

    protected bool isLocked;
    protected void ControlIsLocked(bool isLocked)
    {
        this.isLocked = isLocked;
    }

    bool _bossHasStarted;

    private void Awake()
    {
        AwakeFunction();
    }

    float GetHealthFromStat()
    {
        if (LocalHandler.instance == null)
        {
            Debug.Log("no local hanadler");
        }

        float health = 0;
        int round = LocalHandler.instance.round;

        

        for (int i = 0; i < _bossData.initialStatList.Count; i++)
        {
            var item = _bossData.initialStatList[i];

            if (item.stat == StatType.Health)
            {
                health = item.value;
            }
        }

        for (int i = 0; i < _bossData.scaleStatList.Count; i++)
        {
            var item = _bossData.scaleStatList[i];

            if (item.stat == StatType.Health)
            {
                health =+ (item.value * round);
            }
        }


        return health;
    }
    float GetSpeedFromStat()
    {
        for (int i = 0; i < _bossData.initialStatList.Count; i++)
        {
            var item = _bossData.initialStatList[i];

            if (item.stat == StatType.Speed)
            {
                return item.value;
            }
        }

        return 0;
    }

    protected virtual void AwakeFunction()
    {       

        SetActionindexMax(attackClassArray.Length);

        InitAnimation();
        id = MyUtils.GetRandomID();
    }

    private void Start()
    {
        StartFunction();
    }
    protected virtual void StartFunction()
    {
        health_Total = GetHealthFromStat();
        health_Current = health_Total;


        _agent.speed = GetSpeedFromStat();
        _speed_Base = _agent.speed;
    }

    public bool IsStunned { get { return _entityStat.isStunned; } }


    public virtual void ResetForPool()
    {
        gameObject.SetActive(false);

        isChargingAttack = false;
        IsActing = false;


        isDead = false;
        gameObject.layer = 6;

        _boxCollider.enabled = true;
        //_agent.enabled = true;

        SetActionIndexCurrent(-1);

        _animator.applyRootMotion = false;

        ControlWeight(1, 1);
        ControlWeight(2, 1);

        _bossHasStarted = false;
    }

    protected override void UpdateFunction()
    {


        if (_healthShouldUpdateBossUI && !_bossHasStarted) return;
        if (isLocked) return;



        if (PlayerHandler.instance._playerResources.isDead)
        {           
            StopAgent();
            ControlWeight(1, 0);
            ControlWeight(2, 0);
            ControlWeight(3, 0);
            return;
        }

        if (isDead)
        {
            return;
        }

        if (IsActing)
        {
            HandleChargingAttack();
            return;
        }
        if (isMoving)
        {

        }


        base.UpdateFunction();
    }

    public virtual void StartBossUI()
    {

        health_Total = GetHealthFromStat();
        health_Current = health_Total;

        UIHandler.instance._BossUI.OpenBossHealth(_bossData.enemyName);
        UIHandler.instance._BossUI.UpdateHealth(health_Current, health_Total);
    }

    public virtual void StartBoss()
    {
        //we increase its stats if necessary.

        _bossHasStarted = true;

    }

    public BossPortal _bossPortal { get; private set; }
    public void SetBossPortal(BossPortal bossPortal)
    {
        _bossPortal = bossPortal;
    }

    public void EndBoss()
    {
        UIHandler.instance._BossUI.CloseBossHealth();
    }


    #region PATHING
    bool isMoving;
    protected Vector3 currentAgentTargetPosition;


    public void SetDestinationForPathfind(Vector3 targetPos)
    {
        if(_agent == null)
        {
            Debug.Log("no agent found");
            return;
        }
        if (isDead) return;

        if (_healthShouldUpdateBossUI && !_bossHasStarted) return;


        _agent.enabled = true;

        _agent.isStopped = false;
        _agent.destination = targetPos;


        isMoving = true;
        currentAgentTargetPosition = targetPos;
    }

    public void StopAgent()
    {
        _agent.isStopped = true;
        _agent.velocity = Vector3.zero;

        isMoving = false;

    }

    #endregion

    #region DAMAGEABLE
    [Separator("SHIELD")]
    [SerializeField] AudioClip _shieldClip;
    [SerializeField] protected bool isShielded;

    public void ControlIsShielded(bool isShielded)
    {
        this.isShielded = isShielded;
    }


    public void ApplyBD(BDClass bd)
    {
        
    }

    public string GetID()
    {
        return id;
    }

    public GameObject GetObjectRef()
    {
        return gameObject;
    }

    public float GetTargetCurrentHealth()
    {
        return 0;
    }

    public float GetTargetMaxHealth()
    {
        return 0;
    }

    public bool IsDead()
    {
        return isDead;
    }

    public void RestoreHealth(float value)
    {
       
    }

    public virtual void TakeDamage(DamageClass damageRef)
    {

        if (isDead) return;

        //this here is checking if the player can damage the shield.
        bool hasShieldBlocked = DoesShieldBlock();

        if (hasShieldBlocked)
        {
            Debug.Log("shield?");
            _enemyCanvas.CreateShieldPopUp();

            if(_shieldClip != null)
            {
                GameHandler.instance._soundHandler.CreateSfx_WithAudioClip(_shieldClip, transform, 0.7f);
            }

            return;
        }

        if (damageRef.projectilTransform != null)
        {
            PSScript ps = GameHandler.instance._pool.GetPS(PSType.Blood_01, damageRef.projectilTransform);
            ps.transform.SetParent(psContainer);
            ps.StartPS();
        }


        DamageClass damage = new DamageClass(damageRef);
        damage.UpdateDamageList_Enemy(_bossData); //this is checking
        _entityEvent.CallDelegate_DealDamageToEntity(ref damage);
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

        health_Current -= totalDamage;
        _enemyCanvas.UpdateHealth(health_Current, health_Total);

        if (_healthShouldUpdateBossUI)
        {
            UIHandler.instance._BossUI.UpdateHealth(health_Current, health_Total);
        }

        if (health_Current <= 0)
        {
            //death
            Die();

        }
        
    }


    protected virtual void Die()
    {
        //you only gain points in the end.
        //PlayerHandler.instance._entityEvents.OnKillEnemy(this, true);

        PlayerHandler.instance._playerResources.GainPoints(POINTS_PERKILL);
        PlayerHandler.instance._playerResources.Bless_Gain(5);
        PlayerHandler.instance._playerInventory.AddBossSigil(_sigilType); //we inform as a new item, and we show this in the pause ui.

        isDead = true;
        gameObject.layer = 0;
        //_agent.enabled = false;
        StopAgent();
        _rb.velocity = Vector3.zero;        
        _boxCollider.enabled = false;
        
        StartCoroutine(DeathProcess());


    }

    IEnumerator  DeathProcess()
    {

        //PlayerHandler.instance._playerResources.GainPoints(POINTS_PERKILL);

        _animator.applyRootMotion = true;
        //need to aply an especial effect for 
        CallAnimation("Death", 0); //anmd
        //reeduce the weight of all body parts

        ControlWeight(1, 0);
        ControlWeight(2, 0);
        ControlWeight(3, 0);




        if(_bossPortal != null)
        {
            GameHandler.instance._soundHandler.CreateSfx(SoundType.AudioClip_Boss_End);
            UIHandler.instance._BossUI.CallBossDefeatedWarn();

            _bossPortal.KillAllEnemies();
        }


        yield return new WaitForSeconds(3);

       PSScript _ps = GameHandler.instance._pool.GetPS(PSType.BlackWholeForBossCollection_01, transform);
        _ps.gameObject.SetActive(true);
        _ps.gameObject.transform.position = _graphicHolder.transform.GetChild(0).position + new Vector3(0, 5, 2);
        

        //_deathPS.gameObject.SetActive(true);
       // _deathPS.Clear();
        //_deathPS.Play();

        yield return new WaitForSeconds(5);

        _graphicHolder.transform.DOLocalMove(_graphicHolder.transform.localPosition + new Vector3(0, -5, 0), 3f);

        yield return new WaitForSeconds(8);

        //_deathPS.gameObject.SetActive(false);
        _bossPortal.EndBoss();

        GameHandler.instance._pool.Boss_Release(_bossData, this);
    }



    bool DoesShieldBlock()
    {

        //for boss as long as he is shielded then it will block.
        return isShielded;

    }

    #endregion

    #region ACTIONS

    [SerializeField] int debugForceActionIndex = -1;
    public bool IsActing { get; private set; }

    [field:SerializeField]public int actionIndex_Current { get; private set; }
    public int actionIndex_Max { get; private set; } //the number of index it has.

    float actionCooldown_Total;
    float actionCooldon_Current;

    int lastActionIndex;

    bool _actionShouldFacePlayer;


    public void ControlIsActing(bool isActing)
    {
        this.IsActing = isActing;
    }

    public bool ShouldChangeAction()
    {
        if(actionCooldon_Current > actionCooldown_Total)
        {
            actionCooldon_Current = 0;
            return true;
        }
        else
        {
            actionCooldon_Current += Time.deltaTime;
            return false;
        }
    }

    public void StartAction(string actionAnimation, int animationLayer, bool shouldFacePlayer = false)
    {
        IsActing = true;
        _actionShouldFacePlayer = shouldFacePlayer;
        StopAgent();

        
        CallAnimation(actionAnimation, animationLayer);
    }
    public void StopAction()
    {
        IsActing = false;
    }

    public void SetActionindexMax(int newLimit)
    {
        actionIndex_Current = -1;
        actionCooldown_Total = 10;
        actionIndex_Max = newLimit;
    }

    public void SelectRandomAction()
    {

       
        int newAction = -1;

        if(debugForceActionIndex != -1)
        {
            newAction = debugForceActionIndex;
        }
        else
        {
            int safeBreak = 0;

            while(newAction == -1)
            {

                safeBreak++;

                if(safeBreak > 1000)
                {
                    newAction = 1;
                }

                int random = UnityEngine.Random.Range(0, actionIndex_Max);

                if (random == lastActionIndex) continue;

                int roll = UnityEngine.Random.Range(0, 101);


                if (attackClassArray[random].chance > roll)
                {
                    newAction = random;

                    if (attackClassArray[random].cannotRepeat)
                    {
                        lastActionIndex = random;
                    }
                    else
                    {
                        lastActionIndex = -1;
                    }
                }

            }

        }

        //we need to roll for chance as well.

        SetActionIndexCurrent(newAction);
    }

    public void SetActionIndexCurrent(int newAction)
    {
        actionIndex_Current = newAction;
        actionCooldon_Current = 0;
    }

    #endregion

    #region ATTACK

    float attackDuration_Total;
    float attackDuration_Current;
    bool isChargingAttack;

    public virtual void StartChargingAttack()
    {
        if(actionIndex_Current == -1)
        {
            Debug.Log("it got a -1 here");
            return;
        }

        

        _animator.SetFloat("AttackSpeed", 0); //we freeze the animation
        //we have to tthe shaep of attack, the direction and the duration in the attack class.
        //Debug.Log(attackClassArray.Length);
        //Debug.Log(actionIndex_Current);
        Debug.Log(actionIndex_Current + " " + actionIndex_Max);
        attackClassArray[actionIndex_Current].ControlPSAttackCharge(true);
        
        if (attackClassArray[actionIndex_Current].sound_Charge != null)
        {
            GameHandler.instance._soundHandler.CreateSfx_WithAudioClip(attackClassArray[actionIndex_Current].sound_Charge);
        }
        

        attackDuration_Total = attackClassArray[actionIndex_Current].duration;
        attackDuration_Current = 0;
        _abilityCanvas.gameObject.SetActive(true);
        isChargingAttack = true;
    }
    void HandleChargingAttack()
    {
        if (!isChargingAttack) return;

        if (_actionShouldFacePlayer)
        {
            Vector3 dir = PlayerHandler.instance.transform.position - transform.position;
            dir.y = 0;//This allows the object to only rotate on its y axis
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, 0.85f * Time.deltaTime);
        }

        if (attackDuration_Current > attackDuration_Total + 0.3f)
        {

            StopChargingAttack();
        }
        else
        {
            attackDuration_Current += Time.deltaTime;
            CallUI(attackDuration_Current, attackDuration_Total);
        }
    }
    void StopChargingAttack()
    {
        isChargingAttack = false;     
        _animator.SetFloat("AttackSpeed", 1);
        CallUI(0, 0);
        _abilityCanvas.gameObject.SetActive(false);
        attackClassArray[actionIndex_Current].ControlPSAttackCharge(false);
        CalculateAttack();


        if (attackClassArray[actionIndex_Current].sound_Release != null)
        {
            GameHandler.instance._soundHandler.CreateSfx_WithAudioClip(attackClassArray[actionIndex_Current].sound_Release);
        }

        SetActionIndexCurrent(-1);


        
    }

    protected virtual void CallUI(float current, float total)
    {

    }

    #endregion

    #region ANIMATION

    string ANIMATIONID = "";

    public void InitAnimation()
    {
        ANIMATIONID = "Animation_" + _bossData.enemyName +"_";
    }



    public void CallAnimation(string nameID, int layer)
    {
        if(nameID == "Death")
        {
            Debug.Log("death was called " + ANIMATIONID + nameID);
        }


        _animator.Play(ANIMATIONID + nameID , layer);
    }
    public bool IsAnimationRunning(string nameID, int layer)
    {
        return _animator.GetCurrentAnimatorStateInfo(layer).IsName(ANIMATIONID + nameID);
    }
    public void ControlWeight(int layer, float weight)
    {
        _animator.SetLayerWeight(layer, weight);
    }

    #endregion

    #region UTILS

    public bool IsTargetPosWalkable(Vector3 pos)
    {
        //we create a cast in the area and check if we have spotted a walkable surface.
       return _agent.Raycast(pos, out NavMeshHit hit);
    }

    #endregion


    public void SetNewSpeed(float additionalSpeed)
    {
        _agent.speed = _speed_Base + additionalSpeed;
    }


    public virtual void HandleFootStep(int index)
    {


        AudioClip _clip = _bossData.audio_footstepArray[index];
        GameHandler.instance._soundHandler.CreateSfx_WithAudioClip(_clip, transform, 0.5f);
    }

    public virtual void CalculateAttack()
    {
        
    }
}

//the first attack will be a circle around the boss
//the second creates a thin rectagular.
//the third doesnt trigger 
//the fourth also doesnt trigger it.


[System.Serializable]
public class AttackClass
{
    //damage
    //animation _id
    //trigger to call.
    //
    [SerializeField] string attackName; //just for editor.
    [field:SerializeField] public float damage { get; private set; }
    [field: SerializeField] public float duration { get; private set; }
    [field: SerializeField] public float range { get; private set; }

    [field: SerializeField] public bool cannotRepeat { get; private set; }
    [Range(1, 100)] public float chance;


    [SerializeField] ParticleSystem[] ps_AttackCharge;

    public void ControlPSAttackCharge(bool isVisible)
    {
        for (int i = 0; i < ps_AttackCharge.Length; i++)
        {
            var item = ps_AttackCharge[i];
            item.gameObject.SetActive(isVisible);

            if (isVisible)
            {
                item.Play();
            }
        }
    }

    //sound for charging and sound for release


    [field: SerializeField] public AudioClip sound_Charge { get; private set; }
    [field: SerializeField] public  AudioClip sound_Release { get; private set; }



}


