using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBomber : EnemyBase
{

    //the behavioor is the same but just the attack
    //
    [SerializeField] Animator _animator;
    LayerMask targetLayers;

    //its not showing the attack now for some reason.
    public bool isExploding {  get; private set; }

    float originalSpeed;

    protected override void StartFunction()
    {
        originalSpeed = _agent.speed;
        base.StartFunction();
        UpdateTree(GetBehavior());
        _enemyGraphicHandler.SelectRandomGraphic();
    }

    protected override void UpdateFunction()
    {
        //if (isExploding) return;

        float distance = Vector3.Distance(transform.position, PlayerHandler.instance.transform.position);

        if(distance <= data.attackRange)
        {
            CallAttack();
        }

        
        base.UpdateFunction();
    }

    public override void ResetEnemyForPool()
    {
        StopAllCoroutines();
        SetSpeed(originalSpeed);
        isExploding = false;
        _abilityIndicatorCanvas.StopCircleIndicator();
        base.ResetEnemyForPool();
        _enemyGraphicHandler.SelectRandomGraphic();
        
    }

    public override void CallAttack()
    {

        //instead of that i will call 
        if (isExploding) return;
        StartCoroutine(ExplodeProcess());
       
    }

    //once triggered reduce the speed.
    //i dont want to wait the attack.
    IEnumerator ExplodeProcess()
    {
        isExploding = true;

       
        _abilityIndicatorCanvas.StartCircleIndicator(data.attackRange * 1.2f);


        float total = 1.1f;
        float current = 0;

        //_entityAnimation.CallAnimation_Idle();
        //_animator.Play("Animation_Enemy_Attack_02", 2);

        while(total > current)
        {
            current += Time.deltaTime;
            _abilityIndicatorCanvas.ControlCircleFill(current, total);
            yield return new WaitForSeconds(Time.deltaTime);
        }


        GameHandler.instance._pool.GetPS(PSType.Explosion_02, transform);
        GameHandler.instance._soundHandler.CreateSfx(SoundType.AudioClip_Explosion_01);

        _abilityIndicatorCanvas.StopCircleIndicator();

        GameHandler.instance._soundHandler.CreateSfx_WithAudioClip(data.audio_Attack, transform);

        targetLayers |= (1 << 3);
        targetLayers |= (1 << 8);

        RaycastHit[] targets = Physics.SphereCastAll(transform.position, data.attackRange * 1.15f, Vector3.up, 0, targetLayers);

        DamageClass damage = GetDamage();

        PlayerHandler.instance.TryToCallExplosionCameraEffect(transform, 1);


        foreach (var item in targets)
        {
            IDamageable targetDamageable = item.collider.GetComponent<IDamageable>();

            if (targetIdamageable == null) continue;
            targetDamageable.TakeDamage(damage);
            //push it from teh palyer too
        }

        Die(false);

        

    }


    public override void CallAbilityIndicator(float current, float total)
    {
        base.CallAbilityIndicator(current, total);


        return;
        if(total <= 0)
        {
            _abilityIndicatorCanvas.StopCircleIndicator();
        }
        else
        {
            _abilityIndicatorCanvas.StartCircleIndicator(data.attackRange * 1.1f);
            _abilityIndicatorCanvas.ControlCircleFill(current, total);
        }

        

    }


    Sequence2 GetBehavior()
    {
        return new Sequence2(new List<Node>
        {
            new BehaviorChase(this),

        });
    }

}
