using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    DamageClass _damage;
    public void SetUp(DamageClass _damage)
    {
        this._damage = _damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;

        PlayerHandler.instance._playerResources.TakeDamage(_damage);
    }


}
