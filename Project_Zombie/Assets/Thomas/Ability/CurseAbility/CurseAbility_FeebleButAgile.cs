using UnityEngine;


[CreateAssetMenu(menuName = "Ability / Curse / FeebleButAgile")]
public class CurseAbility_FeebleButAgile : AbilityPassiveData
{

    public override void Add(AbilityClass ability)
    {
        base.Add(ability);

        BDClass bd = new BDClass("Agile", StatType.Dodge, 0.5f, 0, 0);

        AddBDToPlayer(bd);
        PlayerHandler.instance._entityEvents.eventDamageTaken += RollStun;

    }
    public override void Remove(AbilityClass ability)
    {
        base.Remove(ability);

        RemoveBDFromPlayer("Agile");
        PlayerHandler.instance._entityEvents.eventDamageTaken -= RollStun;
    }

    void RollStun()
    {
        int random = Random.Range(0, 101);

        if(random <= 5)
        {
            BDClass bd = new BDClass("Feeble", BDType.Stun, 2);
            AddBDToPlayer(bd);
        }
    }

    public override bool IsCursed()
    {
        return true;
    }
}
