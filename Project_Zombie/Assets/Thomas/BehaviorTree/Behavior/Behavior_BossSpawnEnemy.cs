using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_BossSpawnEnemy : Sequence2
{

    EnemyBoss _boss;
    float _cooldown_Total;
    float _cooldown_Current;
    List<EnemyData> _enemyDataList;
    public Behavior_BossSpawnEnemy(EnemyBoss boss, float cooldown_Total,  List<EnemyData> enemyList)
    {
        _boss = boss;
        _cooldown_Total = cooldown_Total;
        _cooldown_Current = Random.Range(_cooldown_Total * 0.6f, _cooldown_Total * 1.3f);
        _enemyDataList = enemyList;
    }

    public override NodeState Evaluate()
    {
        if(_cooldown_Current > 0)
        {
            _cooldown_Current -= Time.deltaTime;
            return NodeState.Success;
        }
        if(_boss._bossPortal == null)
        {
            return NodeState.Success;
        }

        _boss._bossPortal.SendEnemyToSpawn(_enemyDataList);

        _cooldown_Current = Random.Range(_cooldown_Total * 0.6f, _cooldown_Total * 1.3f);

        return NodeState.Success;
    }


}
