using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss_Twin_Small_Collider : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 3) return;

        DamageClass damage = new DamageClass(20, DamageType.Physical, 0);
        PlayerHandler.instance._playerResources.TakeDamage(damage);
        BDClass bd = new BDClass("Small_Speed", StatType.Speed, 0, 0, -0.4f);
        bd.MakeTemp(2);
        bd.MakeShowInUI();
        PlayerHandler.instance._playerResources.ApplyBD(bd);

    }
}
