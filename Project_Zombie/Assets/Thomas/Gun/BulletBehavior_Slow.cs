using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Bullet / BulletSlow")]
public class BulletBehavior_Slow : BulletBehavior
{
    [Range(0,1)][SerializeField] float slotPercent = 0.3f;

    public override void ApplyContact(IDamageable target, DamageClass damage)
    {
        //it apply a slow debuff to the target.

        BDClass bd_Slow = new BDClass("BulletBehaviorSnow", StatType.Speed, 0, -slotPercent,0);
        bd_Slow.MakeTemp(2f);
        bd_Slow.MakeShowInUI();
        target.ApplyBD(bd_Slow);



    }
}
