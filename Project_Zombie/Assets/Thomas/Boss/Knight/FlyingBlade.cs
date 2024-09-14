using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FlyingBlade : BulletScript
{
    //this should be bullet script 

    Transform _entityToRotateAround;
    Vector3 randomDir;
    LayerMask groundLayer;
    float _rotationSpeed;
    bool hasArrivedToPosition;
    //

    public void SetUp_FlyingBlade(float damage, float speed)
    {
        //damage and 
        groundLayer |= (10 << 1);

        _rotationSpeed = 150;

        DamageClass damageClass = new DamageClass(damage, DamageType.Physical, 0);
        MakeDamage(damageClass, 0, 0);

        MakeSpeed(speed, 0, 0);

        if(_entityToRotateAround == null)
        {
            float randomX = Random.Range(-1, 1);
            float randomZ = Random.Range(-1, 1);

            if(randomX == 0 && randomZ == 0)
            {
                randomX = 1;
            }

            randomDir = new Vector3(randomX, 0, randomZ);
        }

        //_destroySelf.SetUpDestroy((int)ProjectilType.FlyingSwords, 50, this);
        Debug.Log("this flying blade was set up");

        canMove = true;
    }
    public void Make_FlyingBlade_RotateAroundTarget(Transform _entityToRotateAround, Vector3 pos, float timer)
    {
        this._entityToRotateAround = _entityToRotateAround;
        transform.DOLocalMove(transform.localPosition + pos, timer).SetEase(Ease.Linear).OnComplete(CallHasArrivedToPosition);
    }

    void CallHasArrivedToPosition()
    {
        hasArrivedToPosition = true;
    }

    public override void ResetToReturnToPool()
    {
        base.ResetToReturnToPool();

        hasArrivedToPosition = false;
    }

    private void OnDestroy()
    {
        Debug.Log("this was destroyed");
    }

    protected override void UpdateFunction()
    {

        transform.Rotate(new Vector3(0, _rotationSpeed * Time.deltaTime, 0));


        if(_entityToRotateAround != null)
        {
            if (hasArrivedToPosition)
            {
                transform.RotateAround(_entityToRotateAround.position, Vector3.up, speed * Time.deltaTime);
            }
            //rotate around this target.
            
        }
        else
        {
            //this gets a random movement. and go to that, it keeps checking for ledges and walls.
            transform.position += randomDir.normalized * speed * Time.fixedDeltaTime;
            //it checks for a wall.
            //or a ledge.

            if(IsOnLedge())
            {
                GetOppositeNewRandomDir();
            }

        }


    }

    void GetOppositeNewRandomDir()
    {
        randomDir *= 1;

        //but i also want the change so we have random new directions.

    }

    bool IsOnLedge()
    {
        bool isOnLedge = Physics.Raycast(transform.position + randomDir, Vector3.down, 500, groundLayer);
        return !isOnLedge;
    }

    protected override void TriggerEnterFunction(Collider other)
    {



        if (other.gameObject.tag == "Wall" && _entityToRotateAround == null)
        {
            //then we get a random direction opposite to this direction
            GetOppositeNewRandomDir();
            return;
        }

        if(other.gameObject.tag == "Player")
        {
            //deals damage to the player.
            PlayerHandler.instance._playerResources.TakeDamage(damage);
        }


    }


    public override FlyingBlade GetFlyingBlade => this;

}
