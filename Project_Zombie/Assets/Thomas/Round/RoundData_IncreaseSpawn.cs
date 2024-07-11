using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RoundType / IncreaseSpawn")]
public class RoundData_IncreaseSpawn : RoundData
{
    //make it spawn more.

    public override void OnRoundStart()
    {
        base.OnRoundStart();

        LocalHandler.instance.SetSpawnCap(150);
        LocalHandler.instance.SetRoundSpawnModifier(1.3f);
    }

    public override void OnRoundEnd()
    {
        base.OnRoundEnd();

        LocalHandler.instance.SetSpawnCap(100);
        LocalHandler.instance.SetRoundSpawnModifier(1);
    }

}
