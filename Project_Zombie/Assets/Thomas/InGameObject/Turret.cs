using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : AllyBase
{
    //how are things here?
    //this should also use the bullet pooling





    [SerializeField] protected BulletScript bulletTemplate;

    [SerializeField] protected List<StatClass> scaleClass = new(); //each uses it the way it wants.

    [SerializeField] protected float range;

    protected GameObject targetObject;
    protected IDamageable target;

    DamageClass damage;

    protected float attackCooldownCurrent;
    protected float attackCooldownTotal;

    [SerializeField] protected LayerMask targetLayer;
    [SerializeField]protected LayerMask enemyLayer;


    [SerializeField] protected GameObject graphic;
    [SerializeField] protected GameObject head;
    [SerializeField] protected Transform gunPointTransform;
    [SerializeField] Transform[] eyeArray;
    private void Awake()
    {
        enemyLayer |= (1 << 6);

        targetLayer |= (1 << 6);
        targetLayer |= (1 << 9);
        //targetLayer |= (1 << 7);
    }

    public virtual void SetUp()
    {
        //what does it need to know?



    }

    private void FixedUpdate()
    {
        Debug.Log("in sight " + IsInSight(enemyteste.transform));
    }


    public void SetDamage(DamageClass damage)
    {
        this.damage = damage;


    }

    public void SetTarget()
    {
        //it either targets player for now it will always target 
    }

    //first what i must do. first i will make sure the enemy are able to target allies.

 

    void LookForTarget()
    {
        //we ciclecast in area around.
        //we shoot a raycast in all found. starting from the closests to the farthest.
        //we turn to the target. and once we stop rotating we start shooting.
        //we change the target only once that original is dead or out of range.

    }

    protected bool IsInSight(Transform targetTransform)
    {
        int amountFound = 0;
        foreach (var item in eyeArray)
        {
            Vector3 targetPos = targetTransform.position - item.position; //.normalized;
            Ray ray = new Ray(item.position, targetPos);

            if (Physics.Raycast(ray, out RaycastHit hit, 50, targetLayer))
            {
                if(hit.collider.gameObject.layer == 6)
                {
                    //
                    amountFound++;
                }
                if(hit.collider.gameObject.layer == 9)
                {
                   
                }
            }
            else
            {
                
            }
        }

        Debug.Log(amountFound);
        return amountFound >= 2;
    }

    [SerializeField] EnemyBase enemyteste;
    private void OnDrawGizmosSelected()
    {
        Vector3 targetPos = enemyteste.transform.position - eyeArray[0].position; //.normalized;
        Ray ray = new Ray(eyeArray[0].position, targetPos);
        Gizmos.DrawRay(ray);

        Vector3 targetPos_1 = enemyteste.transform.position - eyeArray[1].position; //.normalized;
        Ray ray_1 = new Ray(eyeArray[1].position, targetPos_1);
        Gizmos.DrawRay(ray_1);

        Vector3 targetPos_2 = enemyteste.transform.position - eyeArray[2].position; //.normalized;
        Ray ray_2 = new Ray(eyeArray[2].position, targetPos_2);
        Gizmos.DrawRay(ray_2);

    }
}
