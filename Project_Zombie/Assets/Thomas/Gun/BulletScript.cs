using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BulletScript : MonoBehaviour
{

    string ownedID;
    bool isEnemy;
    [SerializeField] protected Vector3 dir;
    protected bool canMove;
    [SerializeField] protected DestroySelf _destroySelf;
    [SerializeField] SpawnPSOnTriggerScript psSpawnOnTriggerScript;
    [SerializeField] BoxCollider _collider;
    [SerializeField]TrailRenderer _trailRenderer;
    [SerializeField] TrailRenderer _trailRenderer2;
    
    //we will create a place to store for teh player.

    LayerMask _layer;


    public void SetPSOnDisable()
    {

    }

    public virtual void ResetToReturnToPool()
    {
        if (_trailRenderer != null)
        {
            _trailRenderer.Clear();
            
        }
        if (_trailRenderer2 != null)
        {
            _trailRenderer2.Clear();
        }


    }

    private void Awake()
    {
        _layer |= (1 << 14);
    }

    public void SetUp(string id, Vector3 dir, float destroyDuration = 7)
    {


        gameObject.name = Random.Range(0, 1000).ToString();

        ownedID = id;
        this.dir = dir;
        canMove = true;
        _collider.enabled = true;
        //none of them have range.
        int index = 0;
        if (isEnemy)
        {
            index = (int)ProjectilType.EnemySpit;
        }
        else
        {
            index = (int)ProjectilType.PlayerRegular;
        }

        _destroySelf.SetUpDestroy(index , destroyDuration, this);

    }
    public void MakeEnemy()
    {
        isEnemy = true;
    }



    private void Update()
    {
        //



     

    }
    private void FixedUpdate()
    {
        if (!canMove)
        {
            Debug.Log("stuck");
            return;
        }
        //the bullet keeps on moving. 
        UpdateFunction();
        
    }

    protected virtual void UpdateFunction()
    {
        transform.position += dir.normalized * speed * Time.fixedDeltaTime;

    }


    void TEST()
    {
        RaycastHit hit;
        bool isWall = Physics.Raycast(transform.position, dir.normalized, out hit, 500, _layer);



        if (isWall && hit.collider != null)
        {

            //we check the distance and if its low enough we assume its going to hit anyway
            if (hit.distance <= 0.2f) //if we are at this distance we will assume hit already.
            {
                //so what we must do instead  because if its a shield then we should stop.
                canMove = false;

                //have to check 

                IDamageable damageable = hit.collider.GetComponent<IDamageable>();

                if (damageable != null)
                {
                    CalculateDamageable(damageable);
                }


                return;
            }

        }
    }

    #region DAMAGE

    protected DamageClass damage;
    float damageChangeAfterCollision;
    float damageChangeAfterBounce;

    public Vector3 posWhenShot { get; private set; }

    [SerializeField] List<BulletBehavior> bulletBehaviorList = new();

    public void MakeDamage(DamageClass damage, float damageChangeAfterCollision, float damageChangeAfterBounce)
    {
        this.damage = damage;
        this.damage.Make_Projectil(transform);

        this.damageChangeAfterBounce = damageChangeAfterBounce;
        this.damageChangeAfterCollision = damageChangeAfterCollision;

    }

    public void MakePlayerDamageableRef(IDamageable damageable)
    {
        damage.Make_Attacker(damageable);
        damage.Make_PlayerPosWhenCreated(PlayerHandler.instance.transform.position);

    }
    public void MakePlayerPosWhenShot(Vector3 shootPos)
    {
        posWhenShot = shootPos;
    }

    public void MakeBulletBehavior(List<BulletBehavior> bulletBehaviorList)
    {
        this.bulletBehaviorList = bulletBehaviorList;
    }

    #endregion

    #region SPEED
    protected float speed;
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
        collisionsAllowedTotal = collisionTotal;
    }



    protected void CalculateDamageable(IDamageable damageable)
    {
        Debug.Log("collision");
        if (damageable.GetID() == ownedID)
        {


        }
        else
        {
            if (damageable.GetTargetMaxHealth() == -5)
            {
                //THEN THIS MEANS ITS A SHIELD
                collisionsAllowedCurrent += 9999;
            }

            foreach (var item in bulletBehaviorList)
            {

                item.ApplyContact(damageable, damage);
            }

            CheckCollision();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        TriggerEnterFunction(other);
       
    }

    protected virtual void TriggerEnterFunction(Collider other)
    {


        canMove = false;
        //then we apply everything and check if this fella can continue.
        if (other.gameObject.tag == "Wall")
        {
            //then its a wall. we bounce if we can. otherwise we destroy.
            CheckBounce();
            return;
        }
        


        if (isEnemy && other.gameObject.tag == "Enemy" && other.gameObject.layer != 8)
        {
            canMove = true;
            return;
        }
        if (!isEnemy && other.gameObject.tag == "Player" || !isEnemy && other.gameObject.layer == 8)
        {
            canMove = true;
            return;
        }

        IDamageable damageable = other.GetComponent<IDamageable>();

        //Debug.Log("deal damage to " + other.gameObject.name + " " + damage.baseDamage);

        if (damage == null)
        {
            //Debug.Log("no damage here");
        }

        if (damageable != null)
        {
            CalculateDamageable(damageable);
        }
    }

    void CheckBounce()
    {
        if (bounceCurrent >= bounceTotal)
        {
            int index = 0;

            if (isEnemy) index = 1;


            GameHandler.instance._pool.Bullet_Release(index, this);
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
            //Debug.Log("Destroyed. total_Damage and current " + collisionsAllowedTotal + " : " + collisionsAllowedCurrent);
            canMove = false;
            _collider.enabled = false;

            int index = 0;

            if (isEnemy) index = 1;

            GameHandler.instance._pool.Bullet_Release(index,this);
        }
        else
        {
            //Debug.Log(" Didnt destroy. total_Damage and current " + collisionsAllowedTotal + " : " + collisionsAllowedCurrent);
            collisionsAllowedCurrent++;
            //reduce damage or speed
        }
    }



    #endregion

    public virtual FlyingBlade GetFlyingBlade { get { return null; } }


}
