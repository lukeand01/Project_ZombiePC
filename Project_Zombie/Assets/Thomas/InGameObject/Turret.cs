using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour, IDamageable
{
    //this will look fro the right target, aim and shoot.
    //there may be certain behaviors.
    //how to define the certain behaviors?


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

    private void Awake()
    {
        enemyLayer |= (1 << 6);

        targetLayer |= (1 << 6);
        targetLayer |= (1 << 9);
        targetLayer |= (1 << 7);
    }

    public virtual void SetUp()
    {
        //what does it need to know?



    }


    public void SetDamage(DamageClass damage)
    {
        this.damage = damage;


    }

    public void SetTarget()
    {
        //it either targets player for now it will always target 
    }

    private void Update()
    {
        
    }

    void LookForTarget()
    {
        //we ciclecast in area around.
        //we shoot a raycast in all found. starting from the closests to the farthest.
        //we turn to the target. and once we stop rotating we start shooting.
        //we change the target only once that original is dead or out of range.

    }

    #region DAMAGEABLE
    public void TakeDamage(DamageClass damage)
    {
        
    }

    public void ApplyBD(BDClass bd)
    {
        
    }

    public string GetID()
    {
        return "";
    }

    public bool IsDead()
    {
        return false;
    }

    public float GetTargetMaxHealth()
    {
        return 0;
    }

    public float GetTargetCurrentHealth()
    {
        return 0;
    }

    #endregion
}
