using DG.Tweening;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBear : TrapBase
{




    [Separator("BEAR TRAP")]
    [SerializeField] Transform trapClaw_01;
    [SerializeField] Transform trapClaw_02;
    [SerializeField] float damage;
    [SerializeField] float delay;
    [SerializeField] float range;


    public override void ResetForPool()
    {
        trapClaw_01.DOLocalRotate(new Vector3(0, 0, 0), 0);
        trapClaw_02.DOLocalRotate(new Vector3(0, 0, 0), 0);
    }

    public override void CallTrap()
    {
        StartCoroutine(TrapProcess());
    }

    protected override void ReleaseTrap()
    {
        GameHandler.instance._pool.Trap_Release(TrapType.BearTrap, this);
    }

    IEnumerator TrapProcess()
    {
        alreadyCalled = true;



        trapClaw_01.DOKill();
        trapClaw_01.DOLocalRotate(new Vector3(-70, 0, 0), delay);

        trapClaw_02.DOKill();
        trapClaw_02.DOLocalRotate(new Vector3(70, 0, 0), delay);

        GameHandler.instance._soundHandler.CreateSfx(SoundType.AudioClip_BearTrap, transform);

        yield return new WaitForSeconds(delay * 0.8f);

        bool isPlayerCLoseEnough = Vector3.Distance(transform.position, PlayerHandler.instance.transform.position) < range;

        if(isPlayerCLoseEnough)
        {
            PlayerResources playerResource = PlayerHandler.instance._playerResources;

            DamageClass damageClass = new DamageClass(damage, DamageType.Physical, 90);
            damageClass.Make_CannotDodge();

            BDClass bd = new BDClass("Beartrap", BDDamageType.Bleed, playerResource, 1, 4, 4);
            bd.MakeStack(3, false);
            bd.MakeTemp(3);


            playerResource.ApplyBD(bd);
            playerResource.TakeDamage(damageClass);

        }


        //go down and reset 
        transform.DOKill();
        transform.DOLocalMove(transform.position + new Vector3(0, -10, 0), 5);

        yield return new WaitForSeconds(5);




    }


    //we wait a second and deal damage if the player is close enough.

    //who can lay this? we should put in the pool.


}
