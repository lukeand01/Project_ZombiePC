using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMage : EnemyBase
{
    [Separator("MAGE")]
    [SerializeField] Transform[] eyeArray;
    [SerializeField] AreaDamage areaDamageTemplate;
    [SerializeField] float damageRadius;
    [SerializeField] float damageTimer;

    protected override void StartFunction()
    {
        UpdateTree(GetBehavior());
        base.StartFunction();
    }

    protected override void UpdateFunction()
    {
        RotateTarget(PlayerHandler.instance.transform.position);

        base.UpdateFunction();
    }

    Sequence2 GetBehavior()
    {
        return new Sequence2(new List<Node>
        {
            new BehaviorChase(this),
            new BehaviorCheckSight(this, eyeArray),
            new BehaviorAttack(this)
        });
    }


    public override void CallAttack()
    {
        //select an area.
        //in time it deals damage to area.
        //also create a warning for the player to see.

        GameHandler.instance._soundHandler.CreateSfx(data.audio_Attack, transform);

        Vector3 playerPosition = PlayerHandler.instance.transform.position;

        AreaDamage newObject = Instantiate(areaDamageTemplate, playerPosition, Quaternion.identity);
        newObject.SetUp(playerPosition, damageRadius, damageTimer, GetDamage(), 3);


        //base.CallAttack();
    }
}
