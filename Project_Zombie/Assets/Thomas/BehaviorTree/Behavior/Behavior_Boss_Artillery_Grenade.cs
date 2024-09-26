using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_Boss_Artillery_Grenade : Sequence2
{

    EnemyBoss_Artillery _boss;

    float cooldown_Total;
    float cooldown_Current;


    public Behavior_Boss_Artillery_Grenade(EnemyBoss_Artillery boss)
    {
        _boss = boss;

        cooldown_Total = 2;
        cooldown_Current = Random.Range(cooldown_Total * 0.5f, cooldown_Total * 1.3f);
    }

    public override NodeState Evaluate()
    {

        if (cooldown_Current > 0)
        {
            cooldown_Current -= Time.deltaTime;
        }
        else
        {
            cooldown_Current = Random.Range(cooldown_Total * 0.5f, cooldown_Total * 1.3f);

            //then we pick a random fella from the ones carrying the 
            //we need to trigger a normal damage area.
            //i want something to do the arc

        }

        return NodeState.Success;
    }
}
