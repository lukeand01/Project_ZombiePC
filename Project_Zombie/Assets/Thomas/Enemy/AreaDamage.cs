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

    //what if i have different types of esecial effect.
    //we will have different processes inside here.


    //for the first one we will 

    float shakeModifier;

    //but the speed must be based 
    public void SetUp(Vector3 pos, float radius, float timer, DamageClass damage, int layer, float shakeModifier, AreaDamageVSXType _areaDamageVSX)
    {
        pos.y = 0;
        transform.position = pos;

        this.radius = radius;

        _indicatorCanvas.gameObject.SetActive(true);
        _indicatorCanvas.StartCircleIndicator(radius);



        CallVSX(_areaDamageVSX);

        isDone = false;

       _damage = damage;

        total = timer;

        targetLayer |= (1 << layer);

        this.shakeModifier = shakeModifier;
    }

    //we need to create a projectile coming down on this

    bool isDone = false;

    private void FixedUpdate()
    {

        Debug.Log(IsAnimationPlaying());

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


        RaycastHit[] targets = Physics.SphereCastAll(transform.position, radius, Vector3.up, 0, targetLayer);

        foreach(var item in targets) 
        {
            IDamageable targetDamageable = item.collider.GetComponent<IDamageable>();

            if (targetDamageable == null) continue;
            targetDamageable.TakeDamage(_damage);

        }


        PlayerHandler.instance.TryToCallExplosionCameraEffect(transform, shakeModifier);

    }

    public void ResetForPool()
    {
        CallVSX(AreaDamageVSXType.Nothing);
        _indicatorCanvas.gameObject.SetActive(true);
        _indicatorCanvas.StopCircleIndicator();
        current = 0;

        gameObject.SetActive(false);
    }

    #region VSX
    [Separator("VSX")]
    [SerializeField] Animator[] vsxHolderArray;
    Animator currentAnimator;

    void CallVSX(AreaDamageVSXType _vsxType)
    {

        currentAnimator = null;
        foreach (var item in vsxHolderArray)
        {
            item.gameObject.SetActive(false);
        }
        if (_vsxType == AreaDamageVSXType.Nothing)
        {
            Debug.Log("this?");
            return;
        }


        vsxHolderArray[(int)_vsxType].gameObject.SetActive(true);
        currentAnimator = vsxHolderArray[(int)_vsxType];

    }

   //

    #endregion

}

public enum AreaDamageVSXType
{
    Nothing = -1,
    Fireball_Explosion = 0
}