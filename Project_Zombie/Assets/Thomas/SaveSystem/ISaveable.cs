using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable
{
    object CaptureState();
    void RestoreState(object state);

}




//at the start i can assign all the data

//we can do it bit different
//instead of an generic arguemtn i will give a class.
//the saveable entity will receive the saveclass and just give the information it requires.