using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_Boss_Ghost_BlindArea : Sequence2
{

    EnemyBoss _boss;
    int _actionIndex;

    float _cooldown_Total;
    float _cooldown_Current;


    public Behavior_Boss_Ghost_BlindArea(EnemyBoss boss, float cooldown_Total, int actionIndex)
    {
        _boss = boss;
        _actionIndex = actionIndex;

        _cooldown_Total = cooldown_Total;
        _cooldown_Current = Random.Range(_cooldown_Total * 0.5f, _cooldown_Total * 1.5f); ;
    }


    public override NodeState Evaluate()
    {
        //this doesnt make the ghost stop. it simply throws a thing off
        //it has a cooldown assigned to it.
        if (_cooldown_Current > 0)
        {
            _cooldown_Current -= Time.deltaTime;
            return NodeState.Success;
        }

        if (_boss.IsActing)
        {
            Debug.Log("that");
            return NodeState.Success;
        }


        Debug.Log("1");
       AreaDamage _areaDamage =  GameHandler.instance._pool.GetAreaDamage(_boss.transform);
        Vector3 areaPos = MyUtils.GetRandomPointInAnnulus(PlayerHandler.instance.transform.position, 2, 5);

        int safeBreak = 0;
        
        while(_boss.IsTargetPosWalkable(areaPos))
        {
            areaPos = MyUtils.GetRandomPointInAnnulus(PlayerHandler.instance.transform.position, 2, 5);
            safeBreak++;
            if(safeBreak > 1000)
            {
                areaPos = PlayerHandler.instance.transform.position;
                break;
            }

        }



        _areaDamage.SetUp_Continuously(areaPos, 4.7f, 3, 23.3f, new DamageClass(5, DamageType.Physical, 0), 3, 0, AreaDamageVSXType.Ghost_Orb);

        BDClass bd_Slow = new BDClass("Ghost_Slow", StatType.Speed, 0, -0.2f, 0);
        bd_Slow.MakeShowInUI();
        bd_Slow.MakeTemp(2);
        bd_Slow.MakeStack(5, true);

        BDClass bd_Blind = new BDClass("Ghost_Blind", BDType.Blind, 5);
        bd_Slow.MakeShowInUI();
        bd_Slow.MakeTemp(5);
        bd_Slow.MakeStack(0, true);

        BDClass[] bdArray = {bd_Slow};

        _areaDamage.Make_BD(bdArray);
        
        _cooldown_Current = Random.Range(_cooldown_Total * 0.8f, _cooldown_Total * 1.5f); 


        return NodeState.Success;
    }

}
