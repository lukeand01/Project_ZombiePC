using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletScript : MonoBehaviour
{

    string ownedID;
    bool isEnemy;
    [SerializeField] Vector3 dir;
    bool canMove;
    [SerializeField] DestroySelf _destroySelf;






    public void SetUp(string id, Vector3 dir)
    {
        ownedID = id;
        this.dir = dir;
        canMove = true;

        //none of them have range.
        _destroySelf.SetUpDestroy(7);
    }
    public void MakeEnemy()
    {
        isEnemy = true;
    }


    private void Update()
    {
        //
        if (!canMove) return;
        //the bullet keeps on moving.

        transform.position += dir.normalized * speed * Time.deltaTime;


     

    }


    #region DAMAGE

    DamageClass damage;
    float damageChangeAfterCollision;
    float damageChangeAfterBounce;


    List<BulletBehavior> bulletBehaviorList = new();

    public void MakeDamage(DamageClass damage, float damageChangeAfterCollision, float damageChangeAfterBounce)
    {
        this.damage = damage;
        this.damageChangeAfterBounce = damageChangeAfterBounce;
        this.damageChangeAfterCollision = damageChangeAfterCollision;

    }

    public void MakeBulletBehavior(List<BulletBehavior> bulletBehaviorList)
    {
        this.bulletBehaviorList = bulletBehaviorList;
    }

    #endregion

    #region SPEED
    float speed;
    float speedChangeAfterCollision;
    float speedChangeAfterBounce;

    public void MakeSpeed(float speed, float speedChangeAfterCollision, float speedChangeAfterBounce)
    {
        this.speed = speed;
        this.speedChangeAfterCollision = speedChangeAfterCollision;
        this.speedChangeAfterBounce = speedChangeAfterBounce;
    }

    #endregion


    #region COLLISION
    int bounceTotal;
    int bounceCurrent;

    int collisionsAllowedTotal;
    int collisionsAllowedCurrent;

    public void MakeBounce(int bounceTotal)
    {
        this.bounceTotal = bounceTotal;
    }
    public void MakeCollision(int collisionTotal)
    {
        this.collisionsAllowedTotal = collisionTotal;
    }


    private void OnTriggerEnter(Collider other)
    {



        //then we apply everything and check if this fella can continue.
        if (other.gameObject.tag == "Wall")
        {
            //then its a wall. we bounce if we can. otherwise we destroy.
            CheckBounce();
            return;
        }

        if (isEnemy && other.gameObject.tag == "Enemy") return;
        if (!isEnemy && other.gameObject.tag == "Player") return;

        IDamageable damageable = other.GetComponent<IDamageable>();


        if (damageable != null)
        {
            if (damageable.GetID() == ownedID)
            {


            }
            else
            {

                foreach (var item in bulletBehaviorList)
                {
                    item.ApplyContact(damageable, damage);
                }

                CheckCollision();
            }
        }
    }

    void CheckBounce()
    {
        if (bounceCurrent >= bounceTotal)
        {
            Destroy(gameObject);
        }
        else
        {
            bounceCurrent++;
            //bounce in the other direction and reduce damage or speed.
        }
    }
    void CheckCollision()
    {
        if (collisionsAllowedCurrent >= collisionsAllowedTotal)
        {
            Destroy(gameObject);
        }
        else
        {
            collisionsAllowedCurrent++;
            //reduce damage or speed
        }
    }



    #endregion

}
