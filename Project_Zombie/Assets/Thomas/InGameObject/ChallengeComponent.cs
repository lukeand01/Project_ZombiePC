using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChallengeComponent : MonoBehaviour
{
    //this will be a class used on it.

    //we will create two things using this for now:
    //create a area and check if the player is still there, while you send waves towards the player
    //spawn an objcet in the center the player must damage. the object will slowly move. 

    public abstract void GiveReward();
    
    public abstract void StartChallengeComponent();
    
    public abstract void HandleChallengeComponent();
    
    public abstract void StopChallengeComponent();
    


    

}
