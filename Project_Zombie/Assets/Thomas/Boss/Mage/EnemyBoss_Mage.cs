using DG.Tweening;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss_Mage : EnemyBoss
{

    [Separator("EYE")]
    [SerializeField] Transform[] _eyesArray;

    [Separator("MAGE")]
    [SerializeField] MageBarrier _mageBarrier;
    [SerializeField] Transform[] seekerPosArray;
    [SerializeField] Transform seekerContainer;

    [Separator("SOUND FOR MAGE")]
    [SerializeField] AudioClip _audio_summonOrb;
    [SerializeField] AudioClip _audio_summonSeeker;
    [SerializeField] AudioSource _audioSource;

    public bool CanCallMageBarrier { get { return !_mageBarrier.gameObject.activeInHierarchy; } }


    public void CallMageBarrier()
    {
        _mageBarrier.gameObject.SetActive(true);
        //call an effect
        //sound 
    }
    void HandleMageBarrier()
    {
        //keep placing it between player and object.

        if (!_mageBarrier.gameObject.activeInHierarchy) return;




    }

    protected override void UpdateFunction()
    {

        HandleMageBarrier();

        base.UpdateFunction();
    }

    protected override void AwakeFunction()
    {

        base.AwakeFunction();
    }

    protected override void StartFunction()
    {
        UpdateTree(GetBehavior());
        base.StartFunction();
    }

    Sequence2 GetBehavior()
    {
        return new Sequence2(new List<Node>
        {
            new Behavior_Boss_Chase(this, _bossData), //it simply chases the player.           
            new Behavior_Boss_Mage_Meteor(this, _eyesArray),
            //new Behavior_Boss_Mage_Shoot(this),
            new Behavior_Boss_Mage_Barrage(this),

        });
    }


    public override void CalculateAttack()
    {
        if (actionIndex_Current == 0)
        {
            //we call the meteor now
            CalculateAttack_Orb();
            GameHandler.instance._soundHandler.CreateSfx_WithAudioClip(_audio_summonOrb);
            return;
        }
        if (actionIndex_Current == 1)
        {
            CalculateAttack_Meteor();
            return;
        }

        if (actionIndex_Current == 2)
        {
            CalculateAttack_Seeker();
            GameHandler.instance._soundHandler.CreateSfx_WithAudioClip(_audio_summonSeeker);
            return;
        }

        if (actionIndex_Current == 3)
        {
            CalculateAttack_Shield();
            return;
        }

    }

    void CalculateAttack_Orb()
    {
        Vector3 shootDir = PlayerHandler.instance.transform.position - transform.position;

        BulletScript newObject = GameHandler.instance._pool.GetBullet(ProjectilType.DarkOrb, transform);

        newObject.MakeEnemy();
        newObject.MakeSpeed(12, 0, 0);
        newObject.MakeDamage(new DamageClass(250, DamageType.Physical, 0), 0, 0);
        newObject.SetUp("MinibossMage", shootDir);
    }

    void CalculateAttack_Meteor()
    {
        //it calls this then


    }

    void CalculateAttack_Seeker()
    {
        //we have spots for seeker spawn.
        //we shoot 10 between interval.s
        //they try to follow the player.

        StartCoroutine(SeekerProcess());

    }
    public bool isRunningSeeker { get; private set; }   
    IEnumerator SeekerProcess()
    {
        //they gradually appear
        //then tehy gradually become active

        isRunningSeeker = true;

        List<BulletScript> bulletList = new();

        DamageClass _damage = new DamageClass(80, DamageType.Magical, 0);


        for (int i = 0; i < seekerPosArray.Length; i++)
        {
            //spawn this fella

            BulletScript newBullet = GameHandler.instance._pool.GetBullet(ProjectilType.Skull_Seeker, seekerPosArray[i]);
            newBullet.MakeEnemy();
            newBullet.MakeDamage(_damage, 0, 0);
            newBullet.MakeSpeed(6, 0, 0);
            bulletList.Add(newBullet);

            newBullet.transform.SetParent(seekerContainer);

        }

        yield return new WaitForSeconds(2.5f);

        int safeBreak = 0;


        while (bulletList.Count > 0)
        {
            safeBreak++;
            if (safeBreak > 1000)
            {
                Debug.Log("brea this");
                yield break;
            }

            int random = Random.Range(0, bulletList.Count);

            bulletList[random].transform.SetParent(null);
            Vector3 shootDir = PlayerHandler.instance.transform.position - transform.position;
            bulletList[random].SetUp("Mage", shootDir, 7);
            //Debug.Log("called this fella " + bulletList[random].name);
            bulletList.RemoveAt(random);
            

            yield return new WaitForSeconds(Random.Range(2, 5));

        }


        isRunningSeeker = false;

        yield return null;
    }


    void CalculateAttack_Shield()
    {
        //put a shield?



    }

    public override void ResetForPool()
    {
        _audioSource.enabled = true;
        _graphicHolder.transform.DOLocalMove(new Vector3(0,1,0), 0);
        base.ResetForPool();
    }
    protected override void Die()
    {
        _audioSource.Stop();
        _audioSource.enabled = false;
        _graphicHolder.transform.DOLocalMove(Vector3.zero, 2);
        base.Die();
    }

}


//the spots wont be random. they always appear together and in the same place.
//they appear first as black smoke
//then they scale up.
//


//this fellas goes after the player. she keeps on moving
//if the player is on sight she throws projectiles.
//if the player is not in sight but its in range then she will throw meteors at the player. same as magic, but bigger and deal more damage.
//at cooldown the mage raises a wall that is always rotate between the mage and the player. it can be destroyed though.
//

//then the next is shooting projectiles.
//it has two projectiles. one that follows the player, and the other that goes straight.
//
