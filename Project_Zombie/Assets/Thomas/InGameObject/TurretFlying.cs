using MyBox;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TurretFlying : Turret
{
    //this turret will also have the behavior where it shoots.
    //but it will also fly around the player.
    //you can have more than one.
    //they will have a random spot that is far away enough from anyother flying, and they always try to have the same position as the player moves.
    [Separator("TURRET FLYING")]
    [SerializeField] List<BulletBehavior> bulletBehaviorList;
    [SerializeField] float orbitSpeed;
    [SerializeField] float radius;
    Transform refPoint;
    Vector3 targetPos;

    float angle = 0;

    Transform[] playerEyeArray;
    public string id { get; private set; }

    private void Start()
    {
        id = Guid.NewGuid().ToString();

        playerEyeArray = PlayerHandler.instance.eyeArray;
    }

    protected override void CallEndDuration()
    {
        PlayerHandler.instance.RemoveTurretFly(id);
        GameHandler.instance._pool.TurretFly_Release(this);

    }

    public void ResetToReturnToPool()
    {

    }

    public void SetRefPoint(Transform refPoint)
    {
        this.refPoint = refPoint;
    }

    public void SetAroundTransform(Transform refPoint)
    {
        //we set this fella in a random position around the refpoint.
        //this point cannot be occupied by another fella.
        //we will check the position that is the farthests from  the sides.

        this.refPoint = refPoint;


        Vector3 randomOffset = UnityEngine.Random.insideUnitSphere * radius;
        randomOffset.y = Mathf.Abs(randomOffset.y); // Ensure the object stays above the head

        targetPos = refPoint.position + randomOffset;
        transform.position = targetPos;

    }

    void FlyAroundTheHead()
    {



        angle += orbitSpeed * Time.fixedDeltaTime;
        if (angle >= 360f) angle -= 360f;

        // Calculate the new position using trigonometric functions
        float radian = angle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Cos(radian), 0, Mathf.Sin(radian)) * radius;
        //transform.position = refPoint.position + offset;

        transform.position = Vector3.MoveTowards(transform.position, refPoint.position + offset, Time.fixedDeltaTime * orbitSpeed);

    }

    protected override void FixedUpdateFunction()
    {

        //i dont want to be creating the fella here.
        //


        if (refPoint == null) return;


        base.FixedUpdateFunction();


        FlyAroundTheHead();

        if (targetObject != null)
        {
            //         
            //Debug.Log("yo");
            Quaternion targetRotation = RotateToTarget();
            CheckIfShouldShoot(targetRotation);
            CheckIfTargetIsNull();
        }
        else
        {
            //Debug.Log("not yo");
            graphic.transform.Rotate(new Vector3(0, 0.5f, 0));
            LookForTarget();
        }

    }


    void CheckIfShouldShoot(Quaternion targetRotation)
    {

        if (Quaternion.Angle(graphic.transform.rotation, targetRotation) <= 5)
        {
            //can now shoot

            Shoot();

        }
    }

    void Shoot()
    {

        if (attackCooldownCurrent <= 0)
        {
            Vector3 direction = (targetObject.transform.position - transform.position).normalized;
            BulletScript newBullet = Instantiate(bulletTemplate, gunPointTransform.position, Quaternion.identity);
            newBullet.SetUp("Turret Ally", direction);

            newBullet.MakeBulletBehavior(bulletBehaviorList);
            newBullet.MakeDamage(new DamageClass(50), 0, 0);
            newBullet.MakeSpeed(25, 0, 0);

            attackCooldownCurrent = attackCooldownTotal;
        }
        else
        {
            attackCooldownCurrent -= Time.deltaTime;
        }
    }

    protected override bool IsInSight(Transform targetTransform)
    {

        //the only different is that this will use the player´s eyearray.

        int amountFound = 0;
        foreach (var item in playerEyeArray)
        {
            Vector3 targetPos = (targetTransform.position - item.position).normalized;
            targetPos.y = 0;

            Ray ray = new Ray(item.position, targetPos);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 50, targetLayer))
            {
                //Debug.Log("caught " + hit.collider.gameObject.layer);
                //Debug.Log("caught " + hit.collider.gameObject.name);
                if (hit.collider.gameObject.layer == 6)
                {
                    amountFound++;
                }
                if (hit.collider.gameObject.layer == 9)
                {

                }
            }
            else
            {

            }

           

        }

        return amountFound >= 2;
    }

}


//instead of shooting from the turret we should shoot from the player this one here.