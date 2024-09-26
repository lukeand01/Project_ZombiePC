using DG.Tweening;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss_Artillery : EnemyBoss
{
    [Separator("EYE")]
    [SerializeField] Transform[] _eyesArray;
    [SerializeField] Transform cannonTransform;
    [SerializeField] Artillery_DamageArea[] _artilleryDamageAreaArray;


    bool stateShootNearEnemies;

    protected override void UpdateFunction()
    {
        base.UpdateFunction();


        if (stateShootNearEnemies)
        {
            //then its supposed to rotate to face the player
            //it should slowly doing that as it keeps

            Vector3 direction = PlayerHandler.instance.transform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
            transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, targetRotation, Time.deltaTime);

        }

    }

    [ContextMenu("Near")]
    public void DebugNear()
    {
        ChangeCombatState(true);
    }
    [ContextMenu("Far")]
    public void DebugFar()
    {
        ChangeCombatState(false);
    }


    public void ChangeCombatState(bool isSupposedToShootNear)
    {
        if (isSupposedToShootNear == stateShootNearEnemies) return;

        if (isSupposedToShootNear)
        {
            cannonTransform.DOLocalRotate(new Vector3(0, 0, 0), 2);
        }
        else
        {
            cannonTransform.DOLocalRotate(new Vector3(-90, 0, 0), 2);
        }

        stateShootNearEnemies = isSupposedToShootNear;
    }

    protected override void StartFunction()
    {
        UpdateTree(GetBehavior());
        base.StartFunction();
    }

    public void ReturnProjectil(Artillery_DamageArea artilleryProjectil)
    {
        artilleryProjectil.gameObject.SetActive(false);
        artilleryProjectil.transform.position = transform.position;
    }

    Sequence2 GetBehavior()
    {
        return new Sequence2(new List<Node>
        {
            new Behavior_Boss_Artillery_LongRange(this, _eyesArray),
           // new Behavior_Boss_Artillery_ShortRange(this),
           // new Behavior_Boss_Artillery_Grenade()
            
        });
    }


    public int CanShootProjectil()
    {
        for (int i = 0; i < _artilleryDamageAreaArray.Length; i++)
        {
            var item = _artilleryDamageAreaArray[i];
            if (!item.gameObject.activeInHierarchy) return i;
        }

        return -1;
    }

    public void ShootProjectil()
    {
        int index = CanShootProjectil();

        if(index == -1)
        {
            Debug.Log("something went wrong");
            return;
        }

        //we get the player position. we always aim the player.
        Artillery_DamageArea shell = _artilleryDamageAreaArray[index];

        shell.transform.position = PlayerHandler.instance.transform.position;
        shell.gameObject.SetActive(true);
        shell.Set_Explosion(5, 5);
    }

}


//if not in sight it will shoot on cooldown
//

//the damage area will be its own code. just ebcause fuck it