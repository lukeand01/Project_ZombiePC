using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "QuestSystem / Reward / Gun")]
public class Quest_RewardData_Gun : Quest_RewardData
{
    public override void ReceiveReward(Quest_RewardClass _rewardClass)
    {
        //we inform someone to get a gun_Perma based in the 

        int index = _rewardClass.value;


    }
}
