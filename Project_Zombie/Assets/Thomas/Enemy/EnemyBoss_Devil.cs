using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss_Devil : EnemyBoss
{

    float healthRequired_Phase1 = 0;
    float healthRequired_Phase2 = 0;
    float healthRequired_Phase3 = 0;

    public int currentPhase { get; private set; } 

    protected override void StartFunction()
    {

        UpdateTree(GetBehavior());

        base.StartFunction();

        currentPhase = 1;
        healthRequired_Phase1 = health_Total;
        healthRequired_Phase2 = health_Total * 0.5f;
        healthRequired_Phase3 = health_Total * 0.2f;
    }


    public override void TakeDamage(DamageClass damageRef)
    {
        base.TakeDamage(damageRef);

        CheckForBossPhase();
    }

    void CheckForBossPhase()
    {

        if (healt_Current <= healthRequired_Phase2 && currentPhase != 2)
        {
            //then we start the second phase.
            StartPhase2();
            return;
        }

        if (healt_Current <= healthRequired_Phase3 && currentPhase != 3)
        {
            //then we start the third phase.
            StartPhase3();
            return;
        }

    }


    Sequence2 GetBehavior()
    {
        return new Sequence2(new List<Node>
        {
            new Behavior_Boss_CheckAction(this),
            new Behavior_Boss_Devil_Slash(this),
            new Behavior_Boss_Devil_Fireball(this),

        });
    }

    float AttackRange
    {
        get
        {
            float range = 0;

            if (currentPhase <= 1)
            {
                range = 15;
            }
            if (currentPhase > 1)
            {
                range = 20;
            }

            return range;
        }
    }

    public void CallSlash()
    {
        StartCoroutine(SlashProcess(AttackRange));
    }

    IEnumerator SlashProcess(float range)
    {
        ControlIsActing(true);
        SetActionIndexCurrent(0);
        _animator.Play("Animation_Devil_Slash", 1);
        _animator.Play("Animation_Devil_Idle", 2);
        

        float duration_Total = 1;
        float duration_Current = 0;

        _abilityCanvas.gameObject.SetActive(true);
        _abilityCanvas.StartCircleIndicator(range);

        while (duration_Total > duration_Current)
        {
            duration_Current += Time.deltaTime;
            _abilityCanvas.ControlCircleFill(duration_Current, duration_Total);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        _animator.SetFloat("AttackSpeed", 1);
        
        yield return new WaitForSeconds(0.1f);

        _abilityCanvas.gameObject.SetActive(false);


        ControlIsActing(false);

    }

    [SerializeField] LayerMask targetLayer;

    void DamageSlash()
    {
        //need to deal damage on allies as well

        
        targetLayer |= (1 << 3);
        RaycastHit[] foundArray = Physics.SphereCastAll(transform.position, 10, Vector2.up, 50, targetLayer);



        DamageClass damage = new DamageClass(50, DamageType.Physical, 0);

        for (int i = 0; i < foundArray.Length; i++)
        {
            var item = foundArray[i];

            IDamageable damageable = item.collider.GetComponent<IDamageable>();

            if (damageable == null) continue;


            damageable.TakeDamage(damage);

        }

    }

    public override void StartChargingAttack()
    {
        _animator.SetFloat("AttackSpeed", 0);
    }


    protected override void CallUI(float current, float total)
    {
        base.CallUI(current, total);

        if(actionIndex_Current == 0)
        {

        }


    }
    public override void CalculateAttack()
    {
        base.CalculateAttack();

        if(actionIndex_Current == 0)
        {
            //call attack in the area. do the same thing as the knife.
            //create damagearea in the target.
            DamageSlash();

        }
        if(actionIndex_Current == 1)
        {
            DamageFireball();
        }

    }


    //phase 2 just change the behavior.
    void StartPhase2()
    {
        //we summon the two blades here.

        StartCoroutine(Phase2Process());
        
    }

    IEnumerator Phase2Process()
    {
        SummonFlyingBlades();
        currentPhase = 2;

        yield return null;
    }

    void SummonFlyingBlades()
    {

    }

    void StartPhase3()
    {
        //we allow to do hellSummon

        StartCoroutine(Phase3Process());

    }

    IEnumerator Phase3Process()
    {
        currentPhase = 3;

        yield return null;
    }

    //
    public void CallHellSummon()
    {
        StartCoroutine(HellSummonProcess());
    }

    IEnumerator HellSummonProcess()
    {
        int rounds = 5;

        //get portals in the boss room and spawn hellhounds.

        for (int i = 0; i < rounds; i++)
        {
            //stay idle. 
            //



        }




        yield return null;

    }


    public void CallFireball()
    {
        StartCoroutine(FireballProcess());
    }

    IEnumerator FireballProcess()
    {
        ControlIsActing(true);
        SetActionIndexCurrent(1);
        _animator.Play("Animation_Devil_Ranged_01", 1);
        _animator.Play("Animation_Devil_Idle", 2);


        yield return new WaitForSeconds(2);

        ControlIsActing(false);
    }

    void DamageFireball()
    {
        float value = 3;


        float radius = Fireball_AttackRadius;
        int quantity = FireballQuantity;


        DamageClass damage = new DamageClass(60, DamageType.Magical, 0);

        //the first one is always on the player
        for (int i = 0; i < quantity; i++)
        {
            Vector3 attackPos = PlayerHandler.instance.transform.position;
            if (i != 0)
            {
                //if its here.
                float x = Random.Range(-value, value);
                float z = Random.Range(-value, value);
                attackPos = attackPos + new Vector3(x, 0, z);
            }

            AreaDamage areaDamage_Regular = GameHandler.instance._pool.GetAreaDamage(transform);

            areaDamage_Regular.SetUp_Regular(attackPos, radius, 1, damage, 3, 0.5f, AreaDamageVSXType.Fireball_Explosion);

            AreaDamage areaDamage_Const = GameHandler.instance._pool.GetAreaDamage(transform);
            areaDamage_Const.SetUp_Continuously(attackPos, radius, 2, 1.3f, damage, 3, 0, AreaDamageVSXType.Nothing); //we need a fireburning.

        }

        
    }


    float Fireball_AttackRadius
    {
        get
        {
            float radius = 0;

            if (currentPhase <= 1)
            {
                radius = 3;
            }
            if (currentPhase > 1)
            {
                radius = 5.5f;
            }

            return radius;
        }
    }


    int FireballQuantity
    {
        get
        {
            int quantity = 0;

            if (currentPhase <= 1)
            {
                quantity = 4;
            }
            if (currentPhase > 1)
            {
                quantity = 8;
            }

            return quantity;
        }
    }

}


//first we check if it can slash.

//phase 1
//moves towards the player. on range it trigger the slash.
//teh slash creates areas of fire
//on range it throws fireballs. it sets the area on fire for a duration_Total.
//

//phase 2
//it summons two fireblades that keep moving randomly and changing direction everytime they touch 
//increases the radius for slash and fireball.

//phase 3
//gains an attack that it stops and starts emiting waves that the player must dodge to not take damage.
//also summon hellhounds from the corners.
//gains speed
//its steps create areas of fire now.

//need an animation for spawn.

//for slash
//for death
//
//

//need something for the fireball.


//it needs to be mmore interesting.


//it will constantly move towareds the player
//it uses a two handed sword
//he uses chains.
//

//it has phases.
//1: 
//2:summons blades that keep moving around the room. they bounc off walls; he charges at the player;
//3 when he turns creates areas of damage that can only be dodged, by action dodging; he summons firehounds;

//after every action he throws fire around and creates damage areas.
//it slashes forward.
//
