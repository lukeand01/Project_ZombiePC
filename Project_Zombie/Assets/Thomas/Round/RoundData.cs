using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundData : ScriptableObject
{

    //three types of now:
    //dark moon, which decreases the spawn rate but double the stats of everyone.  should also make their eyes glow red.
    //Madness : increase spawn rate.
    //Hound : spawn way more hounds.

    //and also there is a chance for them to spawn, and a limit. for example they can only appear every 10 rounds.

    public string roundName;
    public Sprite roundSprite;

    public virtual void OnRoundStart()
    {
        
    }
    public virtual void OnRoundEnd()
    {

    }


}

[System.Serializable]
public class RoundClass
{
    public RoundData data;

    [Range(0,101)]public int chance;

    public int MinimalRoundToTrigger;
    public int RoundsPassedPerTrigger;
    int currentRoundsPassed; 

    public RoundClass(RoundData data, int minAllowed, int minPassed)
    {
        this.data = data;
        MinimalRoundToTrigger = minAllowed;
        RoundsPassedPerTrigger = minPassed;
    }

    public void PassRound()
    {
        currentRoundsPassed ++;
    }

    public bool CanTrigger()
    {
        bool isSuccess = currentRoundsPassed >= RoundsPassedPerTrigger;

        if(isSuccess)
        {
            currentRoundsPassed = 0;
        }       

        return isSuccess;
    }

}