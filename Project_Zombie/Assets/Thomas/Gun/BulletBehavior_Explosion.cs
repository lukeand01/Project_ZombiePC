using UnityEngine;


[CreateAssetMenu(menuName = "Bullet / Explosion")]
public class BulletBehavior_Explosion : BulletBehavior
{

    [SerializeField] float explosionRadius;
    [Range(0,1)][SerializeField] float explosionPercentDamage = 1;
    public override void ApplyContact(IDamageable target, DamageClass damage)
    {


        //but where do ise tht eexplosion percent damage?

        Transform posRef = target.GetObjectRef().transform;

        DamageClass damageClassForExplosion = new DamageClass(damage.GetTotalDamage() * explosionPercentDamage, damage.damageList[0]._damageType, 0) ;
        damageClassForExplosion.Make_Explosion();

        LayerMask targetLayers = 0;
        targetLayers |= (1 << 6);


        RaycastHit[] targetArray = Physics.SphereCastAll(posRef.position, explosionRadius, Vector3.up, 0, targetLayers);


        foreach (var item in targetArray)
        {
            IDamageable damageable = item.collider.GetComponent<IDamageable>();

            if(damageable == null)
            {
                continue;
            }
            if(damageable.GetID() == target.GetID())
            {

                continue;
            }

            damageable.TakeDamage(damageClassForExplosion);
        }

    }
}
