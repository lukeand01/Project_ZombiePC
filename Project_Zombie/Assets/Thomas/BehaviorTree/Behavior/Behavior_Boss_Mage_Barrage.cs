using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_Boss_Mage_Barrage : Sequence2
{

    EnemyBoss_Mage _boss;

    float cooldown_Current;
    float cooldown_Total;

    public Behavior_Boss_Mage_Barrage(EnemyBoss_Mage boss)
    {
        _boss = boss;


        cooldown_Total = 1;
        //cooldown_Current = Random.Range(cooldown_Total * 0.5f, cooldown_Total * 1.3f);
        cooldown_Current = 1;
    }

    public override NodeState Evaluate()
    {

        if (_boss.isRunningSeeker) return NodeState.Success;
        if (_boss.IsActing) return NodeState.Success;


        if (cooldown_Current <= 0)
        {
            //shoot the projectile.
            _boss.SetActionIndexCurrent(2);
            _boss.StartAction("CastSpell", 1);

            cooldown_Current = Random.Range(cooldown_Total * 0.5f, cooldown_Total * 1.3f);
        }
        else
        {
            cooldown_Current -= Time.deltaTime;
        }


        return NodeState.Success;
    }


}
