using DG.Tweening;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class AreaDamage : MonoBehaviour
{
    //this will receive cordinates. go to the cordinate and prepare the ui
    //then deal damage in time.

    [SerializeField] AbilityIndicatorCanvas _indicatorCanvas;

    DamageClass _damage;

    float current;
    float total;

    float radius;

    LayerMask targetLayer;


    BDClass[] _bdArray;
    //what if i have different types of esecial effect.
    //we will have different processes inside here.


    //for the first one we will 

    float shakeModifier;

    //but the speed must be based 
    public void SetUp_Regular(Vector3 pos, float radius, float timer, DamageClass damage, int layer, float shakeModifier, AreaDamageVSXType _areaDamageVSX)
    {
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
        _bdArray = bdArray;
    }

    //the difference here is:
    //it should call stuff on intervals.
    //the first tick should always be instant.
    //


    public void SetUp_Continuously(Vector3 pos, float radius, float timer, DamageClass damage, int layer, float shakeModifier, AreaDamageVSXType _areaDamageVSX)
    {
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


    }
    //public void Set


    //we need to create a projectile coming down on this

    bool isDone = false;

    private void FixedUpdate()
    {


        if (isDone )
        {
            if(!IsAnimationPlaying())
            {
                currentAnimator = null;
                GameHandler.instance._pool.AreaDamage_Release(this);
            }         

            return;
        }

        if(current > total)
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

    }

    void ApplyBD(IDamageable damageable)
    {
        for (int i = 0; i < _bdArray.Length; i++)
        {
            var item = _bdArray[i];
            damageable.ApplyBD(item);
        }
    }


    float tick_current;
    float tick_Total;

    //this only works while we find something inside
    //in the moment we no longer find something then we reset the tick.
    void HandleTick()
    {

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


    void DecideWhichVSX(AreaDamageVSXType _vsxType)
    {
        //we are going to check for every single one. not good, but fuck it.


        if(_vsxType == AreaDamageVSXType.Fireball_Explosion)
        {
            CallVSX_Animator(_vsxType);
            return;
        }

        if(_vsxType == AreaDamageVSXType.Ghost_Orb)
        {
            CallVSX_PS(_vsxType);
            return;
        }


        CallVSX_Animator(_vsxType); //we do this to force the rest in case the vsx type is nothing
    }

    [Separator("VSX_ANIMATOR")]
    [SerializeField] Animator[] vsxHolderArray_Animator;
    Animator currentAnimator;

    void CallVSX_Animator(AreaDamageVSXType _vsxType)
    {

        currentAnimator = null;
        for (int i = 0; i < vsxHolderArray_Animator.Length; i++)
        {
            var item = vsxHolderArray_Animator[i];
            item.gameObject.SetActive(false);
        }
        if (_vsxType == AreaDamageVSXType.Nothing)
        {

            return;
        }


        vsxHolderArray_Animator[(int)_vsxType].gameObject.SetActive(true);
        currentAnimator = vsxHolderArray_Animator[(int)_vsxType];

    }

    [Separator("VSX_PS")]
    [SerializeField] ParticleSystem[] vsxHolderArray_PS;
    //we dont need a current because it working should be irrelevant 


    void CallVSX_PS(AreaDamageVSXType _vsxType)
    {
        //
        for (int i = 0; i < vsxHolderArray_PS.Length; i++)
        {
            var item = vsxHolderArray_PS[i];
            item.gameObject.SetActive(false);
        }

        if (_vsxType == AreaDamageVSXType.Nothing)
        {
            return;
        }

        vsxHolderArray_PS[(int)_vsxType].gameObject.SetActive(true);

    }

    #endregion

}

public enum AreaDamageVSXType
{
    Nothing = -1,
    Fireball_Explosion = 0,
    Ghost_Orb = 0 
}


//i need the ability to apply stuf
//and i need to way to apply something in period of times.
//so the goal is:
//fix the areadamagevsxType, one is using animator and the other is using ps
//should fit both.