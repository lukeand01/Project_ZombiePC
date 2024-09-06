using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Ability / Passive / Thunder")]
public class AbilityPassiveDataThunder : AbilityPassiveData
{
    [Separator("Thunder")]
    [SerializeField] float detectionRadius;

    public override void Add(AbilityClass ability)
    {
        base.Add(ability);

        //add the bd with the current value to the player

        BDClass bd = new BDClass("AbilityThunder", ability);
        bd.MakeTemp(10);
        bd.MakeShowInUI();
        PlayerHandler.instance._entityStat.AddBD(bd);

    }
    public override void Remove(AbilityClass ability)
    {
        base.Remove(ability);

        PlayerHandler.instance._entityStat.RemoveBdWithID("AbilityThunder");
    }
    public override void Call(AbilityClass ability)
    {
        base.Call(ability);

        //we have to check the distance of each one.

        int numberOfAttacks = ability.level;

        LayerMask enemyLayer = 6;
        enemyLayer = (1 << 6);

        //get enemies close to you. 
        Transform playerTransform = PlayerHandler.instance.transform;
        Collider[] hitColliders = Physics.OverlapSphere(playerTransform.position, detectionRadius, enemyLayer);

        for (int i = 0; i < numberOfAttacks; i++)
        {
            if(hitColliders.Length == i)
            {

                return;
            }

            IDamageable damageable = hitColliders[i].GetComponent<IDamageable>();

            if (damageable == null) continue;

            damageable.TakeDamage(new DamageClass(_firstValue, DamageType.Magical, 0));
        }



    }
}
