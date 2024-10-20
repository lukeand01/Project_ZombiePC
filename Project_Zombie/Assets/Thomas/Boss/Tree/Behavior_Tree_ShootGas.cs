using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_Tree_ShootGas : Sequence2
{

    //shoot just one always facing
    //

    EnemyBoss_Tree _boss;
    float _cooldown_Total;
    float _cooldown_Current;
    bool _shootForward;

    public Behavior_Tree_ShootGas(EnemyBoss_Tree boss, float cooldown_Total,  bool shootForward)
    {
        _boss = boss;
        _cooldown_Total = cooldown_Total;
        _shootForward = shootForward;
    }

    public override NodeState Evaluate()
    {
        //

        if(_cooldown_Current > 0)
        {
            _cooldown_Current -= Time.deltaTime;
            return NodeState.Success;
        }

        BulletScript bullet = GameHandler.instance._pool.GetBullet(ProjectilType.EnemySpit, _boss.shootPos.transform) ;
        Vector3 dir = Vector3.zero;

        if(_shootForward)
        {
            Vector3 shootDir = PlayerHandler.instance.transform.position - _boss.transform.position;
            shootDir.y = 0;
            dir = shootDir;
        }
        else
        {
            float randomX = Random.Range(-1, 1);
            float randomZ = Random.Range(-1, 1);

            dir = new Vector3(randomX, 0, randomZ);
        }


        bullet.MakeEnemy();
        bullet.MakeSpeed(2, 0, 0);
        bullet.MakeCollision(999);
        bullet.MakeDamage(new DamageClass(90, DamageType.Physical, 0), 0, 0);
        bullet.transform.localScale = Vector3.one * 3;

        bullet.SetUp("Shootgas", dir, 15);


        PutOnCooldown();

        return NodeState.Success;
    }

    void PutOnCooldown()
    {
        _cooldown_Current = Random.Range(_cooldown_Total * 0.7f, _cooldown_Total * 1.2f);
    }

}
