using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss_Twin : EnemyBoss
{

    [SerializeField] EnemyBoss_Twin_Small[] _minionArray;
    [SerializeField] GameObject _minionHolder;
    public Transform _minionPos;

    protected override void AwakeFunction()
    {
        base.AwakeFunction();
    }
    protected override void StartFunction()
    {
        base.StartFunction();

        UpdateTree(GetBehavior());
    }

    #region BEHAVIORS
    Sequence2 GetBehavior()
    {
        return new Sequence2(new List<Node>
        {
           new Behavior_Boss_Twin_Minions(this, 5, _minionArray),
           new Behavior_Boss_Twin_Meteor(this, 5, 4),
           new Behavior_Boss_Twin_Seeking(this, 5),
           new Behavior_Boss_Twin_HealOrb(this, 15)
        });
    }
    Sequence2 GetBehavior_2()
    {
        return new Sequence2(new List<Node>
        {
           new Behavior_Boss_Twin_Minions(this, 5, _minionArray),
           new Behavior_Boss_Twin_Meteor(this, 5, 4),
           new Behavior_Boss_Twin_Seeking(this, 5),
           new Behavior_Boss_Twin_HealOrb(this, 15)
        });
    }
    Sequence2 GetBehavior_3()
    {
        return new Sequence2(new List<Node>
        {
           new Behavior_Boss_Twin_Minions(this, 5, _minionArray),
           new Behavior_Boss_Twin_Meteor(this, 5, 4),
           new Behavior_Boss_Twin_Seeking(this, 5),
           new Behavior_Boss_Twin_HealOrb(this, 15)
        });
    }

    #endregion

    #region RECEIVE DAMAGE
    public int _phaseLevel;

    public override void TakeDamage(DamageClass damageRef)
    {
        base.TakeDamage(damageRef);

        CheckBossPhase();

    }

    void CheckBossPhase()
    {
        if (health_Current <= health_Total * 0.75f && health_Current > health_Total * 0.4f && _phaseLevel != 2)
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
        UpdateTree(GetBehavior_2());
    }


    [ContextMenu("DEBUG PHASE 3")]
    public void StartPhase3()
    {
        _phaseLevel = 3;
        UpdateTree(GetBehavior_3());
        //StartCoroutine(Phase3Process());
    }


    #endregion



    public void CallSeeker()
    {
        StartCoroutine(SeekerProcess());
    }
    IEnumerator SeekerProcess()
    {
        yield return null;
    }

    public void CallHealOrb()
    {
        StartCoroutine(HealOrbProcess());    
    }
    IEnumerator HealOrbProcess()
    {
        yield return null;
    }


    

    //i dont like this attack.
    //i think attacks should try to land in the player instead of moving by side.
    //
}


//an ugly and small twin that chases the player. he is supposed to be annoying.
//a beatufiul and big twin that shoots the player. he is supposed to deal most of the damage.

//its not a twin, its a soldier.
//every phase we will spawn a new soldier. each soldier does something different?
//so when they hit the player they disappear, if the player is stunned the small does not move.
//and a new one is spawned from the mage. always need to have the same as the phase level.
//the small has the autoattack, which stun and desrtroy itself. and it has a long range with an indicator that deal a little damage and slows.


//the small will constantly follow the player
//its goal is to try and get the player to stop moving
//it applies slow and stun
//small does not receive damage. it will show as shielded.
//

//PHASE 1
//fire seeking projtiles.
//fire meteors.
//at period it will summon little green orbs in the outer part from the portals. it will travel to the big and can be destroyed. if it reaches big it will heal it


//PHASE 2
//increase small speed. spawn another.
//increase amount of attacks and lower cooldown
//


//PHASE 3
//increase small speed. spawn another.
//
