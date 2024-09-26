using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artillery_DamageArea : MonoBehaviour
{
    //i will hard code two scenarios here.
    //
    [SerializeField] EnemyBoss_Artillery _bossArtillery;
    [SerializeField] AbilityIndicatorCanvas _abilityCanvas;
    [SerializeField] GameObject _shellObject;
    DamageClass _damage;

    LayerMask _targetLayer;


    float _uiTimer_Total;
    float _uiTimer_Current;
    float _radius;
    float _radius_Explosion;

    bool _isSecondExplosion;

    float _explosion_Current;
    float _explosion_Total;


    public void Set_Explosion(float damageTimer, float radius)
    {
        _shellObject.transform.localPosition = new Vector3(0,20,0);
        _shellObject.transform.DOLocalMove(Vector3.zero, damageTimer).SetEase(Ease.Linear).OnComplete(FirstExplosion);

        _radius = radius;
        _abilityCanvas.StartCircleIndicator(radius);

        _radius_Explosion = radius * 2;


        _targetLayer |= (1 << 3);

    }

    void FirstExplosion()
    {
        PlayerHandler.instance.TryToCallExplosionCameraEffect(transform, 1);
        DealDamage(_radius);
        GameHandler.instance._pool.GetPS(PSType.Explosion_01, transform);
        Invoke(nameof(TriggerSecondExplosion), 1);
    }

    void TriggerSecondExplosion()
    {
        _isSecondExplosion = true;
        _explosion_Current = 0;
        _explosion_Total = 5;
        

    }

    void SecondExplosion()
    {
        DealDamage(_radius_Explosion);
        GameHandler.instance._pool.GetPS(PSType.Explosion_02, transform);

        Invoke(nameof(Complete), 0.5f);
    }

    //then we start the next timer.



    void DealDamage(float radius)
    {
        //we check in this area and deal damage to those you cannot.

        if (_damage == null) return;

        RaycastHit[] targets = Physics.SphereCastAll(transform.position, radius, Vector3.up, 0, _targetLayer);

        foreach (var item in targets)
        {
            IDamageable targetDamageable = item.collider.GetComponent<IDamageable>();

            if (targetDamageable == null) continue;
            targetDamageable.TakeDamage(_damage);

        }


    }


    void Complete()
    {
        //then we finish everything and return to the artillery
        _isSecondExplosion = false;
        _uiTimer_Current = 0;
        _explosion_Current = 0;
        _bossArtillery.ReturnProjectil(this);
        
    }

    //we need to call a huge explosion
    //and a smaller explosion for the first one.

    private void Update()
    {

        if (_isSecondExplosion)
        {
            if(_explosion_Current > _explosion_Total)
            {
                //then we deal damage.
                //we explode
                //and after a timer 

                _isSecondExplosion = false;
                SecondExplosion();
            }
            else
            {
                _explosion_Current += Time.deltaTime;
                _abilityCanvas.ControlCircleFill(_explosion_Current, _explosion_Total);
            }


            return;
        }
        


        if(_uiTimer_Current > _uiTimer_Total)
        {

        }
        else
        {
            _uiTimer_Current += Time.deltaTime;
            _abilityCanvas.ControlCircleFill(_uiTimer_Current, _uiTimer_Total);
        }
    }

}
