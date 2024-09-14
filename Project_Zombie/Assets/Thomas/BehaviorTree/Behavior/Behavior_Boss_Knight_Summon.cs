using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_Boss_Knight_Summon : Sequence2
{
    EnemyBoss_Knight _boss;
    int _actionIndex;
    float _damage;

    float cooldown_Total; //
    float cooldown_Current;

    public Behavior_Boss_Knight_Summon(EnemyBoss_Knight _boss, float damage, int _actionIndex)
    {
        this._boss = _boss;
        this._actionIndex = _actionIndex;
        this._damage = damage;

        cooldown_Total = 2;
        cooldown_Current = 0;
    }

    public override NodeState Evaluate()
    {
        //we keep increase teh cooldown. even if we choose this fella we should reset if its not out of cooldown stil..
        if(cooldown_Current > 0)
        {
            cooldown_Current -= Time.deltaTime;
        }

        if (_boss.actionIndex_Current != _actionIndex) return NodeState.Success;

        if (cooldown_Current > 0)
        {
            return NodeState.Success;
        }

        _boss.StartAction("Summon", 2);

        
        float x = Random.Range(-1, 1);
        float z = Random.Range(-1, 1);
        Vector3 randomPos = new Vector3(x, 0, z);

        for (int i = 0; i < 3; i++)
        {
            var bullet = GameHandler.instance._pool.GetBullet(ProjectilType.FlyingSwords, _boss.transform);
            FlyingBlade _blade = bullet.GetFlyingBlade;
            bullet.gameObject.SetActive(true);

            if(i == 2)
            {
                _blade.SetUp_FlyingBlade(_damage, 150);
            }
            else
            {
                _blade.SetUp_FlyingBlade(_damage, 150);
                _blade.Make_FlyingBlade_RotateAroundTarget(_boss.transform, randomPos, 1);
                randomPos *= -1;
            }

        }

        cooldown_Current = cooldown_Total;

        return NodeState.Success;
    }

}
