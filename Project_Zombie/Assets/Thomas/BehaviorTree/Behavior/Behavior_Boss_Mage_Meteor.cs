using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_Boss_Mage_Meteor : Sequence2
{
    EnemyBoss_Mage _boss;
    Transform[] _eyesArray;
    float _range;
    int _actionIndex;
    Transform playerTransform;
    LayerMask wallAndPlayerTargetLayer;

    float cooldown_Total;
    float cooldown_Current;

    public Behavior_Boss_Mage_Meteor(EnemyBoss_Mage boss, Transform[] eyesArray)
    {
        _boss = boss;
        _eyesArray = eyesArray;

        _range = 50;
        _actionIndex = 3;

        playerTransform = PlayerHandler.instance.transform;

        cooldown_Total = 5;


        wallAndPlayerTargetLayer |= (1 << 3);
        wallAndPlayerTargetLayer |= (1 << 7);
        wallAndPlayerTargetLayer |= (1 << 9);
    }


    //this will check sight.
    //if not in sight then we will check for cooldown, if possible then shoot a meteor
    //if is in sight go to the next
    //



    public override NodeState Evaluate()
    {
        //if its not in


        if (_boss.IsActing) return NodeState.Success;

        if (_eyesArray.Length == 0)
        {
            Debug.Log("no eye array");
        }

        int eyeDetecting = 0;

        for (int i = 0; i < _eyesArray.Length; i++)
        {
            var item = _eyesArray[i];

            Vector3 targetPos = (playerTransform.position - item.position).normalized;
            Ray ray = new Ray(item.position, targetPos);

            if (Physics.Raycast(ray, out RaycastHit hit, 50, wallAndPlayerTargetLayer))
            {
                if (hit.collider.tag == "Player")
                {
                    eyeDetecting++;
                }
            }

        }



        if(eyeDetecting == 0)
        {
            //this means the player is completely behind a wall.
            HandleShootingMeteor();
            return NodeState.Failure;
        }
        
        if(eyeDetecting <= 2)
        {
            return NodeState.Failure;
        }

        return NodeState.Success;
    }

    void HandleShootingMeteor()
    {
        //then 
        if(cooldown_Current > cooldown_Total)
        {
            AreaDamage areaDamage = GameHandler.instance._pool.GetAreaDamage(_boss.transform);
            Vector3 playerPosition = playerTransform.position;
            DamageClass newDamage = new DamageClass(50, DamageType.Physical, 0);
            areaDamage.SetUp_Regular(playerPosition, 5, 5, newDamage, 3, 1, AreaDamageVSXType.Meteor);

            cooldown_Current = 0;
        }
        else
        {
            cooldown_Current += Time.deltaTime;
        }
        


    }


}
