using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArtillery : EnemyBase
{

    [Separator("ARTILLERY")]
    [SerializeField] AreaDamage areaDamageTemplate;
    [SerializeField] float damageRadius;
    [SerializeField] float damageTimer;
    [SerializeField] AudioClip artilleryClip;

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
            new BehaviorAttack(this)
        });
    }

    public override void CallAttack()
    {

        CreateAudioSource(artilleryClip);

        Vector3 playerPosition = PlayerHandler.instance.transform.position;

        AreaDamage newObject = Instantiate(areaDamageTemplate, playerPosition, Quaternion.identity);
        newObject.SetUp(playerPosition, damageRadius, damageTimer, GetDamage(), 3);

    }
}
