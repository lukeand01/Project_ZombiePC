using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss_Tree : EnemyBoss
{


    [SerializeField] TreeSeed[] _seedArray;
    [SerializeField] EnemyData _enemyDataForSpawn;

    [Separator("EYUE GRPAHIC")]
    [SerializeField] MeshRenderer[] _eyeArray;
    [SerializeField] Material _eyeMaterial_Default;
    [SerializeField] Material _eyeMaterial_Angry;

    [Separator("LOG HOLDER")]
    [SerializeField] GameObject _logHolder;
    [SerializeField] TreeLog[] _logArray;


    public Transform shootPos;
    protected override void AwakeFunction()
    {
        base.AwakeFunction();

        StartBoss();
    }

    protected override void StartFunction()
    {
        UpdateTree(GetBehavior());

        base.StartFunction();


        //its always rotating to the player.

        


    }

    public override void StartBoss()
    {
        base.StartBoss();

        _logHolder.transform.SetParent(null);
    }

    protected override void UpdateFunction()
    {
        base.UpdateFunction();

        

        if (!IsActing)
        {
            Vector3 direction = PlayerHandler.instance.transform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
            transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, targetRotation, 3 * Time.deltaTime);
        }
    }

    


    public override void ResetForPool()
    {
        base.ResetForPool();

        SetEyesToDefault();

        UpdateTree(GetBehavior());
        StopAllCoroutines();

        _logHolder.transform.SetParent(transform);

        for (int i = 0; i < _logArray.Length; i++)
        {
            var item = _logArray[i];
            item.gameObject.SetActive(false);
        }
    }


    Sequence2 GetBehavior()
    {

        return new Sequence2(new List<Node>
        {
            new Behavior_Tree_Seed(this, 5),
            new Behavior_BossSpawnEnemy(this, 4, new List<EnemyData>(){_enemyDataForSpawn, _enemyDataForSpawn, _enemyDataForSpawn}),
            new Behavior_Tree_ShootGas(this, 5, true),         
            //new Behavior_Tree_Log()
        });
    }
    Sequence2 GetBehaviorPhase2()
    {
        return new Sequence2(new List<Node>
        {
            new Behavior_BossSpawnEnemy(this, 3.6f, new List<EnemyData>(){_enemyDataForSpawn, _enemyDataForSpawn, _enemyDataForSpawn, _enemyDataForSpawn}),
            new Behavior_Tree_ShootGas(this, 2, true),
            new Behavior_Tree_Seed(this, 3.5f),
            new Behavior_Tree_Log(this, 10)
        });
    }
    Sequence2 GetBehaviorPhase3()
    {
        return new Sequence2(new List<Node>
        {
            new Behavior_BossSpawnEnemy(this, 2.5f, new List<EnemyData>(){_enemyDataForSpawn, _enemyDataForSpawn, _enemyDataForSpawn, _enemyDataForSpawn}),
            new Behavior_Tree_ShootGas(this, 1, true),
            new Behavior_Tree_Seed(this, 2.5f),
            new Behavior_Tree_Log(this, 7)
        });
    }
    #region PHASES

    int _phaseLevel;

    public override void TakeDamage(DamageClass damageRef)
    {
        base.TakeDamage(damageRef);

        CheckBossPhase();

    }

    void CheckBossPhase()
    {
        if(health_Current <= health_Total * 0.75f && health_Current > health_Total * 0.4f && _phaseLevel != 2)
        {
            StartPhase2();
        }
        if (health_Current <= health_Total * 0.4f && _phaseLevel != 3)
        {
            StartPhase3();
        }
    }

    void StartPhase2()
    {
        _phaseLevel = 2;
        UpdateTree(GetBehaviorPhase2());
    }


    [ContextMenu("DEBUG PHASE 3")]
    public void StartPhase3()
    {
        _phaseLevel = 3;
        UpdateTree(GetBehaviorPhase3());
        StartCoroutine(Phase3Process());


    }

    public void SetEyesToDefault()
    {
        for (int i = 0; i < _eyeArray.Length; i++)
        {
            var item = _eyeArray[i];

            item.material = _eyeMaterial_Default;
        }
    }
    public void SetEyesToAngry()
    {
        //we will call an audio to show its anger.
        for (int i = 0; i < _eyeArray.Length; i++)
        {
            var item = _eyeArray[i];

            item.material = _eyeMaterial_Angry;
        }
    }

    IEnumerator Phase3Process()
    {
        //let out a scream. its eye turn red. this will keep looping till its defeated.
        SetEyesToAngry();
        ControlIsActing(true);


        //we start roting as we keep shooting the gas

        float startTime = Time.time;
        float dashTime = 50f;
        float dashSpeed = 60;

        float shoot_Cooldown_Total = 0.14f;
        float shoot_Cooldown_Current = 0;


        while (Time.time < startTime + dashTime )
        {
            transform.Rotate(new Vector3(0, 80 * Time.deltaTime, 0));


            if(shoot_Cooldown_Current > shoot_Cooldown_Total)
            {
                BulletScript bullet = GameHandler.instance._pool.GetBullet(ProjectilType.EnemySpit, shootPos.transform);

                bullet.MakeEnemy();
                bullet.MakeSpeed(15, 0, 0);
                bullet.MakeCollision(999);
                bullet.MakeDamage(new DamageClass(90, DamageType.Physical, 0), 0, 0);
                bullet.transform.localScale = Vector3.one * 3;

                bullet.SetUp("Shootgas", transform.forward, 15);

                shoot_Cooldown_Current = 0;
            }
            else
            {
                shoot_Cooldown_Current += Time.deltaTime;
            }


            yield return new WaitForSeconds(Time.deltaTime);
        }


        Debug.Log("2");

        ControlIsActing(false);
        SetEyesToDefault();
        yield return new WaitForSeconds(10);

        StartCoroutine(Phase3Process());


    }

    #endregion

    protected override void Die()
    {
        StopAllCoroutines();
        base.Die();

        
    }

    public void ShootSeed()
    {
        StartCoroutine(ShootSeedProcess());
    }

    IEnumerator ShootSeedProcess()
    {

        Vector3 endPos = PlayerHandler.instance.transform.position;
        Vector3 startPos = transform.position;

        //it needs to have an offset.
        int seedIndex = GetFreeSeed();

        if (seedIndex == -1)
        {
            Debug.Log("broke off");
            yield break;
        }

        TreeSeed selectedSeed = _seedArray[seedIndex];

        RollForRandomInSeed(selectedSeed);


        float duration = 1f;   // Time it takes to travel the arc
        float timeElapsed = 0f;
        float arcHeight = 10;

        int safeBreak = 0;


        selectedSeed.gameObject.SetActive(true);

        AreaDamage _areaDamage = GameHandler.instance._pool.GetAreaDamage(transform);
        DamageClass _damage = new DamageClass(30, DamageType.Physical, 0);
        _areaDamage.SetUp_Regular(endPos, 3, duration, _damage, 3, 0.5f, AreaDamageVSXType.Nothing);
        _areaDamage.transform.position = endPos;

        while (timeElapsed < duration)
        {
            safeBreak++;

            if (safeBreak > 1000)
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
            selectedSeed.transform.position = currentPosition;

            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }

        selectedSeed.Explode(selectedSeed.transform.position);

    }


    //what we are going to do is that we spawn 2 plants and then a trap.

    int _seedSpawnLogic;
   
    void RollForRandomInSeed(TreeSeed treeSeed)
    {      
        if(_seedSpawnLogic >= 2)
        {
            treeSeed.SetSeed(SeedSpawnType.Trap, _phaseLevel);
            _seedSpawnLogic = 0;
        }
        else
        {
            _seedSpawnLogic++;
            treeSeed.SetSeed(SeedSpawnType.Plant, _phaseLevel);
        }
       
       
    }

    int GetFreeSeed()
    {
        for (int i = 0; i < _seedArray.Length; i++)
        {
            var item = _seedArray[i];

            if (!item.gameObject.activeInHierarchy) return i;
        }

        return -1;
    }


    public void CallTreeLog()
    {
        StartCoroutine(TreeLogProcess());
    }

    IEnumerator TreeLogProcess()
    {



        yield return null;
    }
}

//Tree behavior
//the tree rotates to the player


//Phase 1:
//it spits a slow moving cloud of poison
//throw little seeds in teh ground. those seeds can become plants that shoot or can become traps
//the spawn for enemies is increased here.
//when facing the player, it will spawn two logs at teh sides, blocking the player from moving. enemeis should be able to move past it.
//


//Phase 2:
//the flowers deal more damage.
//more seeds.
//more enemies spawning
//it starts shooting gases in all direction with delay after each other.


//Phase 3:
//it gets madned and it rotates shooting straigh
