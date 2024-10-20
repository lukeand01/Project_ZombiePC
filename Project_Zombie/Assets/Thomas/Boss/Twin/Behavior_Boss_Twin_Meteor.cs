using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_Boss_Twin_Meteor : Sequence2
{

    EnemyBoss_Twin _boss;

    float _cooldown_Total;
    float _cooldown_Current;

    Transform _playerTransform;
    int _quantity;

    public Behavior_Boss_Twin_Meteor(EnemyBoss_Twin boss, float cooldown_Total, int quantity)
    {
        _boss = boss;
        _cooldown_Total = cooldown_Total;

        _playerTransform = PlayerHandler.instance.transform;
        _quantity = quantity;
    }

    public override NodeState Evaluate()
    {
        if (_boss.IsActing) return NodeState.Success;
        if (_cooldown_Current > 0)
        {
            _cooldown_Current -= Time.deltaTime;
            return NodeState.Success;
        }

        //call a meteor.
        Vector3 playerPosition = _playerTransform.position;
        float value = 5;

        for (int i = 0; i < _quantity; i++)
        {
            AreaDamage areaDamage = GameHandler.instance._pool.GetAreaDamage(_boss.transform);

            float x = Random.Range(-value, value);
            float z = Random.Range(-value, value);

            DamageClass newDamage = new DamageClass(50, DamageType.Physical, 0);
            areaDamage.SetUp_Regular(playerPosition + new Vector3(x, 0, z), 5, 5, newDamage, 3, 1, AreaDamageVSXType.Meteor);
        }




        SetOnCooldown();

        return NodeState.Success;
    }

    void SetOnCooldown()
    {
        _cooldown_Current = Random.Range(_cooldown_Total * 0.7f, _cooldown_Total * 1.2f);
    }

}
