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

    protected override void StartFunction()
    {
        UpdateTree(GetBehavior());
        base.StartFunction();
    }

    protected override void UpdateFunction()
    {
        RotateTarget(PlayerHandler.instance.transform.position);

        base.UpdateFunction();
    }

    public override void ResetEnemyForPool()
    {
        base.ResetEnemyForPool();

        SetIsAttack(false);
        StopAllCoroutines();
    }

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

        yield return new WaitForSeconds(1);

        //call the attack

        for (int i = 0; i < 6; i++)
        {
            GameHandler.instance._soundHandler.CreateSfx(data.audio_Attack, transform);
            Vector3 playerPosition = PlayerHandler.instance.transform.position;

            float value = 1.8f;
            float x = Random.Range(-value, value);
            float z = Random.Range(-value, value);
            Vector3 offset = new Vector3(x, 0, z);

            AreaDamage newObject =  GameHandler.instance._pool.GetAreaDamage(transform);

            
            AreaDamageVSXType type = AreaDamageVSXType.Fireball_Explosion;
            Debug.Log("here " + type);
            newObject.SetUp(playerPosition + offset, damageRadius, damageTimer, GetDamage(), 3, 0.1f,type);

            float timerRandom = Random.Range(1, 1.5f);

            yield return new WaitForSeconds(timerRandom);
        }

        float cooldownRandom = Random.Range(3, 4);

        yield return new WaitForSeconds(cooldownRandom);

        SetIsAttack(false);

    }

    //the animation needs to fade. otherwise it looks weird.

}
