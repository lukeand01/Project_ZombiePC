using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    //i will have another stuff.
    //this will just be for _enemy


    //everyone has the same controller, but people call different things?
    //no, it will be easier to have different controls and name them the same thing, i just need the _id


    string nameID;

    public void SetID(string nameID)
    {
        this.nameID = nameID;
    }



}
