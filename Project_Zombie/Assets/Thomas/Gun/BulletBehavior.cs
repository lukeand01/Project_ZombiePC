using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Bullet / BulletDamageBase")]
public class BulletBehavior : ScriptableObject
{
    
    public virtual void ApplyContact(IDamageable target, DamageClass damage)
    {
        //this will apply the basic of dealing damage.
        //this will take a target and some reference to the wielder of this thing in case it wants to use 


        if (target == null)
        {
            Debug.Log("no target");
            return;
        } 
        if(damage == null)
        {
            Debug.Log("no damage");
            return;
        }

        target.TakeDamage(damage);

    }


}
