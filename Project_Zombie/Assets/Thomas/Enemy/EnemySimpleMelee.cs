using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySimpleMelee : EnemyBase
{
    [SerializeField] bool shouldOnlyMoveWhenFacing;

    [SerializeField] int attackLayer = 2;
    [SerializeField] bool isHound;

    float _cooldown_Bark;


    protected override void AwakeFunction()
    {
        base.AwakeFunction();     

    }

    protected override void UpdateFunction()
    {
        base.UpdateFunction();

        if (isHound)
        {
            if(_cooldown_Bark > 0)
            {
                _cooldown_Bark -= Time.deltaTime;
            }
            else
            {
                int random = Random.Range(0, 2);

                if(random == 0)
                {
                    GameHandler.instance._soundHandler.CreateSfx(SoundType.AudioClip_Bark_01, transform);
                }
                if(random >= 1)
                {
                    GameHandler.instance._soundHandler.CreateSfx(SoundType.AudioClip_Bark_02, transform);
                }

                Debug.Log("called it " + random);
                _cooldown_Bark = Random.Range(10, 15);
            }
        }


        if(shouldOnlyMoveWhenFacing && isMoving)
        {
            //we check if we are facing 

            float angle = Vector3.Angle(transform.forward, currentAgentTargetPosition);

            // Check if the angle is within the threshold
            if (angle < 15)
            {
                Debug.Log("Agent is facing the target.");
            }
            else
            {
                Debug.Log("Agent is not facing the target.");
            }

        }
    }

    public override void ResetEnemyForPool()
    {
        base.ResetEnemyForPool();
        _enemyGraphicHandler.SelectRandomGraphic();

        _cooldown_Bark = 0;
    }

    protected override void StartFunction()
    {
        UpdateTree(GetBehavior());
        _enemyGraphicHandler.SelectRandomGraphic();
        base.StartFunction();
    }

    Sequence2 GetBehavior()
    {
        return new Sequence2(new List<Node>
        {
            new BehaviorChase(this),
            new BehaviorAttack(this, attackLayer)

        });
    }


}
