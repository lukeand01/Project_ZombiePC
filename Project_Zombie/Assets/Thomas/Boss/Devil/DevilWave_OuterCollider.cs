using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilWave_OuterCollider : MonoBehaviour
{

    //this deals damage.


    bool _alreadyDamaged;
    DamageClass _damage;
    private void Start()
    {
        _damage = new DamageClass(70, DamageType.Physical, 0);
    }

    private void OnDisable()
    {
        _alreadyDamaged = false;
    }

    public void SetAlreadyDamaged()
    {
        _alreadyDamaged = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_alreadyDamaged) return;



        if(other.gameObject.layer == 3 && !PlayerHandler.instance._entityStat.IsImmune)
        {
            Debug.Log("player");

            PlayerHandler.instance._playerResources.TakeDamage(_damage);
            _alreadyDamaged = true;
        }
    }



}
