using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName = "Quest / Reward / Bless")]
public abstract class Quest_RewardData : ScriptableObject
{
    //we can have multiple things to do.
    [TextArea]public string rewardDescription;

    public abstract void ReceiveReward(Quest_RewardClass _rewardClass);
    

}

[System.Serializable]
public class Quest_RewardClass 
{
    [field:SerializeField] public Quest_RewardData data { get; private set; }
    [field: SerializeField] public int value {  get; private set; }

    public Quest_RewardClass(Quest_RewardData data, int value)
    {
        this.data = data;
        this.value = value;

    }
}