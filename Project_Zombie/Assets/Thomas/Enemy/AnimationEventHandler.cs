using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{

    //now we can more carefuly hand this stuff

    EnemyBase _enemy;
    [SerializeField] EnemyBoss _enemyBoss;
    private void Awake()
    {
        _enemy = transform.parent.parent.GetComponent<EnemyBase>();
    }

    public void CallAttack()
    {
        _enemy.CallAttack();
    }

    public void AnimationEnded()
    {

        _enemy.SetIsAttacking_Animation(false);
    }

    public void StartAttackCharge_Boss()
    {
        //we stop the animation and we hold it.
        //after the time is done we allow the attack
        _enemyBoss.StartChargingAttack();
    }

    public void CalculateAttackDirectly_Boss()
    {
        _enemyBoss.CalculateAttack();
    }

    public void AnimationEnded_Boss()
    {
        _enemyBoss.StopAction();
    }
}
