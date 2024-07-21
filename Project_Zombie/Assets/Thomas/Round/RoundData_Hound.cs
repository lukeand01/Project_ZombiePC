using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RoundType / Hound")]
public class RoundData_Hound : RoundData
{
    //so we can create a especial list and a chance for the list
    //
    [Separator("ESPECIAL LIST FOR SPAWNING")]
    [SerializeField] List<EnemyChanceSpawnClass> enemyChanceList = new();
    [SerializeField][Range(0,100)] int preferenceForEspecialList;
    public override void OnRoundStart()
    {
        base.OnRoundStart();

        LocalHandler.instance.SetEspecialList(enemyChanceList, preferenceForEspecialList);

    }

    public override void OnRoundEnd()
    {
        base.OnRoundEnd();

        LocalHandler.instance.ResetEspecialList();

    }


}
