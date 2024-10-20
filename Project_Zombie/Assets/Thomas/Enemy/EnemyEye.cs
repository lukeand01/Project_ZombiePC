using DG.Tweening;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEye : EnemyBase
{
    [Separator("EYE")]
    [SerializeField] Transform[] eyeArray;
    [SerializeField] LineRenderer _lineRend;
    [SerializeField] LineRenderer _lineRend2;
    [SerializeField] GameObject _graphicHolder;
    [SerializeField] Animator _animator;
    [SerializeField] AudioSource _audioSource_Aim;
    [SerializeField] AudioSource _audioSource_Shoot;
    LayerMask wallAndPlayerLayer;

    protected override void StartFunction()
    {

        wallAndPlayerLayer |= (1 << 3);
        wallAndPlayerLayer |= (1 << 7);
        wallAndPlayerLayer |= (1 << 9);

        UpdateTree(GetBehavior());
        base.StartFunction();

        timeToStartDealingDamage_Total = 6;
        intervalBetweenDamage_Total = 2;
        damage = new DamageClass(1, DamageType.Pure, 0);

        _lineRend.positionCount = 2;
    }

    //


    Sequence2 GetBehavior()
    {
        return new Sequence2(new List<Node>
        {
            new BehaviorChase(this),
            new BehaviorCheckSight(this, eyeArray),
            new BehaviorAttack_WaitForAttackToComplete(this)

        });
    }

    public override void CallAttack()
    {
        base.CallAttack();

        StopAgent();
        SetIsAttack(true);
        //while its staring at the player i want it to stare at the player. 
        //starts doing damage to the player. cause debuff?
        
    }


    bool HasEyesOnPlayer()
    {
        foreach (var item in eyeArray)
        {
            Vector3 targetPos = (PlayerHandler.instance.transform.position - item.position).normalized;
            Ray ray = new Ray(item.position, targetPos);

            if (Physics.Raycast(ray, out RaycastHit hit, 50, wallAndPlayerLayer))
            {
                if (hit.collider.tag != "Player")
                {
                    return false;
                }

            }
            else
            {
                return false;
            }
        }

        return true;


    }

    public override void ResetEnemyForPool()
    {
        base.ResetEnemyForPool();
        
        timeToStartDealingDamage_Current = 0;
        intervalBetweenDamage_Current = 0;

        _graphicHolder.transform.localPosition = new Vector3(0, 2, 0);

        _animator.SetLayerWeight(1, 0);
        _animator.Play("Animation_Eye_Idle", 1);
    }

    DamageClass damage;

    float timeToStartDealingDamage_Current;
    float timeToStartDealingDamage_Total;

    float intervalBetweenDamage_Current;
    float intervalBetweenDamage_Total;

    protected override void UpdateFunction()
    {

        if(timeToStartDealingDamage_Current > 0 && timeToStartDealingDamage_Current < timeToStartDealingDamage_Total)
        {
            _audioSource_Aim.gameObject.SetActive(true);
        }
        else
        {
            _audioSource_Aim.gameObject.SetActive(false);
        }

        if(timeToStartDealingDamage_Current >= timeToStartDealingDamage_Total)
        {
            _audioSource_Shoot.gameObject.SetActive(true);
        }
        else
        {
            _audioSource_Shoot.gameObject.SetActive(false);
        }


        base.UpdateFunction();

        if(isAttacking)
        {

            //rotate the _enemy to the player
            Vector3 direction = PlayerHandler.instance.transform.position - transform.position;
            Vector3 directionNormalized = direction.normalized;
            Quaternion targetRotation = Quaternion.LookRotation(directionNormalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1 * Time.fixedDeltaTime);



            if (HasEyesOnPlayer())
            {
                UpdateColorGradient();

                _lineRend.gameObject.SetActive(true);
                _lineRend.SetPosition(0, eyeArray[1].position);
                _lineRend.SetPosition(1, PlayerHandler.instance.transform.position);


                if(timeToStartDealingDamage_Total > timeToStartDealingDamage_Current)
                {
                    timeToStartDealingDamage_Current += Time.fixedDeltaTime;
                }
                else
                {
                    //we deal damage to the player. very little damage.

                    timeToStartDealingDamage_Current = timeToStartDealingDamage_Total;

                    if(intervalBetweenDamage_Total > intervalBetweenDamage_Current)
                    {
                        intervalBetweenDamage_Current += Time.fixedDeltaTime;   
                    }
                    else
                    {
                        PlayerHandler.instance._playerResources.TakeDamage(damage);
                        intervalBetweenDamage_Current = 0;
                    }

                    


                }
            }
            else
            {
                StopEyeAttack();
            }

        }
        else
        {
            StopEyeAttack();
            
        }

    }

    public override void Die(bool wasKilledByPlayer = true)
    {
        
        base.Die(wasKilledByPlayer);
        _animator.SetLayerWeight(1, 1);
        _animator.Play("Animation_Eye_Death_00", 1);
        _graphicHolder.transform.DOLocalMove(new Vector3(0,-0.25f,0), 0.5f);
    }
    void StopEyeAttack()
    {
        timeToStartDealingDamage_Current = 0;
        intervalBetweenDamage_Current = 0;
        SetIsAttack(false);
        _lineRend.gameObject.SetActive(false);


    }

    //we slowly move 


    void UpdateColorGradient()
    {
        float value = timeToStartDealingDamage_Current / timeToStartDealingDamage_Total;

        Vector3 newPos = Vector3.Lerp(eyeArray[1].position, PlayerHandler.instance.transform.position, value); 



        _lineRend2.SetPosition(0, eyeArray[1].position);
        _lineRend2.SetPosition(1, newPos);

        Gradient gradient = new Gradient();
        gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(Color.white, 0) },
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 1), new GradientAlphaKey(1.0f, 1) }
            );

        //it pulsate based on the time for every attack.

        float value_Width = intervalBetweenDamage_Current / intervalBetweenDamage_Total;
        value_Width *= 0.1f;
        value_Width *= 0.8f;


        _lineRend2.startWidth = 0.1f + value_Width;
        _lineRend2.endWidth = 0.1f + value_Width;

        //i want it to become complete

        _lineRend.colorGradient = gradient;
    }


}
