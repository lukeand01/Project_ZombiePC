using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMage : EnemyBase
{
    [Separator("MAGE")]
    [SerializeField] Transform[] eyeArray;
    [SerializeField] AreaDamage areaDamageTemplate;
    [SerializeField] float damageRadius;
    [SerializeField] float damageTimer;
    [SerializeField] Animator _animator;

    //i can just create my own system here. its bad but fuck it.


    protected override void StartFunction()
    {
        UpdateTree(GetBehavior());
        base.StartFunction();
    }

    protected override void UpdateFunction()
    {

        

        if (!IsDead())
        {
            RotateTarget(PlayerHandler.instance.transform.position);


            if (_agent.velocity == Vector3.zero)
            {
                _animator.Play("Animation_Enemy_Idle_03", 0);
            }
        }

        

        base.UpdateFunction();
    }

    public override void ResetEnemyForPool()
    {
        base.ResetEnemyForPool();

        SetIsAttack(false);
        StopAllCoroutines();
    }




    public override void CallAttack()
    {
        //select an area.
        //in time it deals damage to area.
        //also create a warning for the player to see.
       

        StartCoroutine(AttackProcess());
        //base.CallAttack();
    }

    IEnumerator AttackProcess()
    {
        SetIsAttack(true);
        StopAgent();

        //yield return new WaitForSeconds(1);

        //call the attack

        for (int i = 0; i < 6; i++)
        {
            GameHandler.instance._soundHandler.CreateSfx(SoundType.AudioClip_MeteorExplosion, transform);
            Vector3 playerPosition = PlayerHandler.instance.transform.position;

            float value = 5;
            float x = Random.Range(-value, value);
            float z = Random.Range(-value, value);
            Vector3 offset = new Vector3(x, 0, z);

            AreaDamage newObject =  GameHandler.instance._pool.GetAreaDamage(transform);

            
            AreaDamageVSXType type = AreaDamageVSXType.Fireball_Explosion;

            newObject.SetUp_Regular(playerPosition + offset, damageRadius, damageTimer, GetDamage(), 3, 0.1f,type);
            
            float timerRandom = Random.Range(0.4f, 0.6f);

            yield return new WaitForSeconds(timerRandom);
        }

        float cooldownRandom = Random.Range(3, 4);


        _animator.Play("Animation_Enemy_Idle_03", 2);

        yield return new WaitForSeconds(cooldownRandom);


        Debug.Log("truly done");
        SetIsAttack(false);
        SetIsAttacking_Animation(false);
    }

    Sequence2 GetBehavior()
    {
        return new Sequence2(new List<Node>
        {
            new BehaviorChase(this),
            new BehaviorCheckSight(this, eyeArray),
            new BehaviorAttack(this)

        });
    }
}
