using DG.Tweening;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class EnemyBoss_Artillery : EnemyBoss
{
    [Separator("EYE")]
    [SerializeField] Transform[] _eyesArray;
    [SerializeField] Transform cannonTransform;
    [SerializeField] Artillery_DamageArea[] _artilleryDamageAreaArray;
    [SerializeField] AudioSource _shootFarSound;
    
    public Transform shootPos;

    [SerializeField] bool stateShootNearEnemies;


    [SerializeField] GameObject _maskDamagedObject;
    [SerializeField] Animator _faceAnimator;
    [SerializeField] ArtilleryWheel[] _wheelArray;
    [SerializeField] GameObject _artilleryMainFrame;
    [SerializeField] AudioClip _deathSound;
    //the animation is idle - i want it to slowly move from one place to another.
    //the other is just the face being dragged by the black hole.

    protected override void UpdateFunction()
    {

        if (isDead) return;

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


    public override void ResetForPool()
    {
        base.ResetForPool();

        _faceAnimator.Play("Animation_ArtilleryFace_Idle");
        _artilleryMainFrame.transform.DOLocalMove(new Vector3(0, 0, 0), 0);

        for (int i = 0; i < _wheelArray.Length; i++)
        {
            var item = _wheelArray[i];

            item.ResetWheel();

        }

        for (int i = 0; i < _grenadeArray.Length; i++)
        {
            var item = _grenadeArray[i];
            item.SetActive(false);
        }

        _faceAnimator.gameObject.SetActive(true);

    }

    public void ChangeCombatState(bool isSupposedToShootNear)
    {
        if (isSupposedToShootNear == stateShootNearEnemies) return;

        cannonTransform.DOKill();

        if (isSupposedToShootNear)
        {
            cannonTransform.DOLocalRotate(new Vector3(0, 0, 0), 1);
        }
        else
        {
            cannonTransform.DOLocalRotate(new Vector3(-90, 0, 0), 1);
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
            new Behavior_Boss_Artillery_ShortRange(this),
            new Behavior_Boss_Artillery_Grenade(this)
            
        });
    }

    protected override void Die()
    {
        Debug.Log("called death");
        PlayerHandler.instance._playerResources.GainPoints(POINTS_PERKILL);
        PlayerHandler.instance._playerResources.Bless_Gain(5);
        PlayerHandler.instance._playerInventory.AddBossSigil(_sigilType); //we inform as a new item, and we show this in the pause ui.

        isDead = true;
        gameObject.layer = 0;
        //_agent.enabled = false;
        StopAgent();
        _rb.velocity = Vector3.zero;
        _boxCollider.enabled = false;

        StartCoroutine(DeathProcess());
        //we trigger the animation.
        _faceAnimator.Play("Animation_ArtilleryFace_Death");
        _artilleryMainFrame.transform.DOLocalMove(new Vector3(0, -0.5f, 0), 1);

        for (int i = 0; i < _wheelArray.Length; i++)
        {
            var item = _wheelArray[i];

            item.KickWheel(transform.position);
        }

    }

    IEnumerator DeathProcess()
    {
        _animator.applyRootMotion = true;
        //need to aply an especial effect for 
        CallAnimation("Death", 0); //anmd
        //reeduce the weight of all body parts

        ControlWeight(1, 0);
        ControlWeight(2, 0);
        ControlWeight(3, 0);

        yield return new WaitForSeconds(1.5f);

        PSScript _ps = GameHandler.instance._pool.GetPS(PSType.BlackWholeForBossCollection_01, _graphicHolder.transform);
        _ps.gameObject.SetActive(true);
        _ps.gameObject.transform.position = _graphicHolder.transform.GetChild(0).position + new Vector3(0, 5, 0);


        //_deathPS.gameObject.SetActive(true);
        // _deathPS.Clear();
        //_deathPS.Play();

        yield return new WaitForSeconds(15);

        for (int i = 0; i < _wheelArray.Length; i++)
        {
            var item = _wheelArray[i];
            item.LockWheel();
        }
        _faceAnimator.gameObject.SetActive(false);
        _graphicHolder.transform.DOLocalMove(new Vector3(0, -10, 0), 5);

       

        Debug.Log("done");
    }

    public override void TakeDamage(DamageClass damageRef)
    {
        Debug.Log("take damage in artillery");
        base.TakeDamage(damageRef);

        StopCoroutine(nameof(DamageProcess));
        StartCoroutine(DamageProcess());
    }

    IEnumerator DamageProcess()
    {
        _maskDamagedObject.SetActive(true);
        yield return new WaitForSeconds(0.15f);
        _maskDamagedObject.SetActive(false);
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
        shell.Set_Explosion(1.5f, 5);
        _shootFarSound.Play();
    }

    public void CallShootAudio()
    {
        _shootFarSound.Play();
    }


    [SerializeField] GameObject[] _grenadeArray;

    public void ThrowGrenade()
    {
        StartCoroutine(GrenadeProcess());
    }

    int GetFreeGrenade()
    {
        for (int i = 0; i < _grenadeArray.Length; i++)
        {
            var item = _grenadeArray[i];

            if (!item.gameObject.activeInHierarchy) return i;
        }

        return -1;
    }

    IEnumerator GrenadeProcess()
    {
        Vector3 endPos = PlayerHandler.instance.transform.position;
        Vector3 startPos = _faceAnimator.transform.position;

        //at the same time we create a damage area as well.

        int grenadeIndex = GetFreeGrenade();


        if(grenadeIndex == -1)
        {
            Debug.Log("broke off");
            yield break;
        }

        GameObject selectedGrenade = _grenadeArray[grenadeIndex];

        float duration = 1f;   // Time it takes to travel the arc
        float timeElapsed = 0f;
        float arcHeight = 10;

        int safeBreak = 0;


        selectedGrenade.SetActive(true);

       AreaDamage _areaDamage =  GameHandler.instance._pool.GetAreaDamage(transform);
        DamageClass _damage = new DamageClass(50, DamageType.Physical, 0);
        _areaDamage.SetUp_Regular(endPos, 3, duration, _damage, 3, 0.5f, AreaDamageVSXType.Nothing);
        _areaDamage.transform.position = endPos;

        while (timeElapsed < duration)
        {

            safeBreak++;

            if(safeBreak > 1000)
            {
                yield break;
            }

            timeElapsed += Time.fixedDeltaTime;

            // Calculate the normalized time (0 to 1)
            float t = timeElapsed / duration;

            // Calculate the new position using linear interpolation (Lerp)
            Vector3 currentPosition = Vector3.Lerp(startPos, endPos, t);

            // Add an arc by adjusting the Y position
            float parabolicArc = 4 * arcHeight * (t - t * t);  // Parabolic equation for arc height
            currentPosition.y += parabolicArc;

            // Move the object to the calculated position
            selectedGrenade.transform.position = currentPosition;

            yield return new WaitForSeconds(Time.fixedDeltaTime) ;
        }

        PSScript _psScript = GameHandler.instance._pool.GetPS(PSType.Explosion_01, selectedGrenade.transform);
        _psScript.transform.position = endPos;
        GameHandler.instance._soundHandler.CreateSfx(SoundType.AudioClip_Explosion_01, selectedGrenade.transform);

        selectedGrenade.gameObject.SetActive(false);

    }


}

//the helpers 



///we must shoot the artillery
///check for the areadamage, it should deal damage and then deal damage again
///we should add sound for each explosion
///the artillery does a sound after every attack, whihc can be heard from any point in the map
///it rotates and shoots when the player is close
//the face can throw grenades
///the damaged thing, it can be damaged and it shows
//then we need to show its death. need to synch with the black hole.
//


//sound for death
///sound for shooting nearstuff
//the grenades are thrown by the face.
//the synch iwth teh blackhole
//it should still go down, but its after the thing is pulled into the black hole
//sound for grenade


//GOAL
//work on spawn system for 




//the

//each helper will have the thing. 
//would aiming 


//if not in sight it will shoot on cooldown
//

//the damage area will be its own code. just ebcause fuck it