using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability / Passive / Converter")]
public class AbilityPassiveData_Converter : AbilityPassiveData
{

    //we are going to flat increase damage.
    //and at the most we are going to roll for a chance to converte this fella.
    //flat chance. leadership.

    public override void Add(AbilityClass ability)
    {
        base.Add(ability);


        float firstValue = GetFirstValue(ability.stackList);

        BDClass bd = new BDClass("Ability_Converter", StatType.Leadership, firstValue, 0, 0);
        AddBDToPlayer(bd);

        if(ability.level >= 5)
        {
            PlayerHandler.instance._entityEvents.eventKilledEnemy += (enemy, isPlayer) => TryConvertEnemy(ability, enemy, isPlayer);
        }
       

    }
    public override void Remove(AbilityClass ability)
    {
        base.Remove(ability);

        RemoveBDFromPlayer("Ability_Converter");


        PlayerHandler.instance._entityEvents.eventKilledEnemy -= (enemy, isPlayer) => TryConvertEnemy(ability, enemy, isPlayer);

    }

    void TryConvertEnemy(AbilityClass _ability, EnemyBase _enemy, bool isPlayer)
    {
        //we check for the chance of converting.
        //we inform that this fella will no longer die

        Debug.Log("here");

        if (!isPlayer)
        {
            return;
        }
        if (!_enemy.GetData().CanTurnAlly) return;

        int roll = Random.Range(0, 101);
        int requiredRoll = 100;

        if(requiredRoll > roll)
        {
            _enemy.PreventDeath();
            _enemy.MakeAlly(10);
        }

    }
}
