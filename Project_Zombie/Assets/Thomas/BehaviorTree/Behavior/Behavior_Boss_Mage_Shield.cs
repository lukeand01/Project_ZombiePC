using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_Boss_Mage_Shield : Sequence2
{

    EnemyBoss_Mage _boss;

    float cooldown_Current;
    float cooldown_Total;

    public Behavior_Boss_Mage_Shield(EnemyBoss_Mage boss)
    {
        _boss = boss;
    }

    //call 
    public override NodeState Evaluate()
    {

        if (_boss.IsActing) return NodeState.Success;


        if (cooldown_Current > cooldown_Total)
        {
            //shoot the projectile.
            cooldown_Current = 0;
        }
        else
        {
            cooldown_Current += Time.deltaTime;
        }


        return NodeState.Success;
    }

}
