using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_Boss_Artillery_ShortRange : Sequence2
{

    EnemyBoss_Artillery _boss;

    float cooldown_Total;
    float cooldown_Current;


    public Behavior_Boss_Artillery_ShortRange(EnemyBoss_Artillery boss)
    {
        _boss = boss;

        cooldown_Total = 2;
        cooldown_Current = Random.Range(cooldown_Total * 0.5f, cooldown_Total * 1.3f);
    }

    public override NodeState Evaluate()
    {
        //on cooldown we will simply shoot a bullet forward.

        if(cooldown_Current > 0)
        {
            cooldown_Current -= Time.deltaTime;
        }
        else
        {
            cooldown_Current = Random.Range(cooldown_Total * 0.5f, cooldown_Total * 1.3f);

            //shoot a ball forward.
            //
        }


        return NodeState.Success;
    }

}
