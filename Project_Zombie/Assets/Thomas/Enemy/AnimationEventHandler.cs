using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{

    //now we can more carefuly hand this stuff

    EnemyBase _enemy;

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

}
