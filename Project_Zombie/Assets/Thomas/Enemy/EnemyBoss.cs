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

    string id;
    bool isDead;

    [Separator("BOSS")]
    [SerializeField] protected EnemyData _bossData;
    [SerializeField] BossSigilType _sigilType;
    [SerializeField] protected AttackClass[] attackClassArray;



    [Separator("SCRIPTS")]
    [SerializeField] EntityEvents _entityEvent;
    [SerializeField] EntityStat _entityStat;
    [SerializeField] EnemyGraphicHandler _enemyGraphicHandler;

    [Separator("COMPONENTS")]
    [SerializeField] NavMeshAgent _agent;
    [SerializeField] Rigidbody _rb;
    [SerializeField] BoxCollider _boxCollider;
    [SerializeField] Animator _animator;

    [Separator("CANVAS")]
    [SerializeField] EnemyCanvas _enemyCanvas;
    [SerializeField] protected AbilityIndicatorCanvas _abilityCanvas;

    [Separator("CONTAINERS")]
    [SerializeField] Transform psContainer;

    [Separator("PS")]
    [SerializeField] ParticleSystem _deathPS;
    [SerializeField] Transform _graphicHolder;
    float health_Total;
    float healt_Current;

    float _speed_Base;

    const int POINTS_PERKILL = 1500;




    private void Awake()
    {
        AwakeFunction();
    }

    float GetHealthFromStat()
    {
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
        health_Total = GetHealthFromStat();
        healt_Current = health_Total;


        _agent.speed = GetSpeedFromStat();
        _speed_Base = _agent.speed;

        SetActionindexMax(attackClassArray.Length);
        

        InitAnimation();
        id = MyUtils.GetRandomID();
    }


    public bool IsStunned { get { return _entityStat.isStunned; } }


    public void ResetForPool()
    {
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
    }

    protected override void UpdateFunction()
    {
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


    #region PATHING
    bool isMoving;
    protected Vector3 currentAgentTargetPosition;



    public void SetDestinationForPathfind(Vector3 targetPos)
    {

       

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

    public void TakeDamage(DamageClass damageRef)
    {
        
        if (isDead) return;

        //this here is checking if the player can damage the shield.
        bool hasShieldBlocked = DoesShieldBlock();

        if (hasShieldBlocked)
        {
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

        healt_Current -= totalDamage;
        _enemyCanvas.UpdateHealth(healt_Current, health_Total);



        if (healt_Current <= 0)
        {

            //death
            Die();

        }
        
    }


    void Die()
    {
        //you only gain points in the end.
        //PlayerHandler.instance._entityEvents.OnKillEnemy(this, true);
        Debug.Log("called death");
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

    IEnumerator DeathProcess()
    {

        //PlayerHandler.instance._playerResources.GainPoints(POINTS_PERKILL);

        _animator.applyRootMotion = true;
        //need to aply an especial effect for 
        CallAnimation("Death", 0); //anmd
        //reeduce the weight of all body parts

        ControlWeight(1, 0);
        ControlWeight(2, 0);
        ControlWeight(3, 0);

        yield return new WaitForSeconds(3);

       PSScript _ps = GameHandler.instance._pool.GetPS(PSType.BlackWholeForBossCollection_01, transform);
        _ps.gameObject.SetActive(true);
        _ps.gameObject.transform.position = transform.position;
        _ps.transform.position += new Vector3(0, 0, -5);

        //_deathPS.gameObject.SetActive(true);
       // _deathPS.Clear();
        //_deathPS.Play();

        yield return new WaitForSeconds(5);

        _graphicHolder.transform.DOLocalMove(_graphicHolder.transform.localPosition + new Vector3(0, -5, 0), 3f);

        yield return new WaitForSeconds(8);

        //_deathPS.gameObject.SetActive(false);
        Debug.Log("done");

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

    public void StartChargingAttack()
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
    //animation id
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


