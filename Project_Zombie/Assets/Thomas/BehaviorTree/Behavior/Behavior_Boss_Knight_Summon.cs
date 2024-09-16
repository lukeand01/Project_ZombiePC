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
            _boss.SelectRandomAction(); //we reeselect an action.
            return NodeState.Success;
        }

        _boss.StartAction("Summon", 2);
        _boss.CallAnimation("Idle", 1);


        Vector3 increment = new Vector3(1, 0, 0);
        


        for (int i = 0; i < 2; i++)
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
                _blade.SetUp_FlyingBlade(_damage, 50);
                _blade.Make_FlyingBlade_RotateAroundTarget(_boss.transform, increment, 16, 1.2f);
                increment *= -1;
                
            }

        }

        cooldown_Current = cooldown_Total;

        return NodeState.Success;
    }

}
