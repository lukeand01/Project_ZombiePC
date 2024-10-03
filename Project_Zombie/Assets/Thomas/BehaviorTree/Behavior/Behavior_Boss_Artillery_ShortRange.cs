using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_Boss_Artillery_ShortRange : Sequence2
{

    EnemyBoss_Artillery _boss;

    float cooldown_Total;
    float cooldown_Current;

    DamageClass _damage;
    public Behavior_Boss_Artillery_ShortRange(EnemyBoss_Artillery boss)
    {
        _boss = boss;

        cooldown_Total = 2;
        cooldown_Current = Random.Range(cooldown_Total * 0.5f, cooldown_Total * 1.3f);


        _damage = new DamageClass(200, DamageType.Physical, 0);
    }

    public override NodeState Evaluate()
    {
        //on cooldown we will simply shoot a bullet forward.



        _boss.ChangeCombatState(true);

        if(cooldown_Current > 0)
        {
            cooldown_Current -= Time.deltaTime;
        }
        else
        {



            cooldown_Current = Random.Range(cooldown_Total * 0.5f, cooldown_Total * 1.3f);

            Vector3 shootDir = PlayerHandler.instance.transform.position - _boss.transform.position;

            _boss.CallShootAudio();
            

            BulletScript newBullet = GameHandler.instance._pool.GetBullet(ProjectilType.ArtillerySlow, _boss.shootPos);
            Quaternion targetRotation = Quaternion.LookRotation(shootDir.normalized);
            newBullet.transform.DORotate(targetRotation.eulerAngles, 0);

            newBullet.MakeSpeed(10, 0, 0);
            newBullet.MakeDamage(_damage, 0, 0);
            newBullet.MakeEnemy();
            newBullet.SetUp("Artillery", shootDir);

            //shoot a ball forward.
            //
        }


        return NodeState.Success;
    }

}
