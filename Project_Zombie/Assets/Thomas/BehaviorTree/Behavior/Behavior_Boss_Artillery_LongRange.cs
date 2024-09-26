using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_Boss_Artillery_LongRange : Sequence2
{
    EnemyBoss_Artillery _boss;
    Transform[] _eyesArray;
    Transform _playerTransform;
    LayerMask wallAndPlayerTargetLayer;

    float cooldown_Total;
    float cooldown_Current;

    public Behavior_Boss_Artillery_LongRange(EnemyBoss_Artillery boss, Transform[] eyeArray)
    {
        _boss = boss;
        _eyesArray = eyeArray;

        _playerTransform = PlayerHandler.instance.transform;

        wallAndPlayerTargetLayer |= (1 << 3);
        wallAndPlayerTargetLayer |= (1 << 7);
        wallAndPlayerTargetLayer |= (1 << 9);


        cooldown_Total = 5;
        cooldown_Current = Random.Range(cooldown_Total * 0.6f, cooldown_Total * 1.3f);
    }

    public override NodeState Evaluate()
    {



        int eyeDetecting = 0;

        for (int i = 0; i < _eyesArray.Length; i++)
        {
            var item = _eyesArray[i];

            Vector3 targetPos = (_playerTransform.position - item.position).normalized;
            Ray ray = new Ray(item.position, targetPos);

            if (Physics.Raycast(ray, out RaycastHit hit, 50, wallAndPlayerTargetLayer))
            {
                if (hit.collider.tag == "Player")
                {

                    eyeDetecting++;
                }
            }

        }

        if (eyeDetecting >= 2) return NodeState.Success;

        //if it gets here is because we can shoot.

        _boss.ChangeCombatState(false);

        if(cooldown_Current > 0)
        {
            cooldown_Current -= Time.deltaTime;
        }
        else if(_boss.CanShootProjectil() != -1)
        {
            //we shoot a custom damagearea.
            _boss.ShootProjectil();
            cooldown_Total = 5;
            cooldown_Current = Random.Range(cooldown_Total * 0.6f, cooldown_Total * 1.3f);
        }

        return NodeState.Success;
    }

}
