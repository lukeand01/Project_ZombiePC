using DG.Tweening;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

//i will rewrite this to be more flexible.
//currently is too much trouble.
//so as it stands we are using animator and ps for effects.
//i syhould just do once, instead of using animator we use only 
//lets just use the animator. this is no time to bother about perfections
//

//

public class AreaDamage : MonoBehaviour
{
    //this will receive cordinates. go to the cordinate and prepare the ui
    //then deal damage in time.

    [SerializeField] AbilityIndicatorCanvas _indicatorCanvas;

    DamageClass _damage;

    float current;
    float total;
    float delay_Current;
    float delay_Total;
    float radius;


    float _animationSpeed;

    LayerMask targetLayer;

    bool isUsingAnimation;

    List<BDClass> _bdList = new();
    //what if i have different types of esecial effect.
    //we will have different processes inside here.

    private void Awake()
    {
        _animationSpeed = 1;
    }

    //for the first one we will 

    float shakeModifier;
    bool isContinous;
    //but the speed must be based 
    public void SetUp_Regular(Vector3 pos, float radius, float timer, DamageClass damage, int layer, float shakeModifier, AreaDamageVSXType _areaDamageVSX)
    {

        isContinous = false;

        pos.y = 0;
        transform.position = pos;

        this.radius = radius;

        _indicatorCanvas.gameObject.SetActive(true);
        _indicatorCanvas.StartCircleIndicator(radius);



        DecideWhichVSX(_areaDamageVSX);

        isDone = false;

       _damage = damage;

        total = timer;

        targetLayer |= (1 << layer);

        this.shakeModifier = shakeModifier;



    }

    //if it deals damage in whaterver way it applies this as well.
    public void Make_BD(BDClass[] bdArray)
    {
        _bdList.Clear();
        for (int i = 0; i < bdArray.Length; i++)
        {
            var item = bdArray[i];
            _bdList.Add(item);
        }


       
    }

    public void Make_AnimationSpeed(float animationSpeed)
    {
        _animationSpeed = animationSpeed;
    }

    bool showDelayInUI;
    public void MakeDelayShowInUI()
    {
        showDelayInUI = true;
    }

    public void SetUp_Continuously(Vector3 pos, float radius, float delay, float timer, DamageClass damage, int layer, float shakeModifier, AreaDamageVSXType _areaDamageVSX)
    {
        isContinous = true;

        pos.y = 0;
        transform.position = pos;
        this.radius = radius;

        _indicatorCanvas.gameObject.SetActive(true);
        _indicatorCanvas.StartCircleIndicator(radius);

        isDone = false;

        _damage = damage;

        total = timer;

        targetLayer |= (1 << layer);

        this.shakeModifier = shakeModifier;

        DecideWhichVSX(_areaDamageVSX);

        delay_Total = delay;
        delay_Current = 0;

        tick_Total = 0.1f;

    }


    bool isDone = false;

    private void FixedUpdate()
    {
        if(delay_Current < delay_Total)
        {
            delay_Current += Time.fixedDeltaTime;

            if (showDelayInUI)
            {
                //show ui.
                _indicatorCanvas.ControlCircleFill(delay_Current, delay_Total);
            }

            return;
        }



        if (isContinous)
        {
            //i am doing like this because its better than creating a new system
            //we check for thick.

            if (isDone)
            {
                if (!IsAnimationPlaying())
                {
                    currentAnimator = null;
                    GameHandler.instance._pool.AreaDamage_Release(this);
                }


                return;
            }

            HandleTick();


            if (current > total)
            {
                //i want this to fade?
                isDone = true;
            }
            else
            {
                current += Time.fixedDeltaTime;
            }
        }
        else
        {
            if (isDone)
            {
                if (!IsAnimationPlaying())
                {
                    currentAnimator = null;
                    GameHandler.instance._pool.AreaDamage_Release(this);
                }


                return;
            }

            //not every fella will use this.
            if (current > total)
            {
                DealDamage();
                _indicatorCanvas.gameObject.SetActive(false);
                isDone = true;

            }
            else
            {
                _indicatorCanvas.ControlCircleFill(current, total);
                current += Time.fixedDeltaTime;
            }
        }

        
    }

