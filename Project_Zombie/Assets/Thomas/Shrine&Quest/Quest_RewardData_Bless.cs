using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "QuestSystem / Reward / Bless")]
public class Quest_RewardData_Bless : Quest_RewardData
{
    public override void ReceiveReward(Quest_RewardClass _rewardClass)
    {
        PlayerHandler.instance._playerResources.Bless_Gain(_rewardClass.value);

    }
}
