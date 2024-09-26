using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{

    //now we can more carefuly hand this stuff
    [SerializeField] bool ignoreEnemy;
    EnemyBase _enemy;
    [SerializeField] EnemyBoss _enemyBoss;
    private void Awake()
    {
        if (ignoreEnemy) return;

         _enemy = transform.parent.parent.GetComponent<EnemyBase>();
        
        
        Debug.Log("here");
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


    public void TriggerFootStepSound(int index)
    {
        //we have to inform whoever is here about this.


        if(_enemyBoss != null)
        {
            _enemyBoss.HandleFootStep(index);
        }
    }


    [Separator("SOUND")]
    [SerializeField] SoundType _sound;
    public void CallSoundThroughAnimation()
    {
        GameHandler.instance._soundHandler.CreateSfx(_sound, transform);
    }

    //

}



//how should we aim to trigger it?


//doing through animation is out of question - because i want the effect to remain after the animation is done.
//