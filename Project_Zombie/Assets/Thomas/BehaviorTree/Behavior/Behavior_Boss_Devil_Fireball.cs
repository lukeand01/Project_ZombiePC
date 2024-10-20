using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_Boss_Devil_Fireball : Sequence2
{

    EnemyBoss_Devil _boss;
    Transform _playerTransform;


    float _cooldown_Current;
    float _cooldown_Total;

    DamageClass _damage;

    public Behavior_Boss_Devil_Fireball(EnemyBoss_Devil boss)
    {
        _boss = boss;
        _playerTransform = PlayerHandler.instance.transform;


        _cooldown_Total = 10;
        _cooldown_Current = Random.Range(_cooldown_Total * 0.6f, _cooldown_Total * 1.3f) / 2;

        _damage = new DamageClass(60, DamageType.Magical, 0);

    }

    public override NodeState Evaluate()
    {
        if (_cooldown_Current > 0)
        {
            _cooldown_Current -= Time.deltaTime;
            return NodeState.Success;
        }

        if (_boss.IsActing) return NodeState.Success;
        

        Debug.Log("yo thissds");
        //float radius = AttackRadius;
        //int quantity = FireballQuantity;
        //i need to call animation first 


        //it needs to be a bit far.
        float distance = Vector3.Distance(_boss.transform.position, _playerTransform.position);
        if(distance > 10)
        {
            _boss.CallFireball();
            _cooldown_Current = Random.Range(_cooldown_Total * 0.6f, _cooldown_Total * 1.3f);
        }

        

        return NodeState.Success;
    }

    float AttackRadius
    {
        get
        {
            float radius = 0;

            if (_boss.currentPhase <= 1)
            {
                radius = 3;
            }
            if (_boss.currentPhase > 1)
            {
                radius = 5.5f;
            }

            return radius;
        }
    }


    int FireballQuantity
    {
        get
        {
            int quantity = 0;

            if (_boss.currentPhase <= 1)
            {
                quantity = 4;
            }
            if (_boss.currentPhase > 1)
            {
                quantity = 8;
            }

            return quantity;
        }
    }

}