    bool IsAnimationPlaying()
    {
        if (currentAnimator == null) return false;

        if (currentAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            return true;
        else
            return false;
    }

    void DealDamage()
    {
        //we check in this area and deal damage to those you cannot.

        if (_damage == null) return;

        RaycastHit[] targets = Physics.SphereCastAll(transform.position, radius, Vector3.up, 0, targetLayer);

        foreach(var item in targets) 
        {
            IDamageable targetDamageable = item.collider.GetComponent<IDamageable>();

            if (targetDamageable == null) continue;
            ApplyBD(targetDamageable);
            targetDamageable.TakeDamage(_damage);

        }


        PlayerHandler.instance.TryToCallExplosionCameraEffect(transform, shakeModifier);

        if (_callSound)
        {
            GameHandler.instance._soundHandler.CreateSfx(_soundType, transform, 0.3f);
        }

    }

    void ApplyBD(IDamageable damageable)
    {
        if (_bdList == null) return;

        for (int i = 0; i < _bdList.Count; i++)
        {
            var item = _bdList[i];

            BDClass newBD = new BDClass(item);


            damageable.ApplyBD(newBD);
        }
    }

    bool _callSound;
    SoundType _soundType;

    public void CallSoundOnDamaged(SoundType soundType)
    {
        _soundType = soundType; 
        _callSound = true;
    }

    float tick_current;
    float tick_Total;

    //this only works while we find something inside
    //in the moment we no longer find something then we reset the tick.
    void HandleTick()
    {
        //maybe i should redo this.

        //we only do this when we first detected the player.

        RaycastHit[] targets = Physics.SphereCastAll(transform.position, radius, Vector3.up, 0, targetLayer);

        //when we detect that we apply then we start counting the tick

        if(targets.Length > 0)
        {

            if(tick_current > tick_Total)
            {
                foreach (var item in targets)
                {
                    IDamageable targetDamageable = item.collider.GetComponent<IDamageable>();

                    if (targetDamageable == null) continue;
                    ApplyBD(targetDamageable);
                    targetDamageable.TakeDamage(_damage);

                }

                tick_current = 0;
            }
            else
            {
                tick_current += Time.fixedDeltaTime;
            }

        }
        else
        {
            tick_current = 0;
        }
        



        if (tick_current > tick_Total)
        {

        }
        else
        {

        }

    }



    public void ResetForPool()
    {
        CallVSX_Animator(AreaDamageVSXType.Nothing);
        _indicatorCanvas.gameObject.SetActive(true);
        _indicatorCanvas.StopCircleIndicator();
        current = 0;

        gameObject.SetActive(false);
    }

    #region VSX

    void ResetVSX()
    {
        for (int i = 0; i < vsxHolderArray_Animator.Length; i++)
        {
            var item = vsxHolderArray_Animator[i];

            item.gameObject.SetActive(false);
        }
        
    }

    void DecideWhichVSX(AreaDamageVSXType _vsxType)
    {
        //we are going to check for every single one. not good, but fuck it.

        ResetVSX();

        _indicatorCanvas.gameObject.SetActive(true);
        
        CallVSX_Animator(_vsxType); //we do this to force the rest in case the vsx type is nothing
    }

    [Separator("VSX_ANIMATOR")]
    [SerializeField] Animator[] vsxHolderArray_Animator;
    Animator currentAnimator;

    void CallVSX_Animator(AreaDamageVSXType _vsxType)
    {

        currentAnimator = null;

        if (_vsxType == AreaDamageVSXType.Nothing)
        {
            return;
        }

        vsxHolderArray_Animator[(int)_vsxType].gameObject.SetActive(true);
        currentAnimator = vsxHolderArray_Animator[(int)_vsxType];
        currentAnimator.speed = _animationSpeed;
    }

    //we dont need a current because it working should be irrelevant 

    //i need a time to delay the activation.

   

    #endregion

}

public enum AreaDamageVSXType
{
    Nothing = -1,
    Fireball_Explosion = 0,
    Ghost_Orb = 1 ,
    Meteor = 2,
    Fire = 3
}


//it needs to trigger its begining. dealy is taking care of that, just need to synch it.


//i need the ability to apply stuf
//and i need to way to apply something in period of times.
//so the goal is:
//fix the areadamagevsxType, one is using animator and the other is using ps
//should fit both.