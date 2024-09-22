using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_Boss_Mage_Shoot : Sequence2
{

    EnemyBoss_Mage _boss;

    //she wont ahve many animations
    float cooldown_Current;
    float cooldown_Total;

    float minDistance = 0;
    //the mage will be flying, so no animation for legs.
    //



    public Behavior_Boss_Mage_Shoot(EnemyBoss_Mage boss)
    {
        _boss= boss;

        
        cooldown_Total = 2;
        cooldown_Current = cooldown_Total;

        minDistance = 10;


    }

    public override NodeState Evaluate()
    {
        //we need to show somehow 
        //this attack is a big and slow dark sphere.

        float distance = Vector3.Distance(_boss.transform.position, PlayerHandler.instance.transform.position);

        if(distance <= minDistance)
        {
            _boss.StopAgent();
        }

        if (_boss.IsActing) return NodeState.Success;

       

        if(cooldown_Current > cooldown_Total)
        {
            Vector3 shootDir = PlayerHandler.instance.transform.position - _boss.transform.position;

            BulletScript newObject = GameHandler.instance._pool.GetBullet(ProjectilType.DarkOrb, _boss.transform);

            newObject.MakeEnemy();
            newObject.MakeSpeed(12, 0, 0);
            newObject.MakeDamage(new DamageClass(250, DamageType.Physical, 0), 0, 0);
            newObject.SetUp("MinibossMage", shootDir);

            cooldown_Current = 0;

            Debug.Log("shoot this");
        }
        else
        {
            cooldown_Current += Time.deltaTime;
        }



        return NodeState.Success;
    }
}


//but the amge should never try to get close.
//