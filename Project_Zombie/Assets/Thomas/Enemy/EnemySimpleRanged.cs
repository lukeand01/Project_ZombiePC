using MyBox;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemySimpleRanged : EnemyBase
{
    //
    [Separator("Simple Ranged")]
    [SerializeField] BulletScript bulletTemplate;
    [SerializeField] Transform[] eyeArray;
    [SerializeField] Transform shootingPos;

    bool canShoot = false;

    public override void ResetEnemyForPool()
    {
        base.ResetEnemyForPool();
        _enemyGraphicHandler.SelectRandomGraphic();
    }
    protected override void StartFunction()
    {
        UpdateTree(GetBehavior());
        _enemyGraphicHandler.SelectRandomGraphic();
        base.StartFunction();
    }

    protected override void CreateKeyForAnimation_Attack()
    {
        _entityAnimation.AddEnemyID("Attack", 3, 3); //IT CAN ONLY USE THE ANIMATION 3
    }

    protected override void UpdateFunction()
    {
        base.UpdateFunction();

        canShoot = RotateTarget(PlayerHandler.instance.transform.position);

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

    public override void CallAttack()
    {


        //but can only attack if we facing the target.
        if (!canShoot)
        {
            return;
        }

        GameHandler.instance._soundHandler.CreateSfx_WithAudioClip(data.audio_Attack, transform);

        Vector3 shootDir = PlayerHandler.instance.transform.position - transform.position;

        BulletScript newObject = GameHandler.instance._pool.GetBullet(ProjectilType.EnemySpit, shootingPos);

        newObject.MakeEnemy();
        newObject.SetUp("SimpleRanged", shootDir);
       
        newObject.MakeSpeed(30, 0, 0);
        newObject.MakeDamage(GetDamage(), 0, 0);



    }

}
