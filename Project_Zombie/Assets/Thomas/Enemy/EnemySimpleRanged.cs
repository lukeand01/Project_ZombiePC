using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySimpleRanged : EnemyBase
{
    //
    [Separator("Simple Ranged")]
    [SerializeField] BulletScript bulletTemplate;
    [SerializeField] Transform[] eyeArray;

    private void Start()
    {
        UpdateTree(GetBehavior());
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
        Vector3 shootDir = transform.position - PlayerHandler.instance.transform.position;

        BulletScript newObject = Instantiate(bulletTemplate, transform.position, Quaternion.identity);
        newObject.SetUp("SimpleRanged", shootDir);
        newObject.MakeEnemy();
        newObject.MakeDamage(GetDamage(), 0, 0);


        base.CallAttack();
    }

}
