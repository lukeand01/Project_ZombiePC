using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FireballProjectil : MonoBehaviour
{

    LayerMask enemyLayer;
    float damageModifier = 0;
    DamageType damageType;
    Vector3 dir;
    float speed = 25;
    bool canMove = false;

    private void Awake()
    {
        enemyLayer |= (1 << 6);
        speed = 20;
    }

    private void Start()
    {
        
    }

    public void SetUp(Vector3 dir, float damageModifier)
    {
        canMove = true;
        this.dir = dir; 
        this.damageModifier = damageModifier;
    }

    private void Update()
    {
        if (!canMove) return;
        transform.position += dir.normalized * speed * Time.deltaTime;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 6 && other.gameObject.layer != 9) return;

        DamageClass damage = new DamageClass(damageModifier, damageType, 0);
        DamageClass damageSecond = new DamageClass(damageModifier * 0.5f, damageType, 0);
        float burnDamageModifier = 1;

        //BDClass bd = new BDClass("Fireball", BDDamageType.Burn)


        IDamageable damageable = other.GetComponent<IDamageable>();

        string blockedID = "";

        if(damageable != null)
        {
            BDClass bd = new BDClass("Fireball", BDDamageType.Burn, damageable, burnDamageModifier, 5, 1.5f);
            damageable.TakeDamage(damage);
            damageable.ApplyBD(bd);
            blockedID = damageable.GetID();

        }


        RaycastHit[] found = Physics.SphereCastAll(transform.position, 15, Vector3.forward, 0, enemyLayer);

        foreach (var item in found)
        {
            IDamageable foundDamageable = item.collider.GetComponent<IDamageable>();



            if (foundDamageable == null) continue;

            if(foundDamageable.GetID() == blockedID) continue;

            BDClass bd = new BDClass("Fireball", BDDamageType.Burn, foundDamageable, burnDamageModifier, 5, 1.5f);
            foundDamageable.TakeDamage(damageSecond);
            foundDamageable.ApplyBD(bd);

        }


        Destroy(gameObject);

    }

}
