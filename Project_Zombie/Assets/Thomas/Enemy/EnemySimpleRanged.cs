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


    protected override void StartFunction()
    {
        UpdateTree(GetBehavior());
        base.StartFunction();
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

        GameHandler.instance._soundHandler.CreateSfx(data.audio_Attack, transform);

        Vector3 shootDir = PlayerHandler.instance.transform.position - transform.position;

        BulletScript newObject = Instantiate(bulletTemplate, shootingPos.position, Quaternion.identity);


        newObject.SetUp("SimpleRanged", shootDir);
        newObject.MakeEnemy();
        newObject.MakeSpeed(30, 0, 0);
        newObject.MakeDamage(GetDamage(), 0, 0);



    }

}
