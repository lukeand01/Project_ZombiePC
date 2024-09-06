using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatTracker : MonoBehaviour
{
    //we gather all the information here.

    Dictionary<StatTrackerType, float> playerStatTracker_Dictionary = new();
    List<StatTrackerType> refList = new();

    private void Awake()
    {
        ResetStatTracker();
    }

    //we only set the list when we are restarting the player
    public void ResetStatTracker()
    {
        refList = MyUtils.GetStatTrackerRefList();

        playerStatTracker_Dictionary.Clear();

        foreach (var item in refList)
        {
            playerStatTracker_Dictionary.Add(item, 0);
        }
    }

    public void StopTimer()
    {
        //
    }

    //time is the only one stored here.

    public void ChangeStatTracker(StatTrackerType statTrackerType, float changeValue)
    {
        if(playerStatTracker_Dictionary.ContainsKey(statTrackerType))
        {
            playerStatTracker_Dictionary[statTrackerType] += changeValue;
        }
        else
        {
            Debug.Log("didnt find this value. something wrong");
        }
    }

    public Dictionary<StatTrackerType, float> GetStatTrackDictionary() => playerStatTracker_Dictionary;

    void ClearStatList()
    {

    }

}

public enum StatTrackerType
{
    TimeSpent,
    EnemiesKilled,
    PointsGained,
    PassiveAbilitiesFound,
    GunChestsUsed,
    ResourceChestsFound,
    DamageTaken,
    DamageDealt_Total,
    DamageDealt_Physical,
    DamageDealt_Magical,
    DamageDealt_Plasma,
    DamageDealt_Corruption,
    DamageDealt_Pure,
}
