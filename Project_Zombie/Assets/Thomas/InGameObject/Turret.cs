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

    bool cannotShoot;

    TurretToBuy _turretBuy;

    private void Awake()
    {
        enemyLayer |= (1 << 6);

        targetLayer |= (1 << 6);
        targetLayer |= (1 << 9);
        //targetLayer |= (1 << 7);

        attackCooldownTotal = 1.5f;

        SetUp_Ally(50, 35);
    }

    public virtual void SetUp()
    {
        //what does it need to know?



    }

    public void SetTurretBuy(TurretToBuy _turretBuy)
    {
        this._turretBuy = _turretBuy;   
    }

    private void FixedUpdate()
    {
        //Debug.Log("in sight " + IsInSight(enemyteste.transform));

        if (cannotShoot) return;

        FixedUpdateFunction();
    }

    public void ControlCannotShoot(bool cannotShoot)
    {
        this.cannotShoot = cannotShoot; 
    }

    public void SetDamage(DamageClass damage)
    {
        this.damage = damage;


    }

    public void SetTarget()
    {
        //it either targets player for now it will always target 
    }

    public void CallDurationToStart(float duration)
    {
        StartCoroutine(DurationToStartProcess(duration));
    }
    IEnumerator DurationToStartProcess(float duration)
    {
        yield return new WaitForSeconds(duration);

        //we start
        ControlCannotShoot(false);
        SetUp_Ally_WithBaseValues();

    }

    protected override void CallEndDuration()
    {
        if(_turretBuy != null)
        {
            _turretBuy.StopSentry();
            return;
        }

        base.CallEndDuration();
    }

    protected void LookForTarget()
    {
        //we ciclecast in area around.
        //we shoot a raycast in all found. starting from the closests to the farthest.
        //we turn to the target. and once we stop rotating we start shooting.
        //we change the target only once that original is dead or out of range.

        RaycastHit[] targetsAround = Physics.SphereCastAll(transform.position, range, Vector2.up, 50, enemyLayer);

        foreach (var item in targetsAround)
        {
            //we will get the first to be the right one because its generaly the closest.
            if (IsInSight(item.collider.transform))
            {

                IDamageable damageable = item.collider.GetComponent<IDamageable>();

                if (damageable == null) continue;

                target = damageable;
                targetObject = item.collider.gameObject;
                return;

            }
            else
            {
                Debug.Log("not caught");
            }

        }

        target = null;
        targetObject = null;


    }


    protected void CheckIfTargetIsNull()
    {

        float distanceFromTarget = Vector3.Distance(transform.position, targetObject.transform.position);

        if (distanceFromTarget > range)
        {
            target = null;
            targetObject = null;
            return;
        }


        if (!IsInSight(targetObject.transform))       
        {
            target = null;
            targetObject = null;
            return;
        }

        if (!targetObject.activeInHierarchy)
        {
            target = null;
            targetObject = null;
            return;
        }

        if (target.IsDead())
        {
            target = null;
            targetObject = null;
        }


    }


    protected Quaternion RotateToTarget()
    {
        Vector3 direction = targetObject.transform.position - transform.position;
        Vector3 directionNormalized = direction.normalized;

        //transform.LookAt(targetObject.transform.position);

        Quaternion targetRotation = Quaternion.LookRotation(directionNormalized);
        graphic.transform.rotation = Quaternion.Slerp(graphic.transform.rotation, targetRotation, 15 * Time.deltaTime);


        return targetRotation;
    }

    //perphap 

    protected bool IsInSight(Transform targetTransform)
    {
        int amountFound = 0;
        foreach (var item in eyeArray)
        {
            Vector3 targetPos = (targetTransform.position - item.position).normalized;
            targetPos.y = 0;

            Ray ray = new Ray(item.position, targetPos);

            if (Physics.Raycast(ray, out RaycastHit hit, 50, targetLayer))
            {
                //Debug.Log("caught " + hit.collider.gameObject.layer);
                //Debug.Log("caught " + hit.collider.gameObject.name);
                if (hit.collider.gameObject.layer == 6)
                {
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

        //Debug.Log(amountFound);
        return amountFound >= 2;
    }

    
}




//