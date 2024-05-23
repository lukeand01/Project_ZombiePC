using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Bullet / Stun")]
public class BulletBehavior_Stun : BulletBehavior
{
    [Range(0, 1)][SerializeField] float stunChance;


    public override void ApplyContact(IDamageable target, DamageClass damage)
    {

        int roll = Random.Range(0, 101);


        if(stunChance * 100 > roll )
        {
            BDClass bd = new BDClass("BulletStun", BDType.Stun, 0.5f);
            target.ApplyBD(bd);
        }




    }

}
